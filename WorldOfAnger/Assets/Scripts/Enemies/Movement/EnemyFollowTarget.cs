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

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 10;
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
                // Debug.Log("Again detected");
            }
            transform.position = Vector3.Lerp(transform.position, enemyData.player.transform.position, 20 * Time.deltaTime);
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
