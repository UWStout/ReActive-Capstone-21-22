using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    protected D_Patrol stateData;
    private int destPoint = 0;
    protected NavMeshAgent agent;
    protected bool keepPatroling;

    /// <summary>
    /// Constructor for the Patrol State
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="stateMachine"></param>
    /// <param name="animBoolName"></param>
    /// <param name="stateData"></param>
    public PatrolState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Patrol stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        agent = entity.GetComponent<NavMeshAgent>();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        agent.autoBraking = false;
        keepPatroling = true;
        GotoNextPoint();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
            //keepPatroling = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public virtual void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (entity.points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = entity.points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % entity.points.Length;
    }
    public virtual int getCurrentDestPoint()
    {
        return destPoint;
    }
}
