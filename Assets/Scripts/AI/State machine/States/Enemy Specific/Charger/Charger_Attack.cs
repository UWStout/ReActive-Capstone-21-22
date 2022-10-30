using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger_Attack : AttackState
{
    Charger enemy;
    float markedTime;
    bool pauseAttacking;
    float cooldownTime;
    private float startTime;
    private float rStartTime;
   [SerializeField] private const float  dur =5f;


    private bool isInTimer=false;
    Rigidbody playerRigidbody;
    Vector3 explosionPos;
    Vector3 knockBackVector;
    float knockBackForce;
    float knockBackTime;
    float knockBackCounter;
    float playerDistance;

    float originalSpeed;
    float originalAccel;

    bool isRoaringTimerBool;

    private const string ATTACK="Attack";

    private const string ROAR="Roar";

    private bool isRoar;
    private bool hasRoared;

    private float closeEnoughToPlayer=1.0f;
    
    private float roarDur;

    



    public Charger_Attack(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Attack stateData, Charger enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        cooldownTime = stateData.AttackCooldownTime;
        playerRigidbody = entity.player.GetComponent<Rigidbody>();

        roarDur = enemy.getRoamAnimDuration();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        isRoar=true;
        isRoaringTimerBool = true;
        hasRoared = false;

        //base.Enter();
        //Debug.Log("entered attack state");

        //Enter move anim

        //add roar here 
       // agent.SetDestination(entity.transform.position);//set the entity to be the current pos,
        agent.ResetPath();
        //entity.RoarLogic(roarDur,ROAR);
        //entity.anim.SetTrigger(ROAR);


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
        base.Exit();
        //isRoar=true;

        agent.speed = originalSpeed;
        agent.acceleration = originalAccel;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    private float CheckDistance(){
        return Vector3.Distance(entity.playerPos,enemy.gameObject.transform.position);
    }
   
    
    public override void PhysicsUpdate()
    {
        
        Roar();
        

         //make a function that checks if the enemy is close, but not touching
        if(entity.GetIsCloseToPlayer() && !entity.GetIsInAttackAnimation() ){
            //play roar then attack
            entity.StartAttackCoroutine(dur,ATTACK);
            
            //pauseAttacking=true;
        }
        
        // then enter attack anim
        // hiccup might be that physcis update is called every frame, idk?

        if (entity.collisionWithPlayer)
        {
            pauseAttacking = true;
            startTime = Time.time;
            agent.ResetPath();
        }
        if (Time.time > startTime + cooldownTime)
        {
            pauseAttacking = false;
        }
        if (!entity.collisionWithPlayer && !pauseAttacking && !isRoar)
        {
            GoToPlayer();
            //Debug.Log("moving to player");
        }
        if (entity.playerIsInDialogue)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }

        //check for exit state:
        // check for exiting state
        playerDistance = Vector3.Distance(entity.entityPos, entity.playerPos);
        //Debug.Log("distance = " + playerDistance);

        if (playerDistance > entity.exitDetectionRadius && !entity.gameObject.GetComponent<AISensor>().GetSpottedStatus())
        {
            Debug.Log("Entering Roam State");
            stateMachine.ChangeState(enemy.roamState);
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

    private void Roar()
    {
        //Debug.Log("the value of is roar is "+ isRoar.ToString());
        // if this is true, commence the timer
        if(isRoaringTimerBool && !hasRoared)
        {
            rStartTime = Time.time;
            isRoaringTimerBool = false;
            hasRoared = true;

            RootScript.SoundManager.PlaySound(enemy.enemyChargerRoar, -1, enemy.transform);
        }


        if(Time.time < rStartTime + roarDur)
        {
//            Debug.Log("inside of timer");
            isRoar=true;
            entity.transform.LookAt(entity.playerPos);
            // begin roar animation, gets called every frame so make a boolean
            //entity.anim.SetTrigger(ROAR);
            entity.anim.SetBool("CanRoar", true);
        }
        else if(!isRoaringTimerBool)
        {
//            Debug.Log("timer has ended");

            //logic for "go to player"
            // go to player should get called every frame though

            
            isRoar=false;
            //this gets set to false at the very end
            isRoaringTimerBool = true;
            entity.anim.SetBool("CanRoar", false);
        }
    }
}
