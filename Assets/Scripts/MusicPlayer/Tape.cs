using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tape
{
    private List<AudioClipWithVolume> tracks;
    public bool ShouldLoop => shouldLoop;
    public bool PersistTracks { get => persistTracks; set => persistTracks = value; }

    public float TrackLocation { get => trackLocation; set => trackLocation = value; }


    #region fading
    public float fadeOutTime;
    public float fadeInTime;
    #endregion

    #region persistance
    private int trackIndex;
    private float trackLocation;
    private bool persistTracks;
    private readonly bool shouldLoop;
    #endregion


    public Tape(List<AudioClipWithVolume> _tracks, bool _shouldLoop = false, bool _persistTracks = false)
    {
        //shallow copy
        this.tracks = _tracks;
        this.shouldLoop = _shouldLoop;
        this.persistTracks = _persistTracks;
        this.trackIndex = -1;
        this.trackLocation = 0;
    }

    public AudioClipWithVolume GetCurrentTrack()
    {
        return tracks[trackIndex];
    }

    public AudioClipWithVolume GetNextTrack()
    {
        if (AtEndOfTape())
        {
            return null;
        } else
        {
            return tracks[trackIndex++];
        }
    }

    public int GetTrackAmount()
    {
        return tracks.Count;
    }

    public bool AtEndOfTape()
    {
        return (trackIndex + 1 == tracks.Count);
    }

    public void RestartTape()
    {
        trackIndex = -1;
        trackLocation = 0;
    }
}
