using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_Idle stateData;
    protected float idleTime;
    protected bool isIdleTimeOver;

    /// <summary>
    /// Constructor for IdleState
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="stateMachine"></param>
    /// <param name="animBoolName"></param>
    /// <param name="stateData"></param>
    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Idle stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        //Enter idle animation here

        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();
        // exit idle animation
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void SetRandomIdleTime()
    {
        //random time for total idle time between min and max
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
