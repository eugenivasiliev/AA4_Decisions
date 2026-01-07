using System;
using System.Collections.Generic;
using Events;

namespace AI.StateMachine {

    public class StateMachine<T> {
        public IState<T>? CurrentState { get; private set; }

        private readonly Dictionary<Type, IState<T>?> allStates = new();
        private Signal<IState<T>?> OnStateChanged { get; set; } = new();

        public T Blackboard { get; private set; }

        public StateMachine(T blackboard) => Blackboard = blackboard;

        public void OnFixedUpdate() => CurrentState?.OnFixedUpdate();

        public void OnUpdate() => CurrentState?.OnUpdate();

        public void Quit() {
            CurrentState?.OnExit();
            CurrentState = null;
        }

        /// <summary>
        /// Switches to a state that was previously added via <see cref="Add{TU}"/>.
        /// </summary>
        public void Switch<TU>() where TU : IState<T> {
            if(!allStates.TryGetValue(typeof(TU), out IState<T>? next) || next is null) {
                Console.WriteLine($"Warning: State {typeof(TU).Name} not found!");
                return;
            }

            CurrentState?.OnExit();
            CurrentState = next;

            //invoke event
            OnStateChanged?.Invoke(CurrentState);

            CurrentState.OnEnter();
        }

        /// <summary>
        /// Creates a new instance of <typeparamref name="TU"/> and registers it.
        /// </summary>
        public void Add<TU>() where TU : IState<T>, new() {
            if(allStates.ContainsKey(typeof(TU))) {
                return;
            }
            IState<T>? stateInstance = new TU();
            allStates.Add(typeof(TU), stateInstance);

            stateInstance.SetOwner(this);
        }

        /// <summary>
        /// Registers an already‑constructed state instance.
        /// </summary>
        public void Add<TU>(TU stateInstance) where TU : IState<T> {
            Type type = typeof(TU);
            if(!allStates.TryAdd(type, stateInstance)) {
                return;
            }

            stateInstance.SetOwner(this);
        }

        /// <summary>
        /// Removes a previously added state from the registry.
        /// </summary>
        public void RemoveState<TU>() {
            allStates.Remove(typeof(TU));
        }
    }

    /// <summary>
    /// Contract that every concrete state must implement.
    /// </summary>
    public interface IState<T> {

        internal void SetOwner(StateMachine<T> owner);

        public void OnEnter();

        public void OnExit();

        public void OnUpdate();

        public void OnFixedUpdate();
    }

    /// <summary>
    /// Helper base class that supplies convenient shortcuts to the owning
    /// <see cref="StateMachine{T}"/> and its blackboard.
    /// </summary>
    public abstract class BaseState<T> : IState<T> {
        public StateMachine<T>? Owner { get; private set; }

        void IState<T>.SetOwner(StateMachine<T>? owner) => Owner = owner;

        protected T Blackboard => Owner.Blackboard;

        public virtual void OnEnter() {
        }

        public virtual void OnExit() {
        }

        public virtual void OnUpdate() {
        }

        public virtual void OnFixedUpdate() {
        }
    }
}