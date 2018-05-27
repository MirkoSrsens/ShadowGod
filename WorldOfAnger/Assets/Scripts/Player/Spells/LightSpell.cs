using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.State;
using UnityEngine;

namespace Player.Mechanic
{
    public class LightSpell : StateForMechanics
    {
        GameObject spellPrefab { get; set; }
        GameObject spawnedObject { get; set; }
        protected override void Initialization_State()
        {
            base.Initialization_State();
            spellPrefab = Resources.Load("SpellLight", typeof(GameObject)) as GameObject;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            spawnedObject = Instantiate(spellPrefab);
        }

        public override void Update_State()
        {
            base.Update_State();
            if(Input.GetKey(KeyCode.LeftShift))
            {
                if(controller.ActiveStateMechanic != this)
                controller.SwapState(this);
            }
            else
            {
                controller.EndState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            spawnedObject.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            Destroy(spawnedObject);
        }
    }
}
