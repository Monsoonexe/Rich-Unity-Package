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
	[Header("---Settings---")]
	[Tooltip("string.Format(format, Min, Val, Max), so Min = {0}, Val = {1}, and Max = {2}.")]
	public string formatString = "{0} | {1} | {2}";

	[Header("---Prefab Refs---")]
	[SerializeField, Required]
	private TextMeshProUGUI readoutUIElement;

	//cached strings for readonly min/max
	private string minString;
	private string maxString;

#if UNITY_EDITOR

	/// <summary>
	/// Shows example output in Inspector.
	/// </summary>
	[ReadOnly, ShowInInspector, PropertyTooltip("Shows example output.")]
	private string OutputString
	{
		get
		{
			try
			{
				SetTargetData(targetData); //refresh cached values
				return string.Format(
					formatString,
					minString,
					targetData.Value.ToString(),
					maxString);
			}
			catch (System.Exception ex) 
			{
				return ex.Message;
			}
		}
	}

#endif

	private void Reset()
	{
		SetDevDescription("I display text. E.g. {0} % \t {0} | {1} | {2} \t {1} credits \t {1:n} \t {#,##0}.");
		readoutUIElement = GetComponent<TextMeshProUGUI>(); //assume you want this one
	}

	protected override void Awake()
	{
		base.Awake();

		//validate
		Debug.Assert(readoutUIElement != null, $"{nameof(readoutUIElement)} is not set.", this);
		Debug.Assert(targetData != null, $"{nameof(targetData)} is not set.", this);

		//init
		SetTargetData(targetData); //cache min/max strings
	}

	protected override void SubscribeToEvents()
	{
		targetData.AddListener(UpdateUI);
	}

	protected override void UnsubscribeFromEvents()
	{
		targetData.RemoveListener(UpdateUI);
	}

	public void SetTargetData(IntVariable newTargetData)
	{
		targetData = newTargetData;

		//cache min/max strings.
		minString = targetData.MinClampValue.ToStringCached();
		maxString = targetData.MaxClampValue.ToStringCached();
	}

	public override void ToggleVisuals()
		=> this.enabled = readoutUIElement.enabled = !readoutUIElement.enabled;

	public override void ToggleVisuals(bool active)
		=> this.enabled = readoutUIElement.enabled = active;

	[Button]
	public override void UpdateUI()
	{
		string outputString = string.Format(
			formatString,
			minString,
			targetData.Value.ToStringCached(),
			maxString);

		readoutUIElement.text = outputString;
	}
}
