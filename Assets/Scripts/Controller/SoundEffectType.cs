using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Sound Effect", menuName = "GameElements/SFX_Type")]
public class SoundEffectType : ScriptableObject
{
    [Range(0f, 100f)]
    public float volume;

    public float playSpeed = 1f;

    public bool loops;

    public List<AudioClip> sounds;

    public AudioMixerGroup mixerGroup;
}
