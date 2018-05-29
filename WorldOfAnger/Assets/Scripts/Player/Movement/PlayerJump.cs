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
            if (MovementData.IsInAir)
            {
                MovementData.rigBody.velocity = new Vector2(MovementData.rigBody.velocity.x, MovementData.rigBody.velocity.y - MovementData.GravityEqualizator * MovementData.Gravity * Time.deltaTime);

                if(controller.ActiveStateMovement == null)
                {
                    controller.ForceSwapState(this);
                }
            }
            else if(controller.ActiveStateMovement == this)
            {
                controller.EndState(this);
            }

            if (Input.GetKeyDown(MovementData.Jump) && !MovementData.IsInAir)
            {
                MovementData.rigBody.velocity = new Vector2(MovementData.rigBody.velocity.x, MovementData.Gravity * MovementData.JumpHeightMultiplicator);
                MovementData.IsInAir = true;
            }

        }

        /// <inheritdoc/>
        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.tag == "Ground")
            {
                MovementData.IsInAir = false;
                MovementData.rigBody.velocity = Vector3.zero;
            }
        }

        /// <inheritdoc/>
        public void OnTriggerExit2D(Collider2D other)
        {
            MovementData.IsInAir = true;
        }

    }
}
