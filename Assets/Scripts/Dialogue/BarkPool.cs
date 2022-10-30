using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "New Bark Pool", menuName = "Bark Pool")]
public class BarkPool : ScriptableObject
{
    [TextArea(1,2)]
    public string[] barkLines;
}
