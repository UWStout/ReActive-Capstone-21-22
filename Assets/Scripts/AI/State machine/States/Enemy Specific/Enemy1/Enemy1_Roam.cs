using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_Roam : RoamState
{
    private Enemy1 enemy;

    private const string WALK_BOOL="isWalk";
    //private bool endState;
    private float playerDistance;
    //LineRenderer renderer = new LineRenderer();

    /// <summary>
    /// Constructor for enemy1 roam state
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="stateMachine"></param>
    /// <param name="animBoolName"></param>
    /// <param name="stateData"></param>
    /// <param name="enemy"></param>
    public Enemy1_Roam(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Roam stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        enemy.SetAnimBool(WALK_BOOL,true);
        
        //endState = false;
        if (entity.debugMode == true)
        {
            entity.mRend.material = stateData.roamMat;
        }
        backToIdle = false;
    }

    public override void Exit()
    {
        enemy.SetAnimBool(WALK_BOOL,false);

        base.Exit();
    }

    public override void GenerateNewDestination()
    {
        base.GenerateNewDestination();
    }

    public override void LogicUpdate()
    {
        //entity.anim.SetTrigger("walk");

        base.LogicUpdate();
        if(pointCount>3)
        {
            //Debug.Log(pointCount);
            //Debug.Log("Changing to patrol state");
            stateMachine.ChangeState(enemy.patrolState);
        }
        if(backToIdle == true)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
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
}
