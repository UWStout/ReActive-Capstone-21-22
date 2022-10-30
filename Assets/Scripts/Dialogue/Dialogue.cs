using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public string characterName;
    public int actionID = 0;
    public Vector3 cameraPos, cameraRot;
    public SoundEffectType soundToPlay;

    [TextArea(1,2)]
    public string[] prompts, responses;

    public Dialogue[] nextPrompts;
}
