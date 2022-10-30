using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charger_Idle : IdleState
{
    private Charger enemy;
    private float playerDistance;
    private NavMeshAgent agent;

    private const string IDLE="Idle";

    public Charger_Idle(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Idle stateData, Charger enemy) : base(enemy, stateMachine, animBoolName, stateData)
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
        entity.anim.SetTrigger(IDLE);//set walk trigger maybe? 
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

