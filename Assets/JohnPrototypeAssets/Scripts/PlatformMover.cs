using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public Transform start, target;
    Transform temp;
    float speed = 0.2f, elapsedTime = 0;
    const float tripTime = 10;

    // Update is called once per frame
    private void Update()
    {
        if(transform.position == target.position)
        {
            temp = target;
            target = start;
            start = temp;

            elapsedTime = 0;
        }

        transform.position = Vector3.Lerp(start.position, target.position, speed * elapsedTime % tripTime);
        elapsedTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            other.transform.parent = null;
        }
    }
}
