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
    class EnemyStop : StateForMovement
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 0;
        }

        public override void Update_State()
        {
            base.Update_State();
            if (!GameInformation.Alarmed
                && controller.ActiveStateMovement != this
                && controller.ActiveStateMechanic != null
                && controller.ActiveStateMechanic.GetType() == typeof(EnemyDetectTarget)
                )
            {
                controller.ForceSwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            MovementData.rigBody.velocity = new Vector2(0, MovementData.rigBody.velocity.y);

            if (GameInformation.Alarmed)
            {
                controller.EndState(this);
            }
        }
    }
}
