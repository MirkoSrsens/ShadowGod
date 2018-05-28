using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.State;
using Player.Movement;
using UnityEngine;

namespace Camera
{
    interface ICameraData
    {
        /// <summary>
        /// Gets or sets player component
        /// </summary>
        GameObject Player { get; set; }

        /// <summary>
        /// Gets or sets state controllerOfThePlayer.
        /// </summary>
        StateController controller { get; set; }

        /// <summary>
        /// Gets or sets <see cref="StopMoving"/> component in player.
        /// </summary>
        StopMoving stopMoving { get; set; }

        /// <summary>
        /// Follow distance of the z axis.
        /// </summary>
        float followDistance { get; set; }

        /// <summary>
        /// Gets or sets movement speed of the camera.
        /// </summary>
        float movementSpeed { get; set; }
    }
}
