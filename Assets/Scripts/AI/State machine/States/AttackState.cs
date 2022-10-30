using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    protected D_Attack stateData;
    //protected GameObject player;
    //private Vector3 playerPos;
    protected NavMeshAgent agent;
    protected bool reachedPlayer;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Attack stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        agent = entity.GetComponent<NavMeshAgent>();
        reachedPlayer = false;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        //agent.SetDestination(entity.playerPos);
        reachedPlayer = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //updates position every frame
        //entity.playerPos = entity.player.transform.position;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //if (agent.remainingDistance < 0.5f)
        //{
        //    //Debug.Log("attackPlayer");
        //}
        //GoToPlayer();
    }

    public virtual void GoToPlayer()
    {
        reachedPlayer = false;
        //Debug.Log("set player position as destination");
        agent.SetDestination(entity.playerPos);
    }

    public virtual void TriggerAttack()
    {
    }

    public virtual void FinishAttack()
    {

    }
}
