using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.State
{
    /// <summary>
    /// Holds function and values for controlling the states.
    /// </summary>
    public class StateController : MonoBehaviour
    {
        /// <summary>
        /// Gets and sets active state.
        /// </summary>
        public StateForMechanics ActiveStateMechanic { get; set; }

        /// <summary>
        /// Gets and sets active state.
        /// </summary>
        public StateForMovement ActiveStateMovement { get; set; }

        /// <summary>
        /// Gets or sets higher priority state.
        /// </summary>
        public HighPriorityState HighPriorityState { get; set; }

        // Use this for initialization
        void Awake()
        {
            ActiveStateMechanic = null;
            ActiveStateMovement = null;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (HighPriorityState != null)
            {
                HighPriorityState.WhileActive_State();
            }
            else
            {
                if (ActiveStateMechanic != null)
                {
                    ActiveStateMechanic.WhileActive_State();
                }
                if (ActiveStateMovement != null)
                {
                    ActiveStateMovement.WhileActive_State();
                }
            }
        }

        /// <summary>
        /// Swap state using prioerity
        /// </summary>
        /// <param name="newState"></param>
        public void SwapState(State newState)
        {
            if (newState == null) return;

            if(newState is HighPriorityState)
            {
                if (HighPriorityState == null || (newState != null && HighPriorityState.Priority < newState.Priority))
                {
                    ForceSwapState(newState);
                }
            }

            if (newState is StateForMechanics)
            {
                if (ActiveStateMechanic == null || (newState != null && ActiveStateMechanic.Priority < newState.Priority))
                {
                    ForceSwapState(newState);
                }
            }
            else if(newState is StateForMovement)
            {
                if (ActiveStateMovement == null || (newState != null && ActiveStateMovement.Priority < newState.Priority))
                {
                    ForceSwapState(newState);
                }
            }
        }

        public void ForceSwapState(State newState)
        {
            if (newState is HighPriorityState)
            {
                if (HighPriorityState != null) HighPriorityState.OnExit_State();
                HighPriorityState = (HighPriorityState)newState;
                HighPriorityState.OnEnter_State();
            }
            if (newState is StateForMechanics)
            {
                if (ActiveStateMechanic != null) ActiveStateMechanic.OnExit_State();
                ActiveStateMechanic = (StateForMechanics)newState;
                ActiveStateMechanic.OnEnter_State();
            }
            else if (newState is StateForMovement)
            {
                if (ActiveStateMovement != null) ActiveStateMovement.OnExit_State();
                ActiveStateMovement = (StateForMovement)newState;
                ActiveStateMovement.OnEnter_State();
            }
        }

        public void EndState(State stateToEnd)
        {
            if (stateToEnd != null) stateToEnd.OnExit_State();

            if (stateToEnd is HighPriorityState)
            {
                HighPriorityState = null;
            }
            if (stateToEnd is StateForMechanics)
            {
                ActiveStateMechanic = null;
            }
            else if (stateToEnd is StateForMovement)
            {
                ActiveStateMovement = null;
            }
        }
    }
}
