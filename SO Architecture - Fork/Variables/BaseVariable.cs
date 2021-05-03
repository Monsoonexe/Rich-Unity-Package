using UnityEngine;

namespace ScriptableObjectArchitecture
{
    public interface IBaseVariable 
    {
        bool IsClamped { get; }
        bool Clampable { get; }
        bool ReadOnly { get; }
        System.Type Type { get; }
        object BaseValue { get; set; }
        void Raise();
    }
    public abstract class BaseVariable<T> : GameEventBase<T>, IBaseVariable
    {
        public virtual T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = SetValue(value);
                Raise();
            }
        }
        public virtual T MinClampValue
        {
            get
            {
                if(Clampable)
                {
                    return _minClampedValue;
                }
                else
                {
                    return default(T);
                }
            }
        }
        public virtual T MaxClampValue
        {
            get
            {
                if(Clampable)
                {
                    return _maxClampedValue;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public virtual bool Clampable { get { return false; } }
        public virtual bool ReadOnly { get { return _readOnly; } }
        public virtual bool IsClamped { get { return _isClamped; } }
        public virtual System.Type Type { get { return typeof(T); } }
        public virtual object BaseValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = SetValue((T)value);
                Raise();
            }
        }

        [SerializeField]
        protected T _value = default(T);
        [SerializeField]
        private bool _readOnly = false;
        [SerializeField]
        private bool _raiseWarning = true;
        [SerializeField]
        protected bool _isClamped = false;
        [SerializeField]
        protected T _minClampedValue = default(T);
        [SerializeField]
        protected T _maxClampedValue = default(T);
        
        public virtual T SetValue(T value)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return _value;
            }
            else if(Clampable && IsClamped)
            {
                return ClampValue(value);
            }

            return value;
        }
        public virtual T SetValue(BaseVariable<T> value)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return _value;
            }
            else if (Clampable && IsClamped)
            {
                return ClampValue(value.Value);
            }

            return value.Value;
        }
        protected virtual T ClampValue(T value)
        {
            return value;
        }
        private void RaiseReadonlyWarning()
        {
            if (!_readOnly || !_raiseWarning)
                return;

            Debug.LogWarning("Tried to set value on " + name + ", but value is readonly!", this);
        }
        public override string ToString()
        {
            return _value == null ? "null" : _value.ToString();
        }
        public static implicit operator T(BaseVariable<T> variable)
        {
            return variable.Value;
        }
    } 
}