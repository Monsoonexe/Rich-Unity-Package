using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Audio
{
	/// <remarks>Does NOT derive from SCOARCH.BaseReference as it should not have 
	/// the "set/get" behavior or events.</remarks>
	[Serializable]
	public class RichAudioClipReference
	{
		[SerializeField, ShowIf(nameof(UseConstant))]
		private AudioClip _constantClip;

		[SerializeField, HideIf(nameof(UseConstant))]
		private RichAudioClip audioClipVariable;

		[SerializeField, ShowIf(nameof(UseConstant))]
		private AudioOptions overrideOptions = AudioOptions.DefaultSfx;

		#region Properties

		[field: SerializeField, LabelText(nameof(UseConstant)),
			PropertyOrder(-1)]
		public bool UseConstant { get; private set; }

		[ShowInInspector, ReadOnly]
		public bool IsValueDefined => UseConstant || audioClipVariable != null;

		public AudioOptions Options => IsValueDefined
			? overrideOptions : AudioOptions.DefaultSfx;

		public AudioClip Clip => (UseConstant || audioClipVariable == null)
			? _constantClip : audioClipVariable;

		#endregion Properties

		#region Constructors

		public RichAudioClipReference()
		{
			UseConstant = false;
		}

		public RichAudioClipReference(AudioClip clip)
		{
			_constantClip = clip;
			UseConstant = true;
		}

		public RichAudioClipReference(AudioOptions options)
			: this(null, options)
		{
			//nada
		}

		public RichAudioClipReference(AudioClip clip, AudioOptions options)
			: this(clip)
		{
			overrideOptions = options;
		}

		#endregion Constructors

		/// <summary>
		/// Procedural form of <see cref="PlaySFX"/>
		/// </summary>
		public void DoPlaySFX() => PlaySFX();

		public AudioID PlaySFX()
			=> AudioManager.PlaySFX(Clip, Options);

		public AudioID PlayBGM()
			=> AudioManager.PlaySFX(Clip, Options);

		public static implicit operator AudioClip (RichAudioClipReference a)
			=> a.Clip;
	}
}
