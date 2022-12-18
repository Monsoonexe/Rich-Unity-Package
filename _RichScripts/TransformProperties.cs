﻿using RichPackage.GuardClauses;
using UnityEngine;

namespace RichPackage
{
	/// <summary>
	/// Properties for a <see cref="Transform"/>.
	/// </summary>
	public struct TransformProperties
	{
		public Space space;
		public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        #region Constructors

        public TransformProperties(Transform transform, Space space = Space.World)
		{
			GuardAgainst.ArgumentIsNull(transform, nameof(transform));

			this.space = space;
			if (space == Space.Self)
			{
                position = transform.localPosition;
                rotation = transform.localRotation;
                scale = transform.localScale;
            }
			else
			{
                position = transform.position;
                rotation = transform.rotation;
                scale = transform.lossyScale;
            }
		}

        #endregion Constructors
    }
}
