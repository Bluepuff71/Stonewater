using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;

//The sound player is a way of playing a playlist of tracks, there should be no way to choose which song to play
public class SoundPlayer : MonoBehaviour
{

    public AudioSource audioSource;

    private bool wasStopped = true;
    private Tape tape;

    void Awake()
    {
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public async UniTaskVoid PlayAsync(float fadeInLength = .1f)
    {
        if (audioSource.isPlaying)
        {
            Debug.LogWarning("2 playing Soundplayers attached to the same audio source! Make sure to stop the soundplayer before playing another one!");
        }
        else
        {
            if (tape == null)
            {
                Debug.LogError("No tape was found. Make sure you load a tape using SwitchTape() before you call play!");
            }
            else
            {
                bool firstLoop = true;
                wasStopped = false;
                while (!wasStopped)
                {
                    if (firstLoop)
                    {
                        audioSource.clip = tape.GetCurrentTrack().audioClip;
                        audioSource.time = tape.TrackLocation;
                        audioSource.volume = 0;
                        audioSource.Play();
                        await CrossFadeSound.CrossFadeClipAsync(audioSource, tape.GetCurrentTrack().volume, fadeInLength);
                    }
                    else
                    {
                        audioSource.clip = tape.GetCurrentTrack().audioClip;
                        audioSource.time = 0;
                        audioSource.volume = tape.GetCurrentTrack().volume;
                        audioSource.Play();
                    }
                    await UniTask.WaitWhile(() => audioSource.isPlaying && !wasStopped);
                    if (!wasStopped)
                    {
                        await NextTrack();
                    }
                }
            }
        }
    }

    public void QuickPlay(AudioClipWithVolume track)
    {
        audioSource.Stop();
        audioSource.clip = track.audioClip;
        audioSource.volume = track.volume;
        audioSource.Play();
    }

    public void QuickPlay(AudioClip audioClip)
    {
        QuickPlay(new AudioClipWithVolume(audioClip, 1));
    }

    public async UniTask SwitchTape(Tape toTape, bool playWhenSwitched = true)
    {
        if (!wasStopped)
        {
            await StopAsync();
        }
        //if (continueIfSameTrackExists)
        //{

        //}
        tape = toTape;
        if (playWhenSwitched)
        {
            PlayAsync().Forget();
        }
    }

    private async UniTask NextTrack()
    {
        //should not be called when the soundplayer is stopped
        if (!wasStopped)
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
                    await StopAsync();
                }
            }
            else
            {
                tape.LoadNextTrack();
            }
        }
    }

    public async UniTask StopAsync(float fadeOutLength = .1f)
    {
        if(tape == null)
        {
            Debug.LogError("You tried to stop a tape but no tape exists!");
        }
        else
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
            await CrossFadeSound.CrossFadeClipAsync(audioSource, 0, fadeOutLength);
            if (audioSource)
            {
                audioSource.Stop();
            }
        }
    }
}
