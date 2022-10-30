using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<MusicSource> musicSource;

    public MusicTrack currentTrack;

    public int currentMusicSource;
    public int lastMusicSource;

    [System.Serializable]
    public class MusicSource
    {
        public AudioSource source;
        public bool active;
        public MusicTrack track;
        
    }

    void Start()
    {
        RootScript.MusicManager = this;
    }

    

    public void PlayMusicTrack(MusicTrack track, float fadein = 0.5f, float fadeout = 0.5f)
    {
        if (musicSource[currentMusicSource].track != track || musicSource[currentMusicSource].active == false)
        {
            StopMusic(fadeout);

            lastMusicSource = currentMusicSource;
            currentMusicSource = (currentMusicSource + 1) % musicSource.Count;

            currentTrack = track;

            musicSource[currentMusicSource].active = true;
            musicSource[currentMusicSource].track = track;

            if (musicSource[currentMusicSource].track.audioClipIntro != null)
            {
                musicSource[currentMusicSource].source.clip = musicSource[currentMusicSource].track.audioClipIntro;
                musicSource[currentMusicSource].source.loop = false;

                StartCoroutine(AudioFadeScript.FadeIn(musicSource[currentMusicSource].source, fadein));
                if (musicSource[currentMusicSource].track.loops)
                {
                    StartCoroutine(SwitchToLoop((double)musicSource[currentMusicSource].track.audioClipIntro.samples / musicSource[currentMusicSource].track.audioClipIntro.frequency));
                }
            }
            else
            {
                musicSource[currentMusicSource].source.clip = musicSource[currentMusicSource].track.audioClipLoop;
                musicSource[currentMusicSource].source.loop = musicSource[currentMusicSource].track.loops;
                StartCoroutine(AudioFadeScript.FadeIn(musicSource[currentMusicSource].source, fadein));
            }
            
        }
        
    }

    public void StopMusic(float fadeout = 0.5f)
    {
        if (musicSource[currentMusicSource].active)
        {
            musicSource[currentMusicSource].active = false;
            StartCoroutine(AudioFadeScript.FadeOut(musicSource[currentMusicSource].source, fadeout));
        }
    }

    IEnumerator SwitchToLoop(double introDuration)
    {
        musicSource[(currentMusicSource + 1) % musicSource.Count].track = musicSource[currentMusicSource].track;
        musicSource[(currentMusicSource + 1) % musicSource.Count].source.clip = musicSource[currentMusicSource].track.audioClipLoop;
        musicSource[(currentMusicSource + 1) % musicSource.Count].source.loop = true;
        musicSource[(currentMusicSource + 1) % musicSource.Count].source.PlayScheduled(AudioSettings.dspTime + introDuration);

        yield return new WaitForSeconds((float)introDuration);

        lastMusicSource = currentMusicSource;
        currentMusicSource = (currentMusicSource + 1) % musicSource.Count;

        musicSource[lastMusicSource].active = false;
        musicSource[currentMusicSource].active = true;

    }
}