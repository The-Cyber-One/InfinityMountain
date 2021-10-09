using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float wallSide = 0;
    public bool canJump = false;
    public bool hookShot;
    public float MovementDirection { get; private set; } = 0;
    public bool OnWall
    {
        get { return _onWall; }
        set
        {
            _onWall = value;
            if (value) canJump = true;
        }
    }
    public bool ContactWithGround
    {
        get { return _onGround; }
        private set
        {
            _onGround = value;
            if (value) canJump = true;
        }
    }

    [SerializeField]
    [Range(0, 180)]
    float minRotation = 5, maxRotation = 30;

    bool _onWall = false;
    bool _onGround = false;
    float mobileVelocity;

    bool ScreenHasTouch { get { return Input.touchCount > 0; } }
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
        // Reactivate hook
        if (ContactWithGround)
        {
            hookShot = false;
        }

        // Movement direction
        bool rotateRight = Input.acceleration.x < 0;
        mobileVelocity = Mathf.InverseLerp(minRotation / 180, maxRotation / 180, Mathf.Abs(Input.acceleration.x));
        if (rotateRight) mobileVelocity *= -1;

        MovementDirection = Input.GetAxis("Horizontal") + mobileVelocity;

        // Jump input
        if ((ContactWithGround || OnWall) && (Input.GetButtonDown("Jump") || ScreenClicked))
        {
            GameEvents.InputedJump?.Invoke();
            wallSide = 0;
            OnWall = false;
            hookShot = false;
        }

        // Update hook shoot direction
        if (!ContactWithGround && !hookShot && (Input.GetButtonDown("Jump") || ScreenClicked))
        {
            float hookDirection = Input.GetAxis("Horizontal");
            if (ScreenHasTouch)
            {
                hookDirection += Input.GetTouch(0).position.x > Screen.width / 2 ? 1 : -1;
                Debug.Log($"{Screen.width} < {Input.GetTouch(0).position.x} = {hookDirection}");
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
