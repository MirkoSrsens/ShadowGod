using System.Collections;
using Gameinformation;
using General.State;
using Implementation.Data;
using UnityEngine;
using Zenject;

namespace Enemy.State
{
    public class EnemyAttackPlayer : StateForMechanics
    {
        [Inject]
        private IEnemyData enemyData { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 10;
        }

        public override void Update_State()
        {
            base.Update_State();

            if (controller.ActiveStateMechanic != this && GameInformation.Alarmed)
            {
                if (enemyData.IsTargetInRangeOfAttack(transform))
                {
                    controller.SwapState(this);
                }
            }
        }


        public override void WhileActive_State()
        {
            base.WhileActive_State();

            if(!enemyData.IsTargetInRangeOfAttack(transform))
            {
               // Debug.Log("Player not in range of the attack");
            }
            else
            {
                //Debug.Log("Attacking player");
            }

        }
    }
}
