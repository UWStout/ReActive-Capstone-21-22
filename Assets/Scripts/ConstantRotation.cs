using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public Vector3 rotationSpeed;
    public bool localRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (localRotation)
        {
            transform.RotateAround(transform.position, transform.right, rotationSpeed.x * Time.deltaTime);
            transform.RotateAround(transform.position, transform.up, rotationSpeed.y * Time.deltaTime);
            transform.RotateAround(transform.position, transform.forward, rotationSpeed.z * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(transform.position, Vector3.right, rotationSpeed.x * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.up, rotationSpeed.y * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.forward, rotationSpeed.z * Time.deltaTime);
        }
        
    }
}
