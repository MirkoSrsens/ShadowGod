using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.State;
using Player.Movement;
using UnityEngine;

namespace Implementation.Data
{
    public interface ICameraData
    {
        /// <summary>
        /// Gets or sets player component
        /// </summary>
        GameObject Player { get; set; }

        /// <summary>
        /// Gets or sets state controllerOfThePlayer.
        /// </summary>
        StateController Controller { get; set; }

        /// <summary>
        /// Gets or sets <see cref="global::Player.Movement.StopMoving"/> component in player.
        /// </summary>
        StopMoving StopMoving { get; set; }

        /// <summary>
        /// Follow distance of the z axis.
        /// </summary>
        float FollowDistance { get; set; }

        /// <summary>
        /// Gets or sets movement speed of the camera.
        /// </summary>
        float MovementSpeed { get; set; }

        /// <summary>
        /// Gets or sets z axis offset.
        /// </summary>
        float ZAxisOffset { get; set; }
    }
}
