using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    SpriteRenderer renderer;
    [SerializeField]
    Rigidbody2D rigidbody;
    [SerializeField]
    float slopeActivationTime = 0.1f, slopeDeactivationTime = 0.1f;
    [SerializeField]
    float mirrorDetection = 0.05f;

    [Header("Jump animation")]
    [SerializeField]
    Sprite[] startJump, upMovement, downMovement, landingJump;

    Coroutine setOnSlopeCoroutine;

    void OnEnable()
    {
        GameEvents.AvailableHooksChanged += UpdateAnimatorHook;
        GameEvents.OnSlopeChanged += UpdateAnimatorOnSlope;
    }

    void OnDisable()
    {
        GameEvents.AvailableHooksChanged -= UpdateAnimatorHook;
        GameEvents.OnSlopeChanged -= UpdateAnimatorOnSlope;
    }

    void UpdateAnimatorHook(int amount)
    {
        animator.SetInteger("hookAmount", amount);
    }


    void UpdateAnimatorOnSlope(bool value)
    {
        if (setOnSlopeCoroutine != null)
        {
            StopCoroutine(setOnSlopeCoroutine);
        }

        setOnSlopeCoroutine = StartCoroutine(SetOnSlopeValue(value, value ? slopeActivationTime : slopeDeactivationTime));
    }

    IEnumerator SetOnSlopeValue(bool value, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        animator.SetBool("onSlope", value);
    }

    void Update()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rigidbody.velocity.x));

        if (rigidbody.velocity.x >= mirrorDetection)
        {
            renderer.flipX = false;
        }
        else if (rigidbody.velocity.x <= -mirrorDetection)
        {
            renderer.flipX = true;
        }
    }
}
