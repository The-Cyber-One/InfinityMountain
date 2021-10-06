using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float wallSide = 0;
    public bool canJump = false;

    public bool OnWall
    {
        get { return _onWall; }
        set
        {
            _onWall = value;
            if (value) canJump = true;
        }
    }
    public bool OnGround
    {
        get { return _onGround; }
        private set
        {
            _onGround = value;
            if (value) canJump = true;
        }
    }
    public bool HookShot { private get { return _hookShot; } set { _hookShot = value; } }
    public float MovementDirection { get; private set; } = 0;

    [SerializeField]
    float groundCheckDistance = 0.01f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float jumpDetectionTime = 0.3f;
    [SerializeField]
    float minSwipeMovement = 50f;
    [SerializeField]
    [Range(0,1)]
    float rotationIntensity = 0.3f;
    bool _onWall = false;
    bool _onGround = false;
    bool _hookShot = false;
    SpriteRenderer spriteRenderer;

    Gyroscope gyro;
    float xVelocity;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Input.gyro.enabled = true;
    }

    void Update()
    {
        bool screenHasTouch = Input.touchCount > 0;

        // Movement direction
        Gyroscope gyro = null;
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;

            xVelocity = Mathf.InverseLerp(0, 360, gyro.attitude.eulerAngles.z);
            Debug.Log(xVelocity);
        }

        // TODO: make ramp functinality
        MovementDirection = Input.GetAxis("Horizontal") + xVelocity;

        // Ground check
        Bounds spriteBounds = spriteRenderer.bounds;
        OnGround = Physics2D.BoxCast(new Vector2(spriteBounds.center.x, spriteBounds.min.y - groundCheckDistance / 2), new Vector2(spriteBounds.size.x, groundCheckDistance), 0, Vector2.down, groundCheckDistance, groundLayer);

        // Jump input
        bool jumpViaTouch = false;
        if (screenHasTouch)
        {
            jumpViaTouch = Input.GetTouch(0).phase == TouchPhase.Began;
        }

        if (Input.GetButtonDown("Jump") || jumpViaTouch)
        {
            GameEvents.InputedJump?.Invoke();
            wallSide = 0;
            OnWall = false;
            HookShot = false;
        }

        // Reactivate hook
        if (OnGround)
        {
            HookShot = false;
        }

        // Update hook shoot direction
        if (!OnGround && !HookShot && (Input.GetButtonDown("Jump") || screenHasTouch))
        {
            float hookDirection = Input.GetAxis("Horizontal");
            if (screenHasTouch)
            {
                hookDirection += (Mathf.Abs(Input.GetTouch(0).deltaPosition.x) > minSwipeMovement ? Input.GetTouch(0).deltaPosition.x : 0);
            }

            if (hookDirection != 0)
            {
                // Convert the range -2 ~ 2 to either -1 or 1 on one of the axis (0 is filtered by the if statement)
                hookDirection = (hookDirection > 0 ? 1 : 0) * 2 - 1;

                GameEvents.InputedShootHook.Invoke(hookDirection);
            }
        }
    }


}
