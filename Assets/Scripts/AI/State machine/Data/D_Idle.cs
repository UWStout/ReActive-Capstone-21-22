using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/State Data/Idle Data")]

public class D_Idle : ScriptableObject
{
    public float minIdleTime = 2.0f;
    public float maxIdleTime = 6.0f;
    public float PatrolLookAngle = 45f; // loof left 45, then back to center, look right 45
    public Material idleMat;
}
