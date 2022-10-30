using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1_Idle : IdleState
{
    private Enemy1 enemy;
    private float playerDistance;
    private NavMeshAgent agent;

    private const string IDLE_BOOL="isIdle";

    public Enemy1_Idle(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Idle stateData, Enemy1 enemy) : base(enemy, stateMachine, animBoolName, stateData)
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
        enemy.SetAnimBool(IDLE_BOOL,true);
        if (entity.debugMode == true)
        {
            entity.mRend.material = stateData.idleMat;
        }
        agent.ResetPath();
        isIdleTimeOver = false;
    }

    public override void Exit()
    {
        enemy.SetAnimBool(IDLE_BOOL,false);

        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isIdleTimeOver && !entity.playerIsInDialogue)
        {
            //Debug.Log("changing to last state");
            stateMachine.ChangeState(enemy.roamState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        playerDistance = Vector3.Distance(entity.entityPos, entity.playerPos);

        if ((playerDistance <= entity.detectionRadius || entity.gameObject.GetComponent<AISensor>().GetSpottedStatus()) && !entity.playerIsInDialogue)
        {
            //Debug.Log("player detected");
            stateMachine.ChangeState(enemy.attackState);
        }
    }
}
