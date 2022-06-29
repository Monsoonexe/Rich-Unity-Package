using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using System;

namespace RichPackage.UI
{
    /// <summary>
    /// A meter in UI space that fills up based on a <see cref="FloatVariable"/>'s value.
    /// </summary>
    /// <seealso cref="UIMeterInt"/>
    [SelectionBase]
    public class UIMeterFloat : VariableUIElement<FloatVariable>
    {
        private const string ColorLimitsGroup = "Color Limits";

        [SerializeField, Required]
        private Image myImage;

        [BoxGroup(ColorLimitsGroup)]
        public bool applyColorLimits = false;

        [Tooltip("Must be ascending.")]
        [ShowIf(nameof(applyColorLimits)), BoxGroup(ColorLimitsGroup)]
        public ColorLimit[] colorLimits = new ColorLimit[]
        {
            new ColorLimit(15, Color.red),
            new ColorLimit(50, Color.yellow),
            new ColorLimit(100, Color.green),
        };

        /// <summary>
        /// Change sprite used to fill image.
        /// </summary>
        public Sprite FillSprite { get => myImage.sprite; set => myImage.sprite = value; }

        /// <summary>
        /// Change tint of image (Image.color).
        /// </summary>
        public Color FillTint { get => myImage.color; set => myImage.color = value; }

        #region Unity Messages

        protected override void Reset()
        {
            SetDevDescription("A meter in UI space that fills up based on a FloatVariable's value.");
            myImage = GetComponent<Image>();
        }

        public void OnValidate()
        {
            if (targetData != null && !targetData.Clampable)
                Debug.LogError("[UIMeterFloat] sourceValue is not clampable! " + targetData.name, this);
        }

        #endregion Unity Messages

        /// <summary>
        /// Refresh UI element with current data values.
        /// </summary>
        public override void UpdateUI()
        {
            float min = targetData.MinClampValue; //cache
            float range = targetData.MaxClampValue - min;
            myImage.fillAmount = (targetData - min) / range;

            if (applyColorLimits)
                FillTint = GetColorBasedOnLimit(targetData);
        }

        public Color GetColorBasedOnLimit(float value)
        {
            int len = colorLimits.Length;
            for (int i = 0; i < len; ++i)
            {
                ColorLimit col = colorLimits[i];
                if (value < col.limit)
                    return col.color;
            }

            throw new InvalidOperationException("Could not get a " + nameof(ColorLimit) + " from value: " + value);
        }

        #region RichUIElement

        public override void ToggleVisuals(bool active)
            => myImage.enabled = active;

        public override void ToggleVisuals()
            => myImage.enabled = !myImage.enabled;

		#endregion RichUIElement
	}
}
