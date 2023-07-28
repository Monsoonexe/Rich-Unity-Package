﻿using RichPackage.GuardClauses;
using UnityEngine;

namespace RichPackage
{
	/// <summary>
	/// Properties for a <see cref="Transform"/>.
	/// </summary>
	[System.Serializable]
	public struct TransformProperties
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
        /// Loads <paramref name="t"/>'s properties into this object.
        /// </summary>
        public void Load(Transform t)
        {
            // validate
            GuardAgainst.ArgumentIsNull(t, nameof(t));

            // operate
            if (space == Space.Self)
            {
                t.localPosition = position;
                t.localRotation = rotation;
            }
            else
            {
                t.position = position;
                t.rotation = rotation;
            }
        }
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
}
