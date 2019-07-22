using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
//[CustomEditor(typeof(SoundPlayer))]
public class SoundPlayerEditor : Editor
{
    private bool musicHeaderFoldout = false;
    private bool fadeFoldout = false;
    private SoundPlayer soundPlayer;
    private void Awake()
    {
        soundPlayer = (SoundPlayer)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //soundPlayer.audioSource = EditorGUILayout.ObjectField("Audio Source", soundPlayer.audioSource, typeof(AudioSource), allowSceneObjects: true) as AudioSource;
        //if(soundPlayer.audioSource != null)
        //{
        //    #region Music Playlist
        //    musicHeaderFoldout = EditorGUILayout.Foldout(musicHeaderFoldout, "Tape");
        //    if (musicHeaderFoldout)
        //    {
        //        EditorGUI.indentLevel++;
        //        for (int i = 0; i < soundPlayer.tape.tracks.Count; i++)
        //        {
        //            soundPlayer.tracks[i].audioClip = EditorGUILayout.ObjectField(string.Format("Track {0}", i), soundPlayer.tracks[i].audioClip, typeof(AudioClip), allowSceneObjects: false) as AudioClip;
        //            soundPlayer.tracks[i].volume = EditorGUILayout.Slider("Volume", soundPlayer.tracks[i].volume, 0, 1);
        //        }
        //        EditorGUILayout.BeginHorizontal();
        //        if(soundPlayer.tracks.Count == 0 || soundPlayer.tracks[soundPlayer.tracks.Count - 1].audioClip != null)
        //        {
        //            if (GUILayout.Button("Add Track"))
        //            {
        //                soundPlayer.tracks.Add(new AudioClipWithVolume(null, .5f));
        //            }
        //        }
        //        if (soundPlayer.tracks.Count > 0)
        //        {
        //            if (GUILayout.Button("Remove Track"))
        //            {
        //                soundPlayer.tracks.RemoveAt(soundPlayer.tracks.Count - 1);
        //            }
        //        }
        //        EditorGUILayout.EndHorizontal();
        //    }
        //    EditorGUI.indentLevel = 0;
        //    if (soundPlayer.tracks.Count > 0)
        //    {
        //        soundPlayer.loopTrack = EditorGUILayout.ToggleLeft("Loop Track", soundPlayer.loopTrack);
        //        if (soundPlayer.loopTrack)
        //        {
        //            soundPlayer.loopPlaylist = false;
        //        }
        //    }
        //    if (soundPlayer.tracks.Count > 1)
        //    {
        //        soundPlayer.loopPlaylist = EditorGUILayout.ToggleLeft("Loop Playlist", soundPlayer.loopPlaylist);
        //        if (soundPlayer.loopPlaylist)
        //        {
        //            soundPlayer.loopTrack = false;
        //        }
        //    }
        //    //soundPlayer.logPositionWhenStopped = EditorGUILayout.ToggleLeft("Log track when stopped?", soundPlayer.logPositionWhenStopped);
        //    fadeFoldout = EditorGUILayout.Foldout(fadeFoldout, "Fade in/out");
        //    if (fadeFoldout)
        //    {
        //        EditorGUI.indentLevel++;
        //        soundPlayer.fadeInTime = EditorGUILayout.Slider("Fade In Length", soundPlayer.fadeInTime, 0, 5);
        //        soundPlayer.fadeOutTime = EditorGUILayout.Slider("Fade Out Length", soundPlayer.fadeOutTime, 0, 5);
        //        EditorGUI.indentLevel--;
        //    }
        //    soundPlayer.saveTrackLocation = EditorGUILayout.ToggleLeft("Pause music when stopped?", soundPlayer.saveTrackLocation);
        //    #endregion
        //}
    }
}
#endif