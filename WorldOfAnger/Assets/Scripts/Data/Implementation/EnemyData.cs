using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnemyData : IEnemyData
{
    public GameObject player { get; set; }

    public float RangeOfVision { get; set; }

    public EnemyData()
    {
        player = GameObject.Find("Player");

        RangeOfVision = 5;
    }
}
