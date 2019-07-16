using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tape
{
    [SerializeField]
    private List<AudioClipWithVolume> tracks;
    public bool ShouldLoop { get => shouldLoop; set => shouldLoop = value; }
    public bool PersistTracks { get => persistTracks; set => persistTracks = value; }
    public float TrackLocation { get => trackLocation; set => trackLocation = value; }

    #region persistance
    [SerializeField]
    private int trackIndex;
    [SerializeField]
    private float trackLocation;
    [SerializeField]
    private bool persistTracks;
    #endregion
    [SerializeField]
    private bool shouldLoop;


    public Tape(List<AudioClipWithVolume> _tracks, bool _shouldLoop = false, bool _persistTracks = false)
    {
        //deep copy
        this.tracks = new List<AudioClipWithVolume>(_tracks);
        this.shouldLoop = _shouldLoop;
        this.persistTracks = _persistTracks;
        this.trackIndex = 0;
        this.trackLocation = 0;
    }

    public void AddTrack(AudioClipWithVolume track)
    {
        tracks.Add(track);
    }

    public void SetTrackAt(int index, AudioClipWithVolume audioClip)
    {
        if(index > tracks.Count)
        {
            Debug.LogError("The index that you are trying to access is outside the current bounds of the list");
        }
        else
        {
            tracks[index] = audioClip;
        }
    }

    public AudioClipWithVolume GetTrackAt(int index)
    {
        if (index > tracks.Count)
        {
            Debug.LogError("The index that you are trying to access is outside the current bounds of the list");
            return null;
        }
        else
        {
            return tracks[index];
        }
    }

    public void RemoveLastTrack()
    {
        tracks.RemoveAt(tracks.Count - 1);
    }

    public AudioClipWithVolume GetCurrentTrack()
    {
        return tracks[trackIndex];
    }

    public AudioClipWithVolume GetNextTrack(bool shouldPersist = false)
    {
        if (AtEndOfTape())
        {
            return null;
        }
        else if (shouldPersist)
        {
            return tracks[trackIndex++];
        }
        else
        {
            return tracks[trackIndex + 1];
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

    //public void ShuffleTape() MAYBE

    public void RestartTape()
    {
        trackIndex = 0;
        trackLocation = 0;
    }
}
