using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]

public class D_Entity : ScriptableObject
{
    //max health at start
    public float maxHealth = 100f;
    public float detectionRadius = 20f;
    public float detectionExitRadius = 25f;
    //"stun health" stunResistance is how much damage an enemy can take before stunned
}