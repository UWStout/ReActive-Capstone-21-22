using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Entity
{
    public Charger_Idle idleState { get; private set; }
    public Charger_Attack attackState { get; private set; }
    public Charger_Patrol patrolState { get; private set; }
    public Charger_Roam roamState { get; private set; }

    [SerializeField]
    private D_Idle idleStateData;
    [SerializeField]
    private D_Attack attackStateData;
    [SerializeField]
    private D_Patrol patrolStateData;
    [SerializeField]
    private D_Roam roamStateData;

    [SerializeField]
    private float roarAnimationDuration = 5f;

    public SoundEffectType enemyChargerAttack;
    public SoundEffectType enemyChargerRoar;

    /// <summary>
    /// sets and initializes states for the enemy
    /// </summary>
    public override void Start()
    {
        base.Start();
        idleState = new Charger_Idle(this, stateMachine, "idle", idleStateData, this);
        attackState = new Charger_Attack(this, stateMachine, "attack", attackStateData, this);
        patrolState = new Charger_Patrol(this, stateMachine, "move", patrolStateData, this);
        roamState = new Charger_Roam(this, stateMachine, "move", roamStateData, this);

        //starts the enemy in the idle state
        stateMachine.Initialize(patrolState);
    }
    public override void Update()
    {
        base.Update();
        if (this.playerIsInDialogue && (this.stateMachine.currentState != idleState))
        {
            stateMachine.ChangeState(idleState);
        }
    }

    public float getRoamAnimDuration()
    {
        return roarAnimationDuration;
    }
}
