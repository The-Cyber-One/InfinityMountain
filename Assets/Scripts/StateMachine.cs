using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    State currentState = null;
    public Dictionary<int, State> states = new Dictionary<int, State>();

    /// <summary>
    /// Set all states to inactive and activate first state
    /// </summary>
    /// <param name="startStateId"> Index to pick the first state from </param>
    protected void StateMachineSetup(int startStateId = 0)
    {
        foreach (State state in states.Values)
        {
            state.enabled = false;
        }
        TransitionTo(startStateId);
    }

    public void StopCurrentState()
    {
        currentState.enabled = false;
    }

    /// <summary>
    /// Change state and call the End and Start methods
    /// </summary>
    /// <param name="stateId"> New state id to change to </param>
    public void TransitionTo(int stateId)
    {
        if (currentState != null)
        {
            currentState.Exit();
            currentState.enabled = false;
        }
        currentState = states[stateId];

        currentState.SetContext(this);
        currentState.Enter();
        currentState.enabled = true;
    }
}
