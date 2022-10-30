using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1_Attack : AttackState
{
    Enemy1 enemy;
    float markedTime;
    bool pauseAttacking;
    float playerDistance;

    float originalSpeed;
    float originalAccel;

    public Enemy1_Attack(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Attack stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        //enemy.SetAnimBool(ATTACK_BOOL,true);
        base.Enter();
        pauseAttacking = false;
        //Debug.Log("entered attack state");
        
        //Enter move anim

        if (entity.debugMode == true)
        {
            entity.mRend.material = stateData.attackMaterial;
        }

        originalAccel = agent.acceleration;
        originalSpeed = agent.speed;

        agent.speed = stateData.modifiedSpeed;
        agent.acceleration = stateData.modifiedAccel;
    }

    public override void Exit()
    {
        //enemy.SetAnimBool(ATTACK_BOOL,false);

        base.Exit();

        agent.speed = originalSpeed;
        agent.acceleration = originalAccel;
        //Debug.Log("exiting attack state");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(entity.gameObject.GetComponent<NavMeshAgent>().isStopped)
        {
            enemy.SetAnimBool("isIdle", true);
            enemy.SetAnimBool("isWalk", false);
        }
        else if(!enemy.wantsToAttack)
        {
            enemy.SetAnimBool("isIdle", false);
            enemy.SetAnimBool("isWalk", true);

        }
    }

    public override void PhysicsUpdate()
    {
        // make a function that checks if the enemy is close, but not touching
        // then enter attack anim
        // hiccup might be that physcis update is called every frame, idk?

        if(entity.collisionWithPlayer)
        {
            pauseAttacking = true;
            markedTime = Time.time;
            //agent.ResetPath();
            //agent.SetDestination(enemy.transform.position);
            entity.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            //Debug.Log("start time: " + Time.time);
            //Debug.Log("target time: " + (Time.time + 10f));
        }
        if ((Time.time > (markedTime + stateData.AttackCooldownTime)))
        {
            pauseAttacking = false;
            Debug.Log("setting target");
            entity.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }
        if (!entity.collisionWithPlayer && !pauseAttacking)
        {
            GoToPlayer();
        }

        if(entity.playerIsInDialogue)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }

        //check for exit state:
        // check for exiting state
        playerDistance = Vector3.Distance(entity.entityPos, entity.playerPos);
        //Debug.Log("distance = " + playerDistance);

        if (playerDistance > entity.exitDetectionRadius && !entity.gameObject.GetComponent<AISensor>().GetSpottedStatus())
        {
            //Debug.Log("player detected roam");
            stateMachine.ChangeState(enemy.idleState);
        }
    }
    public override void GoToPlayer()
    {
        //if(!pauseAttacking)
        //{
        //    base.GoToPlayer();
        //}
        base.GoToPlayer();
    }
}
