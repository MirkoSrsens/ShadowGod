using System.Collections;
using System.Collections.Generic;
using Implementation.Data;
using UnityEngine;
using Zenject;

namespace General.State
{
    /// <summary>
    /// Defines states that are used for mechanics.
    /// </summary>
    public abstract class StateForMechanics : State
    {
        [Inject]
        protected IMechanicsData MechanicsData;
    }
}
