using System;
using System.Collections.Generic;

namespace Events {

    /// <summary>
    /// Base class that stores a collection of delegates and provides the core
    /// listener‑management methods. It is not intended to be used directly;
    /// derive from it to expose a concrete invoke signature.
    /// </summary>
    public abstract class SignalBase {
        protected readonly List<Delegate> Listeners = new();

        internal void AddListener(Delegate listener) {
            if(listener != null && !Listeners.Contains(listener)) {
                Listeners.Add(listener);
            }
        }

        internal void RemoveListener(Delegate listener) {
            if(listener != null) {
                Listeners.Remove(listener);
            }
        }

        internal void Clear() => Listeners.Clear();
    }

    /// <summary>
    /// Parameter‑less signal that only accepts callbacks of type <see cref="Action"/>.
    /// </summary>
    public class Signal : SignalBase {

        public void AddListener(Action callback) {
            AddListener((Delegate)callback);
        }

        public void RemoveListener(Action callback) {
            RemoveListener((Delegate)callback);
        }

        public void Invoke() {
            foreach(Delegate listener in Listeners) {
                (listener as Action)?.Invoke();
            }
        }
    }

    /// <summary>
    /// Signal that forwards the *sender* object to listeners.  Listeners may be
    /// either <see cref="Action"/> (ignore the sender) or <see cref="Action{object}"/>
    /// (receive the sender as a parameter).
    /// </summary>
    public class SignalSender : SignalBase {

        public void Invoke(object sender) {
            foreach(Delegate listener in Listeners) {
                switch(listener) {
                    case Action action:
                    action.Invoke();
                    break;

                    case Action<object> senderAction:
                    senderAction.Invoke(sender);
                    break;

                    default:
                    break;
                }
            }
        }

        public void AddListener(Action callback) => AddListener((Delegate)callback);

        public void AddListener(Action<object> callback) => AddListener((Delegate)callback);

        public void RemoveListener(Action callback) => RemoveListener((Delegate)callback);

        public void RemoveListener(Action<object> callback) => RemoveListener((Delegate)callback);
    }

    /// <summary>
    /// Signal that dispatches a single argument of type <typeparamref name="T"/>.
    /// Supports both <c>Action&lt;T&gt;</c> and <c>EventHandler&lt;T&gt;</c> signatures.
    /// </summary>
    public class Signal<T> : SignalBase {

        public void AddListener(Action<T> callback) {
            AddListener((Delegate)callback);
        }

        public void RemoveListener(Action<T> callback) {
            RemoveListener((Delegate)callback);
        }

        public void Invoke(T arg) {
            foreach(Delegate listener in Listeners) {
                (listener as Action<T>)?.Invoke(arg);
            }
        }

        public void Invoke(object sender, T arg) {
            foreach(Delegate listener in Listeners) {
                (listener as EventHandler<T>)?.Invoke(sender, arg);
            }
        }

        public void AddListener(EventHandler<T> callback) {
            AddListener((Delegate)callback);
        }
    }

    /// <summary>
    /// Signal that dispatches two arguments of types <typeparamref name="T"/> and
    /// <typeparamref name="U"/>. Only the <c>Action&lt;T,U&gt;</c> signature is
    /// supported (you can easily add an EventHandler version if needed).
    /// </summary>
    public class Signal<T, U> : SignalBase {

        public void AddListener(Action<T, U> callback) => AddListener((Delegate)callback);

        public void RemoveListener(Action<T, U> callback) => RemoveListener((Delegate)callback);

        public void Invoke(T arg1, U arg2) {
            foreach(Delegate listener in Listeners) {
                (listener as Action<T, U>)?.Invoke(arg1, arg2);
            }
        }
    }
}