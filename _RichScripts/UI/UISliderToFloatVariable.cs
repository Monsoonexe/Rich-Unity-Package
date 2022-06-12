//TODO - one for Floats, and another for Ints
//TODO - Connect Max/MinClamp on SO to Slider.Min/Max
//TODO - configure step based on Int/Float type.

using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using ScriptableObjectArchitecture;

namespace RichPackage.UI
{
	/// <summary>
	/// Synchronizes a Slider with a FloatVariable.
	/// </summary>
	[SelectionBase]
	public sealed class UISliderToFloatVariable : RichUIElement<FloatVariable>
	{
		[SerializeField, Required]
		private Slider slider;

		protected override void Reset()
		{
			base.Reset();
			SetDevDescription("Synchronizes a ScriptableObject with input from Slider.Value.");
			slider = GetComponent<Slider>();
		}

		private void OnValidate()
		{
			ConfigureSliderToMatchVariable();
		}

		protected override void Awake()
		{
			base.Awake();
			if(slider == null)
				slider = GetComponent<Slider>();

			ConfigureSliderToMatchVariable();
			Debug.Assert(slider != null, "slider is not set.", this);
		}

		protected override void SubscribeToEvents()
		{
			slider.onValueChanged.AddListener(UpdateUI);
		}

		protected override void UnsubscribeFromEvents()
		{
			slider.onValueChanged.RemoveListener(UpdateUI);
		}

		[Button]
		private void ConfigureSliderToMatchVariable()
		{
			if (slider == null || targetData == null) return;

			slider.minValue = targetData.MinClampValue;
			slider.maxValue = targetData.MaxClampValue;
			slider.value = targetData.Value;

	#if UNITY_EDITOR
			if (!Application.isPlaying)
				UnityEditor.EditorUtility.SetDirty(slider);
	#endif
		}

		/// <summary>
		/// Update the slider value to source value.
		/// </summary>
		[Button]
		public override void UpdateUI()
		{
			slider.value = targetData.Value;
		}

		/// <summary>
		/// Update source value with slider value.
		/// </summary>
		/// <param name="sliderValue"></param>
		public void UpdateUI(float sliderValue)
		{
			targetData.Value = sliderValue;
		}
	}
}
