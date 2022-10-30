using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public D_Entity entityData; //base data for entity

    public bool debugMode = false;

    [HideInInspector]
    public FiniteStateMachine stateMachine { get; private set; } //deals with switching states, for animations
    [HideInInspector]
    public GameObject player { get; private set; }
    [HideInInspector]
    public Vector3 playerPos { get; private set; }
    [HideInInspector]
    public Vector3 entityPos { get; private set; }
    [HideInInspector]
    public float detectionRadius { get; private set; }
    [HideInInspector]
    public float exitDetectionRadius;

    public Transform[] points;

    //animator for enemy
    public Animator anim { get; private set; } //THIS IS NOW DONE THROUGH EDITOR

    [HideInInspector]
    public bool collisionWithPlayer = false;

    //public Animator anim;
    public MeshRenderer mRend { get; private set; }

    private bool isInAttackAnim=false;

    //private GameObject dmGameObject;
    private DialogueManager dialogueManager;

    public bool playerIsInDialogue;


    //virtual allows it to be overwritten in case of inheritence

    /// <summary>
    /// creates variables used at the start of its construction
    /// </summary>
    public virtual void Start()
    {
        entityPos = transform.position;
        exitDetectionRadius = entityData.detectionExitRadius;

        //connects the AnimationToStateMachine class to the game object
        //atsm = this.GetComponent<AnimationToStateMachine>();

        //connects renderer
        anim=GetComponent<Animator>();

        //sets up the state machine for this enemy
        stateMachine = new FiniteStateMachine();

        player = GameObject.FindGameObjectWithTag("Player");

        playerPos = player.transform.position;

        detectionRadius = entityData.detectionRadius;

        // dmGameObject = GameObject.FindGameObjectWithTag("DialogueManager");
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        playerIsInDialogue = false;
    }
    public bool GetIsInAttackAnimation(){
        return isInAttackAnim;
    }
    private IEnumerator EntityAttack(float durr, string attackAnimName){
        isInAttackAnim=true;
        anim.SetTrigger(attackAnimName);
        Debug.Log("Playing attack animation for "+ durr+ " seconds");
        

        yield return new WaitForSeconds(durr);
        //Debug.Log("Leaving Attack Animation ");
        isInAttackAnim=false;

    }

    public void StartAttackCoroutine(float durr, string attackAnimName){
        
        StartCoroutine(EntityAttack(durr,attackAnimName));
    }

    public bool GetIfPlayerIsDisabled()
    {
        if(dialogueManager.GetDialogue())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

 
    /// <summary>
    /// funny distance is close enough to player function 
    /// </summary>
    /// <returns></returns>
    public bool GetIsCloseToPlayer(){
        float playerDistance = Vector3.Distance(entityPos, playerPos);

        return playerDistance <= detectionRadius || GetComponent<AISensor>().GetSpottedStatus();
    }

    /// <summary>
    /// checks logic update every frame
    /// </summary>
    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if(dialogueManager.GetDialogue())
        {
            playerIsInDialogue = true;
        }
        else
        {
            playerIsInDialogue = false;
        }
    }

    /// <summary>
    /// checks physics update using a fixedUpdate
    /// </summary>
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
        entityPos = transform.position;
        playerPos = player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision");

        if (collision.gameObject.name=="Player")
        {
            collisionWithPlayer = true;
            //Debug.Log("collision with player");
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("collision");

        if (collision.gameObject.name == "Player")
        {
            collisionWithPlayer = false;
            //Debug.Log("collision exited");
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, entityData.detectionRadius);

        Gizmos.color = Color.yellow;
        for (int i = 0; i < points.Length; i++)
        {
            if(i!= points.Length-1) // index is not at the last point
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
