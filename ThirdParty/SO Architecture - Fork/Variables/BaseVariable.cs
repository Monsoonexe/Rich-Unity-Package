using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    public abstract class BaseVariable : GameEventBase
    {
        public abstract bool IsClamped { get; }
        public abstract bool Clampable { get; }
        public abstract bool IsInitializeable { get; }
        public abstract bool ReadOnly { get; }
        public abstract Type Type { get; }
        public abstract object BaseValue { get; set; }
    }
    public abstract class BaseVariable<T> : BaseVariable
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

        public override bool Clampable { get { return false; } }
        public override bool ReadOnly { get { return _readOnly; } }
        public override bool IsClamped { get { return _isClamped; } }
        public override bool IsInitializeable { get => false; }
        public override Type Type { get { return typeof(T); } }
        public override object BaseValue
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
        [Tooltip("The starting Value.")]
        protected T _initialValue = default(T);
        [SerializeField]
        [Tooltip("Should this Value start at a specific value OnEnable?")]
        protected bool _initialize = true;
        [SerializeField]
        protected bool _readOnly = false;
        [SerializeField]
        protected bool _raiseWarning = true;
        [SerializeField]
        protected bool _isClamped = false;
        [SerializeField]
        protected T _minClampedValue = default(T);
        [SerializeField]
        protected T _maxClampedValue = default(T);

        protected readonly List<Action<T>> _typedActions = new List<Action<T>>();

        protected virtual void OnEnable()
        {
            Initialize();
        }

        public void AddListener(Action<T> action)
        {
            if (!_typedActions.Contains(action))
                _typedActions.Add(action);
        }
        public void RemoveListener(Action<T> action)
        {
            _typedActions.Remove(action);
        }
        public override void Raise()
        {
            base.Raise();
            Raise(_value);
        }
        public void Raise(T value)
        {
            for (var i = _typedActions.Count - 1; i >= 0; --i)
                _typedActions[i].Invoke(value);               
        }
        /// <summary>
        /// Processes new values. Does not actually change Value property.
        /// Applies clamps, checks for readonly.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Value after processing.</returns>
        protected virtual T SetValue(T value)
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

        protected virtual T ClampValue(T value) => value;

        public void Initialize()
        {
            if (IsInitializeable && _initialize)
                Value = _initialValue;
        }

        protected void RaiseReadonlyWarning()
        {
            if (_readOnly && _raiseWarning)
                Debug.LogWarning("Tried to set value on " + name 
                    + ", but value is readonly!", this);
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
