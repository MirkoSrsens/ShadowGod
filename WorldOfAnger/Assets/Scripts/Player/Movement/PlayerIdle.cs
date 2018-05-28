using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.State;

namespace Player.Movement
{
    public class PlayerIdle: StateForMovement
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = -10;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            animationController.SetAnimation(this.GetType().Name);
        }
        public override void Update_State()
        {
            if(controller.ActiveStateMovement == null)
            {
                controller.SwapState(this);
            }
        }
    }
}
