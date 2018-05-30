using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Implementation.Data
{
    public interface IEnemyData
    {
        /// <summary>
        /// Gets or sets player
        /// </summary>
        GameObject player { get; set; }

        /// <summary>
        /// Gets or sets range of vision. 
        /// </summary>
        float RangeOfVision { get; set; }

        /// <summary>
        /// Gets or sets higher angle of vision.
        /// </summary>
        float AngleOfVisionHigher { get; set; }

        /// <summary>
        /// Gets or sets lower angle of vision.
        /// </summary>
        float AngleOfVisionLower { get; set; }

        /// <summary>
        /// Gets or sets range of the attack.
        /// </summary>
        float RangeOfAttack { get; set; }

        /// <summary>
        /// Displays is the player is visible to enemy.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool IsTargetDetected(Transform position);

        /// <summary>
        /// Checks if target is in range of the attack;
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool IsTargetInRangeOfAttack(Transform position);
    }
}