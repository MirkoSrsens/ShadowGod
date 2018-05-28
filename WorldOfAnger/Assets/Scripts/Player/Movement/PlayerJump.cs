using General.State;
using UnityEngine;

namespace Player.Movement
{
    /// <summary>
    /// Class that defines state of jumping.
    /// </summary>
    public class PlayerJump : StateForMovement
    {
        /// <inheritdoc/>
        public override void Update_State()
        {
            if (PlayerMovementData.IsInAir)
            {
                PlayerMovementData.rigBody.velocity = new Vector3(PlayerMovementData.rigBody.velocity.x, PlayerMovementData.rigBody.velocity.y - PlayerMovementData.GravityEqualizator * PlayerMovementData.Gravity * Time.deltaTime, PlayerMovementData.rigBody.velocity.z);

                if(controller.ActiveStateMovement == null)
                {
                    controller.ForceSwapState(this);
                }
            }
            else if(controller.ActiveStateMovement == this)
            {
                controller.EndState(this);
            }

            if (Input.GetKeyDown(PlayerMovementData.Jump) && !PlayerMovementData.IsInAir)
            {
                PlayerMovementData.rigBody.velocity = new Vector3(PlayerMovementData.rigBody.velocity.x, PlayerMovementData.Gravity * PlayerMovementData.JumpHeightMultiplicator, PlayerMovementData.rigBody.velocity.z);
                PlayerMovementData.IsInAir = true;
            }

        }

        /// <inheritdoc/>
        public void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Ground")
            {
                PlayerMovementData.IsInAir = false;
                PlayerMovementData.rigBody.velocity = Vector3.zero;
            }
        }

        /// <inheritdoc/>
        public void OnTriggerExit(Collider other)
        {
            PlayerMovementData.IsInAir = true;
        }

    }
}
