using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger_Patrol : PatrolState
{
    private Charger enemy;
    //private bool endState;
    private float playerDistance;

    private const string WALK="Walk";

    /// <summary>
    /// Constructor for the enemy 1 patrol state
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="stateMachine"></param>
    /// <param name="animBoolName"></param>
    /// <param name="stateData"></param>
    /// <param name="enemy"></param>
    public Charger_Patrol(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Patrol stateData, Charger enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        entity.anim.SetTrigger(WALK);
        //entity.anim.SetBool("notAttack",true);
        //endState = false;
        if (entity.debugMode == true)
        {
            entity.mRend.material = stateData.patrolMat;
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        //base.LogicUpdate();
        //if (keepPatroling)
        //{
        //    //enemy.stateMachine.ChangeState(enemy.roamState);
        //}
        //else 
        //{
        //    enemy.stateMachine.ChangeState(enemy.roamState);
        //}
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
