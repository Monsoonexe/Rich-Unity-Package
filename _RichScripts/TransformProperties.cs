using DG.Tweening;
using RichPackage.GuardClauses;
using System;
using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Properties for a <see cref="Transform"/>.
    /// </summary>
    [Serializable]
    public struct TransformProperties : IEquatable<Transform>, IEquatable<TransformProperties>
    {
        public Space space;
        public Vector3 position;
        public Quaternion rotation;

        #region Constructors

        public TransformProperties(Transform transform, Space space = Space.World)
        {
            // validate
            GuardAgainst.ArgumentIsNull(transform, nameof(transform));

            // operate
            this.space = space;
            position = default;
            rotation = default;
            Store(transform);
        }

        #endregion Constructors

        // TODO - Load and Store names SUCK!!! I can never remember which is which.

        /// <summary>
        /// Sets this object's properties to <paramref name="t"/>'s.
        /// </summary>
        /// <remarks>this = that</remarks>
        public void Store(Transform t)
        {
            // validate
            GuardAgainst.ArgumentIsNull(t, nameof(t));

            // operate
            if (space == Space.Self)
            {
                position = t.localPosition;
                rotation = t.localRotation;
            }
            else
            {
                position = t.position;
                rotation = t.rotation;
            }
        }

        /// <summary>
        /// Sets this object's properties to <paramref name="t"/>'s.
        /// </summary>
        /// <remarks>this = that</remarks>
        public void Store(Transform t, Space space)
        {
            this.space = space;
            Store(t);
        }

        /// <summary>
        /// Set's <paramref name="t"/>'s properties to those stored in this object.
        /// </summary>
        /// <remarks>that = this</remarks>
        public void Load(Transform t)
        {
            // validate
            GuardAgainst.ArgumentIsNull(t, nameof(t));

            // operate
            if (space == Space.Self)
                t.SetLocalPositionAndRotation(position, rotation);
            else
                t.SetPositionAndRotation(position, rotation);
        }

        #region IEquatable

        public bool Equals(Transform other)
        {
            return this.Equals(new TransformProperties(other, space));
        }

        public bool Equals(TransformProperties other)
        {
            // throw? or just return false? A mixmatch is probably a mistake, right?
            // it's possible that world and local space can be identical...
            if (this.space != other.space)
                throw new InvalidOperationException($"The two comparands are in different spaces and cannot be compared: 'this' is {this.space} space but 'other' is {other.space} space!");

            return this.position == other.position
                && this.rotation == other.rotation;
        }

        #endregion IEquatable
    }

    public static class TransformPropertiesExtensions
    {
        /// <summary>
        /// Sets the world space position and rotation of <paramref name="t"/>.
        /// </summary>
        public static void SetPositionAndRotation(this Transform t, TransformProperties props)
        {
            // validate
            Debug.Assert(props.space == Space.World);

            // operate
            t.SetPositionAndRotation(props.position, props.rotation);
        }

        /// <summary>
        /// Sets the local space position and rotation of <paramref name="t"/>.
        /// </summary>
        public static void SetLocalPositionAndRotation(this Transform t, TransformProperties props)
        {
            // operate
            props.space = Space.Self;
            t.localPosition = props.position;
            t.localRotation = props.rotation;
        }
    }

    public static class TransformPropertiesDOTweenExtensions
    {
        /// <summary>
        /// Tweens <paramref name="target"/>'s position and rotation to the target <paramref name="endValue"/>.
        /// </summary>
        public static Sequence DOOrient(this Transform target, TransformProperties endValue, float duration)
        {
            Sequence seq = DOTween.Sequence(); // result

            if (endValue.space == Space.Self)
            {
                seq.Append(target.DOMove(endValue.position, duration))
                    .Join(target.DORotate(endValue.rotation.eulerAngles, duration));
            }
            else
            {
                seq.Append(target.DOLocalMove(endValue.position, duration))
                    .Join(target.DOLocalRotate(endValue.rotation.eulerAngles, duration));
            }

            return seq;
        }
    }
}

