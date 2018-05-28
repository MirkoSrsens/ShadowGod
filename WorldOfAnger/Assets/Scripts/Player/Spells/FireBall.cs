﻿using System.Collections;
using System.Collections.Generic;
using General.State;
using UnityEngine;

namespace Player.Mechanic
{
    public class FireBall : StateForMechanics
    {
        private float startPosition { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 20;
            startPosition = 0.5f;
            playerMechanicsData.spellPrefab = Resources.Load("FireBall", typeof(GameObject)) as GameObject;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            if(transform.rotation == Quaternion.Euler(0, 0, 0))
                playerMechanicsData.spawnedObject = Instantiate(playerMechanicsData.spellPrefab, new Vector2(transform.position.x + startPosition, transform.position.y), this.transform.rotation, null);
            else
            {
                playerMechanicsData.spawnedObject = Instantiate(playerMechanicsData.spellPrefab, new Vector2(transform.position.x - startPosition, transform.position.y), this.transform.rotation, null);
            }
        }

        public override void Update_State()
        {
            base.Update_State();

            if(Input.GetKeyDown(playerMechanicsData.actionKey2))
            {
                controller.SwapState(this);
            }
            else if(controller.ActiveStateMechanic == this)
            {
                controller.EndState(this);
            }
        }
    }
}
