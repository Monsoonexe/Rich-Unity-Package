using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

public class VariableText : RichUIElement
{
    [Header("---Scene Refs---")]
    [SerializeField]
    private TextMeshProUGUI tmp;

    [Header("---Data Refs---")]
    [SerializeField]
    private BaseVariable data;

    private void OnEnable()
    {
        data.AddListener(UpdateVisuals);
    }

    private void OnDisable()
    {
        data.RemoveListener(UpdateVisuals);
    }

    private void Start()
    {
        UpdateVisuals();
    }

    /// <summary>
    /// Update UI elements with current values.
    /// </summary>
    public void UpdateVisuals()
    {
        tmp.text = data.ToString();
    }
}
