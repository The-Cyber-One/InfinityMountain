using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : State
{
    PlayerMovement playerMovement;

    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float jumpHeight = 2f;

    void Start()
    {
        GameEvents.InputedJump += Jump;
    }

    public override void Enter()
    {
        playerMovement = context as PlayerMovement;
    }

    public override void Exit() { }

    public void FixedUpdate()
    {
        // Walking
        if (playerMovement.playerInput.ContactWithGround && !playerMovement.playerInput.hookShot)
        {
            playerMovement.rigidbody.velocity = playerMovement.playerInput.MovementDirection * Vector2.right * Time.fixedDeltaTime * speed + playerMovement.rigidbody.velocity.y * Vector2.up;
        }
    }

    private void Jump()
    {
        if (!playerMovement.playerInput.canJump) return;

        playerMovement.playerInput.canJump = false;

        playerMovement.rigidbody.isKinematic = false;

        // Assumes gravity to face downward
        if (playerMovement.playerInput.ContactWithGround)
        {
            playerMovement.rigidbody.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -3.0f * Physics.gravity.y) 
                + playerMovement.rigidbody.velocity.x * Vector2.right;
        }
    }
}
