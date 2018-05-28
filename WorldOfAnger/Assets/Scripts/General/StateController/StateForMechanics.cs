using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace General.State
{
    /// <summary>
    /// Defines states that are used for mechanics.
    /// </summary>
    public class StateForMechanics : State
    {
        [Inject]
        protected IMechanicsData playerMechanicsData;
    }
}
