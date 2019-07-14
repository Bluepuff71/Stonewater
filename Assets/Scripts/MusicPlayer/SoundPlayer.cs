using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO finish soundplayer, track persistance etc

//The sound player is a way of playing a playlist of tracks, there should be no way to choose which song to play
public class SoundPlayer : MonoBehaviour
{

    public AudioSource audioSource;

    public List<AudioClipWithVolume> tracks;

    #region Track Persistance
    public bool saveTrackLocation;
   // public bool logPositionWhenStopped; MAYBE
    private int trackIndex;
    private float trackLocation;
    private bool wasStopped;
    #endregion

    #region fading
    public float fadeOutTime;
    public float fadeInTime;
    #endregion

    public bool loopPlaylist;

    public bool loopTrack;
    private void Awake()
    {
        wasStopped = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (loopTrack)
            {
                Play(trackIndex);
            }
            else if (loopPlaylist && trackIndex + 1 == tracks.Count)
            {
                Play(0);
            }
            else if (trackIndex + 1 != tracks.Count)
            {
                Play(trackIndex + 1);
            }
        }
    }

    public void Play(int forceIndex = 0)
    {
        if (audioSource.isPlaying)
        {
            Debug.LogWarning("2 playing Soundplayers attached to the same audio source! Make sure to stop the soundplayer before playing another one!");
        }
        else
        {
            if (wasStopped)
            {
                if (saveTrackLocation)
                {
                    SwitchTrack(tracks[trackIndex]);
                }
                else
                {
                    SwitchTrack(tracks[forceIndex]);
                }
                audioSource.volume = 0;
                CrossFadeSound.CrossFadeClip(audioSource, tracks[trackIndex].volume, fadeInTime);
            }
            wasStopped = false;
            audioSource.Play();
        }
    }

    private void SwitchTrack(AudioClipWithVolume to)
    {
        audioSource.clip = to.audioClip;
        if (saveTrackLocation)
        {
            audioSource.time = trackLocation;
        }
        audioSource.volume = to.volume;
        trackIndex = tracks.FindIndex((track) =>
        {
            return track.audioClip == audioSource.clip;
        });
    }

    public void Stop()
    {
        if (saveTrackLocation)
        {
            trackLocation = audioSource.time;
        }
        wasStopped = true;
        CrossFadeSound.CrossFadeClip(audioSource, 0, fadeOutTime, () => audioSource.Stop());
    }
}
