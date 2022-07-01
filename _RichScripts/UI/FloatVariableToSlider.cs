using UnityEngine;
using UnityEngine.UI;
using RichPackage.Animation;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace RichPackage.UI
{
	/// <summary>
	/// Sets a UI <see cref="Slider"/> to the value of a <see cref="FloatVariable"/>.
	/// </summary>
	/// <seealso cref="UISliderToFloatVariable"/>
	[SelectionBase]
	public sealed class FloatVariableToSlider : RichUIElement<FloatVariable>
	{
		[SerializeField, Required]
		private Slider slider;

		[Title("Animation")]
		public bool AnimateSlider = true;
		public TweenOptions tweenOptions = new TweenOptions()
		{
			Duration = 0.5f,
			Ease = Ease.InQuad
		};

		public Tween Animation { get; private set; }

		[ShowInInspector, ReadOnly]
		public bool IsPlaying
		{ 
			get => Animation != null && Animation.IsPlaying();
		}

		#region Unity Messages

		protected override void Reset()
		{
			SetDevDescription("Sets a Slider based on the value of the " + nameof(targetData));
			if (!TryGetComponent<Slider>(out slider))
				slider = GetComponentInChildren<Slider>();
		}

		private void Start()
		{
			ConfigureSliderWithVariableValues();
		}

		#endregion Unity Messages

		public override void UpdateUI()
		{
			if (AnimateSlider)
			{
				// maybe combine?
				if (IsPlaying)
					Animation.Kill(complete: false);

				// animate
				Animation = MoveMeter(targetData);
			}
			else
			{
				slider.value = targetData.Value;
			}
		}

		protected override void SubscribeToEvents()
		{
			targetData.AddListener(UpdateUI);
		}

		protected override void UnsubscribeFromEvents()
		{
			targetData.RemoveListener(UpdateUI);
		}

		[Button("Configure Slider")]
		private void ConfigureSliderWithVariableValues()
		{
			slider.minValue = targetData.MinClampValue;
			slider.maxValue = targetData.MaxClampValue;
			slider.value = targetData.Value;
		}

		[Button, DisableInEditorMode]
		private Tween MoveMeter(float newValue)
		{
			var tween = DOTween.To(
				GetSliderValue,
				SetSliderValue,
				newValue,
				tweenOptions.Duration);
			tween.SetEase(tweenOptions.Ease);
			tween.OnComplete(OnAnimationComplete);

			return tween;
		}

		private float GetSliderValue()
		{
			return slider.value;
		}

		private void SetSliderValue(float value)
		{
			slider.value = value;
		}

		private void OnAnimationComplete()
		{
			Animation = null;
		}
	}
}
