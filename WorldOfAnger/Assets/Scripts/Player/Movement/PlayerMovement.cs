using System.Collections;
using System.Collections.Generic;
using General.State;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerMovement : StateForMovement
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 10;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            animationController.SetAnimation(this.GetType().Name);
        }

        /// <inheritdoc/>
        public override void Update_State()
        {
            MovementData.HorizontalMovement = (Input.GetKey(MovementData.Right) ? 1 : 0) + (Input.GetKey(MovementData.Left) ? -1 : 0);

            if (MovementData.HorizontalMovement != 0 && controller.ActiveStateMovement != this)
            {
                controller.SwapState(this);
            }
            else if (controller.ActiveStateMovement == this && MovementData.HorizontalMovement == 0)
            {
                MovementData.rigBody.velocity = new Vector2(0, MovementData.rigBody.velocity.y);
                controller.EndState(this);
            }
        }

        public override void WhileActive_State()
        {
            if (MovementData.HorizontalMovement > 0)
            {
                MovementData.rigBody.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (MovementData.HorizontalMovement < 0)
            {
                MovementData.rigBody.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            MovementData.rigBody.velocity = new Vector2(MovementData.HorizontalMovement * MovementData.MovementSpeed, MovementData.rigBody.velocity.y);
        }
    }
}
