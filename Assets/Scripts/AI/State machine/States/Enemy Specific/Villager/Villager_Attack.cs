using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager_Attack : AttackState
{
    Villager enemy;
    public Villager_Attack(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Attack stateData, Villager enemy) : base(entity, stateMachine, animBoolName, stateData)
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

    public override void GoToPlayer()
    {
        base.GoToPlayer();
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
