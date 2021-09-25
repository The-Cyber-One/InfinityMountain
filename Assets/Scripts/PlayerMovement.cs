using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float jumpHeight = 1f;

    Rigidbody2D rigidbody;
    PlayerInput playerInput;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        GameEvents.PlayerJump += Jump;
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
        // Assumes gravity to face downward
        rigidbody.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -3.0f * Physics.gravity.y) + rigidbody.velocity.x * Vector2.right;
    }
}
