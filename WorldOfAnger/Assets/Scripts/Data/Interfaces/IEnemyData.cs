using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
}
