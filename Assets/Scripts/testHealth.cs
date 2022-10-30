using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic script that tests the healing function.
/// </summary>
public class testHealth : MonoBehaviour
{
    /// <summary>
    /// Reference to the HealthManager script.
    /// </summary>
    public HealthManager hm;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the health manager script from the player.
        hm = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
    }

    /// <summary>
    /// When the player runs into this game object, heal the player
    /// </summary>
    /// <param name="collision"> The object this gameobject collides with.</param>
    void OnCollisionEnter(Collision collision)
    {
        //If the player hits this game object, increase their health by 1 and destroy the object.
        if (collision.gameObject.tag == "Player")
        {
            hm.gainHealth(1);
            Destroy(gameObject);
        }
    }
}
