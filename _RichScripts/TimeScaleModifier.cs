using UnityEngine;
using ScriptableObjectArchitecture;

/// <summary>
/// Sets the timeScale.
/// </summary>
public class TimeScaleModifier : RichMonoBehaviour
{
    [SerializeField]
    private FloatVariable timeScale;

    /// <summary>
    /// Setting this value updates Time.timeScale immediately.
    /// </summary>
    public float TimeScale
    {
        get => timeScale;
        set => timeScale.Value = value;
    }

    private void Start()
    {
        UpdateTimeScale(timeScale);
    }

    private void OnEnable()
    {
        timeScale.AddListener(UpdateTimeScale);
    }

    private void OnDisable()
    {
        timeScale.RemoveListener(UpdateTimeScale);
    }

    public void UpdateTimeScale(float newScale)
    {
        Time.timeScale = newScale;
    }
}
