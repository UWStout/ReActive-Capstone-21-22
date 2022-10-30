using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/State Data/Roam Data")]


public class D_Roam : ScriptableObject
{
    public float walkRadius = 5.0f;
    public Material roamMat;
}
