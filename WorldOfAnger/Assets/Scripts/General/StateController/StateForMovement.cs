using System.Collections;
using System.Collections.Generic;
using Implementation.Data;
using UnityEngine;
using Zenject;

namespace General.State
{
    /// <summary>
    /// Defines stat that is used for movement.
    /// </summary>
    public abstract class StateForMovement : State
    {
        [Inject]
        /// Gets or sets player Movement.
        protected IMovementData MovementData { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            MovementData.rigBody = GetComponent<Rigidbody2D>();
        }
    }
}
