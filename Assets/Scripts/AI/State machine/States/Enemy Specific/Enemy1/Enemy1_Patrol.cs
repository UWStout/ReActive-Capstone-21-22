using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_Patrol : PatrolState
{
    private Enemy1 enemy;
    private const string WALK_BOOL="isWalk";

    //private bool endState;
    private float playerDistance;

    /// <summary>
    /// Constructor for the enemy 1 patrol state
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="stateMachine"></param>
    /// <param name="animBoolName"></param>
    /// <param name="stateData"></param>
    /// <param name="enemy"></param>
    public Enemy1_Patrol(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Patrol stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        enemy.SetAnimBool(WALK_BOOL,true);
        //endState = false;
        if (entity.debugMode == true)
        {
            entity.mRend.material = stateData.patrolMat;
        }
            
    }

    public override void Exit()
    {
        enemy.SetAnimBool(WALK_BOOL,false);

        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            //GotoNextPoint();
            enemy.stateMachine.ChangeState(enemy.roamState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        playerDistance = Vector3.Distance(entity.entityPos, entity.playerPos);
        //Debug.Log("distance = " + playerDistance);

        if ((playerDistance <= entity.detectionRadius || entity.gameObject.GetComponent<AISensor>().GetSpottedStatus()) && !entity.playerIsInDialogue)
        {
            //Debug.Log("player detected");
            stateMachine.ChangeState(enemy.attackState);
        }
    }
    public override void GotoNextPoint()
    {
        base.GotoNextPoint();
        
    }
}
