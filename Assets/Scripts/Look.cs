using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public Transform alternativeTarget;

    public bool lookAtPlayer = true;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (lookAtPlayer)
        {
            transform.LookAt(player.transform, Vector3.up);
        }
        transform.LookAt(alternativeTarget, Vector3.up);
    }
}
