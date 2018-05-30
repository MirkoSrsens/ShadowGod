using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.State;
using Gameinformation;
using Implementation.Data;
using Zenject;
using UnityEngine;

namespace Enemy.State
{
    public class AttackPlayer : StateForMechanics
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
                controller.EndState(this);
            }

            Debug.Log("Attacking player");
        }
    }
}
