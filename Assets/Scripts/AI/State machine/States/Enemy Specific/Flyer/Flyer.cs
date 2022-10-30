using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : Entity
{
    [SerializeField]
    private float IdleRotationSpeed = 5f;
    [SerializeField]
    private float IdleRotationAngle = 45f;
    [SerializeField]
    private float FlySpeed = 500f;
    [SerializeField]
    private float projectileSpeed = 10f;
    [SerializeField]
    private float projectileCooldown = 2f;
    [SerializeField]
    private GameObject model = null;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawn;

    [SerializeField]private Animator flyerAnimator;

    [SerializeField]
    private AnimationToStateMachine atsm;
    

    public Flyer_Idle idleState { get; private set; }
    public Flyer_Attack attackState { get; private set; }
    public Flyer_Patrol patrolState { get; private set; }

    [SerializeField]
    private D_Idle idleStateData;
    [SerializeField]
    private D_Attack attackStateData;
    [SerializeField]
    private D_Patrol patrolStateData;

    private float rotationSpeed = 12f;

    [SerializeField] private SoundEffectType enemyFlyerShootSound;


    GameObject proj;

    public override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody>();
        idleState = new Flyer_Idle(this, stateMachine, "idle", idleStateData, this);
        attackState = new Flyer_Attack(this, stateMachine, "attack", attackStateData, this);
        patrolState = new Flyer_Patrol(this, stateMachine, "move", patrolStateData, this);

        //starts the enemy in the idle state
        stateMachine.Initialize(idleState);
        atsm.setAttackState(attackState);
    }
    public override void Update()
    {
        base.Update();
        if(this.stateMachine.currentState != idleState)
        {
            try
            {
                model.transform.eulerAngles += new Vector3(0, 0, 10) * Time.deltaTime * rotationSpeed;
            }
            catch(Exception e)
            {
                //Debug.Log("stupid unity error");
                //Debug.Log(e);
            }
        }
        if (this.playerIsInDialogue && (this.stateMachine.currentState != idleState))
        {
            stateMachine.ChangeState(idleState);
        }

    }

    public float getIdleRotationSpeed()
    {
        return IdleRotationSpeed;
    }
    public float getIdleRotationAngle()
    {
        return IdleRotationAngle;
    }
    public float getFlySpeed()
    {
        return FlySpeed;
    }
    public GameObject getModel()
    {
        return model;
    }
    public GameObject getProjectilePrefab()
    {
        return projectilePrefab;
    }
    public Transform getProjectileSpawn()
    {
        return projectileSpawn;
    }
    public float getProjectileSpeed()
    {
        return projectileSpeed;
    }
    public float getProjectileCooldown()
    {
        return projectileCooldown;
    }
    public GameObject spawnProjectile()
    {
        if(proj != null)
        {
            Destroy(proj);
        }
        proj = Instantiate(projectilePrefab, new Vector3(model.transform.position.x, model.transform.position.y, model.transform.position.z), Quaternion.identity);
        Debug.Log("shot projectile");

        RootScript.SoundManager.PlaySound(enemyFlyerShootSound, -1, transform);
                
        proj.transform.LookAt(playerPos);
        return proj;
    }
    public Animator GetAnimator(){
        return flyerAnimator;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, entityData.detectionExitRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, entityData.detectionRadius);

        Gizmos.color = Color.yellow;
        for (int i = 0; i < points.Length; i++)
        {
            if (i != points.Length - 1) // index is not at the last point
            {
                //Debug.Log(i);
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
            else // index is at the last point
            {
                Gizmos.DrawLine(points[i].position, points[0].position);
            }
        }
    }


}
