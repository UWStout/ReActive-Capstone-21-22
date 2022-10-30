using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTesting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 20;
    public float leadTime = 2;

    private GameObject player;
    private Rigidbody playerBody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerBody = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            FireProjectile(TargetPlayer());
        }
    }

    Vector3 TargetPlayer()
    {
        Vector3 playerVel = playerBody.velocity;

        float a = Mathf.Pow(playerVel.x, 2) + Mathf.Pow(playerVel.y, 2) + Mathf.Pow(playerVel.z, 2) - Mathf.Pow(projectileSpeed, 2);
        float b = 2 * (
            playerVel.x * (player.transform.position.x - transform.position.x) + 
            playerVel.y * (player.transform.position.y - transform.position.y) + 
            playerVel.z * (player.transform.position.z - transform.position.z));
        float c = (
            Mathf.Pow((player.transform.position.x - transform.position.x), 2) + 
            Mathf.Pow((player.transform.position.y - transform.position.y), 2) + 
            Mathf.Pow((player.transform.position.z - transform.position.z), 2));
        float d = Mathf.Pow(b, 2) - (4 * a * c);

        if(d >= 0)
        {
            float t1 = ((-1*b + Mathf.Sqrt(d)) / (2*a));
            float t2 = ((-1*b - Mathf.Sqrt(d)) / (2*a));
            float t;

            if(Mathf.Min(t1, t2) > 0)
            {
                t = Mathf.Min(t1, t2);
            }
            else
            {
                t = Mathf.Max(t1, t2);
            }

            float targetX = playerVel.x * t + player.transform.position.x;
            float targetY = playerVel.y * t + player.transform.position.y;
            float targetZ = playerVel.z * t + player.transform.position.z;

            Debug.Log(a + ", " + b + ", " + c);
            Debug.Log(t1 + ", " + t2);
            Debug.Log(t);
            if(t > 0)
            {
                return new Vector3(targetX, targetY, targetZ);
            }
        }

        return Vector3.zero;
    }

    void FireProjectile(Vector3 target)
    {
        Debug.Log("Player Velocity: " + playerBody.velocity);
        Debug.Log("Locked Target:   " + target);

        if(target != Vector3.zero)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject;
            projectile.transform.forward = Vector3.Normalize(target - projectile.transform.position);
            projectile.GetComponent<Rigidbody>().velocity = Vector3.Normalize(target - projectile.transform.position) * projectileSpeed;
        }
    }
}
