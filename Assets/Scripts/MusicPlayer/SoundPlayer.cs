using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The sound player is a way of playing a playlist of tracks, there should be no way to choose which song to play
public class SoundPlayer : MonoBehaviour
{

    public AudioSource audioSource;

    public Tape tape;

    private bool wasStopped;

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && !wasStopped)
        {
            NextTrack();
        }
    }

    public void Play()
    {
        if (audioSource.isPlaying)
        {
            Debug.LogWarning("2 playing Soundplayers attached to the same audio source! Make sure to stop the soundplayer before playing another one!");
        }
        else
        {
            audioSource.clip = tape.GetCurrentTrack().audioClip;
            audioSource.time = tape.TrackLocation;
            audioSource.volume = 0;
            CrossFadeSound.CrossFadeClip(audioSource, tape.GetCurrentTrack().volume, tape.fadeInTime);
            audioSource.Play();
        }
    }

    private void NextTrack()
    {
        if (tape.AtEndOfTape())
        {
            if (tape.ShouldLoop)
            {
                tape.RestartTape();
            }
            else
            {
                Debug.LogWarning("No more tracks found. Stopping tape.");
                Stop();
            }
        }
        AudioClipWithVolume nextTrack = tape.GetNextTrack(shouldPersist: true); //This shouldn't been null because we checked earlier
        audioSource.clip = nextTrack.audioClip;
        audioSource.volume = nextTrack.volume;
    }

    public void Stop()
    {
        if (tape.PersistTracks)
        {
            tape.TrackLocation = audioSource.time;
        }
        else
        {
            tape.RestartTape();
        }
        wasStopped = true;
        CrossFadeSound.CrossFadeClip(audioSource, 0, tape.fadeOutTime, () => audioSource.Stop());
    }
}
