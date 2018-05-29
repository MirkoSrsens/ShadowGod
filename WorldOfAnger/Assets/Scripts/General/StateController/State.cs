using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.State
{
    /// <summary>
    /// Core state for all actions.
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets <see cref="StateController"/>
        /// </summary>
        public StateController controller;

        /// <summary>
        /// Gets or sets animation contriller;
        /// </summary>
        public AnimationController animationController;

        /// <summary>
        /// Gets or sets state priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Initializes components and parameters.
        /// </summary>
        protected virtual void Initialization_State()
        {

        }

        /// <summary>
        /// Starts on begining onf the state.
        /// </summary>
        public virtual void OnEnter_State()
        {

        }

        /// <summary>
        /// Checks when is the best time for state to becom active.
        /// </summary>
        public virtual void Update_State()
        {

        }

        /// <summary>
        /// While state is active.
        /// </summary>
        public virtual void WhileActive_State()
        {

        }

        /// <summary>
        /// Happens when state is being swaped by other state.
        /// </summary>
        public virtual void OnExit_State()
        {

        }

        // Use this for initialization
        void Start()
        {
            controller = GetComponent<StateController>();
            animationController = GetComponent<AnimationController>();
            Initialization_State();
        }

        // Update is called once per frame
        void Update()
        {
            Update_State();
        }
    }
}
