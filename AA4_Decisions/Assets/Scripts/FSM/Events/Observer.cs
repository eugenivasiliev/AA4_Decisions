using System;
using System.Collections.Generic;

namespace Events {

    public class Observer {
        private readonly Dictionary<SignalBase, List<Delegate>> tracked = new();

        public void Add(Signal signal, Action callback) {
            signal.AddListener(callback);
            Track(signal, callback);
        }

        public void Add(SignalBase signal, Delegate callback) {
            if(signal == null || callback == null) return;

            signal.AddListener(callback);
            Track(signal, callback);
        }

        public void Add<T>(Signal<T> signal, Action<T> callback) {
            signal.AddListener(callback);
            Track(signal, callback);
        }

        public void Add<T>(Signal<T> signal, EventHandler<T> callback) {
            signal.AddListener(callback);
            Track(signal, callback);
        }

        public void Add<T, TU>(Signal<T, TU> signal, Action<T, TU> callback) {
            signal.AddListener(callback);
            Track(signal, callback);
        }

        private void Track(SignalBase signal, Delegate callback) {
            if(!tracked.TryGetValue(signal, out var list)) {
                list = new List<Delegate>();
                tracked[signal] = list;
            }
            list.Add(callback);
        }

        public void Clear() {
            foreach((SignalBase signal, List<Delegate> value) in tracked) {
                foreach(Delegate callback in value) {
                    signal.RemoveListener(callback);
                }
            }
            tracked.Clear();
        }
    }
}