using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;

public static class CrossFadeSound
{
    public static async UniTask CrossFadeClipAsync(this AudioSource audioSource, float volume, float duration)
    {
        if (duration != 0)
        {
            float currentVolume = audioSource.volume;


            float counter = 0;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(currentVolume, volume, counter / duration);
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
        }
        else
        {
            audioSource.volume = volume;
        }
    }
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
        if(duration != 0)
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
        else
        {
            audioSource.volume = volume;
        }
        if (action != null)
        {
            action.Invoke();
        }
    }
}
