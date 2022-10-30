using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldFollow : MonoBehaviour
{
    public Vector3 pos;
 
    public GameObject robot;
    public Camera camera;
    private Vector3 roboPos;
    private RectTransform rt;
    public RectTransform canvasRT;
    private Vector3 roboScreenPos;
    private Renderer roboRenderer;
 
    // Use this for initialization
    void Start () {
        roboPos = robot.transform.position;
        //roboRenderer = robot.GetComponent<Renderer>();
 
        rt = GetComponent<RectTransform>();
        roboScreenPos = camera.WorldToViewportPoint(robot.transform.TransformPoint(roboPos));
        rt.anchorMax = roboScreenPos;
        rt.anchorMin = roboScreenPos;
    }
   
    // Update is called once per frame
    void Update () {
        roboScreenPos = camera.WorldToViewportPoint(robot.transform.position);
        roboScreenPos.y = 0.8f;
        //roboScreenPos = camera.WorldToViewportPoint(robot.transform.TransformPoint(roboPos));
        rt.anchorMax = roboScreenPos;
        rt.anchorMin = roboScreenPos;
    }
}
