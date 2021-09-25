using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MovementDirection { get { return _movementDirection; } }
    public bool OnGround { get { return _onGround; } }
    public bool Jump { get { return _jump; } }

    [SerializeField]
    float groundRadiusCheck = 0.01f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float jumpDetectionTime = 0.1f;

    float _movementDirection;
    bool _onGround;
    bool _jump;
    bool hookShot;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        bool screenHasTouch = Input.touchCount > 0;

        // Movement direction
        float x = 0;
        if (screenHasTouch)
        {
            x = (Screen.currentResolution.width / 2 > Input.GetTouch(0).position.x) ? -1 : 1;
        }
        _movementDirection = Input.GetAxis("Horizontal") + x;

        // Ground check
        Bounds spriteBounds = spriteRenderer.bounds;
        _onGround = Physics2D.CircleCast(new Vector2(spriteBounds.center.x, spriteBounds.min.y), groundRadiusCheck, Vector2.down, groundRadiusCheck, groundLayer);

        // Jump input
        bool jumpViaTouch = false;
        if (screenHasTouch)
        {
            jumpViaTouch = Input.GetTouch(0).deltaTime < jumpDetectionTime;
        }

        if (_onGround && (Input.GetButton("Jump") || jumpViaTouch))
        {
            GameEvents.PlayerJump?.Invoke();
        }

        // Hook 
        Vector2 hookDirection = Vector2.zero;

        // Reactivate hook
        if (_onGround)
        {
            hookShot = false;
        }

        // Update hook shoot direction
        if (!_onGround && !hookShot && (Input.GetButtonDown("Jump") || screenHasTouch))
        {
            hookShot = true;

            hookDirection.x = Input.GetAxis("Horizontal") + (screenHasTouch ? Input.GetTouch(0).deltaPosition.x : 0);
            hookDirection.y = Input.GetAxis("Vertical") + (screenHasTouch ? Input.GetTouch(0).deltaPosition.y : 0);

            // Convert the range -2 ~ 2 to either -1, 0 or 1
            hookDirection.x = Mathf.Floor(Mathf.InverseLerp(-2, 2, hookDirection.x) * 3 - 1);
            hookDirection.y = Mathf.Floor(Mathf.InverseLerp(-2, 2, hookDirection.y) * 3 - 1);

            GameEvents.PlayerShootsHook.Invoke(hookDirection);
        }
    }
}
