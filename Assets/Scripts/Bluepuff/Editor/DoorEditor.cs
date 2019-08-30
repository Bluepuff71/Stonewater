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
    [CustomEditor(typeof(Door))]
    public class DoorEditor : Editor
    {
        private Door door;
        private void Awake()
        {
            door = (Door)target;
        }
        public void OnSceneGUI()
        {
            if (door.cameraObjs[0])
            {
                Handles.DrawLine(door.transform.position, door.cameraObjs[0].transform.position);
            }
            if (door.cameraObjs[1])
            {
                Handles.DrawLine(door.transform.position, door.cameraObjs[1].transform.position);
            }
        }
        public override void OnInspectorGUI()
        {
            door.behaviour = (DoorBehaviour)EditorGUILayout.EnumPopup("Door Behaviour", door.behaviour);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            switch (door.behaviour)
            {
                case DoorBehaviour.SWITCH_CAMERAS:
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.LabelField("Cameras to switch between.");
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.BeginChangeCheck();
                        Camera[] temp = new Camera[2];
                        temp[0] = EditorGUILayout.ObjectField((door.cameraObjs[0]) ? door.cameraObjs[0].GetComponent<Camera>() : null, typeof(Camera), true) as Camera;
                        temp[1] = EditorGUILayout.ObjectField((door.cameraObjs[1]) ? door.cameraObjs[1].GetComponent<Camera>() : null, typeof(Camera), true) as Camera;
                        if(temp[0] != null && temp[1] != null && temp[0] == temp[1])
                        {
                            Debug.LogError("The selected camera has already been selected. Changes reverted.");
                        }
                        else if(temp[0] != temp[1])
                        {
                            try
                            {
                                door.cameraObjs[0] = temp[0].gameObject;
                            }
                            catch (System.NullReferenceException)
                            {
                                door.cameraObjs[0] = null;
                            }
                            try
                            {
                                door.cameraObjs[1] = temp[1].gameObject;
                            }
                            catch (System.NullReferenceException)
                            {
                                door.cameraObjs[1] = null;
                            }
                        }
                        
                        if (EditorGUI.EndChangeCheck() && !Application.isPlaying)
                        {
                            EditorUtility.SetDirty(door);
                            EditorSceneManager.MarkSceneDirty(door.gameObject.scene);
                        }
                        break;
                    }
                default:
                    break;
            }

        }
    }
}
#endif