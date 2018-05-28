using System.Collections;
using System.Collections.Generic;
using General.State;
using Player.Movement;
using UnityEngine;

namespace Camera
{
    public class CameraData : ICameraData
    {
        /// <inheritdoc/>
        public GameObject Player { get; set; }

        /// <inheritdoc/>
        public StateController controller { get; set; }

        /// <inheritdoc/>
        public StopMoving stopMoving { get; set; }

        /// <inheritdoc/>
        public float followDistance { get; set; }

        /// <inheritdoc/>
        public float movementSpeed { get; set; }
    }
}
