using System.Collections;
using System.Collections.Generic;
using Gameinformation;
using General.State;
using UnityEngine;

namespace Player.Mechanic
{
    public class StopTimeSpell : HighPriorityState
    {

        Rigidbody2D rig;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            rig = GetComponent<Rigidbody2D>();
            Priority = 10000;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
        }

        public override void Update_State()
        {
            base.Update_State();
            if (GameInformation.TimeStoped && controller.HighPriorityState != this)
            {
                controller.SwapState(this);
            }
            else if (!GameInformation.TimeStoped && controller.HighPriorityState == this)
            {
                controller.EndState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            if (rig.bodyType == RigidbodyType2D.Dynamic)
            {
                rig.bodyType = RigidbodyType2D.Kinematic;
            }

            rig.velocity = Vector2.zero;
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            if (rig.bodyType == RigidbodyType2D.Kinematic)
            {
                rig.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
