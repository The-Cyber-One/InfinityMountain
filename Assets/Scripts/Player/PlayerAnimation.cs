using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Rigidbody2D rigidbody;

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
        animator.SetBool("onSlope", value);
    }

    void Update()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rigidbody.velocity.x));
        
    }
}
