using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MovementDirection { get { return _movementDirection; } }
    public bool OnGround { get { return _onGround; } }
    public bool HookShot { set { _hookShot = value; } }

    [SerializeField]
    float groundRadiusCheck = 0.01f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float jumpDetectionTime = 0.1f;
    [SerializeField]
    float minSwipeMovement = 50f;

    float _movementDirection;
    bool _onGround;
    bool _hookShot;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool screenHasTouch = Input.touchCount > 0;

        // Movement direction
        float x = 0;
        if (screenHasTouch)
        {
            x = (Screen.width / 2 > Input.GetTouch(0).position.x) ? -1 : 1;
        }
        // TODO: make ramp functinality
        _movementDirection = Input.GetAxis("Horizontal") + x;

        // Ground check
        Bounds spriteBounds = spriteRenderer.bounds;
        _onGround = Physics2D.CircleCast(new Vector2(spriteBounds.center.x, spriteBounds.min.y), groundRadiusCheck, Vector2.down, groundRadiusCheck, groundLayer);

        // Jump input
        bool jumpViaTouch = false;
        if (Input.touchCount > 1)
        {
            jumpViaTouch = Input.GetTouch(1).phase == TouchPhase.Began;
        }

        if (_onGround && (Input.GetButton("Jump") || jumpViaTouch))
        {
            GameEvents.PlayerJump?.Invoke();
        }

        // Hook 
        float hookDirection = 0;

        // Reactivate hook
        if (_onGround)
        {
            _hookShot = false;
        }

        // Update hook shoot direction
        if (!_onGround && !_hookShot && (Input.GetButtonDown("Jump") || screenHasTouch))
        {
            hookDirection = Input.GetAxis("Horizontal");
            if (screenHasTouch)
            {
                hookDirection += (Mathf.Abs(Input.GetTouch(0).deltaPosition.x) > minSwipeMovement ? Input.GetTouch(0).deltaPosition.x : 0);
            }

            if (hookDirection != 0)
            {
                // Convert the range -2 ~ 2 to either -1 or 1 on one of the axis (0 is filtered by the if statement)
                hookDirection = (hookDirection > 0 ? 1 : 0) * 2 - 1;

                GameEvents.PlayerShootsHook.Invoke(hookDirection);
            }
        }
    }
}
