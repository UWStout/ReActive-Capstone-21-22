    using UnityEngine;
    using System.Collections;
     
    public static class AudioFadeScript
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 1f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * 0.025f / FadeTime;
 
            yield return new WaitForSeconds(0.025f);
        }
 
        audioSource.Stop();
        audioSource.volume = 1f;

        yield return null;
    }
 
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 1f;
 
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * 0.025f / FadeTime;
 
            yield return new WaitForSeconds(0.025f);
        }

        audioSource.volume = 1f;

        yield return null;
    }
}
 