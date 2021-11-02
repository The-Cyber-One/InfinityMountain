using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerOnGroundState))]
[RequireComponent(typeof(PlayerInAirState))]
[RequireComponent(typeof(ShootingHookState))]
[RequireComponent(typeof(PlayerOnWallState))]
public class PlayerMovement : StateMachine
{
    public enum StateOptions
    {
        OnGround,
        InAir,
        ShootingHook,
        OnWall
    }
    public PlayerInput playerInput;
    public HookData hookData;
    public Rigidbody2D rigidbody;
    public SpriteRenderer spriteRenderer;
    public bool OnGround
    {
        get
        {
            return Physics2D.BoxCast(
                new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.min.y - groundCheckDistance / 2),
                new Vector2(spriteRenderer.bounds.size.x - 0.1f, groundCheckDistance),
                0,
                Vector2.down, groundCheckDistance, groundLayer);
        }
    }
    public bool OnSlope { get; private set; }

    [SerializeField]
    float groundCheckDistance = 0.01f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float slopePrecision = 0.80f;


    private void Start()
    {
        states.Add((int)StateOptions.OnGround, GetComponent<PlayerOnGroundState>());
        states.Add((int)StateOptions.InAir, GetComponent<PlayerInAirState>());
        states.Add((int)StateOptions.ShootingHook, GetComponent<ShootingHookState>());
        states.Add((int)StateOptions.OnWall, GetComponent<PlayerOnWallState>());

        StateMachineSetup((int)StateOptions.OnGround);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!OnGround) return;
        float dot = Vector2.Dot(collision.GetContact(0).normal, Vector2.up);
        OnSlope = dot < slopePrecision;
    }
}
