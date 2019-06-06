using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    List<AudioClip> sounds = new List<AudioClip>();

    public AudioSource audioSource;

    public bool saveTrackLocation;

    public bool loopPlaylist;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(int index = 0, bool fadeOut = false)
    {
        SwitchTrack(sounds[index]);
        audioSource.Play();
    }

    public void Play(AudioClip clip)
    {
        SwitchTrack(clip);
        audioSource.Play();
    }

    private void SwitchTrack(AudioClip to)
    {
        audioSource.clip = to;
    }
}
