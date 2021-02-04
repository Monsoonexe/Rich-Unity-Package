using UnityEngine;

/// <summary>
/// SCriptable Reference
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ScriptableReference<T> : RichScriptableObject
{
    [SerializeField]
    protected T value;
    public T Value
    {
        get => value;
        set
        {
            if (isReadonly && readonlyThrowsError)
            {
                Debug.LogError("[ScriptableReference] This object is readonly!", this);
            }
            else
            {
                this.value = value;
            }
        }
    }

    [Header("---Options---")]
    [SerializeField]
    protected bool isReadonly = false;
    public bool IsReadonly { get => isReadonly; }

    [SerializeField]
    [Tooltip("Should an exception be thrown if the value is being modified?")]
    protected bool readonlyThrowsError = true;
    public bool ReadonlyThrowsError { get => readonlyThrowsError; }

    /// <summary>
    /// Use this object as if it were the object it is wrapping.
    /// </summary>
    /// <param name="a"></param>
    public static implicit operator T(ScriptableReference<T> a) => a.value;
}
