using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager_Roam : RoamState
{
    Villager enemy;
    public Villager_Roam(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Roam stateData, Villager enemy) : base(entity, stateMachine, animBoolName, stateData)
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
            enemy.mRend.material = stateData.roamMat;
        }  
        backToIdle = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void GenerateNewDestination()
    {
        base.GenerateNewDestination();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (pointCount > 3)
        {
            //Debug.Log(pointCount);
            //Debug.Log("Changing to patrol state");
            stateMachine.ChangeState(enemy.patrolState);
        }
        if (backToIdle == true)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
