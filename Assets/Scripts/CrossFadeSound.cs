using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossFadeSound
{
    public static void CrossFadeClip(this AudioSource audioSource, float volume, float duration)
    {
        MonoBehaviour mnbhvr = audioSource.GetComponent<MonoBehaviour>();
        mnbhvr.StartCoroutine(CrossFadeClipCOR(audioSource, volume, duration));
    }

    public static IEnumerator CrossFadeClipCOR(AudioSource audioSource, float volume, float duration)
    {
        float currentVolume = audioSource.volume;


        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(currentVolume, volume, counter / duration);
            yield return null;
        }
    }
}
