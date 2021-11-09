using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerOnGroundState))]
[RequireComponent(typeof(PlayerInAirState))]
[RequireComponent(typeof(ShootingHookState))]
[RequireComponent(typeof(PlayerOnWallState))]
public class PlayerMovement : StateMachine
{
    public static PlayerMovement instance;

    void Awake()
    {
        instance = this;
    }

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
    public Collider2D collider;
    public float groundCheckDistance = 0.01f;
    public LayerMask groundLayer;

    public bool OnSlope { get; private set; }
    public bool OnGround
    {
        get
        {
            return Physics2D.BoxCast(
                new Vector2(collider.bounds.center.x, collider.bounds.min.y - groundCheckDistance / 2),
                new Vector2(collider.bounds.size.x - 0.1f, groundCheckDistance),
                0,
                Vector2.down, groundCheckDistance, groundLayer);
        }
    }

    [SerializeField]
    float slopePrecision = 0.8f, slopeBounceForce = 1f;

    void Start()
    {
        states.Add((int)StateOptions.OnGround, GetComponent<PlayerOnGroundState>());
        states.Add((int)StateOptions.InAir, GetComponent<PlayerInAirState>());
        states.Add((int)StateOptions.ShootingHook, GetComponent<ShootingHookState>());
        states.Add((int)StateOptions.OnWall, GetComponent<PlayerOnWallState>());

        StateMachineSetup((int)StateOptions.OnGround);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            rigidbody.velocity = (transform.position - collision.transform.position) * collision.transform.GetComponent<EnemyExplosion>().explosionPower;
            collision.transform.GetComponent<EnemyExplosion>().Explode();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!OnGround) return;
        bool previousOnSlope = OnSlope;
        OnSlope = true;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            float dot = Vector2.Dot(contact.normal, Vector2.up);
            if (dot > slopePrecision)
            {
                OnSlope = false;
                break;
            }
        }

        if (previousOnSlope != OnSlope)
            GameEvents.OnSlopeChanged?.Invoke(OnSlope);

        if (OnSlope)
            rigidbody.AddForce(collision.GetContact(0).normal * slopeBounceForce, ForceMode2D.Impulse);
    }
}
