// ========================================================================================
// Signals - A typesafe, lightweight messaging lib for Unity.
// ========================================================================================
// 2017-2019, Yanko Oliveira  / http://yankooliveira.com / http://twitter.com/yankooliveira
// Special thanks to Max Knoblich for code review and Aswhin Sudhir for the anonymous 
// function asserts suggestion.
// ========================================================================================
// Inspired by StrangeIOC, minus the clutter.
// Based on http://wiki.unity3d.com/index.php/CSharpMessenger_Extended
// Converted to use strongly typed parameters and prevent use of strings as ids.
//
// Supports up to 3 parameters. More than that, and you should probably use a VO.
//
// Usage:
//    1) Define your class, eg:
//          ScoreSignal : ASignal<int> {}
//    2) Add listeners on portions that should react, eg on Awake():
//          Signals.GetPrint<ScoreSignal>().AddListener(OnScore);
//    3) Dispatch, eg:
//          Signals.GetPrint<ScoreSignal>().Dispatch(userScore);
//    4) Don't forget to remove the listeners upon destruction! Eg on OnDestroy():
//          Signals.GetPrint<ScoreSignal>().RemoveListener(OnScore);
//    5) If you don't want to use global Signals, you can have your very own SignalHub
//       instance in your class
//
// ========================================================================================

/* TODO -  implement IUnityEvent
 * 
 */

using System;
using System.Collections.Generic;

namespace RichPackage.Events.Signals
{
    /// <summary>
    /// Base interface for Signals
    /// </summary>
    public interface ISignal
    {
        string Hash { get; }
    }

    /// <summary>
    /// Signals main facade class for global, game-wide signals
    /// </summary>
    public static class GlobalSignals
    {
        private static readonly SignalHub hub = new SignalHub();

        public static SType Get<SType>()
            where SType : ISignal, new()
            => hub.Get<SType>();

        public static ISignal Get(string hash)
            => hub.Get(hash);

        public static void AddListenerToHash(string signalHash, Action handler)
            => hub.AddListenerToHash(signalHash, handler);

        public static void RemoveListenerFromHash(string signalHash, Action handler)
            => hub.RemoveListenerFromHash(signalHash, handler);
    }

    /// <summary>
    /// A hub for Signals you can implement in your classes
    /// </summary>
    public class SignalHub
    {
        private readonly Dictionary<Type, ISignal> signals = new Dictionary<Type, ISignal>();

        /// <summary>
        /// Manually provide a SignalHash and bind it to a given listener
        /// (you most likely want to use an AddListener, unless you know exactly
        /// what you are doing)
        /// </summary>
        /// <param name="signalHash">Unique hash for signal</param>
        /// <param name="handler">Callback for signal listener</param>
        public void AddListenerToHash(string signalHash, Action handler)
            => (Get(signalHash) as ASignal)?.AddListener(handler);

        /// <summary>
        /// Remove listener from manually-provided hash.
        /// </summary>
        /// <param name="signalHash">Unique hash for signal</param>
        /// <param name="handler">Callback for signal listener</param>
        public void RemoveListenerFromHash(string signalHash, Action handler)
            => (Get(signalHash) as ASignal)?.RemoveListener(handler);

        /// <summary>
        /// Getter for a signal of a given type
        /// </summary>
        /// <typeparam name="SType">Type of signal</typeparam>
        /// <returns>The proper signal binding</returns>
        public SType Get<SType>()
            where SType : ISignal, new()
            => (SType)GetInternal(typeof(SType));

        /// <summary>
        /// Getter for a <see cref="ISignal"/> where <paramref name="signalHash"/> 
        /// is an object that implements <see cref="ISignal"/>.
        /// </summary>
        public ISignal Get(string signalHash)
            => GetInternal(Type.GetType(signalHash));

        private ISignal GetInternal(Type signalType)
        {
            if (!signals.TryGetValue(signalType, out ISignal signal))
			{   // bind
                signal = Activator.CreateInstance(signalType) as ISignal; // new SignalType()
                signals.Add(signalType, signal);
            }
            return signal;
        }

        public void Clear() => signals.Clear();
    }

    /// <summary>
    /// Abstract class for Signals, provides hash by type functionality
    /// </summary>
    public abstract class ABaseSignal : ISignal
    {
        protected string _hash;

