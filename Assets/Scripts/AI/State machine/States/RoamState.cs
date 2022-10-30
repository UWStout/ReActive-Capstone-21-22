using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamState : State
{
    protected D_Roam stateData;
    private NavMeshAgent agent;
    protected bool keepRoaming;
    protected int pointCount; // used to count how many times the AI has traveled to random points
    protected bool backToIdle;
    /// <summary>
    /// Constructor for Roam State
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="stateMachine"></param>
    /// <param name="animBoolName"></param>
    /// <param name="stateData"></param>
    public RoamState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Roam stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        agent = entity.GetComponent<NavMeshAgent>();
        keepRoaming = true;
        //Debug.Log("roam init");
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        keepRoaming = true;
        //Debug.Log(pointCount);
        if(pointCount > 4)
        {
            pointCount = 0;
        }
        GenerateNewDestination();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //If the AI is within range, add to pointCount and assign new Destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            pointCount++;
            //Debug.Log("point reached");
            backToIdle = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    //Generates a new random destination for the AI
    public virtual void GenerateNewDestination()
    {
        // Returns if no points have been set up
        if (!keepRoaming)
            return;

        //TODO: create wireframe to show walk radius
        //gets a random direction within a radius around the entity
        Vector3 randomDirection = Random.insideUnitSphere * stateData.walkRadius;
        randomDirection += entity.entityPos; // adds the vector to the entity's position

        NavMeshHit hit;
        // assigns a NavMeshHit using the randomDirection
        NavMesh.SamplePosition(randomDirection, out hit, stateData.walkRadius, 1);
        Vector3 finalPosition = hit.position; 
        agent.SetDestination(finalPosition);
        //keepRoaming = false
        
    }
}
