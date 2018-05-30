using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gameinformation;
using General.State;
using Implementation.Data;
using UnityEngine;
using Zenject;

namespace Enemy.State
{
    public class EnemyDetectTarget : StateForMechanics
    {
        [Inject]
        private IEnemyData enemyData { get; set; }

        /// <summary>
        /// Gets or sets alarm flag for preventing multiple coroutine functions.
        /// </summary>
        private bool alarmFlag { get; set; }

        /// <summary>
        /// Gets or sets time of waiting before activating alarm.
        /// </summary>
        private float timeToWaitBeforeAlarm { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 1;
            timeToWaitBeforeAlarm = 2;
            alarmFlag = false;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            if (!alarmFlag)
            {
                alarmFlag = true;
                StartCoroutine(StartAlarm());
            }
        }

        public override void Update_State()
        {
            base.Update_State();

            if (controller.ActiveStateMechanic != this)
            {
                var isTargetInRange = enemyData.IsTargetDetected(transform);
                if (isTargetInRange && controller.ActiveStateMechanic != this)
                {
                    controller.SwapState(this);
                }
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            StopCoroutine(StartAlarm());
        }

        public IEnumerator StartAlarm()
        {
            yield return new WaitForSeconds(timeToWaitBeforeAlarm);
            GameInformation.Alarmed = true;
            Debug.Log("Alarmed");
            alarmFlag = false;
        }
    }
}
