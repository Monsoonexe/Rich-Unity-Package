using RichPackage.UI.Transitions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RichPackage.UI.Framework
{
	[SelectionBase,
		RequireComponent(typeof(Canvas)),
		RequireComponent(typeof(GraphicRaycaster))]
	public abstract class AUIScreen<TProps> : MonoBehaviour, IUIScreen
		where TProps : IScreenProperties
	{
		[SerializeField, Required]
		protected GameObject container;
		protected GraphicRaycaster graphicRaycaster;

		[Title("Transitions")]
		[Tooltip("Transition IN animation.")]
		[SerializeField]
		private ATransitionComponent transitionINAnimator;
		public ATransitionComponent TransitionINAnimator
		{
			get => transitionINAnimator;
			set => transitionINAnimator = value;
		}

		[Tooltip("Transition OUT animation.")]
		[SerializeField]
		private ATransitionComponent transitionOUTAnimator;
		public ATransitionComponent TransitionOUTAnimator
		{
			get => transitionOUTAnimator;
			set => transitionOUTAnimator = value;
		}

		/// <summary>
		/// This is the data payload and settings for this screen. 
		/// You can rig this directly in a prefab and-or pass it when you show this screen.
		/// </summary>
		[Title("Settings")]
		[Tooltip("This is the data payload and settings for this screen. You can rig this directly in a prefab and/or pass it when you show this screen.")]
		[SerializeField]
		private TProps properties;

		/// <summary>
		/// This is the data payload and settings for this screen. 
		/// You can rig this directly in a prefab and/or pass it when you show this screen.
		/// </summary>
		protected TProps Properties { get => properties; }

		/// <summary>
		/// Unique Identifier for this ID. Maybe it's prefab name?
		/// </summary>
		[SerializeField,
			Tooltip("[Optional] If this is null/empty, it'll be set to its prefab name."),
			CustomContextMenu("Assign Name", "@screenId = gameObject.name")]
		private string screenId = string.Empty;

		public string ScreenID
		{
			get => screenId;
			set => screenId = value;
		}

		/// <summary>
		/// Is this screen currently visible?
		/// </summary>
		/// <value>true if visible; otherwise, false.</value>
		[ReadOnly, ShowInInspector]
		public bool IsOpen { get; private set; }

		/// <summary>
		/// Is a transition animation on going? Generally user input should be ignored during 
		/// while a transition is active.
		/// </summary>
		public bool IsTransitioning
		{
			get => TransitionINAnimator != null && TransitionINAnimator.IsAnimating
				|| TransitionOUTAnimator != null && TransitionOUTAnimator.IsAnimating;
		}

		#region Events

		/// <summary>
		/// Get called when Transition IN Animation is complete.
		/// </summary>
		public event Action<IUIScreen> OnTransitionInFinishedCallback;

		/// <summary>
		/// Get called when Transition OUT Animation is complete.
		/// </summary>
		public event Action<IUIScreen> OnTransitionOutFinishedCallback;

		/// <summary>
		/// Called from <see cref="MonoBehaviour.OnDestroy"/> event.
		/// </summary>
		/// <value>The destruction action.</value>
		public event Action<IUIScreen> OnScreenDestroyed;

		#endregion Events

		#region Unity Messages

		protected virtual void Reset()
		{
			// add Canvas to limit redraw.
			gameObject.GetOrAddComponent<Canvas>();
			gameObject.GetOrAddComponent<GraphicRaycaster>();
			container = gameObject;
			screenId = gameObject.name;
		}

		protected virtual void OnDestroy()
		{
			// death rattle
			OnScreenDestroyed?.Invoke(this);

			// release refs
			OnTransitionInFinishedCallback = null;
			OnTransitionOutFinishedCallback = null;
			OnScreenDestroyed = null;
		}

		#endregion Unity Messages

		#region Animation

		private void Animate(ATransitionComponent animator,
			Action onCompleteCallback, bool isVisible)
		{
			if (animator)
			{
				// if trying to be shown and not already shown
				if (isVisible && !container.activeSelf)
					container.SetActive(true);

				animator.Animate(transform, onCompleteCallback);
			}
			else
			{
				// just toggle and move on
				container.SetActive(isVisible);
				onCompleteCallback?.Invoke();
			}
		}

		private void OnTransitionINFinished()
		{
			IsOpen = true;
			OnInPlace(); // user code
			OnTransitionInFinishedCallback?.Invoke(this);
		}

		private void OnTransitionOUTFinished()
		{
			IsOpen = false;
			container.SetActive(false);
			OnHidden(); // user code
			OnTransitionOutFinishedCallback?.Invoke(this);
		}

		/// <summary>
		/// Called after the screen has animated into place and is ready to be interacted with.
		/// </summary>
		/// <remarks>No need to call base.</remarks>
		protected virtual void OnInPlace() { }

		#endregion Animation

		#region AUIScreenController

		/// <summary>
		/// Use this to set load data from properties. Called when Screen is Shown before animations.
		/// </summary>
		/// <remarks>Basically 'OnShowing'. The screen is about to become visible. No need to call base.</remarks>
		protected virtual void OnPropertiesSet() { }

		/// <summary>
		/// Called when the screen has finished its hide animation. Can use this to clean up or 
		/// put into an inactive state.
		/// </summary>
		/// <remarks>The screen is inactive when this is called. No need to call base.</remarks>
		protected virtual void OnHidden() { }

		/// <summary>
		/// Called while the screen is still visible and beginning to close,
		/// but before any animations or visuals.
		/// </summary>
		/// <remarks>No need to call base.</remarks>
		protected virtual void OnHiding() { }

		/// <summary>
		/// Framework method that configures and sets the properties.
		/// </summary>
		/// <param name="payload">Data sent to the screen.</param>
		/// <seealso cref="OnPropertiesSet"/>
		protected virtual void SetProperties(TProps payload)
		{
			properties = payload;
		}

		/// <summary>
		/// Override if any special behavior to be called when the hierarchy is adjusted.
		/// </summary>
		/// <remarks>No need to call base.</remarks>
		protected virtual void OnHierarchyChanged() { }

		#endregion AUIScreenController

		#region Hide/Show Interface

		void IUIScreen.Hide(bool animate)
		{
			// cancel in animation
			transitionINAnimator?.Stop(); // 

			OnHiding();

			// do animation
			Animate(animate ? transitionOUTAnimator : null,
				OnTransitionOUTFinished, false);
		}

		void IUIScreen.Show(IScreenProperties payload)
		{
			// set screen properties
			if (payload != null)
			{
				if (payload is TProps props)
				{
					SetProperties(props);
				}
				else
				{
					Debug.LogError($"[{nameof(AUIScreen<TProps>)}] Properties passed have wrong type! " +
						$"({payload.GetType()} instead of {typeof(TProps)}).", this);

					// default to Inspector values
				}
			}

			OnHierarchyChanged(); // react to change in hierarchy

			// catch exception, but don't let it interrupt the opening of the window.
			try
			{
				OnPropertiesSet(); // validate and load data
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, this);
			}

			if (IsOpen) // if currently showing
			{
				OnTransitionINFinished();
			}
			else // already visible, so just do OnFinish callback
			{
				// animate with this animator, when finished call this, show?
				Animate(transitionINAnimator, OnTransitionINFinished, true);
			}
		} // end function

		#endregion Hide/Show Interface
	}
}
