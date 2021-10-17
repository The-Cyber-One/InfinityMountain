using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : State
{
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float jumpHeight = 2f;
    [SerializeField]
    float slopePrecision = 0.80f;
    bool onSlope;

    public override void Enter()
    {
        GameEvents.InputedJump += Jump;
        GetContext<PlayerMovement>().rigidbody.gravityScale = 1;
        GetContext<PlayerMovement>().playerInput.hookShot = false;
    }

    public override void Exit()
    {
        GameEvents.InputedJump -= Jump;
    }

    public void FixedUpdate()
    {
        // Walking
        Vector3 vector = GetContext<PlayerMovement>().playerInput.MovementDirection * Vector2.right * speed * Time.fixedDeltaTime + GetContext<PlayerMovement>().rigidbody.velocity.y * Vector2.up;
        GetContext<PlayerMovement>().rigidbody.velocity = vector;

        // Transition Checks
        if (!GetContext<PlayerMovement>().OnGround || onSlope) context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float dot = Vector2.Dot(collision.GetContact(0).normal, Vector2.up);
        onSlope = dot < slopePrecision;
        Debug.Log(dot);
    }

    private void Jump()
    {
        GetContext<PlayerMovement>().rigidbody.isKinematic = false;

        // Assumes gravity to face downward
        if (GetContext<PlayerMovement>().OnGround)
        {
            GetContext<PlayerMovement>().rigidbody.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -3.0f * Physics.gravity.y)
                + GetContext<PlayerMovement>().rigidbody.velocity.x * Vector2.right;
        }
    }
}
