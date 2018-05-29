using System.Collections;
using System.Collections.Generic;
using Gameinformation;
using General.State;
using Implementation.Data;
using UnityEngine;
using Zenject;

namespace Enemy.State
{
    public class EnemyDetection : StateForMechanics
    {

        [Inject]
        private IEnemyData enemyData;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 10;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            GameInformation.Alarmed = true;
        }

        public override void Update_State()
        {
            base.Update_State();
            if (Vector2.Distance(transform.position, enemyData.player.transform.position) < enemyData.RangeOfVision)
            {
                controller.SwapState(this);
            }
        }
    }
}
