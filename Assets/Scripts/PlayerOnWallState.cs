using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnWallState : State
{
    [SerializeField]
    float upWallJumpHeight = 2f;
    [SerializeField]
    float upWallJumpDistance = 1f;
    [SerializeField]
    float sideWallJumpHeight = 1f;
    [SerializeField]
    float sideWallJumpDistance = 2f;

    void Start()
    {
        GameEvents.InputedJump += Jump;
    }

    public override void Enter()
    {

    }

    public override void Exit() { }

    public void Jump()
    {
        PlayerMovement playerMovement = context as PlayerMovement;
        // Check if player wants to jump away from wall
        if ((playerMovement.playerInput.MovementDirection > 0 && playerMovement.playerInput.wallSide < 0) || (playerMovement.playerInput.MovementDirection < 0 && playerMovement.playerInput.wallSide > 0))
        {
            playerMovement.rigidbody.velocity = Vector2.up * Mathf.Sqrt(sideWallJumpHeight * -3.0f * Physics.gravity.y) + -playerMovement.playerInput.wallSide * sideWallJumpDistance * Vector2.right;
        }
        else
        {
            playerMovement.rigidbody.velocity = Vector2.up * Mathf.Sqrt(upWallJumpHeight * -3.0f * Physics.gravity.y) + -playerMovement.playerInput.wallSide * upWallJumpDistance * Vector2.right;
        }
    }
}
