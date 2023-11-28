using RichPackage.GuardClauses;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

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

            foreach (VariableEntry entry in source.entries)
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
            return LookupEntryOrThrow(name, EType.Bool).BoolValue;
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
            return LookupEntryOrThrow(name, EType.Float).FloatValue;
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
            return LookupEntryOrThrow(name, EType.String).StringValue;
        }

        #endregion Strings

        #region Chars

        public void SetCharValue(UniqueID name, char value = default)
        {
            VariableEntry entry = GetOrCreateEntry(name, EType.Char);
            VerifyType(entry.Type, EType.Char);
            entry.CharValue = value;
        }

        public char GetCharValue(UniqueID name)
        {
            return LookupEntryOrThrow(name, EType.Bool).CharValue;
        }

        #endregion Chars

        #region Delete

        public void Delete(UniqueID name)
        {
            int i = entries.IndexOf(v => v.Name == name);
            if (i > -1)
                entries.QuickRemove(i);
        }

        public void Clear() => entries.Clear();

        #endregion Delete

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
            foreach (VariableEntry e in entries)
            {
                if (e.Name == name)
                    return e;
            }

            return null;
        }

        /// <exception cref="KeyNotFoundException"></exception>
        private VariableEntry LookupEntryOrThrow(UniqueID name)
        {
            return LookupEntry(name) ?? throw new KeyNotFoundException(name);
        }

        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private VariableEntry LookupEntryOrThrow(UniqueID name, EType type)
        {
            VariableEntry entry = LookupEntryOrThrow(name);
            VerifyType(entry.Type, type);
            return entry;
        }

        #endregion Lookup

        #region Copying

        public void CopyTo(VariableTable other) => other.AddRange(entries);

        public void CopyFrom(VariableTable other) => AddRange(other.entries);

        #endregion Copying

        #region Collection

        private void AddRange(IEnumerable<VariableEntry> entries)
        {
            foreach (VariableEntry e in entries)
            {
                e.CopyTo(GetOrCreateEntry(e.Name, e.Type));
            }
        }

        #endregion Collection

        /// <exception cref="InvalidOperationException"></exception>
        private static void VerifyType(EType actual, EType expected)
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
            Bool = 0,
            Int = 1,
            Float = 2,
            String = 3,
            Char = 4,
        }

        // TODO - custom json serializer to really trim the fat
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

            [ShowInInspector, ShowIf("@Type == EType.Char")]
            public char CharValue
            {
                // use the integer field as the backing store to save some space
                get => (char)IntValue;
                set => IntValue = value;
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
                        case EType.Char:
                            return CharValue;
                        default:
                            throw ExceptionUtilities.GetInvalidEnumCaseException(Type);
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
                        case char charValue:
                            Type = EType.Char;
                            CharValue = charValue;
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

            #region Copying

            public void CopyTo(VariableEntry other)
            {
                this.Name = other.Name;
                this.Type = other.Type;
                this.IntValue = other.IntValue;
                this.FloatValue = other.FloatValue;
                this.StringValue = other.StringValue;
            }

            public void CopyFrom(VariableEntry other)
            {
                other.Name = this.Name;
                other.Type = this.Type;
                other.IntValue = this.IntValue;
                other.FloatValue = this.FloatValue;
                other.StringValue = this.StringValue;
            }

            #endregion Copying

        }
    }
}
