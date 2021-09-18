using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public float jumpHeight = 1f;
    public float gravity = -9.81f;
    public float groundRadiusCheck = 0.01f;
    public Transform playerBottom;
    public LayerMask groundLayer;

    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Walking
        rigidbody.velocity = Input.GetAxis("Horizontal") * Vector2.right * Time.fixedDeltaTime * speed + rigidbody.velocity.y * Vector2.up;

        // Jumping
        bool isGrounded = Physics2D.CircleCast(playerBottom.position, groundRadiusCheck, Vector2.down, groundRadiusCheck, groundLayer);

        if (Input.GetButton("Jump") && isGrounded)
        {
            rigidbody.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -3.0f * gravity) + rigidbody.velocity.x * Vector2.right;
        }
    }
}
