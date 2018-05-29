using UnityEngine;

namespace Implementation.Data
{
    /// <summary>
    /// Holds implementation of <see cref="IMechanicsData"/>
    /// </summary>
    public class MechanicsData : IMechanicsData
    {
        /// <inheritdoc/>
        public GameObject spellPrefab { get; set; }

        /// <inheritdoc/>
        public GameObject spawnedObject { get; set; }

        /// <inheritdoc/>
        public KeyCode actionKey1 { get; set; }

        /// <inheritdoc/>
        public KeyCode actionKey2 { get; set; }

        /// <inheritdoc/>
        public KeyCode actionKey3 { get; set; }

        public MechanicsData()
        {
            actionKey1 = KeyCode.LeftShift;
            actionKey2 = KeyCode.I;
            actionKey3 = KeyCode.O;
        }
    }
}
