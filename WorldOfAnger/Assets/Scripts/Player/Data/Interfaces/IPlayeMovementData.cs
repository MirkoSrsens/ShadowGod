using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayeMovementData{

    ////////GLOBAL///////////////

    /// <summary>
    /// Gets or sets rigidbody component.
    /// </summary>
    Rigidbody rigBody { get; set; }

    /// <summary>
    /// Gets or sets horizontal movement parameter.
    /// </summary>
    float HorizontalMovement { get; set; }

    /// <summary>
    /// Defines key for jumping
    /// </summary>
    KeyCode Jump { get; set; }

    /// <summary>
    /// Defines key for going left.
    /// </summary>
    KeyCode Left { get; set; }

    /// <summary>
    /// Defines key for going right.
    /// </summary>
    KeyCode Right { get; set; }

    /// <summary>
    /// Gets or sets movement speed.
    /// </summary>
    float MovementSpeed { get; set; }

    //// JUMPING /////

    /// <summary>
    /// Gets or sets flag for defining is character in air.
    /// </summary>
    bool IsInAir { get; set; }

    /// <summary>
    /// Gets or sets gravity equalizator.
    /// </summary>
    float GravityEqualizator { get; set; }

    /// <summary>
    /// Gets or sets gravity parameter.
    /// </summary>
    float Gravity { get; set; }

    /// <summary>
    /// Gets or sets jump height multiplicator.
    /// </summary>
    float JumpHeightMultiplicator { get; set; }
}
