using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains implementation of <see cref="IMovementData"/>.
/// </summary>
public class MovementData : IMovementData
{
    /// <inheritdoc/>
    public Rigidbody2D rigBody { get; set; }

    /// <inheritdoc/>
    public float HorizontalMovement { get; set; }

    /// <inheritdoc/>
    public KeyCode Jump { get; set; }

    /// <inheritdoc/>
    public KeyCode Left { get; set; }

    /// <inheritdoc/>
    public KeyCode Right { get; set; }

    /// <inheritdoc/>
    public float MovementSpeed { get; set; }

    /// <inheritdoc/>
    public bool IsInAir { get; set; }

    /// <inheritdoc/>
    public float GravityEqualizator { get; set; }

    /// <inheritdoc/>
    public float Gravity { get; set; }

    /// <inheritdoc/>
    public float JumpHeightMultiplicator { get; set; }

    public MovementData()
    {
        Jump = KeyCode.Space;
        Left = KeyCode.A;
        Right = KeyCode.D;
        MovementSpeed = 2;
        GravityEqualizator = 2.5f;
        Gravity = 5.81f;
        JumpHeightMultiplicator = 1.2f;
    }
}
