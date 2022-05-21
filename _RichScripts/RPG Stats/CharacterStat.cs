using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using RichPackage.RPGStats;

/// <summary>
/// S.P.E.C.I.A.L.
/// </summary>
/// <seealso cref="AgilityStat"
[Serializable]
public class CharacterStat
{
    private const string _NAME = "Character Stat";

    [SerializeField]
    protected float baseValue = 1;

    /// <summary>
    /// For permanent stat changes, like leveling up.
    /// </summary>
    public float BaseValue
    {
        get => baseValue;
        set
        {
            lastBaseValue = baseValue = value;
            _value = CalculateFinalValue();
            OnValueChangedEvent?.Invoke(this);
        }
    }

    public virtual string statName { get => _NAME; }
    public static string StatName { get => _NAME; }

	protected bool isDirty = true;
	protected float lastBaseValue;

	protected float _value;
	public virtual float Value
    {
		get {
			if(isDirty || lastBaseValue != baseValue) {
				lastBaseValue = baseValue;
				_value = CalculateFinalValue();
				isDirty = false;
			}
			return _value;
		}
	}

	protected readonly List<StatModifier> statModifiers;
	public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    //events
    /// <summary>
    /// Called whenever modifier is changed (not when base value is changed).
    /// </summary>
    public event Action<CharacterStat> OnValueChangedEvent;

    #region Constructors

    public CharacterStat()
	{
		statModifiers = new List<StatModifier>();
		StatModifiers = statModifiers.AsReadOnly();
	}

	public CharacterStat(float baseValue) : this()
	{
		BaseValue = baseValue;
    }

    #endregion

    public virtual void AddModifier(StatModifier mod)
	{
		isDirty = true;
		statModifiers.Add(mod);
		statModifiers.Sort(CompareModifierOrder);
        OnValueChangedEvent?.Invoke(this);
    }

	public virtual bool RemoveModifier(StatModifier mod)
	{
        var removed = false;
		if (statModifiers.Remove(mod))
		{
			isDirty = true;
            removed =  true;
            OnValueChangedEvent?.Invoke(this);
        }
        return removed;
	}

	public virtual bool RemoveAllModifiersFromSource(object source)
	{
		bool didRemove = false;

		for (int i = statModifiers.Count - 1; i >= 0; i--)
		{
			if (statModifiers[i].Source == source)
			{
				isDirty = true;
				didRemove = true;
				statModifiers.RemoveAt(i);
			}
		}
        if(didRemove)
            OnValueChangedEvent?.Invoke(this);
        return didRemove;
	}

	protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
	{
		if (a.Order < b.Order)
			return -1;
		else if (a.Order > b.Order)
			return 1;
		return 0; //if (a.Order == b.Order)
	}
		
	protected virtual float CalculateFinalValue()
	{
		float finalValue = BaseValue;
		float sumPercentAdd = 0;

		for (int i = 0; i < statModifiers.Count; i++)
		{
			StatModifier mod = statModifiers[i];

			if (mod.Type == EStatModType.Flat)
			{
				finalValue += mod.Value;
			}
			else if (mod.Type == EStatModType.PercentAdd)
			{
				sumPercentAdd += mod.Value;

				if (i + 1 >= statModifiers.Count || 
                    statModifiers[i + 1].Type != EStatModType.PercentAdd)
				{
					finalValue *= 1 + sumPercentAdd;
					sumPercentAdd = 0;
				}
			}
			else if (mod.Type == EStatModType.PercentMult)
			{
				finalValue *= 1 + mod.Value;
			}
		}

		return (float)Math.Round(finalValue, 4);
	}
}

