using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace General.State
{
    /// <summary>
    /// Defines stat that is used for movement.
    /// </summary>
    public class StateForMovement : State
    {
        [Inject]
        /// Gets or sets player Movement.
        protected IMovementData PlayerMovementData { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            PlayerMovementData.rigBody = GetComponent<Rigidbody2D>();
        }
    }
}
