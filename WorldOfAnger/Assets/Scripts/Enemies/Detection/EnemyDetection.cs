using System.Collections;
using System.Collections.Generic;
using General.State;
using UnityEngine;

public class EnemyDetection : StateForMechanics {

    protected override void Initialization_State()
    {
        base.Initialization_State();
    }

    public override void Update_State()
    {
        base.Update_State();
        if(controller.ActiveStateMechanic == null)
        {
            controller.SwapState(this);
        }
    }

    public override void WhileActive_State()
    {
        base.WhileActive_State();

    }

}
