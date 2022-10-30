using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public State currentState { get; private set; }
    public State lastState { get; private set; }

    /// <summary>
    /// Initialize is used to pick the starting state
    /// </summary>
    public void Initialize(State startingState)
    {
        currentState = startingState;
        lastState = startingState;
        currentState.Enter();
    }

    /// <summary>
    /// ChangeState is used to change the current state
    /// </summary>
    public void ChangeState(State newState)
    {
        lastState = currentState;
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
