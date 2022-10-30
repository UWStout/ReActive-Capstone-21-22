using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager_Patrol : PatrolState
{
    Villager enemy;
    public Villager_Patrol(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Patrol stateData, Villager enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        if (entity.debugMode == true)
        {
            //entity.mRend.material = stateData.patrolMat;
        }
            
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void GotoNextPoint()
    {
        base.GotoNextPoint();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            //GotoNextPoint();
            enemy.stateMachine.ChangeState(enemy.roamState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
