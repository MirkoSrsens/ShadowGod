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
        public float AngleOfVisionHigher { get; set; }

        /// <inheritdoc/>
        public float AngleOfVisionLower { get; set; }

        /// <inheritdoc/>
        public float RangeOfAttack { get; set; }

        public EnemyData()
        {
            player = GameObject.Find("Player");

            RangeOfVision = 3.5f;
            AngleOfVisionLower = 0;
            AngleOfVisionHigher = 75;
            RangeOfAttack = 1;
        }

        /// <inheritdoc/>
        public bool IsTargetDetected(Transform position)
        {
            var angle = Vector2.Angle(position.transform.right, player.transform.position - position.transform.position);

            if (angle > AngleOfVisionLower && angle < AngleOfVisionHigher)
            {
                RaycastHit2D hit = Physics2D.Raycast(position.transform.position, player.transform.position - position.transform.position, RangeOfVision);
                if (hit.collider != null && hit.collider.tag == "Player")
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsTargetInRangeOfAttack(Transform position)
        {
            var angle = Vector2.Angle(position.transform.right, player.transform.position - position.transform.position);

            if (angle > AngleOfVisionLower && angle < AngleOfVisionHigher)
            {
                RaycastHit2D hit = Physics2D.Raycast(position.transform.position, player.transform.position - position.transform.position, RangeOfAttack);
                if (hit.collider != null && hit.collider.tag == "Player")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
