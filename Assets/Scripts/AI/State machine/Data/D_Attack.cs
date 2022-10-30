using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/State Data/Attack Data")]

public class D_Attack : ScriptableObject
{
    public Material attackMaterial;
    public Material approachMaterial;
    public float AttackCooldownTime = 3.0f;
    public float modifiedSpeed = 20;
    public float modifiedAccel = 40;
}