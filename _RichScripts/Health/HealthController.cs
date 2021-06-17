using UnityEngine;
using UnityEngine.Events;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

/// <summary>
/// 
/// </summary>
/// <remarks></remarks>
public class HealthController : RichMonoBehaviour, 
    IDamageable, IHealable
{
    [Header("---Resources---")]
    [SerializeField] 
    private int currentVitality = 100;

    [SerializeField] 
    [MinValue(1)]
    private int maxVitality = 100; 

    public float MaxVitality { get => maxVitality; }
    public float CurrentVitality { get => currentVitality; }

    /// <summary>
    /// Current / max health.
    /// </summary>
    public float VitalityRatio { get => (float)currentVitality / maxVitality; }
    public bool IsDamaged { get => currentVitality < maxVitality; }
    public bool IsDead { get; private set; }

    //general
    public readonly UnityEvent revivedEvent = new UnityEvent();

    [SerializeField]
    private UnityEvent deadEvent = new UnityEvent();
    public UnityEvent DeadEvent { get => deadEvent; }

    //health
    public readonly UnityEvent vitalityLostEvent = new UnityEvent();
    public readonly UnityEvent vitalityGainedEvent = new UnityEvent();

    /// <summary>
    /// Recover given health. 0 > amount means ALL health.
    /// </summary>
    /// <param name="recoverAmount">negative amount for total heal.</param>
    public void RecoverHealth(int recoverAmount = -1)
    {
        if (recoverAmount < 0) //heal all wounds
        {
            currentVitality = maxVitality;
        }
        else
        {
            currentVitality = RichMath.Clamp(
                currentVitality + recoverAmount, 0, maxVitality);
        }

        vitalityGainedEvent.Invoke();
    }

    [Button("Recover Full Health")]
    public void RecoverFullHealth()
        => Revive(1);

    /// <summary>
    /// Make not dead and set current health to given ratio of max health.
    /// Range: 0 lt value lte 1.0f
    /// </summary>
    /// <param name="healthRatio">Range: 0 lt value lte 1.0f</param>
    public void Revive(float healthRatio = 1)
    {
        IsDead = false;
        RecoverHealth((int)(healthRatio * maxVitality));
    }
    
    /// <summary>
    /// Change amount of max health available.
    /// </summary>
    /// <param name="amount"></param>
    public void SetMaxVitality(int amount)
    {
        maxVitality = amount;
        currentVitality = currentVitality < maxVitality // clamp currentHealth to new max
            ? currentVitality : maxVitality;
    }

    /// <summary>
    /// Reduce damage.
    /// </summary>
    /// <param name="damageAmount">negative amount for total kill.</param>
    public void TakeDamage(int damageAmount)
    {
        if (damageAmount < 0) //insta-kill
        {
            damageAmount = currentVitality;
        }
        currentVitality = RichMath.Clamp(
            currentVitality - damageAmount, 0, maxVitality);
        vitalityLostEvent.Invoke();

        if (currentVitality <= 0 && !IsDead)//if this is the moment of death
        {
            IsDead = true;
            deadEvent.Invoke();
        }
    }
}
