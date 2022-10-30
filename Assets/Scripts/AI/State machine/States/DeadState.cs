using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    public D_Dead stateData;
    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Dead stateData) : base(entity, stateMachine, animBoolName)
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //if (Time.time > deathStartTime + stateData.deathAnimTime)
        //{
        //    entity.gameObject.SetActive(false);
        //    Debug.Log("enemy has died");
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
