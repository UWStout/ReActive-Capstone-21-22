using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_Dead : DeadState
{
    public Enemy1_Dead(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Dead stateData) : base(entity, stateMachine, animBoolName, stateData)
    {

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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
