using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackTest : MonoBehaviour
{
    public float forcePower = 100f;
    public float upwardForce = 70;
    public bool destroyOnContact = false;
    private HealthManager hm;
    private bool canBeHit = true;
    public bool projectile = false;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        hm = GameObject.FindWithTag("Player").GetComponent<HealthManager>();




        if (rb != null && hm != null && canBeHit)
        {

            if (collision.gameObject.tag == "Player")
            {
                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0;
                rb.velocity = Vector3.zero;

                rb.AddForce(direction * forcePower, ForceMode.Impulse);
                rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

                hm.takeDamage(1);
                int currentHealth = RootScript.TheGameManager.GetHealth();
            }
            canBeHit = false;
        }
        if (destroyOnContact && collision.gameObject.layer != 7) // make sure the projectile isnt hitting an enemy
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider targetCollider = other.GetComponent<Collider>();
        hm = GameObject.FindWithTag("Player").GetComponent<HealthManager>();
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        DialogueManager dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        bool readyToDelete = false;


        if (targetCollider != null && hm != null)
        {

            if (targetCollider.gameObject.tag == "Player" && projectile)
            {

                Vector3 direction = targetCollider.transform.position - transform.position;
                direction.y = 0;
                rb.velocity = Vector3.zero;

                rb.AddForce(direction * forcePower, ForceMode.Impulse);
                rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

                hm.takeDamage(1);
                int currentHealth = RootScript.TheGameManager.GetHealth();
            }
            readyToDelete = true;
        }
        else
        {
            readyToDelete = true;
        }

        if (destroyOnContact && other.gameObject.layer != 7 && readyToDelete) // make sure the projectile isnt hitting an enemy
        {
            DeleteProjectile();
            readyToDelete = false;
        }
        if(dialogueManager.DialgoueIsActive())
        {
            DeleteProjectile();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        canBeHit = true;
    }

    public void SetCanBeHit(bool value)
    {
        canBeHit = value;
    }
    public void DeleteProjectile()
    {
        Destroy(this.gameObject);
    }
}
