using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager_Idle : IdleState
{
    Villager enemy;
    private NavMeshAgent agent;
    public Villager_Idle(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Idle stateData, Villager enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        agent = entity.GetComponent<NavMeshAgent>();
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
            entity.mRend.material = stateData.idleMat;
        }
            
        agent.ResetPath();
        isIdleTimeOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isIdleTimeOver && !enemy.stayIdle && !entity.playerIsInDialogue)
        {
            //Debug.Log("changing to last state");
            stateMachine.ChangeState(enemy.roamState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
