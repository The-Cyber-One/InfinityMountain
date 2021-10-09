using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateMachine context;

    /// <summary>
    /// Sets the statemachine context to this state
    /// </summary>
    /// <param name="context"></param>
    public void SetContext(StateMachine context)
    {
        this.context = context;
    }

    /// <summary>
    /// Enter is called when this state is activated
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// Exit is called when this state is deactivated
    /// </summary>
    public abstract void Exit();
}
