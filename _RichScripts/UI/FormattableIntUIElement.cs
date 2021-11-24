using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

/// <summary>
/// Writes int with settings to a single ui text.
/// </summary>
[SelectionBase]
public class FormattableIntUIElement : RichUIElement<IntVariable>
{
    private enum EFormat
    {
        /// <summary>
        /// just show current value
        /// </summary>
        Single = 0,

        /// <summary>
        /// curr / max
        /// </summary>
        Curr_Max = 1,

        /// <summary>
        /// min / curr / max
        /// </summary>
        Min_Curr_Max = 2,

        /// <summary>
        /// min / curr / max
        /// </summary>
        Max_Curr_Min = 3,

        /// <summary>
        /// min / max
        /// </summary>
        Min_Max = 4,

        /// <summary>
        /// min / curr
        /// </summary>
        Min_Curr = 5,

        /// <summary>
        /// X 256
        /// </summary>
        Prefix_Curr = 6,

        /// <summary>
        /// 256 lives
        /// </summary>
        Curr_Suffix = 7,

        /// <summary>
        /// X 256 lives
        /// </summary>
        Prefix_Curr_Suffix = 8,
    }

    [Header("---Settings---")]
    [SerializeField]
    private EFormat format = EFormat.Curr_Max;

		[HideIf("@UsePrefix || UseSuffix")]
    public string separatorString = " / ";

		[ShowIf("@UsePrefix")]
		public string prefixString = string.Empty;

		[ShowIf("@UseSuffix")]
		public string suffixString = string.Empty;

		public bool UsePrefix => format == EFormat.Prefix_Curr || format == EFormat.Prefix_Curr_Suffix;
		public bool UseSuffix => format == EFormat.Curr_Suffix || format == EFormat.Prefix_Curr_Suffix;

    [Header("---Prefab Refs---")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI readoutUIElement;

    private void Reset()
    {
			SetDevDescription("I help format integers! I'm so helpful!");
			readoutUIElement = GetComponent<TextMeshProUGUI>(); //assume you want this one
    }

    protected override void SubscribeToEvents()
    {
			targetData.AddListener(UpdateUI);
    }

    protected override void UnsubscribeFromEvents()
    {
			targetData.RemoveListener(UpdateUI);
    }

    public override void ToggleVisuals()
			=> this.enabled = readoutUIElement.enabled = !readoutUIElement.enabled;

    public override void ToggleVisuals(bool active)
			=> this.enabled = readoutUIElement.enabled = active;

    [Button]
    public override void UpdateUI()
    {
        //all data into a single string
        var strBuilder = CommunityStringBuilder.Instance;
        string outputString = null;

        switch (format)
        {
            case EFormat.Single: // 999
                outputString = ConstStrings
                    .GetCachedString(targetData);
                break;

            case EFormat.Curr_Max: // 450 / 1000
                strBuilder
                    .Append(ConstStrings.GetCachedString(
                        targetData.Value))
                    .Append(separatorString)
                    .Append(ConstStrings.GetCachedString(
                        targetData.MaxClampValue));
                outputString = strBuilder.ToString();
                break;

            case EFormat.Max_Curr_Min: // 0 / 50 / 100
                strBuilder
                    .Append(ConstStrings.GetCachedString(
                        targetData.MaxClampValue))
                    .Append(separatorString)
                    .Append(ConstStrings.GetCachedString(
                        targetData.Value))
                    .Append(separatorString)
                    .Append(ConstStrings.GetCachedString(
                        targetData.MinClampValue));
                outputString = strBuilder.ToString();
                break;

            case EFormat.Min_Curr_Max: // 0 / 50 / 100
                strBuilder
									.Append(ConstStrings.GetCachedString(
											targetData.MinClampValue))
									.Append(separatorString)
									.Append(ConstStrings.GetCachedString(
											targetData.Value))
									.Append(separatorString)
									.Append(ConstStrings.GetCachedString(
											targetData.MaxClampValue));
                outputString = strBuilder.ToString();
                break;

            case EFormat.Min_Max: // 0 / 100
							strBuilder.Append(ConstStrings.GetCachedString(
									targetData.MinClampValue))
								.Append(separatorString)
								.Append(ConstStrings.GetCachedString(
									targetData.MaxClampValue));
							outputString = strBuilder.ToString();
							break;

            case EFormat.Min_Curr: // 0 / 9
							strBuilder.Append(ConstStrings.GetCachedString(
									targetData.MinClampValue))
								.Append(separatorString)
								.Append(ConstStrings.GetCachedString(
									targetData.Value));
							outputString = strBuilder.ToString();
							break;

            case EFormat.Prefix_Curr: // X 999
							outputString = prefixString + ConstStrings.GetCachedString(targetData.Value);
							break;

						case EFormat.Curr_Suffix: // 999 lives
							outputString = ConstStrings.GetCachedString(targetData.Value) + suffixString;
							break;

						case EFormat.Prefix_Curr_Suffix: // X 999 lives
							strBuilder.Append(prefixString)
								.Append(ConstStrings.GetCachedString(targetData.Value))
								.Append(suffixString);
							outputString = strBuilder.ToString();
							break;

            default:
                Debug.LogErrorFormat("[{0}] Format enum not accounted for: {1}. " +
                    "Add a case.", this.GetType().Name, format.ToString());
                break;
        }

        readoutUIElement.text = outputString;
    }

}
