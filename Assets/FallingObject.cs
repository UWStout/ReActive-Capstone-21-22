using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// class for the Falling object to fall and respawn
/// @author-Jake Reese.

public class FallingObject : MonoBehaviour
{
    //new Falling variables
    /// <summary>
    /// The gameobject for where the object with respawn
    /// </summary>
    [SerializeField] private GameObject pointA;

    /// <summary>
    /// The game object that with be repeatedly drop 
    /// </summary>
    [SerializeField] private GameObject objectToSpawn;

    /// <summary>
    /// The Rigidbody of the game object 
    /// </summary>
    [SerializeField] private Rigidbody RB;

    /// <summary>
    /// The game of the current object that will be falling
    /// </summary>
    [SerializeField] private GameObject currentObject;

    /// <summary>
    /// The transform of the falling game object
    /// </summary>
    private Transform transform;
    

    [SerializeField] private float respawnTimeTimer=3.0f;
    /// <summary>
    /// The position of where the falling object starts from to respawn at that point
    /// </summary>
    private Vector3 StartPostition;

    private GameObject Player;

    float timer = 0;
 private void Start() {

        transform = currentObject.transform;
        Vector3 OrigonalPos = currentObject.transform.position;
        StartPostition = OrigonalPos;
        Player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        float downSpeed= Time.deltaTime/20f;
        RB.AddForce(0, -1, 0);
    }


    /// <summary>
    /// A function that with move the falling gameobject back to its original 
    /// </summary>
    void Reset()
    {
        transform.position = StartPostition;
        RB.velocity = Vector3.zero;

    }

    /// <summary>
    /// A function that will check if falling object hits the player or the floor and then resets the object
    /// </summary>
    void OnCollisionEnter(Collision other) {

        
        if (other.gameObject.CompareTag("Player")) { 
            
            Reset();
            Player.GetComponent<HealthManager>().takeDamage(1);
        }
        
    }
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > respawnTimeTimer)
        //Physics.Raycast(currentObject.transform.position, -Vector3.up, 0.5f)
        {
            Reset();
            timer = 0;
        }
    }
}
