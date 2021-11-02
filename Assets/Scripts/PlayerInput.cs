using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector]
    public float wallSide = 0;
    [HideInInspector]
    public bool onWall;
    public float MovementDirection { get; private set; } = 0;
    public float HookDirection { get; private set; } = 0;

    [SerializeField]
    [Range(0, 180)]
    float minRotation = 5, maxRotation = 30;
    float mobileVelocity;

    public bool ScreenHasTouch { get { return Input.touchCount > 0; } }
    bool ScreenClicked
    {
        get
        {
            if (!ScreenHasTouch) return false;
            return Input.GetTouch(0).phase == TouchPhase.Began;
        }
    }

    void Update()
    {
        // Movement direction
        bool rotateRight = Input.acceleration.x < 0;
        mobileVelocity = Mathf.InverseLerp(minRotation / 180, maxRotation / 180, Mathf.Abs(Input.acceleration.x));
        if (rotateRight) mobileVelocity *= -1;

        MovementDirection = Input.GetAxis("Horizontal") + mobileVelocity;

        // Jump input
        if (Input.GetButtonDown("Jump") || ScreenClicked)
        {
            HookDirection = Input.GetAxis("Horizontal");
            if (ScreenHasTouch)
            {
                HookDirection += Input.GetTouch(0).position.x > Screen.width / 2 ? 1 : -1;
            }

            if (HookDirection != 0)
            {
                // Convert the range -2 ~ 2 to either -1 or 1 on one of the axis (0 is filtered by the if statement)
                HookDirection = (HookDirection > 0 ? 1 : 0) * 2 - 1;
            }

            onWall = false;

            GameEvents.InputedJump?.Invoke();
        }
    }
}
