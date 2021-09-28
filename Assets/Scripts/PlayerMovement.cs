using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float jumpHeight = 2f;
    [SerializeField]
    float upWallJumpHeight = 2f;
    [SerializeField]
    float upWallJumpDistance = 1f;
    [SerializeField]
    float sideWallJumpHeight = 1f;
    [SerializeField]
    float sideWallJumpDistance = 2f;

    Rigidbody2D rigidbody;
    PlayerInput playerInput;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        GameEvents.InputedJump += Jump;
    }

    private void FixedUpdate()
    {
        // Walking
        if (playerInput.OnGround)
        {
            rigidbody.velocity = playerInput.MovementDirection * Vector2.right * Time.fixedDeltaTime * speed + rigidbody.velocity.y * Vector2.up;
        }
    }

    private void Jump()
    {
        if (!playerInput.canJump) return;

        playerInput.canJump = false;

        // Assumes gravity to face downward
        if (playerInput.OnGround)
        {
            rigidbody.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -3.0f * Physics.gravity.y) + rigidbody.velocity.x * Vector2.right;
        }
        else if (playerInput.OnWall)
        {
            rigidbody.gravityScale = 1;

            // Check if player wants to jump away from wall
            if ((playerInput.MovementDirection > 0 && playerInput.wallSide < 0) || (playerInput.MovementDirection < 0 && playerInput.wallSide > 0))
            {
                rigidbody.velocity = Vector2.up * Mathf.Sqrt(sideWallJumpHeight * -3.0f * Physics.gravity.y) + -playerInput.wallSide * sideWallJumpDistance * Vector2.right;
            }
            else
            {
                rigidbody.velocity = Vector2.up * Mathf.Sqrt(upWallJumpHeight * -3.0f * Physics.gravity.y) + -playerInput.wallSide * upWallJumpDistance * Vector2.right;
            }
        }
    }
}
