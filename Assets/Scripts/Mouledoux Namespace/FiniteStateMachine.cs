namespace Mouledoux.Components
{
    public class FiniteStateMachine<T>
    {
        /// <summary>
        /// Constructor for the FSM
        /// </summary>
        /// 
        /// <param name="initState">Initial state for the object</param>
        /// 
        /// Notes: This constructor ensures there is atleast 1 state for the machine at the begining
        public FiniteStateMachine(T initState)
        {
            AddState(m_anyState);
            AddState(initState);
            m_currentState = initState;
        }


        /// <summary>
        /// Returns the current state of the object
        /// </summary>
        /// 
        /// <returns>
        /// Returns the current state
        /// </returns>
        public T GetCurrentState()
        {
            return m_currentState;
        }


        /// <summary>
        /// Adds a state to the list of possible states
        /// </summary>
        /// 
        /// <param name="aState">State to be added to the list of possible states</param>
        /// 
        /// <returns>
        /// 0 the state was added to the list,
        /// 1 the state already exist,
        /// -1 the state already exist as the "any" state
        /// </returns>
        public int AddState(T aState)
        {
            if (aState.ToString().ToLower() == m_anyState.ToString().ToLower())
                return -1;

            if (m_states.Contains(aState))
                return 1;

            m_states.Add(aState);
            return 0;
        }


        /// <summary>
        /// Removes a state from the list of possible states,
        /// and all transitions involving the state
        /// </summary>
        /// 
        /// <param name="aState">State to be Removed from the list of possible states</param>
        /// 
        /// <returns>
        /// 0 the state(and transitions) were removed from the list(s),
        /// 1 the state did not exist, or is the "any" state
        /// -1 the object is currently in that state
        /// </returns>
        public int RemoveState(T aState)
        {
            if (m_currentState.ToString().ToLower() == aState.ToString().ToLower())
                return -1;

            if (!m_states.Contains(aState) || aState.ToString().ToLower() == m_anyState.ToString().ToLower())
                return 1;

            foreach(T bState in m_states)
            {
                if(CheckTransition(aState, bState))
                    RemoveTransition(aState, bState);

                if (CheckTransition(bState, aState))
                    RemoveTransition(bState, aState);
            }
            
            m_states.Remove(aState);
            return 0;
        }


        /// <summary>
        /// Adds a transition of 2 states to the list of valid transitions,
        /// and a delegate to be invoked on successful transition
        /// </summary>
        /// 
        /// <param name="aState">State the object would be starting in</param>
        /// <param name="bState">State the object will be transitioning to</param>
        /// <param name="aHandler">Delegate to be invoked on successful transition</param>
        /// 
        /// <returns>
        /// 0 the transition was successfully added to the list,
        /// 1 the transition already exists,
        /// -1 either state is invalid
        /// </returns>
        public int AddTransition(T aState, T bState, System.Delegate aHandler)
        {
            if (!m_states.Contains(aState) || !m_states.Contains(bState))
                return -1;

            string transitionKey = aState.ToString() + "->" + bState.ToString();
            transitionKey = transitionKey.ToLower();

            if (m_transitions.ContainsKey(transitionKey))
                return 1;

            m_transitions.Add(transitionKey, aHandler);
            return 0;
        }


        public int AddTransitionFromAnyTo(T bState, System.Delegate aHandler)
        {
            return 1;
        }

        public int AddTransitionToAnyFrom(T aState)
        {
            return 1;
        }

        /// <summary>
        /// Removes a transition of 2 states from the list of valid transitions
        /// </summary>
        /// 
        /// <param name="aState">State the object would be starting in</param>
        /// <param name="bState">State the object would be transitioning to</param>
        /// 
        /// <returns>
        /// 0 the transition was successfully removed from the list,
        /// 1 the transition did not exist,
        /// -1 the states are invalid
        /// </returns>
        public int RemoveTransition(T aState, T bState)
        {
            if (!m_states.Contains(aState) || !m_states.Contains(bState))
                return -1;

            string transitionKey = aState.ToString() + "->" + bState.ToString();
            transitionKey = transitionKey.ToLower();

            if (!m_transitions.ContainsKey(transitionKey))
                return 1;

            m_transitions.Remove(transitionKey);
            return 0;
        }


        /// <summary>
        /// Checks if a transition between 2 states is valid
        /// </summary>
        /// 
        /// <param name="aState">State the object would be starting in</param>
        /// <param name="bState">State the object would be transitioning to</param>
        /// 
        /// <returns>
        /// TRUE the transition is valid,
        /// FALSE the transition is not valid
        /// </returns>
        public bool CheckTransition(T aState, T bState)
        {
            string transitionKey = aState.ToString() + "->" + bState.ToString();
            transitionKey = transitionKey.ToLower();

            return (m_transitions.ContainsKey(transitionKey));
        }


        /// <summary>
        /// Makes all valid transitions and their handler invokes
        /// </summary>
        /// 
        /// <param name="bState">State to transition to from the current</param>
        /// 
        /// <returns>
        /// 0 the transition was successful,
        /// 1 it's not a valid transition,
        /// -1 if the new state is invalid
        /// </returns>
        public int MakeTransitionTo(T bState)
        {
            if (!m_states.Contains(bState))
                return -1;

            string transitionKey = m_currentState.ToString() + "->" + bState.ToString();
            transitionKey = transitionKey.ToLower();

            if (!m_transitions.ContainsKey(transitionKey))
                return 1;

            m_transitions[transitionKey].DynamicInvoke();
            m_currentState = bState;

            return 0;
        }



        // Variables //////////
        #region Variables      
        /// <summary>
        /// The current state of the object
        /// </summary>
        private T m_currentState;

        /// <summary>
        /// Empty state for arbitrary transitions
        /// </summary>
        private T m_anyState;

        /// <summary>
        /// List of possible states
        /// </summary>
        private System.Collections.Generic.List<T> m_states =
            new System.Collections.Generic.List<T>();

        /// <summary>
        /// Dictionary of transitions with associated transition handlers
        /// </summary>
        private System.Collections.Generic.Dictionary<string, System.Delegate> m_transitions =
            new System.Collections.Generic.Dictionary<string, System.Delegate>();
        #endregion
    }
}