using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.State
{
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

        // Use this for initialization
        void Awake()
        {
            ActiveStateMechanic = null;
            ActiveStateMovement = null;
        }

        // Update is called once per frame
        void Update()
        {
            if(ActiveStateMechanic != null)
            {
                ActiveStateMechanic.WhileActive_State();
            }
            if (ActiveStateMovement != null)
            {
                ActiveStateMovement.WhileActive_State();
            }
        }

        /// <summary>
        /// Swap state using prioerity
        /// </summary>
        /// <param name="newState"></param>
        public void SwapState(State newState)
        {
            if (newState == null) return;

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
            Debug.Log("Swap state");
            
            if(newState is StateForMechanics)
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
