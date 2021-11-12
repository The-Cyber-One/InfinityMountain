using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LavaAttackState))]
[RequireComponent(typeof(CrystalAttackState))]
[RequireComponent(typeof(VulnerableState))]
public class BossManager : StateMachine
{
    public int health;
    [SerializeField]
    int maxHealth = 3;

    public enum StateOptions
    {
        LavaAttack,
        CrystalAttack,
        Vulnerable
    }

    void Start()
    {
        states.Add((int)StateOptions.LavaAttack, GetComponent<LavaAttackState>());
        states.Add((int)StateOptions.CrystalAttack, GetComponent<CrystalAttackState>());
        states.Add((int)StateOptions.Vulnerable, GetComponent<VulnerableState>());

        StateMachineSetup((int)StateOptions.LavaAttack);

        health = maxHealth;
    }


    public void NextState()
    {
        int state = UnityEngine.Random.Range(0, Enum.GetValues(typeof(StateOptions)).Length);
        TransitionTo(state);
    }
}
