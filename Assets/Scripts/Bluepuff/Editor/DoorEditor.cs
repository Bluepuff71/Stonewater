using System.Collections;
using System.Collections.Generic;
using Bluepuff.TS;
using UnityEngine;
using System;
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
            if (door.teleportObjs[0])
            {
                Handles.DrawLine(door.transform.position, door.teleportObjs[0].transform.position);
            }
            if (door.teleportObjs[1])
            {
                Handles.DrawLine(door.transform.position, door.teleportObjs[1].transform.position);
            }
        }
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Doors cannot be edited while playing.");
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                door.triggerBehaviour = (Door.TriggerBehaviour)EditorGUILayout.EnumPopup("Trigger", door.triggerBehaviour);
                door.behaviour = (Door.Behaviour)EditorGUILayout.EnumPopup("Behaviour", door.behaviour);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                switch (door.behaviour)
                {
                    case Door.Behaviour.SWITCH_CAMERAS:
                        {
                            EditorGUILayout.LabelField("Cameras to switch between.", EditorStyles.boldLabel);
                            EditorGUI.indentLevel++;
                            Camera[] temp = new Camera[2];
                            temp[0] = EditorGUILayout.ObjectField(door.cameraObjs[0] ? door.cameraObjs[0].GetComponent<Camera>() : null, typeof(Camera), true) as Camera;
                            temp[1] = EditorGUILayout.ObjectField(door.cameraObjs[1] ? door.cameraObjs[1].GetComponent<Camera>() : null, typeof(Camera), true) as Camera;
                            try
                            {
                                door.cameraObjs[0] = temp[0].gameObject;
                            }
                            catch (NullReferenceException)
                            {
                                door.cameraObjs[0] = null;
                            }
                            try
                            {
                                door.cameraObjs[1] = temp[1].gameObject;
                            }
                            catch (NullReferenceException)
                            {
                                door.cameraObjs[1] = null;
                            }

                            #region Teleport Player
                            door.TeleportPlayer = EditorGUILayout.ToggleLeft("Teleport Player", door.TeleportPlayer, EditorStyles.boldLabel);
                            if (door.TeleportPlayer)
                            {
                                if (!door.TeleportPlayer)
                                {
                                    DestroyTeleporters();
                                }
                                EditorGUI.indentLevel++;
                                door.teleportObjs[0] = EditorGUILayout.ObjectField(door.teleportObjs[0], typeof(GameObject), true) as GameObject;
                                door.teleportObjs[1] = EditorGUILayout.ObjectField(door.teleportObjs[1], typeof(GameObject), true) as GameObject;
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(EditorGUI.indentLevel * 15);
                                bool autoPlaceButtonPressed = GUILayout.Button("Auto Place Teleport Points");
                                EditorGUILayout.EndHorizontal();
                                if (autoPlaceButtonPressed)
                                {
                                    DestroyTeleporters();
                                    for (int i = 0; i < 2; i++)
                                    {
                                        GameObject teleportObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                                        DestroyImmediate(teleportObj.GetComponent<Collider>());
                                        teleportObj.name = string.Format("{0} Teleporter", door.name);
                                        teleportObj.tag = "EditorOnly";
                                        teleportObj.transform.position = door.transform.position;
                                        teleportObj.transform.parent = door.transform;
                                        Vector3 extents = teleportObj.GetComponent<MeshRenderer>().bounds.extents;
                                        if (Vector3.Distance(teleportObj.transform.position, new Vector3(extents.x, teleportObj.transform.position.y, teleportObj.transform.position.z)) >= Vector3.Distance(teleportObj.transform.position, new Vector3(teleportObj.transform.position.x, teleportObj.transform.position.y, extents.x)))
                                        {
                                            teleportObj.transform.localPosition = door.transform.forward * ((i == 0) ? -2 : 2);
                                        }
                                        else
                                        {
                                            teleportObj.transform.localPosition = door.transform.right * ((i == 0) ? -2 : 2);
                                        }
                                        if (Physics.Raycast(teleportObj.transform.position, -teleportObj.transform.up, out RaycastHit hit, 500))
                                        {
                                            teleportObj.transform.position = new Vector3(teleportObj.transform.position.x, hit.point.y + extents.y, teleportObj.transform.position.z);
                                        }
                                        door.teleportObjs[i] = teleportObj;
                                    }
                                }
                                EditorGUI.indentLevel--;
                            }
                            #endregion
                            EditorGUI.indentLevel--;
                            if (EditorGUI.EndChangeCheck() && !Application.isPlaying)
                            {
                                if ((door.cameraObjs[0] != null && door.cameraObjs[1] != null) && door.cameraObjs[0] == door.cameraObjs[1])
                                {
                                    Debug.LogWarningFormat("The selected cameras for door {0} are the same.", door.name);
                                }
                                EditorUtility.SetDirty(door);
                                EditorSceneManager.MarkSceneDirty(door.gameObject.scene);
                            }
                        }
                        break;
                    case Door.Behaviour.LOAD_TIMESTATE:
                        TimestateScriptableObject timestateSO = null;
                        timestateSO = EditorGUILayout.ObjectField("Timestate", timestateSO, typeof(ScriptableObject), false) as TimestateScriptableObject;

                        break;
                    default:
                        break;
                }
            }
        }
        private void DestroyTeleporters()
        {
            Transform[] childrenTransforms = door.GetComponentsInChildren<Transform>();
            foreach (Transform child in childrenTransforms)
            {
                if (child.name.Contains("Teleporter"))
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}
#endif