using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public float bottom = 0.0f;
    public Color meshColor = Color.red;
    public bool showMesh = true;

    public int scanFrequency = 30;
    public LayerMask occlusionLayers; // what layers block the raycast for line of sight

    private Mesh mesh;
    float scanInterval;
    float scanTimer;
    private GameObject player;
    private bool playerIsSpotted = false;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //scanTimer -= Time.deltaTime;
        //if(scanTimer < 0) // once scan timer hits zero, it scans and is reset back to the scan interval
        //{
        //    scanTimer += scanInterval;
        //    Scan();
        //}
        Scan();
        //Debug.Log("i am running");
    }

    private void Scan()
    {
        //count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        //Objects.Clear();
        //for (int i = 0; i < count; i++)
        //{
        //    GameObject obj = colliders[i].gameObject;
        //    if (IsInSight(obj))
        //    {
        //        Objects.Add(obj);
        //    }
        //}
        if (IsInSight(player))
        {
            playerIsSpotted = true;
            //Objects.Add(player);
            //Debug.Log("PLAYER SEEN");

        }
        else
        {
            playerIsSpotted = false;
            //Debug.Log("PLAYER OUT");
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        // check height
        if(direction.y < bottom || direction.y > height) // checks that the destination is at the correct hight
        {
            //Debug.Log("incorrect height");
            return false;
        }

        // check angle
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if(deltaAngle > angle) // checks if the player is within the angle of the sensor mesh
        {
            //Debug.Log("incorrect angle");
            return false; // returns false if player is outside of the angle
        }

        // check raycast
        origin.y += height / 2;
        dest.y = origin.y; // makes sure the raycast is sent from the middle of the wedge
        if(Physics.Linecast(origin, dest, occlusionLayers))
        {
            //Debug.Log("incorrect raycast");
            return false;
        }

        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangels = (segments * 4) + 2 + 2;
        int numVerticies = numTriangels * 3;

        Vector3[] vertices = new Vector3[numVerticies];
        int[] triangles = new int[numVerticies];

        Vector3 bottomCenter = new Vector3(0, bottom, 0);
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * new Vector3(0, 0, 1) * distance;
        bottomLeft = bottomLeft + Vector3.down * -bottom;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * new Vector3(0, 0, 1) * distance;
        bottomRight = bottomRight + Vector3.down * -bottom;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for(int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomLeft = bottomLeft + Vector3.down * -bottom;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            bottomRight = bottomRight + Vector3.down * -bottom;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;


            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }
        
        
        for(int i = 0; i < numVerticies; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;

    }

    private void OnValidate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            if(showMesh)
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }


        Gizmos.DrawWireSphere(transform.position, distance);
        if (!playerIsSpotted && GetPlayerDistance() <= distance)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(player.transform.position, 1f);
            //Gizmos.DrawSphere(colliders[i].transform.position, 0.3f);
        }

        Gizmos.color = Color.green;
        //foreach(var obj in Objects)

        if (playerIsSpotted && GetPlayerDistance() <= distance)
        {
            Gizmos.DrawSphere(player.transform.position, 1f);
        }
    }

    private float GetPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance;
    }

    public bool GetSpottedStatus()
    {
        if (playerIsSpotted && GetPlayerDistance() <= distance)
            return true;
        else
            return false;
    }
}
