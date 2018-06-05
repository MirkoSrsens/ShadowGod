using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameinformation
{
    /// <summary>
    /// Contains global informations inside of the game.
    /// </summary>
    public class GameInformation : MonoBehaviour
    {

        /// <summary>
        /// Gets or sets flag for stopping the time.
        /// </summary>
        public static bool TimeStoped { get; set; }

        /// <summary>
        /// Gets or sets flag for alarm {get;set;
        /// </summary>
        public static bool Alarmed { get; set; }

        private bool AlarmFlag { get; set; }

        /// <summary>
        /// Defines how much alarm will last.
        /// </summary>
        const float AlarmTime = 10;



        private void Awake()
        {
            TimeStoped = false;

            if(Alarmed && !AlarmFlag)
            {
                AlarmFlag = true;
                StartCoroutine(AlarmTimer());
            }
        }
        private static void startAlarm()
        { 

        }

        public IEnumerator AlarmTimer()
        {
            yield return new WaitForSeconds(AlarmTime);
            Alarmed = false;
            AlarmFlag = false;
            Debug.Log("Alarm expired");
        }
    }
}
