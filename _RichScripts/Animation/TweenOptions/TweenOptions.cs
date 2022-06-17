using System;
using UnityEngine;
using DG.Tweening;

namespace RichPackage.Animation
{
	[Serializable]
	public class TweenOptionsBase
	{
		public float Duration;
		public Ease Ease;
	}

	[Serializable]
	public class Vector3TweenOptions : TweenOptionsBase
	{
		/// <summary>
		/// Position, rotation, or scale.
		/// </summary>
		public Vector3 Value;
	}

	//public static class PunchTweenExtensions
	//{
	//	public static Tween DoPunch(this Transform transform, PunchTweenOptions options)
	//	{
	//		transform.DoPunch
	//	}
	//}
}