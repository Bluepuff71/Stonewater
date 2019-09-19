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
    [CustomEditor(typeof(Tape))]
    public class TapeEditor : Editor
    {
        private Tape tape;
        private void Awake()
        {
            tape = (Tape)target;
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < tape.GetTrackAmount(); i++)
            {
                AudioClipWithVolume track = tape.GetTrackAt(i);
                track.audioClip = EditorGUILayout.ObjectField(string.Format("Track {0}", i), track.audioClip, typeof(AudioClip), allowSceneObjects: false) as AudioClip;
                track.volume = EditorGUILayout.Slider("Volume", track.volume, 0, 1);
                if (!track.Equals(tape.GetTrackAt(i)))
                {
                    tape.SetTrackAt(i, new AudioClipWithVolume(track.audioClip, track.volume));
                }
            }
            EditorGUILayout.BeginHorizontal();
            if (tape.GetTrackAmount() == 0 || tape.GetTrackAt(tape.GetTrackAmount() - 1).audioClip != null)
            {
                if (GUILayout.Button("Add Track"))
                {
                    tape.AddTrack(new AudioClipWithVolume(null, 1));
                }
            }
            if (tape.GetTrackAmount() > 0)
            {
                if (GUILayout.Button("Remove Track"))
                {
                    tape.RemoveLastTrack();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel = 1;
            tape.PersistTracks = EditorGUILayout.ToggleLeft("Pause instead of stopping?", tape.PersistTracks);
            if (tape.GetTrackAmount() > 1)
            {
                tape.ShouldLoop = EditorGUILayout.ToggleLeft("Loop Playlist", tape.ShouldLoop);
            }
            if (EditorGUI.EndChangeCheck() && !Application.isPlaying)
            {
                EditorUtility.SetDirty(tape);
                EditorSceneManager.MarkSceneDirty(tape.gameObject.scene);
            }
        }
    }
}
#endif