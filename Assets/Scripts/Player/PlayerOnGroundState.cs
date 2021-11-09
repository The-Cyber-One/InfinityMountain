using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnGroundState : State
{
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    [Min(0)]
    float minJumpHeight, maxJumpHeight;
    [SerializeField]
    float coyoteTime = 0.2f;

    bool switchingState = false;

    float HeightToVelocity(float height) => Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.magnitude) * height);

    public override void Enter()
    {
        GameEvents.InputedJump += Jump;
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Dynamic;
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
    }

    void Update()
    {
        // Transition Checks
        if (GetContext<PlayerMovement>().OnSlope) SwitchStateToInAir();
        if (!GetContext<PlayerMovement>().OnGround && !switchingState)
        {
            switchingState = true;
            Invoke(nameof(SwitchStateToInAir), coyoteTime);
        }
        if (GetContext<PlayerMovement>().OnGround)
        {
            switchingState = false;
            CancelInvoke(nameof(SwitchStateToInAir));
        }
    }

    void SwitchStateToInAir()
    {
        context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
    }

    void Jump()
    {
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Dynamic;

        StartCoroutine(Jumping());
    }

    IEnumerator Jumping()
    {
        Rigidbody2D rigidbody = GetContext<PlayerMovement>().rigidbody;

        rigidbody.velocity = Vector2.up * HeightToVelocity(maxJumpHeight) + GetContext<PlayerMovement>().rigidbody.velocity.x * Vector2.right;

        yield return new WaitWhile(() => GetContext<PlayerMovement>().playerInput.Jumping);

        float minJumpVelocity = HeightToVelocity(minJumpHeight);

        if (rigidbody.velocity.y > minJumpVelocity)
            rigidbody.velocity = Vector2.up * minJumpVelocity + GetContext<PlayerMovement>().rigidbody.velocity.x * Vector2.right;
    }
}
