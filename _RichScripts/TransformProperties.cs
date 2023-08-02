using RichPackage.GuardClauses;
using System;
using UnityEngine;

namespace RichPackage
{
	/// <summary>
	/// Properties for a <see cref="Transform"/>.
	/// </summary>
	[System.Serializable]
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
			if (space == Space.Self)
			{
                position = transform.localPosition;
                rotation = transform.localRotation;
            }
			else
			{
                position = transform.position;
                rotation = transform.rotation;
            }
		}

        #endregion Constructors

        // TODO - Load and Store names SUCK!!! I can never remember which is whic.

        /// <summary>
        /// Sets this object's properties to <paramref name="t"/>'s.
        /// </summary>
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
        public void Store(Transform t, Space space)
        {
            this.space = space;
            Store(t);
        }

        /// <summary>
        /// Set's <paramref name="t"/>'s properties to those stored in this object.
        /// </summary>
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
            if (other.space != this.space)
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
    
        public static bool Equals(this Transform t, TransformProperties other)
        {
            return other.Equals(t);
        }
    }
}
