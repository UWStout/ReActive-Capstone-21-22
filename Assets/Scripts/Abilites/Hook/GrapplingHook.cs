
using UnityEngine;
using System.Collections;

/// class for the grappling hook
/// @author-Benjamin Odrich.

public class GrapplingHook : MonoBehaviour
{
    //new hook variables
    /// <summary>
    /// the player rb
    /// </summary>
    private Rigidbody playerRb;
    /// <summary>
    /// ray cast hit reference
    /// </summary>
    private RaycastHit hit;

    [Header("The Travel Speed of the Rope")]
    /// <summary>
    /// the travel speed when the player uses the grappling hook
    /// </summary>
    [SerializeField] private float grappleTravelSpeed;
    /// <summary>
    /// is the player grappling
    /// </summary>
    private bool isPlayerGrappling;
    [Header("Where should the line start")]
    /// <summary>
    /// gameobject of were the line should start for the line render 
    /// </summary>
    [SerializeField] private GameObject lineStart;
    [Header("Max Grapple Range")]
    /// <summary>
    /// how far the player can grapple 
    /// </summary>
    
    [SerializeField] private float maxGrappleRange;

    [Header("Hook Prefab")]
    /// <summary>
    /// the hook prefab that will be instantiated at run time
    /// </summary>
    [SerializeField] private GameObject hookPrefab;

    /// <summary>
    /// the line renderer 
    /// </summary>
    private LineRenderer grappleLineRenderer;
    /// <summary>
    /// ref to the hook that is instaniated 
    /// </summary>
    private GameObject hookInstantiated;

    private CharacterMove characterController;

    private PlayerAnimatorController playerAnimatorController;
    /// <summary>
    /// close distance the player can be to the grappling point until it unhooks them
    /// </summary>
    [Header("How far should the player be until they unhook")]
    [SerializeField] private float unHookDistance=2f;

    
	[SerializeField] private SoundEffectType grappleShootSound;
	[SerializeField] private SoundEffectType grapplePullSound;
	[SerializeField] private SoundEffectType grappleReleaseSound;


    public float lerpSpeed = 0.1f;
    public bool isHighlighting = false;
    private RaycastHit hitCheck;


    private void Start()
    {
        isPlayerGrappling = false;
        grappleLineRenderer = GetComponent<LineRenderer>();
        playerRb = GetComponent<Rigidbody>();
        grappleLineRenderer.enabled = false;
        characterController=GetComponent<CharacterMove>();
        playerAnimatorController=FindObjectOfType<PlayerAnimatorController>();



    }


    public RaycastHit GetHit(){
    
        Debug.Log("hit "+ hit.collider.tag);
        return hit;
    }
    /// <summary>
    /// getter function for isGrappling var
    /// </summary>
    /// <returns>isGrappling</returns>
    public bool GetIsGrappling()
    {
        return isPlayerGrappling;
    }
    /// <summary>
    /// update loop handles grapple interaction with the player
    /// </summary>
    private void Update()
    {
        
        /*if the player is grappling...
            -check to see if there close to the grappling point,
            -or check to see if the left mouse button was pressed
            -or check to see if the left mouse button was released
        */
        if (isPlayerGrappling)
        {
            //check to see if the player is close to the point there grappling
            if (CalcDistance() < unHookDistance)
            {
                //set the rb to be zero
                playerRb.velocity = Vector3.zero;

                //add a little bit of force up
                playerRb.AddForce(transform.up * 3, ForceMode.Impulse);

                //unhook the player
                UnHook();
            }
            //check to see if the left mouse button was released
             if (RootScript.Input.Hook.WasReleased)
            {
                //unhook the player
                UnHook();

                //set the players rb to be the zero vector
                playerRb.velocity = Vector3.zero;
            }
            //check to see if the jump button was pressed 
            else if (RootScript.Input.Jump.WasPressed)
            {
                //un hook the player
                UnHook();

                //add a little bit of force for the player
                playerRb.AddForce(transform.up * playerRb.velocity.magnitude * 0.5f, ForceMode.Impulse);
                playerRb.AddForce(transform.forward * playerRb.velocity.magnitude * 0.5f, ForceMode.Impulse);
            }
        }



        //if the player has started holding the left mouse button
        if (RootScript.Input.Hook.WasPressed)
        {
            //check to see if the player can grapple 
            CheckForGrapple();
        }


    }
    /// <summary>
    /// calc the current distance between the player's transform and the hit point
    /// </summary>
    /// <returns> the distance between the player and the hit point</returns>
    private float CalcDistance()
    {
        return Vector3.Distance(transform.position, hit.point);
    }
    /// <summary>
    /// late update handles drawing the line between the line start and the hook we spawn in 
    /// </summary>
    private void LateUpdate()
    {
        //"draw" the grapple line
        DrawLine();
    }
    /// <summary>
    /// fixed update handles moving the player and moving the hook game object 
    /// </summary>
    private void FixedUpdate()
    {
        MovePlayer();
        LerpHook();
    }
    /// <summary>
    /// a reset function for when the player un hooks 
    /// </summary>
    private void UnHook()
    {
        RootScript.SoundManager.PlaySound(grappleReleaseSound, -1, transform);
        RootScript.SoundManager.StopSound(grapplePullSound);

        DestroyHook();
        isPlayerGrappling = false;
        characterController.ResetGravity();
        //playerRb.useGravity = true;
        grappleLineRenderer.enabled = false;
    }
    public void UnHookFromRespawn(){
        UnHook();
    }


