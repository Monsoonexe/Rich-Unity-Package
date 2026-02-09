using RichPackage.GuardClauses;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject.Internal;

namespace RichPackage.Interaction
{
    /// <summary>
    /// Manages an actor's interaction with interactables.
    /// </summary>
    public class InteractionManager : RichMonoBehaviour
    {
        private readonly List<IInteractable> interactables = new List<IInteractable>(8);

        [Title("Settings")]
        public bool debug = false;

        // runtime data
        /// <summary>
        /// The thing performing the interaction.
        /// </summary>
        public IInteractor Actor;

        private IInteractable _target;

        /// <summary>
        /// The current interactable (or <see langword="null"/> if there is none).
        /// </summary>
        public IInteractable Target
        {
            get => _target;
            set
            {
                if (value == _target)
                {
                    return; // no change
                }

                // unfocus the old one
                if (_target != null)
                {
                    if (debug)
                        Debug.Log($"{Actor} is losing focus on {_target}");

                    Actor.OnLoseFocus(_target);
                    _target.OnLoseFocus(Actor); // hover effects
                }

                // take the new one.
                _target = value;
                interactables.AddIfNew(_target);

                // focus the new one
                if (_target != null)
                {
                    if (debug)
                        Debug.Log($"{Actor} is taking focus on {_target}");

                    // note: should these call even when the target is 'null' to indicate 'none'?
                    Actor.OnTakeFocus(_target);
                    _target.OnTakeFocus(Actor); // hover effects
                }

                OnInteractableChanged?.Invoke(Actor, value);
            }
        }

        public bool HasTarget => _target != null;

        /// <summary>
        /// actor, new, old.
        /// </summary>

        public event System.Action<IInteractor, IInteractable> OnInteractableChanged;

        public IReadOnlyList<IInteractable> KnownInteractables => interactables;

        #region Editor
#if UNITY_EDITOR

        [ShowInInspector, LabelText("Actor Name")]
        private string Editor_ActorName
        {
            get => Actor?.ToString() ?? "none";
        }

        [ShowInInspector, LabelText("Target Name")]
        private string Editor_TargetName
        {
            get => Target?.ToString() ?? "none";
        }

        [ShowInInspector, LabelText("Count")]
        public int Count => interactables.Count;

#endif
        #endregion Editor

        #region Unity Messages

        protected override void Reset()
        {
            SetDevDescription("Handles interactions between actors and interactables.");
        }

        private void OnEnable()
        {
            RemoveOutOfRangeInteractables();

            // if we are in range of an interactable when we are enabled,
            // then we need to enter it now
            if (Actor != null && Target != null && Target.IsEnabled)
            {
                Actor.OnTakeFocus(Target);
            }
        }

        private void OnDisable()
        {
            if (App.IsQuitting)
                return;

            if (Target != null && Actor != null)
            {
                Actor.OnLoseFocus(Target);
            }
        }

        #endregion Unity Messages

        public void DoInteraction()
        {
            if (Actor == null)
            {
                Debug.LogError("No actor has been set.", this);
                return;
            }

            if (Target == null)
            {
                Debug.LogError("No interactable has been set.", this);
                return;
            }

            // interact is requested and possible 
            Actor.InteractWith(Target);
        }

        /// <summary>
        /// Adds <paramref name="interactable"/> to the system but does not force taking focus.
        /// </summary>
        /// <remarks>Will only take focus if it is the only known item.</remarks>
        public void Add(IInteractable interactable)
        {
            GuardAgainst.ArgumentIsNull(interactable, nameof(interactable));

            if (AddInternal(interactable))
            {
                if (interactable.IsEnabled)
                    Target = interactable;
            }
        }

        /// <returns>True if <paramref name="interactable"/> was added, or False if already known.</returns>
        private bool AddInternal(IInteractable interactable)
        {
            // track it (once)
            bool added = interactables.AddIfNew(interactable);
            return added;
        }

        /// <summary>
        /// Remove an interactable from the context.
        /// </summary>
        public void Remove(IInteractable interactable)
        {
            GuardAgainst.ArgumentIsNull(interactable, nameof(interactable));

            interactables.QuickRemove(interactable);

            if (interactable == Target)
                Target = null;
        }

        public void RemoveAll()
        {
            while (interactables.Count > 0)
            {
                Remove(interactables.Last()); // last
            }
        }

        /// <remarks>Will try to focus the next best interactable.</remarks>
        public void RemoveOutOfRangeInteractables()
        {
            // approximate our collider's bounds (it's definitely a capsule, right?).
            Vector3 start = transform.position;
            Vector3 end = start.PlusY(2); // height guess
            float radius = 0.5f; // guess
            int layer
                = (1 << 0) // default layer
                | (1 << gameObject.layer);

            // include all known objects in the layer mask
            foreach (var i in interactables
                .Cast<Component>())
            {
                layer |= 1 << i.gameObject.layer;
            }

            // get all interactables within our aproximate range
            using (ZenPools.Spawn(out Collider[] colliders, 16)) // lol what happens if we do this while inside a particle effect???
            using (ZenPools.Spawn(out List<IInteractable> interactablesInRange))
            {
                int count = Physics.OverlapCapsuleNonAlloc(start, end, radius,
                    colliders, layer, QueryTriggerInteraction.Collide);

                // optimize linq if this goes to production.
                colliders
                    .Take(count)
                    .Select((c) => c.GetComponent<IInteractable>())
                    .Where(i => i != null)
                    .ToList(interactablesInRange);

                // iterate backwards due to removal
                for (int i = interactables.Count - 1; i >= 0; i--)
                {
                    var query = interactables[i];
                    // if we are no longer inside this guy's trigger volume
                    if (!interactablesInRange.Contains(query))
                    {
                        Remove(query);
                    }
                }
            }
        }

        public IInteractable GetClosestEnabledInteractable()
        {
            Vector3 pos = Actor.Transform.position;
            return Utility.GetClosestObject(EnumerateEnabledInteractables(), pos);
        }

        /// <summary>
        /// Yields all enabled interactables and their transforms.
        /// </summary>
        public IEnumerable<(IInteractable Interactable, Transform Transform)> EnumerateEnabledInteractables()
        {
            foreach (IInteractable interactable in interactables)
            {
                if (interactable.IsEnabled)
                    yield return (interactable, interactable.InteractionPoint);
            }
        }
    }
}
