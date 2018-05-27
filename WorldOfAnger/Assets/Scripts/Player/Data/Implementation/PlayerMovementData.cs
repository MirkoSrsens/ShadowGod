using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementData : IPlayeMovementData
{
    public Rigidbody rigBody { get; set; }

    public float HorizontalMovement { get; set; }
    public KeyCode Jump { get; set; }
    public KeyCode Left { get; set; }
    public KeyCode Right { get; set; }
    public float MovementSpeed { get; set; }
    public bool IsInAir { get; set; }
    public float GravityEqualizator { get; set; }
    public float Gravity { get; set; }
    public float JumpHeightMultiplicator { get; set; }

    public PlayerMovementData()
    {
        Jump = KeyCode.Space;
        Left = KeyCode.A;
        Right = KeyCode.D;
        MovementSpeed = 10;
        GravityEqualizator = 2;
        Gravity = 9.81f;
        JumpHeightMultiplicator = 2;
    }
}
