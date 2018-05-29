using System.Collections;
using System.Collections.Generic;
using General.State;
using UnityEngine;

public class EnemyPatroll : StateForMovement {

    int directionCorrection = 1;
    public override void Update_State()
    {
        base.Update_State();
        if(controller.ActiveStateMovement == null)
        {
            controller.SwapState(this);
        }
    }

    public override void WhileActive_State()
    {
        base.WhileActive_State();
        MovementData.rigBody.velocity = new Vector2(directionCorrection*20 * Time.deltaTime * MovementData.MovementSpeed, MovementData.rigBody.velocity.y);
        Debug.Log(MovementData.rigBody.velocity.x);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(controller.ActiveStateMovement == this)
        {
            if (this.transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                directionCorrection = -1;
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                directionCorrection = 1;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
