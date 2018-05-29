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
        /// Gets or sets angle of vision.
        /// </summary>
        float AngleOfVision { get; set; }

        /// <summary>
        /// Gets or sets range of the attack.
        /// </summary>
        float RangeOfAttack { get; set; }
    }
}