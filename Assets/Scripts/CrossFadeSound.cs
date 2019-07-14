using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossFadeSound
{
    public static void CrossFadeClip(this AudioSource audioSource, float volume, float duration, System.Action action)
    {
        MonoBehaviour mnbhvr = audioSource.GetComponent<MonoBehaviour>();
        mnbhvr.StartCoroutine(CrossFadeClipCOR(audioSource, volume, duration, action));
    }

    public static void CrossFadeClip(this AudioSource audioSource, float volume, float duration)
    {
        MonoBehaviour mnbhvr = audioSource.GetComponent<MonoBehaviour>();
        mnbhvr.StartCoroutine(CrossFadeClipCOR(audioSource, volume, duration, null));
    }

    public static IEnumerator CrossFadeClipCOR(AudioSource audioSource, float volume, float duration, System.Action action)
    {
        float currentVolume = audioSource.volume;


        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(currentVolume, volume, counter / duration);
            yield return null;
        }
        if (action != null)
        {
            action.Invoke();
        }
    }
}
