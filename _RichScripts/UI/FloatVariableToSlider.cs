using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

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

		protected override void Reset()
		{
			SetDevDescription("Sets a Slider based on the value of the " + nameof(targetData));
			slider = GetComponent<Slider>(); //make a guess
			if (slider == null)
				slider = GetComponentInChildren<Slider>();
		}

		private void Start()
		{
			ConfigureSliderWithVariableValues();
		}

		public override void UpdateUI()
		{
			slider.value = targetData.Value;
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
	}
}
