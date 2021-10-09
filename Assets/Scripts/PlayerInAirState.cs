using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : State
{

    public override void Enter()
    {
        GameEvents.InputedJump += ShootHook;
    }

    public override void Exit()
    {
        GameEvents.InputedJump -= ShootHook;
    }

    void Update()
    {
        // Transition checks
        if (GetContext<PlayerMovement>().playerInput.OnGround) 
            context.TransitionTo((int)PlayerMovement.StateOptions.OnGround);
    }

    void ShootHook()
    {
        if (!GetContext<PlayerMovement>().playerInput.hookShot)
            context.TransitionTo((int)PlayerMovement.StateOptions.ShootingHook);
    }
}
