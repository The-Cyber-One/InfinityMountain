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
    [SerializeField]
    float startSlidingTime = 0.5f;

    bool isSliding;
    Coroutine enumerator;

    public override void Enter()
    {
        GameEvents.InputedJump += Jump;
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Static;

        isSliding = false;
        Invoke(nameof(Sliding), startSlidingTime);
    }

    public override void Exit()
    {
        GameEvents.InputedJump -= Jump;
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    void Sliding()
    {
        isSliding = true;
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    void Update()
    {
        if (!isSliding) return;
        if (GetContext<PlayerMovement>().OnGround || !Physics2D.BoxCast(
                new Vector2(GetContext<PlayerMovement>().collider.bounds.center.x, GetContext<PlayerMovement>().collider.bounds.min.y - GetContext<PlayerMovement>().groundCheckDistance / 2),
                new Vector2(GetContext<PlayerMovement>().collider.bounds.size.x + 0.1f, GetContext<PlayerMovement>().groundCheckDistance),
                0,
                Vector2.down, GetContext<PlayerMovement>().groundCheckDistance, GetContext<PlayerMovement>().groundLayer))
        {
            if (enumerator != null)
            {
                StopCoroutine(enumerator);
            }
            context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
        }
    }

    public void Jump()
    {
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Dynamic;

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

        CancelInvoke(nameof(Sliding));
        GetContext<PlayerMovement>().playerInput.wallSide = 0;
        context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
    }
}
