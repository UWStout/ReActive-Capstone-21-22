using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer_Attack : AttackState
{
    Flyer enemy;

    bool isWaiting = false;
    bool hasShot = false;
    float cooldownTime;

    GameObject projectile;
    float projectileSpeed;
    private Animator flyerAnimator;

    public Flyer_Attack(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Attack stateData, Flyer enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        cooldownTime = enemy.getProjectileCooldown();
        projectileSpeed = enemy.getProjectileSpeed();
        flyerAnimator=enemy.GetAnimator();
    }


    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        //base.Enter();
        //Debug.Log("I am in attack");
        hasShot = true;
    }

    public override void Exit()
    {
        base.Exit();
        projectile.GetComponent<KnockBackTest>().DeleteProjectile();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Timer goes brrrrrrrrrrr
        if(isWaiting)
        {
            startTime = Time.time;
            isWaiting = false;
        }

        if (Time.time > cooldownTime + startTime)
        {
            hasShot = false;
        }

        if (!hasShot && !isWaiting)
        {
            //Debug.Log("shot");
            StartShootAnimation();
            hasShot = true;
            isWaiting = true;
        }

        if (entity.playerIsInDialogue)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        var rotation = Quaternion.LookRotation(entity.playerPos - enemy.transform.position);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * 2f);

        float playerDistance = Vector3.Distance(entity.entityPos, entity.playerPos);
        if (playerDistance > entity.exitDetectionRadius)
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        // Projectile moves towards player's last known position
        if(projectile != null)
        {
            projectile.transform.position += projectile.transform.forward * projectileSpeed * Time.deltaTime;
        }

    }

    public void StartShootAnimation()
    {
        flyerAnimator.SetTrigger("Attack");
    }

    public void Shoot()
    {
        // istantiate prefab
        //GameObject proj = Instantiate()
        
        projectile = enemy.spawnProjectile();

    }

    public override void TriggerAttack()
    {
        Shoot();
    }
}
