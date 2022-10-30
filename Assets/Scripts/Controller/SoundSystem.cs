using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSystem : MonoBehaviour
{
    [System.Serializable]
    public class SoundType
    {
        public string name;

        public SoundEffectType sfxType;

        public AudioSource source;

        public Transform transformSource;
    };

    [Header ("Data")]
    
    [SerializeField] private List<SoundEffectType> soundEffectTypes;
    [SerializeField] private List<SoundType> soundTypes;

    [Header ("Dependencies")]

    [SerializeField] private GameObject audioSourceObject;

    void Start()
    {
        // Create Audio Sources for all SoundEffectTypes
        foreach (SoundEffectType type in soundEffectTypes) CreateAudioSource(type);

        //RootScript.SoundManager = this;

        
    }


    /// <summary>
    /// Create the audio source for a SoundType
    /// </summary>
    /// <param name="type">SoundType to create a audio source for.</param>
    private void CreateAudioSource(SoundEffectType type)
    {
        GameObject newObject = Instantiate(audioSourceObject, Vector3.zero, Quaternion.identity, transform);
        SoundType newSoundType = new SoundType();
    
        newSoundType.name = type.name;
        newSoundType.sfxType = type;

        newSoundType.source = newObject.GetComponent<AudioSource>();
        newSoundType.source.volume = type.volume / 100f;
        newSoundType.source.pitch = type.playSpeed;
        newSoundType.source.loop = type.loops;
        newSoundType.source.outputAudioMixerGroup = type.mixerGroup;
        newSoundType.source.clip = type.sounds[0];

        soundTypes.Add(newSoundType);
    }

    /// <summary>
    /// Plays a sound from a SoundType if possible
    /// </summary>
    /// <param name="type">SoundType to play</param>
    /// <param name="volumeOverride">Optional - Changes the volume of the sound this once, 0f-100f</param>
    /// <returns></returns>
    public bool PlaySound(SoundType type, float volumeOverride = -1f, Transform _transform = null)
    {
        try
        {
            if (_transform != null)
            {
                
                type.source.transform.position = _transform.position;
            }

            type.source.clip = type.sfxType.sounds[UnityEngine.Random.Range(0, type.sfxType.sounds.Count)];
            type.source.Play();
        }
        catch (Exception e)
        {
            //Debug.Log("sound error, try running through main menu");
            //Debug.Log(e);
        }

        return true;
    }

    /// <summary>
    /// Plays a sound from a SoundEffectType if possible
    /// </summary>
    /// <param name="type">SoundEffectType to play</param>
    /// <param name="volumeOverride">Optional - Changes the volume of the sound this once, 0f-100f</param>
    /// <returns></returns>
    public bool PlaySound(SoundEffectType type, float volumeOverride = -1f, Transform _transform = null)
    {
        SoundType foundType = FindSoundTypeByEffectType(type);

        return PlaySound(foundType, volumeOverride, _transform);
    }

    public bool StopSound(SoundEffectType type)
    {
        SoundType foundType = FindSoundTypeByEffectType(type);

        if (foundType != null)
        {
            foundType.source.Stop();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Plays a sound from a SoundType if possible
    /// </summary>
    /// <param name="type">SoundType to play</param>
    /// <param name="volumeOverride">Optional - Changes the volume of the sound this once, 0f-100f</param>
    public IEnumerator PlaySoundCoroutine(SoundType type, float volumeOverride = -1f, Transform _transform = null)
    {
        PlaySound(type, volumeOverride, _transform);
        yield return new WaitForSeconds(type.sfxType.sounds[0].length);
    }

    /// <summary>
    /// Finds a SoundType by name
    /// </summary>
    /// <param name="sourceName">Name of sound type</param>
    /// <returns>Found sound type</returns>
    public SoundType FindSoundTypeByString(string sourceName)
    {
        foreach (SoundType type in soundTypes)
        {
            if (type.name == sourceName) return type;
        }

        Debug.LogError("UNABLE TO FIND SOUND TYPE: " + sourceName);

        return soundTypes[0];
    }

    /// <summary>
    /// Finds a SoundType by SoundEffectType
    /// </summary>
    /// <param name="sourceName">Name of sound type</param>
    /// <returns>Found sound type</returns>
    public SoundType FindSoundTypeByEffectType(SoundEffectType typeEffect)
    {
        foreach (SoundType type in soundTypes)
        {
            if (type.sfxType == typeEffect) return type;
        }

        Debug.LogError("UNABLE TO FIND SOUND EFFECT TYPE: " + typeEffect.name);

        return soundTypes[0];
    }
}
