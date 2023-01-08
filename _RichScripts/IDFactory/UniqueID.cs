using RichPackage;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[Serializable]
public struct UniqueID : IEquatable<UniqueID>
{
    private const int MAX_LENGTH = 16;

    public static Func<string> NewIDProvider { get; set; }
        = () => Guid.NewGuid().ToString().Remove("-").Substring(0, MAX_LENGTH);

    public static UniqueID New
    {
        get => FromString(NewIDProvider());
    }

    [field: SerializeField, LabelText(nameof(ID))]
    public string ID { get; private set; }

    #region Constructors

    public UniqueID(string id)
    {
        ID = id;
    }

    #endregion Constructors

    public bool Equals(UniqueID other) => this == other;

    public override string ToString() => ID;

    public override bool Equals(object obj)
        => obj is UniqueID other && Equals(other);

    public override int GetHashCode() => ID.GetHashCode();

    public static UniqueID FromString(string src)
        => new UniqueID(src);

    public static bool operator ==(UniqueID a, UniqueID b)
        => a.ID.QuickEquals(b.ID);

    public static bool operator !=(UniqueID a, UniqueID b)
        => !(a == b);

    public static implicit operator string(UniqueID id)
        => id.ID;
}
