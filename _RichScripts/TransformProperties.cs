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
        public static TransformProperties Origin => new()
        {
            space = Space.World,
            position = Vector3.zero,
            rotation = Quaternion.identity
        };

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
            if (space == Space.World)
                transform.GetPositionAndRotation(out position, out rotation);
            else
                transform.GetLocalPositionAndRotation(out position, out rotation);
        }

        #endregion Constructors

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
        /// Gets the world position and rotation properties from <paramref name="t"/>.
        /// </summary>
        public static TransformProperties GetPositionAndRotation(this Transform t)
        {
            t.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            return new TransformProperties
            {
                space = Space.World,
                position = position,
                rotation = rotation
            };
        }

        /// <summary>
        /// Gets the local position and rotation properties from <paramref name="t"/>.
        /// </summary>
        public static TransformProperties GetLocalPositionAndRotation(this Transform t)
        {
            t.GetLocalPositionAndRotation(out Vector3 position, out Quaternion rotation);
            return new TransformProperties
            {
                space = Space.Self,
                position = position,
                rotation = rotation
            };
        }

        /// <summary>
        /// Sets the world space position and rotation of <paramref name="t"/>.
        /// </summary>
        public static void SetPositionAndRotation(this Transform t, 
            TransformProperties props)
        {
            // operate
            if (props.space == Space.Self)
                t.SetLocalPositionAndRotation(props.position, props.rotation);
            else
                t.SetPositionAndRotation(props.position, props.rotation);
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

