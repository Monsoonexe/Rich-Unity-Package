using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEngine
{
	public static class AudioSourceExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AudioSource Restart(this AudioSource source)
        {
			source.Stop();
			source.Play();
			return source;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AudioSource SetVolume(this AudioSource source, float volume)
        {
            source.volume = volume;
            return source;
        }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AudioSource PlayClip(this AudioSource source, AudioClip clip)
        {
            source.PlayOneShot(clip);
            return source;
        }
	}
}

namespace DG.Tweening
{
    public static class AudioSourceTweenExtensions
	{
		public static Tween DOFadeIn(this AudioSource source, float duration, Ease ease = Ease.Linear, float volume = 1.0f)
		{
			return source
				.DOFade(volume, duration)
				.From(0) // start quiet
				.SetEase(ease)
				.OnStart(source.Play);
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
