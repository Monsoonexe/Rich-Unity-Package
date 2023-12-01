using System;

namespace RichPackage
{
    public readonly struct ConditionInfo : IEquatable<ConditionInfo>
    {
        public readonly bool status;

        /// <summary>
        /// Why?
        /// </summary>
        public readonly string message;

        public static ConditionInfo Success
        {
            get
            {
                return new ConditionInfo(true);
            }
        }

        public ConditionInfo(bool conditionStatus, string conditionMessage = null)
        {
            status = conditionStatus;
            message = conditionMessage;
        }

        public static ConditionInfo FromFalse(string message) => new ConditionInfo(false, message);

        public static bool operator ==(ConditionInfo a, bool b)
        {
            return a.status == b;
        }

        public static bool operator !=(ConditionInfo a, bool b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return message;
        }

        public bool Equals(ConditionInfo other)
        {
            return status == other.status && Equals(message, other.message);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            return obj is ConditionInfo info && Equals(info);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (status.GetHashCode() * 397) ^ (message?.GetHashCode() ?? 0);
            }
        }

        public static implicit operator bool(ConditionInfo info) => info.status;

        public static implicit operator string(ConditionInfo info) => info.message;
    }
}
