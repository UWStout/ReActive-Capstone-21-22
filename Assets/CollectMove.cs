using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMove : MonoBehaviour
{
    [SerializeField] public float yStart;
    [SerializeField] public float yEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y <= yStart)
        {
            LeanTween.moveY(gameObject, yEnd, 3f);
        } else if (gameObject.transform.position.y >= yEnd)
        {
            LeanTween.moveY(gameObject, yStart, 3f);
        }
    }
}
