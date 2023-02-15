
using System;

namespace RichPackage.Events.Signals
{
    public interface ISignalListener
    {
        void AddListener(Action action);
        void RemoveListener(Action action);
    }

    public interface ISignalListener<T>
    {
        void AddListener(Action<T> action);
        void RemoveListener(Action<T> action);
    }

    public interface ISignalListener<T, U>
    {
        void AddListener(Action<T, U> action);
        void RemoveListener(Action<T, U> action);
    }
    
    public interface ISignalListener<T, U, V>
    {
        void AddListener(Action<T, U, V> action);
        void RemoveListener(Action<T, U, V> action);
    }
    
    public interface ISignalListener<T, U, V, W>
    {
        void AddListener(Action<T, U, V, W> action);
        void RemoveListener(Action<T, U, V, W> action);
    }
    
    public interface ISignalListener<T, U, V, W, X>
    {
        void AddListener(Action<T, U, V, W, X> action);
        void RemoveListener(Action<T, U, V, W, X> action);
    }

}
