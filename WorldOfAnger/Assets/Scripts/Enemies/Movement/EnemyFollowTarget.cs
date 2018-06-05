using General.State;
using Implementation.Data;
using UnityEngine;
using Zenject;
using Gameinformation;
using System.Collections;

namespace Enemy.State
{
    public class EnemyFollowTarget : StateForMovement
    {
        [Inject]
        private IEnemyData enemyData { get; set; }

        private bool raiseFlag { get; set; }

        private float timeToWait { get; set; }

        private Vector2 LastPlaceEnemySawPlayer { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 10;
            timeToWait = 5;
        }
        public override void Update_State()
        {
            base.Update_State();
            if(enemyData.IsTargetDetected(transform) && GameInformation.Alarmed)
            {
                controller.SwapState(this);
            }
        }
        public override void WhileActive_State()
        {
            base.WhileActive_State();
            Debug.Log("FollowPlayer");

            if (!enemyData.IsTargetDetected(transform))
            {
                if (!raiseFlag)
                {
                    StartCoroutine(FollowPlayerForTime());
                    //Debug.Log("Lost sight");
                }
            }
            else
            {
                StopCoroutine(FollowPlayerForTime());
                LastPlaceEnemySawPlayer = enemyData.player.transform.position;
                // Debug.Log("Again detected");
            }

            //// Proper rotation
            var roationAngle = enemyData.player.transform.position.x - transform.position.x;

            if(roationAngle < 0 )
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            transform.position = Vector2.Lerp(transform.position, new Vector2(LastPlaceEnemySawPlayer.x, transform.position.y), MovementData.MovementSpeed * Time.deltaTime);
        }


        public IEnumerator FollowPlayerForTime()
        {
            raiseFlag = true;
            yield return new WaitForSeconds(timeToWait);
            controller.EndState(this);
            raiseFlag = false;
        }

    }
}