    /// <summary>
    /// if the hook is not null, lerp the postion of the grappling hook to the hit point
    /// </summary>
    private void LerpHook()
    {
        if (hookInstantiated != null)
        {
            //move hook towards the hit point so that the line draws correctly and smooth 
            hookInstantiated.transform.position = Vector3.Lerp(hookInstantiated.transform.position, hit.point, lerpSpeed);

        }
    }
    /// <summary>
    /// move the player towards the hit point if the player is grappling
    /// </summary>
    private void MovePlayer()
    {
        if (isPlayerGrappling)
        {

            characterController.SetGravity(0);
            //playerRb.useGravity = false;
            //set the position of the player as a fraction of the travel speed / the remaining distance between the player and the hit point 
            transform.position = Vector3.Lerp(transform.position, hit.point, grappleTravelSpeed * Time.deltaTime / CalcDistance());

        }

    }
    /// <summary>
    /// draws the hook line 
    /// </summary>
    private void DrawLine()
    {
        if (hookInstantiated != null)
        {
            //enable the line renderer and set the 1st pos of the line to be the line start game object and the 2nd pos to be the hook game object that is instantiated
            grappleLineRenderer.enabled = true;
            grappleLineRenderer.SetPosition(0, lineStart.transform.position);
            grappleLineRenderer.SetPosition(1, hookInstantiated.transform.position);

        }
        else
        {
            grappleLineRenderer.enabled = false;
        }
    }
    /// <summary>
    /// checks if the player can grapple and if so, configs it based on what were grappling to
    /// </summary>
    private void CheckForGrapple()
    {
        if(!GameObject.Find("Canvas").GetComponent<DialogueManager>().GetDialogue()){//FIX ME LATER
        //print("trying to grapple");
       // Ray cameraToScreenRay= Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.6f));
        //if the ray cast hits a collider within these params 
        print(HighlightGrapple());
        if (HighlightGrapple())
        {
            GetHit();

            //check to see if the collider has hit a gameobject with the can grapple tag 
            if (hit.collider.tag == "CanGrapple")
            {
                //play grapple animation
                //play an animation with a delay then grapple
                StartCoroutine(ShootGrappleAfterDelay());
            }
        }
    }
    }
      IEnumerator ShootGrappleAfterDelay(){
         //play animation 
         playerAnimatorController.SetGrappleTrigger();
         //then after delay shoot grappling hook
         yield return new WaitForSeconds(0.25f);
            isPlayerGrappling = true;

         InstantiateGrapplingHook();


    }
    

    private bool HighlightGrapple(){
         Ray cameraToScreenRay= Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.6f));
        return  Physics.Raycast(cameraToScreenRay, out hit, maxGrappleRange);
    }

    public bool CheckForGrapplePoint()
    {
        Ray cameraToScreenRay= Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.6f));
        if (Physics.Raycast(cameraToScreenRay, out hitCheck, maxGrappleRange)) return hitCheck.collider.tag == "CanGrapple";
        return false;
    }
    /// <summary>
    /// destory the hook
    /// </summary>
    private void DestroyHook()
    {
        if (hookInstantiated != null)
        {
            Destroy(hookInstantiated);

        }
    }
    /// <summary>
    /// Instantiates the grappling hook so that the line render draws correctly 
    /// </summary>
    private void InstantiateGrapplingHook()
    {
        hookInstantiated = Instantiate(hookPrefab);
        hookInstantiated.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        hookInstantiated.name = "hookSpawned";
        hookInstantiated.transform.position = transform.position;

        RootScript.SoundManager.PlaySound(grappleShootSound, -1, transform);
        RootScript.SoundManager.PlaySound(grapplePullSound, -1, transform);

    }

}