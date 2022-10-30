using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Music Track", menuName = "GameElements/Music Track")]
public class MusicTrack : ScriptableObject
{
    public AudioClip audioClipIntro;
    public AudioClip audioClipLoop;
    public bool loops;
}