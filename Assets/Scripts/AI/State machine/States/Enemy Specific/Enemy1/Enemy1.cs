using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public Enemy1_Idle idleState { get; private set; }
    public Enemy1_Attack attackState { get; private set; }
    public Enemy1_Patrol patrolState { get; private set; }
    public Enemy1_Roam roamState { get; private set; }
    public Enemy1_Dead deadState { get; private set; }

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

    private const string ATTACK_BOOL="isAttack";

    [SerializeField] private SoundEffectType enemyOneAttack;
    public bool wantsToAttack;

    /// <summary>
    /// sets and initializes states for the enemy
    /// </summary>
    public override void Start()
    {
        base.Start();
        idleState = new Enemy1_Idle(this, stateMachine, "idle", idleStateData, this);
        attackState = new Enemy1_Attack(this, stateMachine, "attack", attackStateData, this);
        patrolState = new Enemy1_Patrol(this, stateMachine, "move", patrolStateData, this);
        roamState = new Enemy1_Roam(this, stateMachine, "move", roamStateData, this);
        wantsToAttack = false;

        //starts the enemy in the idle state
        stateMachine.Initialize(patrolState);
    }
    public override void Update()
    {
        base.Update();
        if(this.playerIsInDialogue && (this.stateMachine.currentState != idleState))
        {
            stateMachine.ChangeState(idleState);
        }
    }
    public void SetAnimBool(string name, bool value){
        anim.SetBool(name,value);
    }

    void OnTriggerEnter(Collider playerCol)
    {
        if (playerCol.gameObject.tag.Equals("Player"))
        {
            SetAnimBool(ATTACK_BOOL, true);
            SetAnimBool("isWalk", false);
            wantsToAttack = true;
            RootScript.SoundManager.PlaySound(enemyOneAttack, -1, transform);
        }
    }

    void OnTriggerExit(Collider playerCol)
    {
        if (playerCol.gameObject.tag.Equals("Player"))
        {
            SetAnimBool(ATTACK_BOOL, false);
            wantsToAttack = false;
        }
    }
}
