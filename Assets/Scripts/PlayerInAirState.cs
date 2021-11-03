using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : State
{

    public override void Enter()
    {
        GameEvents.InputedJump += ShootHook;
        GetContext<PlayerMovement>().rigidbody.gravityScale = 1;
    }

    public override void Exit()
    {
        GameEvents.InputedJump -= ShootHook;
        GetContext<PlayerMovement>().playerInput.ResetMovementInput();
    }

    void Update()
    {
        // Transition checks
        if (GetContext<PlayerMovement>().OnGround && !GetContext<PlayerMovement>().OnSlope) 
            context.TransitionTo((int)PlayerMovement.StateOptions.OnGround);
    }

    void ShootHook()
    {
        if (GetContext<PlayerMovement>().hookData.CanShootHook())
            context.TransitionTo((int)PlayerMovement.StateOptions.ShootingHook);
    }
}
