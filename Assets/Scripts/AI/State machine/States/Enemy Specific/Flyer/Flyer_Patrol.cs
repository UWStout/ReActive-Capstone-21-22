using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer_Patrol : PatrolState
{
    private Flyer enemy;
    //private bool endState;
    private Rigidbody rigidbody;
    private int destPoint;
    Vector3 target;
    Vector3 moveDirection;
    private float moveSpeed;
    private bool isRotating;
    private float rotationSpeed = 2f;
    

    public Flyer_Patrol(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Patrol stateData, Flyer enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        rigidbody = enemy.GetComponent<Rigidbody>();
        // moveSpeed = 3f;
        destPoint = 0;
        moveSpeed = enemy.getFlySpeed();
        //Debug.Log("flyspeed: " + moveSpeed);
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        //base.Enter();

        // once rotation is over, enable boolean to start moving to point
        GotoNextPoint();

    }

    public override void Exit()
    {
        base.Exit();
        rigidbody.velocity = Vector3.zero;
    }

    public override void GotoNextPoint()
    {
        isRotating = true;
        
        try
        {
            target = entity.points[destPoint].position;
            destPoint = (destPoint + 1) % entity.points.Length;
        }
        catch(Exception e)
        {
            // array is empty
            //Debug.Log(e);
        }
    }

    public override void LogicUpdate()
    {
        if(isRotating == true)
        {
            rigidbody.velocity = Vector3.zero;
            var rotation = Quaternion.LookRotation(target - enemy.transform.position);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            
            //checks if enemy is looking at the target
            Vector3 checkRotation = (target - enemy.transform.position).normalized;
            float dotProd = Vector3.Dot(checkRotation, enemy.transform.forward);

            if (dotProd > 0.95)
            {
                // ObjA is looking mostly towards ObjB
                isRotating = false;
            }
        }
        else
        {
            moveDirection = target - entity.transform.position;
            rigidbody.velocity = moveDirection.normalized * moveSpeed;
        }

        //check if they have reached the point
        if (getRemainingDistance() < 0.5f)
        {
            //Debug.Log("reached pos");
            enemy.stateMachine.ChangeState(enemy.idleState);

            //GotoNextPoint();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        float playerDistance = Vector3.Distance(entity.entityPos, entity.playerPos);

        if (playerDistance <= entity.detectionRadius && !entity.playerIsInDialogue)
        {
            //Debug.Log("player detected");
            stateMachine.ChangeState(enemy.attackState);
        }
    }
    private float getRemainingDistance()
    {
        float remainingDistance = Vector3.Distance(entity.transform.position, target);
        return remainingDistance;
    }
}
