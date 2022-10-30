using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Idle state is gonna be where the flyer looks around.
// make degrees of turning a variable that can be changed in the editor
public class Flyer_Idle : IdleState
{
    private Flyer enemy;

    private Vector3 currentEulerAngles;
    private float rotationSpeed;
    private float rotationAngle;
    bool ReachedEndOfRightRotation = false;
    float startingOrientation;
    Vector3 orientationTracker;

    //counters for number of rotations
    int rotationCounter;

    public Flyer_Idle(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Idle stateData, Flyer enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        rotationSpeed = enemy.getIdleRotationSpeed();
        rotationAngle = enemy.getIdleRotationAngle();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        rotationCounter = 0;

        currentEulerAngles = entity.transform.eulerAngles;
        Vector3 resetYOrientation = new Vector3(0, currentEulerAngles.y, 0);
        currentEulerAngles = resetYOrientation;
        entity.transform.eulerAngles = resetYOrientation;

        orientationTracker = Vector3.zero;
        startingOrientation = entity.transform.eulerAngles.y;

        //Debug.Log("starting angle: " + startingOrientation);
        //LookAround();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (rotationCounter >= 3 && entity.points.Length != 0)
        {
            enemy.stateMachine.ChangeState(enemy.patrolState);
        }
        //Debug.Log("flyer idle");
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

        LookAround();
    }

    private void LookAround()
    {
        //Vector3 relativePos = enemy.target.position - entity.transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);

        //Quaternion current = entity.transform.localRotation;

        //entity.transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime
        //    * speed);

        if(!ReachedEndOfRightRotation) // turning
        {
            currentEulerAngles += new Vector3(0, 10, 0) * Time.deltaTime * rotationSpeed;
            orientationTracker += new Vector3(0, 10, 0) * Time.deltaTime * rotationSpeed;
            entity.transform.eulerAngles = currentEulerAngles;
            //entity.stopHavingSeizure;

        }
        else
        {
            currentEulerAngles -= new Vector3(0, 10, 0) * Time.deltaTime * rotationSpeed;
            orientationTracker -= new Vector3(0, 10, 0) * Time.deltaTime * rotationSpeed;
            entity.transform.eulerAngles = currentEulerAngles;
        }

        // this keeps track if the enemy has rotated it's designated angle
        if (orientationTracker.y < -rotationAngle && ReachedEndOfRightRotation == true)
        {
            ReachedEndOfRightRotation = false;
            //readyToChangeState = true;

            //Debug.Log("rotation check");
            //Debug.Log("EndofRotation1: " + ReachedEndOfRightRotation);
            //Debug.Log("TrackerAngle1: " + orientationTracker.y);
            //Debug.Log("Target1: " + (-rotationAngle));
            rotationCounter += 1;
        }
        else if (orientationTracker.y > rotationAngle && ReachedEndOfRightRotation == false)
        {
            ReachedEndOfRightRotation = true;
            //Debug.Log("rotation Over");
            //entity.stateMachine.ChangeState(enemy.patrolState);
            //Debug.Log("EndofRotation2: " + ReachedEndOfRightRotation);
            //Debug.Log("TrackerAngle2: " + orientationTracker.y);
            //Debug.Log("Target2: " + (rotationAngle));
            rotationCounter += 1;
        }

    }
}
