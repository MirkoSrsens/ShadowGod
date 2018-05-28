using System.Collections;
using System.Collections.Generic;
using General.State;
using UnityEngine;

public class StopTimeSpell : HighPriorityState {

    Rigidbody rig;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        rig = GetComponent<Rigidbody>();
        Priority = 10000;
    }
    public override void Update_State()
    {
        base.Update_State();
        if (GameInformation.TimeStoped && controller.HighPriorityState != this)
        {
            controller.SwapState(this);
        }
        else if(!GameInformation.TimeStoped && controller.HighPriorityState == this)
        {
            controller.EndState(this);
        }
    }

    public override void WhileActive_State()
    {
        base.WhileActive_State();
        if (!rig.isKinematic)
        {
            rig.isKinematic = true;
        }
    }

    public override void OnExit_State()
    {
        base.OnExit_State();
        if (rig.isKinematic)
        {
            rig.isKinematic = false;
        }
    }
}
