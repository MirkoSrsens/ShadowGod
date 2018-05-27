using General.State;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerJump : StateForMovement
    {
        /// <inheritdoc/>
        public override void Update_State()
        {
            if (PlayerMovementData.IsInAir)
            {
                PlayerMovementData.rigBody.velocity = new Vector3(PlayerMovementData.rigBody.velocity.x, PlayerMovementData.rigBody.velocity.y - PlayerMovementData.GravityEqualizator * PlayerMovementData.Gravity * Time.deltaTime, PlayerMovementData.rigBody.velocity.z);
            }

            if (Input.GetKeyDown(PlayerMovementData.Jump) && !PlayerMovementData.IsInAir)
            {
                PlayerMovementData.rigBody.velocity = new Vector3(PlayerMovementData.rigBody.velocity.x, PlayerMovementData.Gravity * PlayerMovementData.JumpHeightMultiplicator, PlayerMovementData.rigBody.velocity.z);
                PlayerMovementData.IsInAir = true;
            }

        }

        public void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Ground")
            {
                PlayerMovementData.IsInAir = false;
                PlayerMovementData.rigBody.velocity = Vector3.zero;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            PlayerMovementData.IsInAir = true;
        }

    }
}
