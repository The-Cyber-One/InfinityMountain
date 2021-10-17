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
    [HideInInspector]
    public bool hookShot;
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

        //// Update hook shoot direction
        //if (!OnGround && !hookShot && (Input.GetButtonDown("Jump") || ScreenClicked))
        //{
        //    float hookDirection = Input.GetAxis("Horizontal");
        //    if (ScreenHasTouch)
        //    {
        //        hookDirection += Input.GetTouch(0).position.x > Screen.width / 2 ? 1 : -1;
        //    }

        //    if (hookDirection != 0)
        //    {
        //        // Convert the range -2 ~ 2 to either -1 or 1 on one of the axis (0 is filtered by the if statement)
        //        hookDirection = (hookDirection > 0 ? 1 : 0) * 2 - 1;

        //        GameEvents.InputedShootHook?.Invoke(hookDirection);
        //    }
        //}
    }
}
