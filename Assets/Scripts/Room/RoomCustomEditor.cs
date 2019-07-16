using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
[CustomEditor(typeof(Room))]
public class RoomCustomEditor : Editor
{
    private Room room;
    private bool musicHeaderFoldout = false;
    private bool fadeFoldout = false;

    private void Awake()
    {
        room = (Room)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        #region Music Playlist
        musicHeaderFoldout = EditorGUILayout.Foldout(musicHeaderFoldout, "Music");
        if (musicHeaderFoldout)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < room.music.GetTrackAmount(); i++)
            {
                AudioClipWithVolume track = room.music.GetTrackAt(i);
                track.audioClip = EditorGUILayout.ObjectField(string.Format("Track {0}", i), track.audioClip, typeof(AudioClip), allowSceneObjects: false) as AudioClip;
                track.volume = EditorGUILayout.Slider("Volume", track.volume, 0, 1);
                if (!track.Equals(room.music.GetTrackAt(i)))
                {
                    room.music.SetTrackAt(i, new AudioClipWithVolume(track.audioClip, track.volume));
                }
            }
            EditorGUILayout.BeginHorizontal();
            if (room.music.GetTrackAmount() == 0 || room.music.GetTrackAt(room.music.GetTrackAmount() - 1).audioClip != null)
            {
                if (GUILayout.Button("Add Track"))
                {
                    room.music.AddTrack(new AudioClipWithVolume(null, .5f));
                }
            }
            if (room.music.GetTrackAmount() > 0)
            {
                if (GUILayout.Button("Remove Track"))
                {
                    room.music.RemoveLastTrack();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel = 1;
            room.music.PersistTracks = EditorGUILayout.ToggleLeft("Persist When Stopped?", room.music.PersistTracks);
            if (room.music.GetTrackAmount() > 1)
            {
                room.music.ShouldLoop = EditorGUILayout.ToggleLeft("Loop Playlist", room.music.ShouldLoop);
            }
            //fadeFoldout = EditorGUILayout.Foldout(fadeFoldout, "Fade in/out");
            //if (fadeFoldout)
            //{
            //    EditorGUI.indentLevel++;
            //    room.music.fadeInTime = EditorGUILayout.Slider("Fade In Length", room.music.fadeInTime, 0, 5);
            //    room.music.fadeOutTime = EditorGUILayout.Slider("Fade Out Length", room.music.fadeOutTime, 0, 5);
            //    EditorGUI.indentLevel--;
            //}
        }
        #endregion
    }
}
#endif