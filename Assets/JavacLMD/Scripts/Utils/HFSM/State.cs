using System.Collections;
using System.Collections.Generic;

namespace JavacLMD.Utils.HFSM
{

    /// <summary>
    /// Barebone interface for states, can be reused for any bodies own purposes
    /// </summary>
    public interface IState
    {
        void Enter();
        void Exit();
        void Execute();
    }

    /// <summary>
    /// Barebone interface for state machines, can be reused for any bodies own purposes
    /// </summary>
    public interface IStateMachine
    {
        IState CurrentState { get; }
        bool SwitchState(IState targetState);
        void Execute();
    }


    public class StateMachine : IStateMachine
    {
        public State Root { get; private set; }
        public State CurrentState { get; private set; }
        IState IStateMachine.CurrentState => CurrentState;

        private bool _started;
        
        public bool SwitchState(IState targetState)
        {
            if (targetState is not State target) return false;
            if (target == CurrentState) return false;

            ExitUpToCommonAncestor(CurrentState, target);
            CurrentState = target;
            EnterFromAncestorToTarget(target);

            return true;
        }


        /// <summary>Start the machine at the root state.</summary>
        public void Start()
        {
            if (_started) return;
            _started = true;
            Root.Enter();
        }

        /// <summary>Update the machine each frame.</summary>
        public void Execute()
        {
            if (!_started) Start();
            Root.Execute();
        }

        private void ExitUpToCommonAncestor(State? from, State to)
        {
            while (from != null && !IsAncestorOf(from, to))
            {
                from.Exit();
                from = from.ParentState;
            }
        }

        private void EnterFromAncestorToTarget(State target)
        {
            var stack = new Stack<State>();
            var node = target;

            while (node != null && !IsAncestorOf(node, CurrentState))
            {
                stack.Push(node);
                node = node.ParentState;
            }

            while (stack.Count > 0)
            {
                var state = stack.Pop();
                state.StateMachine = this;
                state.Enter();
            }
        }

        private bool IsAncestorOf(State ancestor, State? state)
        {
            var node = state;
            while (node != null)
            {
                if (node == ancestor) return true;
                node = node.ParentState;
            }

            return false;
        }
        
    }
    
    
    public abstract class State : IState
    {
        public IStateMachine StateMachine;
        public State ParentState;
        
        public State ActiveChildState { get; private set; }


        public void Enter()
        {
            if (ParentState != null) ParentState.ActiveChildState = this;
            OnEnterState(); //enter self
            
            //enter child
            ActiveChildState = GetDefaultChildState();
            ActiveChildState?.Enter();
        }

        public void Exit()
        {
            ActiveChildState?.Exit();
            ActiveChildState = null;
            
            OnExitState();
        }

        public void Execute()
        {
            OnExecuteState();
            ActiveChildState?.Execute();
        }

        /// <summary>
        /// Actions for the state when it's entered (not including children)
        /// </summary>
        protected virtual void OnEnterState()
        {
            
        }

        /// <summary>
        /// Actions for the state when it's exited (not including children)
        /// </summary>
        protected virtual void OnExitState()
        {
            
        }

        /// <summary>
        /// Actions for the state when it runs every frame (not including children)
        /// </summary>
        protected virtual void OnExecuteState()
        {
            
        }

        public abstract IEnumerable<State> GetChildrenStates();
        public abstract State GetDefaultChildState();


    }
}