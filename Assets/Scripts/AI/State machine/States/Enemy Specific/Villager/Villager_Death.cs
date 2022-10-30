using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager_Death : DeadState
{
    Villager enemy;
    public Villager_Death(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Dead stateData, Villager enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
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
