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

    public override void Enter()
    {
        GameEvents.InputedJump += Jump;
    }

    public override void Exit()
    {
        GameEvents.InputedJump -= Jump;
        GetContext<PlayerMovement>().rigidbody.gravityScale = 1;
    }

    public void Jump()
    {
        // Check if player wants to jump away from wall
        if ((GetContext<PlayerMovement>().playerInput.HookDirection > 0 && GetContext<PlayerMovement>().playerInput.wallSide < 0) || (GetContext<PlayerMovement>().playerInput.HookDirection < 0 && GetContext<PlayerMovement>().playerInput.wallSide > 0))
        {
            // Jump away
            GetContext<PlayerMovement>().rigidbody.velocity = Vector2.up * Mathf.Sqrt(sideWallJumpHeight * -3.0f * Physics.gravity.y) + -GetContext<PlayerMovement>().playerInput.wallSide * sideWallJumpDistance * Vector2.right;
        }
        else
        {
            // Jump up
            GetContext<PlayerMovement>().rigidbody.velocity = Vector2.up * Mathf.Sqrt(upWallJumpHeight * -3.0f * Physics.gravity.y) + -GetContext<PlayerMovement>().playerInput.wallSide * upWallJumpDistance * Vector2.right;
        }

        GetContext<PlayerMovement>().playerInput.wallSide = 0;
        context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
    }
}
