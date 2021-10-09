using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : State
{
    [SerializeField]
    float groundCheckDistance = 0.01f;
    [SerializeField]
    LayerMask groundLayer;

    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Enter() { }

    public override void Exit() { }

    void Update()
    {
        // Ground check
        Bounds spriteBounds = spriteRenderer.bounds;
        if (Physics2D.BoxCast(new Vector2(spriteBounds.center.x, spriteBounds.min.y - groundCheckDistance / 2), new Vector2(spriteBounds.size.x, groundCheckDistance), 0, Vector2.down, groundCheckDistance, groundLayer))
        {

        }
    }
}
