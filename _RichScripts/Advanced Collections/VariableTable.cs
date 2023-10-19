using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using Sirenix.OdinInspector;
using RichPackage.GuardClauses;

namespace RichPackage.Collections
{
    [Serializable,
        Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.Fields)] // serialize this object with json the same way you would in unity
    public class VariableTable // TODO - ICollection
    {
        [UnityEngine.SerializeField]
        private List<VariableEntry> entries = new List<VariableEntry>();

        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public VariableTable() { }

        /// <summary>
        /// Copy-constructor.
        /// </summary>
        public VariableTable(VariableTable source)
        {
            GuardAgainst.ArgumentIsNull(source, nameof(source));

            foreach (var entry in source.entries)
            {
                entries.Add(new VariableEntry(entry, entry.Name));
            }
        }

        #endregion Constructors

        #region Bools

        public void SetBoolValue(UniqueID name, bool value = default)
        {
            VariableEntry entry = GetOrCreateEntry(name, EType.Bool);
            VerifyType(entry.Type, EType.Bool);
            entry.BoolValue = value;
        }

        public bool GetBoolValue(UniqueID name)
        {
            VariableEntry entry = LookupEntryOrThrow(name);
            VerifyType(entry.Type, EType.Bool);
            return entry.BoolValue;
        }

        #endregion Bools

        #region Ints

        public void SetIntValue(UniqueID name, int value = default)
        {
            VariableEntry entry = GetOrCreateEntry(name, EType.Int);
            VerifyType(entry.Type, EType.Int);
            entry.IntValue = value;
        }

        public int GetIntValue(UniqueID name)
        {
            VariableEntry entry = LookupEntryOrThrow(name);
            VerifyType(entry.Type, EType.Int);
            return entry.IntValue;
        }

        #endregion Ints

        #region Floats

        public void SetFloatValue(UniqueID name, float value = default)
        {
            VariableEntry entry = GetOrCreateEntry(name, EType.Float);
            VerifyType(entry.Type, EType.Float);
            entry.FloatValue = value;
        }

        public float GetFloatValue(UniqueID name)
        {
            VariableEntry entry = LookupEntryOrThrow(name);
            VerifyType(entry.Type, EType.Float);
            return entry.FloatValue;
        }

        #endregion Floats

        #region Strings

        public void SetStringValue(UniqueID name, string value = default)
        {
            VariableEntry entry = GetOrCreateEntry(name, EType.String);
            VerifyType(entry.Type, EType.String);
            entry.StringValue = value;
        }

        public string GetStringValue(UniqueID name)
        {
            VariableEntry entry = LookupEntryOrThrow(name);
            VerifyType(entry.Type, EType.String);
            return entry.StringValue;
        }

        #endregion Strings

        public void Delete(UniqueID name)
        {
            int i = entries.IndexOf(v => v.Name == name);
            if (i > -1)
                entries.RemoveAt(i);
        }

        public void Clear() => entries.Clear();

        #region Lookup

        private VariableEntry GetOrCreateEntry(UniqueID name, EType type)
        {
            VariableEntry entry = LookupEntry(name);

            // do we need to create?
            if (entry == null)
            {
                // declare
                entry = new VariableEntry(name, type);
                entries.Add(entry);
            }

            return entry;
        }

        private VariableEntry LookupEntry(UniqueID name)
        {
            foreach (var e in entries)
            {
                if (e.Name == name)
                    return e;
            }

            return null;
        }

        /// <exception cref="KeyNotFoundException"></exception>
        private VariableEntry LookupEntryOrThrow(UniqueID name)
        {
            return LookupEntry(name)
                ?? throw new KeyNotFoundException(name);
        }

        #endregion Lookup

        private static void VerifyType(EType expected, EType actual)
        {
            if (expected != actual)
            {
                string message = $"Variable type mismatch: Expected {expected} but was {actual}.";
                throw new InvalidOperationException(message);
            }
        }

        public object this[UniqueID name]
        {
            get => LookupEntryOrThrow(name).WeakValue;
            set => GetOrCreateEntry(name, EType.Bool).WeakValue = value; // the type doesn't matter
        }

        /// <summary>
        /// For type-safety.
        /// </summary>
        private enum EType
        {
            Bool,
            Int,
            Float,
            String
        }

        [Serializable,
            Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.Fields)] // serialize this object with json the same way you would in unity]
        private class VariableEntry
        {
            public UniqueID Name;
            public EType Type;

            [ShowIf("@Type == EType.Int")]
            public int IntValue;

            [ShowIf("@Type == EType.Float")]
            public float FloatValue;

            [ShowIf("@Type == EType.String")]
            public string StringValue;

            [ShowInInspector, ShowIf("@Type == EType.Bool")]
            public bool BoolValue
            {
                // use the integer field as the backing store to save some space
                get => IntValue != 0;
                set => IntValue = value.ToInt();
            }

            /// <summary>
            /// Weakly-typed value.
            /// </summary>
            public object WeakValue
            {
                get
                {
                    switch (Type)
                    {
                        case EType.Bool:
                            return BoolValue;
                        case EType.Int:
                            return IntValue;
                        case EType.Float:
                            return FloatValue;
                        case EType.String:
                            return StringValue;
                        default: throw ExceptionUtilities.GetInvalidEnumCaseException(Type);
                    }
                }
                set
                {
                    switch (value)
                    {
                        case bool boolValue:
                            Type = EType.Bool;
                            BoolValue = boolValue;
                            break;
                        case int intValue:
                            Type = EType.Int;
                            IntValue = intValue;
                            break;
                        case float floatValue:
                            Type = EType.Float;
                            FloatValue = floatValue;
                            break;
                        case string stringValue:
                            Type = EType.String;
                            StringValue = stringValue;
                            break;
                        default:
                            throw new InvalidCastException($"The type {value.GetType()} isn't supported. Please use a supported primitive type defined in {nameof(EType)}.");
                    }
                }
            }

            #region Constructors

            public VariableEntry(UniqueID name, EType type)
            {
                Name = name;
                Type = type;
                IntValue = 0;
                FloatValue = 0;
                StringValue = null;
            }

            /// <summary>
            /// Copy-constructor.
            /// </summary>
            public VariableEntry(VariableEntry src, UniqueID id)
            {
                GuardAgainst.ArgumentIsNull(src, nameof(src));

                // set id
                this.Name = id;

                // copy properties
                this.Type = src.Type;
                this.IntValue = src.IntValue;
                this.FloatValue = src.FloatValue;
                this.StringValue = src.StringValue;
            }

            #endregion Constructors

        }
    }
}