        /// <summary>
        /// Unique id for this signal
        /// </summary>
        public string Hash{ get => _hash; }

        public ABaseSignal()
        {
            _hash = this.GetType().ToString();
        }
    }

    #region Class Declarations

    /// <summary>
    /// Strongly typed messages with no parameters
    /// </summary>
    public abstract class ASignal : ABaseSignal
    {
        private Action callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action handler) => callback -= handler;

        /// <summary>
        /// Dispatch this signal
        /// </summary>
        public void Dispatch() => callback?.Invoke();
    }

    /// <summary>
    /// Strongly typed messages with 1 parameter
    /// </summary>
    /// <typeparam name="T">Parameter type</typeparam>
    public abstract class ASignal<T> : ABaseSignal
    {
        private Action<T> callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action<T> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action<T> handler) => callback -= handler;

        /// <summary>
        /// Dispatch this signal with 1 parameter
        /// </summary>
        public void Dispatch(T arg1) => callback?.Invoke(arg1);
    }

    /// <summary>
    /// Strongly typed messages with 2 parameters
    /// </summary>
    /// <typeparam name="T">First parameter type</typeparam>
    /// <typeparam name="U">Second parameter type</typeparam>
    public abstract class ASignal<T, U> : ABaseSignal
    {
        private Action<T, U> callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action<T, U> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action<T, U> handler) => callback -= handler;

        /// <summary>
        /// Dispatch this signal
        /// </summary>
        public void Dispatch(T arg1, U arg2) => callback?.Invoke(arg1, arg2);
    }

    /// <summary>
    /// Strongly typed messages with 3 parameter
    /// </summary>
    /// <typeparam name="T">First parameter type</typeparam>
    /// <typeparam name="U">Second parameter type</typeparam>
    /// <typeparam name="V">Third parameter type</typeparam>
    public abstract class ASignal<T, U, V> : ABaseSignal
    {
        private Action<T, U, V> callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action<T, U, V> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action<T, U, V> handler) => callback -= handler;

        /// <summary>
        /// Dispatch this signal
        /// </summary>
        public void Dispatch(T arg1, U arg2, V arg3) => callback?.Invoke(arg1, arg2, arg3);
    }

    /// <summary>
    /// Strongly typed messages with 4 parameter
    /// </summary>
    /// <typeparam name="T">First parameter type</typeparam>
    /// <typeparam name="U">Second parameter type</typeparam>
    /// <typeparam name="V">Third parameter type</typeparam>
    /// <typeparam name="W">Fourth parameter type</typeparam>
    public abstract class ASignal<T, U, V, W> : ABaseSignal
    {
        private Action<T, U, V, W> callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action<T, U, V, W> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action<T, U, V, W> handler) => callback -= handler;

        /// <summary>
        /// Dispatch this signal
        /// </summary>
        public void Dispatch(T arg1, U arg2, V arg3, W arg4) => callback?.Invoke(arg1, arg2, arg3, arg4);
    }

    /// <summary>
    /// Strongly typed messages with 5 parameter
    /// </summary>
    /// <typeparam name="T">First parameter type</typeparam>
    /// <typeparam name="U">Second parameter type</typeparam>
    /// <typeparam name="V">Third parameter type</typeparam>
    /// <typeparam name="W">Fourth parameter type</typeparam>
    /// <typeparam name="X">Fifth parameter type</typeparam>
    public abstract class ASignal<T, U, V, W, X> : ABaseSignal
    {
        private Action<T, U, V, W, X> callback;

        /// <summary>
        /// Adds a listener to this Signal
        /// </summary>
        /// <param name="handler">Method to be called when signal is fired</param>
        public void AddListener(Action<T, U, V, W, X> handler)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(
                handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                    inherit: false).Length == 0,
                "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
#endif
            callback += handler;
        }

        /// <summary>
        /// Removes a listener from this Signal
        /// </summary>
        /// <param name="handler">Method to be unregistered from signal</param>
        public void RemoveListener(Action<T, U, V, W, X> handler) => callback -= handler;

        /// <summary>
        /// Dispatch this signal
        /// </summary>
        public void Dispatch(T arg1, U arg2, V arg3, W arg4, X arg5) => callback?.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    #endregion
}
