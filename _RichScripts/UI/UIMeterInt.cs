/* TODO - AUIMeter with common behavior
 * 
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using RichPackage.FunctionalProgramming;

namespace RichPackage.UI
{
    /// <summary>
    /// Controls a meter fill amount between min and max.
    /// </summary>
    /// <seealso cref="UIMeterFloat"/>
    [SelectionBase]
    public class UIMeterInt : VariableUIElement<IntVariable>
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
			base.Reset();
            SetDevDescription("Controls a meter fill amount between min and max.");
            myImage = GetComponent<Image>();
		}

		public void OnValidate()
        {
            if (targetData != null && !targetData.Clampable)
                Debug.LogWarning($"{nameof(UIMeterInt)}] {targetData.name} is not clampable!", targetData);
        }

        #endregion Unity Messages

        public Color GetColorBasedOnLimit(float fillPercent)
        {
            int value = (fillPercent * 100).ToInt();
            int len = colorLimits.Length;
            for (int i = 0; i < len; ++i)
            {
                ColorLimit col = colorLimits[i];
                if (value <= col.percentile)
                    return col.color;
            }

            throw new InvalidOperationException("Could not get a " + nameof(ColorLimit) + " from value: " + value);
        }

        /// <summary>
        /// Refresh UI element with current data values.
        /// </summary>
        [Button]
        public override void UpdateUI()
        {
            int min = targetData.MinClampValue; //cache
            float range = targetData.MaxClampValue - min;
            float fillPercent = (targetData - min) / range;
            myImage.fillAmount = fillPercent;

            if (applyColorLimits)
                FillTint = GetColorBasedOnLimit(fillPercent);
        }

        public override void ToggleVisuals(bool active)
            => myImage.enabled = active;

        public override void ToggleVisuals()
            => myImage.enabled = !myImage.enabled;
    }
}
