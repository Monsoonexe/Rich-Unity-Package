using System;
using UnityEngine;

namespace RichPackage.Animation
{
	/// <summary>
	/// A way to package parameters together.
	/// </summary>
	[Serializable]
	public struct PunchTweenOptions
	{
		public Vector3 Scale;
		public float Duration;
		public float Elasticity;
		public int Vibrato;

		public PunchTweenOptions(in Vector3 punch,
			float duration, int vibrato, float elasticity)
		{
			Scale = punch;
			Duration = duration;
			Vibrato = vibrato;
			Elasticity = elasticity;
		}

		public static PunchTweenOptions Default
		{
			get => new PunchTweenOptions(new Vector3(0.25f, 0.25f, 1), 1.0f, 10, 1);
		}
	}
}
