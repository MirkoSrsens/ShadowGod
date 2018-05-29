using System.Collections;
using System.Collections.Generic;
using General.State;
using Player.Movement;
using UnityEngine;

namespace Implementation.Data
{
    public class CameraData : ICameraData
    {
        /// <inheritdoc/>
        public GameObject Player { get; set; }

        /// <inheritdoc/>
        public StateController Controller { get; set; }

        /// <inheritdoc/>
        public StopMoving StopMoving { get; set; }

        /// <inheritdoc/>
        public float FollowDistance { get; set; }

        /// <inheritdoc/>
        public float MovementSpeed { get; set; }

        /// <inheritdoc/>
        public float ZAxisOffset { get; set; }


        public CameraData()
        {
            ZAxisOffset = 4.5f;
            MovementSpeed = 10;
        }
    }
}
