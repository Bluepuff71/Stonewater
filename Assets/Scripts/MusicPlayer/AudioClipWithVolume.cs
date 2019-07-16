using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AudioClipWithVolume
{

    public AudioClip audioClip;
    public float volume;

    public AudioClipWithVolume(AudioClip clip, float volume)
    {
        this.audioClip = clip;
        this.volume = volume;
    }
}
