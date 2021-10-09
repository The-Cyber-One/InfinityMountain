using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerOnGroundState))]
[RequireComponent(typeof(PlayerInAirState))]
[RequireComponent(typeof(ShootingHookState))]
[RequireComponent(typeof(PlayerOnWallState))]
public class PlayerMovement : StateMachine
{
    public enum StateOptions
    {
        OnGround,
        InAir,
        ShootingHook,
        OnWall
    }
    public StateOptions currentState = StateOptions.OnGround;

    public Rigidbody2D rigidbody;
    public PlayerInput playerInput;

    private void Start()
    {
        states.Add((int)StateOptions.OnGround, GetComponent<PlayerOnGroundState>());
        states.Add((int)StateOptions.InAir, GetComponent<PlayerInAirState>());
        states.Add((int)StateOptions.ShootingHook, GetComponent<ShootingHookState>());
        states.Add((int)StateOptions.OnWall, GetComponent<PlayerOnWallState>());

        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        StateMachineSetup((int)StateOptions.OnGround);
    }
}
