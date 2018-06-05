using System.Collections;
using System.Collections.Generic;
using General.State;
using Implementation.Data;
using UnityEngine;
using Zenject;

namespace Enemy.State
{
    /// <summary>
    /// Holds implementation of enemy patroll, while <see cref="GameInformation.Alarmed"/> is false.
    /// </summary>
    public class EnemyPatroll : StateForMovement
    {
        [Inject]
        private IEnemyData enemyData { get; set; }

        int directionCorrection = 1;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 1;
        }

        public override void Update_State()
        {
            base.Update_State();
            if (controller.ActiveStateMovement == null)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            MovementData.rigBody.velocity = new Vector2(directionCorrection * 20 * Time.deltaTime * MovementData.MovementSpeed, MovementData.rigBody.velocity.y);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (controller.ActiveStateMovement == this && collision.gameObject.tag != "Player")
            {
                if (controller.ActiveStateMovement == this)
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
    }
}
