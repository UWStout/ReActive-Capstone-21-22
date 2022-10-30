using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Entity
{
    public bool stayIdle = false;
    public Villager_Idle idleState { get; private set; }
    public Villager_Attack attackState { get; private set; }
    public Villager_Patrol patrolState { get; private set; }
    public Villager_Roam roamState { get; private set; }
    public Villager_Death deadState { get; private set; }

    [SerializeField]
    private D_Idle idleStateData;
    [SerializeField]
    private D_Attack attackStateData;
    [SerializeField]
    private D_Patrol patrolStateData;
    [SerializeField]
    private D_Roam roamStateData;
    [SerializeField]
    private D_Dead deadStateData;


    public override void Start()
    {
        base.Start();
        idleState = new Villager_Idle(this, stateMachine, "idle", idleStateData, this);
        attackState = new Villager_Attack(this, stateMachine, "attack", attackStateData, this);
        patrolState = new Villager_Patrol(this, stateMachine, "move", patrolStateData, this);
        roamState = new Villager_Roam(this, stateMachine, "move", roamStateData, this);
        //deadState = new Enemy1_Dead(this, stateMachine, "dead", deadStateData, this);

        //starts the enemy in the idle state
        if(stayIdle == true)
        {
            stateMachine.Initialize(idleState);
        }
        else
        {
            stateMachine.Initialize(roamState);
        }
    }
    public override void Update()
    {
        base.Update();
        if (this.playerIsInDialogue && (this.stateMachine.currentState != idleState))
        {
            stateMachine.ChangeState(idleState);
        }
    }
}
