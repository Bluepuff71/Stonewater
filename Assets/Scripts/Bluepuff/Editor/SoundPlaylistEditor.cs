using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
#if UNITY_EDITOR
namespace Bluepuff
{
    [CustomEditor(typeof(SoundPlaylist))]
    public class SoundPlaylistEditor : Editor
    {
        private SoundPlaylist soundPlaylist;
        private bool musicHeaderFoldout = false;

        private void Awake()
        {
            soundPlaylist = (SoundPlaylist)target;
        }

        public override void OnInspectorGUI()
        {
            #region Music Playlist
            //soundPlaylist.audioSource = EditorGUILayout.ObjectField("Audio Source", soundPlaylist.audioSource, typeof(AudioSource), allowSceneObjects: true) as AudioSource;

            musicHeaderFoldout = EditorGUILayout.Foldout(musicHeaderFoldout, "Music");
            if (musicHeaderFoldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < soundPlaylist.tape.GetTrackAmount(); i++)
                {
                    AudioClipWithVolume track = soundPlaylist.tape.GetTrackAt(i);
                    track.audioClip = EditorGUILayout.ObjectField(string.Format("Track {0}", i), track.audioClip, typeof(AudioClip), allowSceneObjects: false) as AudioClip;
                    track.volume = EditorGUILayout.Slider("Volume", track.volume, 0, 1);
                    if (!track.Equals(soundPlaylist.tape.GetTrackAt(i)))
                    {
                        soundPlaylist.tape.SetTrackAt(i, new AudioClipWithVolume(track.audioClip, track.volume));
                    }
                }
                EditorGUILayout.BeginHorizontal();
                if (soundPlaylist.tape.GetTrackAmount() == 0 || soundPlaylist.tape.GetTrackAt(soundPlaylist.tape.GetTrackAmount() - 1).audioClip != null)
                {
                    if (GUILayout.Button("Add Track"))
                    {
                        soundPlaylist.tape.AddTrack(new AudioClipWithVolume(null, 1));
                    }
                }
                if (soundPlaylist.tape.GetTrackAmount() > 0)
                {
                    if (GUILayout.Button("Remove Track"))
                    {
                        soundPlaylist.tape.RemoveLastTrack();
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = 1;
                soundPlaylist.tape.PersistTracks = EditorGUILayout.ToggleLeft("Persist When Stopped?", soundPlaylist.tape.PersistTracks);
                if (soundPlaylist.tape.GetTrackAmount() > 1)
                {
                    soundPlaylist.tape.ShouldLoop = EditorGUILayout.ToggleLeft("Loop Playlist", soundPlaylist.tape.ShouldLoop);
                }
            }
            #endregion
            if (GUI.changed)
            {
                EditorUtility.SetDirty(soundPlaylist);
                EditorSceneManager.MarkSceneDirty(soundPlaylist.gameObject.scene);
            }
        }
    }
}
#endif