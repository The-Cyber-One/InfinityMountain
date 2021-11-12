using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LavaAttackState))]
[RequireComponent(typeof(CrystalAttackState))]
[RequireComponent(typeof(VulnerableState))]
public class BossManager : StateMachine
{
    public int health;

    [SerializeField]
    int maxHealth = 3;
    [SerializeField]
    ParticleSystem particleSystem;
    [SerializeField]
    float deathParticleTime = 2f, transitionDelay = 0.5f;

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

    void Update()
    {
        if (health <= 0)
        {
            StopCurrentState();
            enabled = false;
            particleSystem.Play();
            StartCoroutine(LoadNextScene());
            Debug.Log("Won");
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(deathParticleTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void NextState()
    {
        int state = UnityEngine.Random.Range(0, Enum.GetValues(typeof(StateOptions)).Length);
        Debug.Log($"Entering {state}. Boss health: {health}");
        StartCoroutine(DelayedTransition(state));
    }

    IEnumerator DelayedTransition(int state)
    {
        yield return new WaitForSecondsRealtime(transitionDelay);
        TransitionTo(state);
    }
}
