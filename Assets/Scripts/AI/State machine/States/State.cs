using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    public float startTime { get; protected set; }
    protected string animBoolName;//use for animations

    /// <summary>
    /// Constructor for State, contains entity which refers to the enemy game object, statemachine which handles switching between states,
    /// and animBoolName, wich contains the string of which animation to use for the current state
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="stateMachine"></param>
    /// <param name="animBoolName"></param>
    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    /// <summary>
    /// executes code upon entering the state
    /// </summary>
    public virtual void Enter()
    {
        startTime = Time.time;
        //entity.anim.SetBool(animBoolName, true);
        DoChecks();
    }

    /// <summary>
    /// executes code upon exiting the state
    /// </summary>
    public virtual void Exit()
    {
        //entity.anim.SetBool(animBoolName, false);
    }

    /// <summary>
    /// Keeps track of checks for changing states
    /// </summary>
    public virtual void LogicUpdate()
    {

    }

    /// <summary>
    /// keeps track of in-game checks
    /// </summary>
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    /// <summary>
    /// used to check other logic based variables
    /// </summary>
    public virtual void DoChecks()
    {

    }
}