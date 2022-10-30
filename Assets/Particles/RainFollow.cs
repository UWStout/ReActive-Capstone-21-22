using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollow : MonoBehaviour
{
    public Transform targetTransform;
    public float smoothTime = 0.15F;
    private Vector3 velocity = Vector3.zero;

    
    void Update() {
        if (targetTransform == null)
        {
            if (RootScript.CharMove != null) targetTransform = RootScript.CharMove.transform;
        }
        else
        {
            Vector3 targetPosition = targetTransform.position;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        
    }
}
