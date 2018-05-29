using UnityEngine;

namespace Implementation.Data
{
    /// <summary>
    /// Holds implementation of <see cref="IEnemyData"/>.
    /// </summary>
    public class EnemyData : IEnemyData
    {
        /// <inheritdoc/>
        public GameObject player { get; set; }

        /// <inheritdoc/>
        public float RangeOfVision { get; set; }

        /// <inheritdoc/>
        public float AngleOfVision { get; set; }

        /// <inheritdoc/>
        public float RangeOfAttack { get; set; }

        public EnemyData()
        {
            player = GameObject.Find("Player");

            RangeOfVision = 5;
        }
    }
}
