using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace General.State
{
    public class StateForMovement : State
    {
        [Inject]
        protected IPlayeMovementData PlayerMovementData { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            PlayerMovementData.rigBody = GetComponent<Rigidbody>();
        }
    }
}
