using DG.Tweening;
using UnityEngine;

namespace RichPackage.Audio
{
	/// <summary>
	/// 
	/// </summary>
	public static class AudioSourceExtensions
	{
		public static AudioSource Restart(this AudioSource source)
        {
			source.Stop();
			source.Play();
			return source;
		}

		public static Tween DOFadeIn(this AudioSource source, float duration, Ease ease = Ease.Linear, float volume = 1.0f)
		{
			source.volume = 0; // start quiet
			source.Play();
			return source
				.DOFade(volume, duration)
				.SetEase(ease);
		}

		public static Tween DOFadeOut(this AudioSource source, float duration, Ease ease = Ease.Linear)
		{
			return source
				.DOFade(0, duration)
				.SetEase(ease)
				.OnComplete(source.Stop);
		}
	}
}
