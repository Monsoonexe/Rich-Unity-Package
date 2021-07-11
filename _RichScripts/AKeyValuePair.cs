using UnityEngine;

/// <summary>
/// Key-Value pair, like a Dictionary entry. 
/// Value is mutable. If unmutable desired, use System.KeyValuePair instead
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class AKeyValuePair<TKey, TValue>
{
    [SerializeField]
    protected TKey key;

    public virtual TKey Key { get => key; } //immutable

    [SerializeField]
    protected TValue value;

    public virtual TValue Value { get => value; set => this.value = value; } //mutable

    public AKeyValuePair()
    {
        key = default;
        value = default;
    }

    public AKeyValuePair(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }

    public override string ToString()
    {
        return Key.ToString() + Value.ToString();
    }

    public static bool operator ==(AKeyValuePair<TKey, TValue> a, 
        AKeyValuePair<TKey, TValue> b) => a.key.Equals(b.key);

    public static bool operator !=(AKeyValuePair<TKey, TValue> a, 
        AKeyValuePair<TKey, TValue> b) => !a.key.Equals(b.key);

    public override bool Equals(object obj) => key.Equals((TKey) obj);
    public override int GetHashCode() => key.GetHashCode();

    public static implicit operator TKey(AKeyValuePair<TKey, TValue> a) => a.key;
    public static implicit operator TValue(AKeyValuePair<TKey, TValue> a) => a.value;
}
