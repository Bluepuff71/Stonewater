using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The sound player is a way of playing a playlist of tracks, there should be no way to choose which song to play
public class SoundPlayer : MonoBehaviour
{

    public AudioSource audioSource;

    private bool wasStopped = true;
    private Tape tape;

    private void Start()
    {
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && !wasStopped)
        {
            NextTrack();
        }
    }

    public void Play(float fadeInLength = .1f)
    {
        if (audioSource.isPlaying)
        {
            Debug.LogWarning("2 playing Soundplayers attached to the same audio source! Make sure to stop the soundplayer before playing another one!");
        }
        else
        {
            wasStopped = false;
            audioSource.clip = tape.GetCurrentTrack().audioClip;
            audioSource.time = tape.TrackLocation;
            audioSource.volume = 0;
            CrossFadeSound.CrossFadeClip(audioSource, tape.GetCurrentTrack().volume, fadeInLength);
            audioSource.Play();
        }
    }

    public void QuickPlay(AudioClipWithVolume track)
    {
        if (audioSource.isPlaying)
        {
            Debug.LogWarning("2 playing Soundplayers attached to the same audio source! Make sure to stop the soundplayer before playing another one!");
        }
        else
        {
            audioSource.clip = track.audioClip;
            audioSource.volume = track.volume;
            audioSource.Play();
        }
    }

    public void QuickPlay(AudioClip audioClip)
    {
        QuickPlay(new AudioClipWithVolume(audioClip, 1));
    }

    public void SwitchTape(Tape toTape, bool playWhenSwitched = true)
    {
        if (!wasStopped)
        {
            Stop();
        }
        //if (continueIfSameTrackExists)
        //{

        //}
        tape = toTape;
        if (playWhenSwitched)
        {
            Play();
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
        } else
        {
            tape.LoadNextTrack();
            Play(0);
        }
    }

    public void Stop(float fadeOutLength = .1f)
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
        CrossFadeSound.CrossFadeClip(audioSource, 0, fadeOutLength, () => audioSource.Stop());
    }
}
