using System;
using UnityEngine;
using DG.Tweening;

namespace RichPackage.Animation
{
	[Serializable]
	public class TweenOptionsBase
	{
		public float Duration { get; set; }
		public Ease Ease { get; set; }
	}

	[Serializable]
	public sealed class PunchTweenOptions : TweenOptionsBase
	{
		public Vector3 Scale { get; set; }
		public int Vibrato { get; set; }
	}
}