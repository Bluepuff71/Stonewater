using System.Collections;
using System.Collections.Generic;
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
            EditorGUI.BeginChangeCheck();
            door.triggerBehaviour = (TriggerBehaviour)EditorGUILayout.EnumPopup("Trigger", door.triggerBehaviour);
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
            EditorGUI.indentLevel--;
            door.teleportPlayer = EditorGUILayout.BeginToggleGroup("Teleport Player", door.teleportPlayer);
            if (!door.teleportPlayer)
            {
                DestroyTeleporters();
            }
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            door.teleportObjs[0] = EditorGUILayout.ObjectField(door.teleportObjs[0], typeof(GameObject), true) as GameObject;
            door.teleportObjs[1] = EditorGUILayout.ObjectField(door.teleportObjs[1], typeof(GameObject), true) as GameObject;

            if (GUILayout.Button("Auto Place Teleport Points"))
            {
                DestroyTeleporters();
                for (int i = 0; i < 2; i++)
                {
                    GameObject teleportObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    teleportObj.name = string.Format("{0} Teleporter", door.name);
                    teleportObj.tag = "EditorOnly";
                    teleportObj.transform.position = door.transform.position;
                    teleportObj.transform.parent = door.transform;
                    teleportObj.GetComponent<Collider>().isTrigger = true;
                    Vector3 extents = teleportObj.GetComponent<Collider>().bounds.extents;
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
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndToggleGroup();
            EditorGUI.indentLevel--;
            if (EditorGUI.EndChangeCheck() && !Application.isPlaying)
            {
                if((door.cameraObjs[0] != null && door.cameraObjs[1] != null) && door.cameraObjs[0] == door.cameraObjs[1])
                {
                    Debug.LogWarningFormat("The selected cameras for door {0} are the same.", door.name);
                }
                EditorUtility.SetDirty(door);
                EditorSceneManager.MarkSceneDirty(door.gameObject.scene);
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