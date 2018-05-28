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
            PlayerMovementData.HorizontalMovement = (Input.GetKey(PlayerMovementData.Right) ? 1 : 0) + (Input.GetKey(PlayerMovementData.Left) ? -1 : 0);

            if (PlayerMovementData.HorizontalMovement != 0 && controller.ActiveStateMovement != this)
            {
                controller.SwapState(this);
            }
            else if (controller.ActiveStateMovement == this && PlayerMovementData.HorizontalMovement == 0)
            {
                PlayerMovementData.rigBody.velocity = new Vector2(0, PlayerMovementData.rigBody.velocity.y);
                controller.EndState(this);
            }
        }

        public override void WhileActive_State()
        {
            if (PlayerMovementData.HorizontalMovement > 0)
            {
                PlayerMovementData.rigBody.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (PlayerMovementData.HorizontalMovement < 0)
            {
                PlayerMovementData.rigBody.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            PlayerMovementData.rigBody.velocity = new Vector2(PlayerMovementData.HorizontalMovement * PlayerMovementData.MovementSpeed, PlayerMovementData.rigBody.velocity.y);
        }
    }
}
