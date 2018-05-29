using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gameinformation;
using General.State;
using Implementation.Data;
using UnityEngine;
using Zenject;

namespace Enemy.State
{
    public class EnemyFollowTarget : StateForMovement
    {
        [Inject]
        private IEnemyData enemyData { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 100;
        }

        public override void Update_State()
        {
            base.Update_State();
            if (GameInformation.Alarmed && controller.ActiveStateMovement != this)
            {
                controller.SwapState(this);
            }
            else if (!GameInformation.Alarmed && controller.ActiveStateMovement == this)
            {
                controller.EndState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            if (Vector3.Distance(transform.position, enemyData.player.transform.position) > 0.5f)
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(enemyData.player.transform.position.x, transform.position.y), MovementData.MovementSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
