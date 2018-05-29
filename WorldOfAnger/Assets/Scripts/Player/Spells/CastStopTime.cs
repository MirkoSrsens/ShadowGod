using System.Collections;
using System.Collections.Generic;
using General.State;
using UnityEngine;

/// <summary>
/// Uses for player to cast stop time spell.
/// </summary>
public class CastStopTime : StateForMechanics {

    protected override void Initialization_State()
    {
        base.Initialization_State();
        Priority = 30;
    }

    public override void OnEnter_State()
    {
        base.OnEnter_State();
        GameInformation.TimeStoped = true;
        animationController.SetStateAnimation(this.GetType().Name);
    }

    public override void Update_State()
    {
        base.Update_State();
        if(Input.GetKey(MechanicsData.actionKey3))
        {
            controller.SwapState(this);
        }
        else if (controller.ActiveStateMechanic == this)
        {
            controller.EndState(this);
        }
    }

    public override void OnExit_State()
    {
        base.OnExit_State();
        animationController.SetStateAnimation(this.GetType().Name);
        GameInformation.TimeStoped = false;
    }
}
