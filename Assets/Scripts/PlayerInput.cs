using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 InputDirection { get { return _inputDirection; } }
    public bool OnGround { get { return _onGround; } }
    public bool Jump { get { return _jump; } }
    public Vector2 HookDirection { get { return _hookDirection; } }

    [SerializeField]
    float groundRadiusCheck = 0.01f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float jumpDetectionTime = 0.1f;

    Vector2 _inputDirection;
    bool _onGround;
    bool _jump;
    Vector2 _hookDirection;
    bool _hookShot;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        bool screenHasTouch = Input.touchCount > 0;

        // Input direction
        float x = 0;
        if (screenHasTouch)
        {
            x = (Screen.currentResolution.width / 2 > Input.GetTouch(0).position.x) ? -1 : 1;
        }
        _inputDirection.x = Input.GetAxis("Horizontal") + x;
        _inputDirection.y = Input.GetAxis("Vertical");

        // Ground check
        Bounds spriteBounds = spriteRenderer.bounds;
        _onGround = Physics2D.CircleCast(new Vector2(spriteBounds.center.x, spriteBounds.min.y), groundRadiusCheck, Vector2.down, groundRadiusCheck, groundLayer);

        // Jump input
        bool jumpViaTouch = false;
        if (screenHasTouch)
        {
            jumpViaTouch = Input.GetTouch(0).deltaTime < jumpDetectionTime;
        }
        _jump = _onGround && (Input.GetButton("Jump") || jumpViaTouch);

        // Hook 
        _hookDirection = Vector2.zero;
        if (_onGround)
        {
            _hookShot = false;
        }

        if (!_onGround && !_hookShot)
        {
            // TODO: get correct swipe direction
            _hookShot = true;
            _hookDirection.x = Input.GetAxis("Horizontal");
        }
    }
}
