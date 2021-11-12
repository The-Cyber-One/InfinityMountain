using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LavaAttackState))]
[RequireComponent(typeof(CrystalAttackState))]
[RequireComponent(typeof(VulnerableState))]
public class BossManager : StateMachine
{
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
    }

    public void NextState()
    {

    }
}
