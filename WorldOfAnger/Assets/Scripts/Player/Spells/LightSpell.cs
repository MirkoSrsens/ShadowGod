using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.State;
using UnityEngine;

namespace Player.Mechanic
{
    /// <summary>
    /// Spell used for ligting
    /// </summary>
    public class LightSpell : StateForMechanics
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            MechanicsData.spellPrefab = Resources.Load("SpellLight", typeof(GameObject)) as GameObject;
            Priority = 10;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            MechanicsData. spawnedObject = Instantiate(MechanicsData.spellPrefab);
        }

        public override void Update_State()
        {
            base.Update_State();
            if (Input.GetKey(MechanicsData.actionKey1))
            {
                if (controller.ActiveStateMechanic != this)
                    controller.SwapState(this);
            }
            else if (controller.ActiveStateMechanic == this)
            {
                controller.EndState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            MechanicsData.spawnedObject.transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            Destroy(MechanicsData.spawnedObject);
        }
    }
}
