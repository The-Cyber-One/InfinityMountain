using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : State
{

    public override void Enter()
    {
        GameEvents.InputedJump += ShootHook;
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void Exit()
    {
        GameEvents.InputedJump -= ShootHook;
    }

    void Update()
    {
        // Transition checks
        if (GetContext<PlayerMovement>().OnGround && !GetContext<PlayerMovement>().OnSlope) 
            context.TransitionTo((int)PlayerMovement.StateOptions.OnGround);
    }

    void ShootHook()
    {
        if (GetContext<PlayerMovement>().hookData.CanShootHook() && GetContext<PlayerMovement>().playerInput.HookDirection != 0)
            context.TransitionTo((int)PlayerMovement.StateOptions.ShootingHook);
    }
}
