using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoomBuilder : EditorWindow
{
    string roomName = "Room";
    Vector3 roomPos = Vector3.zero;
    Vector3 cameraPos = Vector3.zero;
    Vector3 cameraRotVector = Vector3.zero;
    Quaternion cameraRotQuad = new Quaternion();

    bool isMainCamera = false;
    GameObject roomOrganizer;
    AudioClip audioClip = null;
    bool continueMusicWhereLeftOff = false;
    bool loopRoomMusic = false;

    List<AudioClip> roomMusic = new List<AudioClip>();
    int roomMusicSize = 0;
    float musicVolume = .5f;

    bool musicFoldout = true;
    enum TeleporterShape
    {
        Box = 0,
        Cylinder = 1,
    }
    TeleporterShape teleporterShape = TeleporterShape.Box;
    Vector3 teleportPos = Vector3.zero;
    float teleporterHeight;
    AudioClip teleportSound = null;

    AudioClip arriveSound = null;
    Teleporter_OLD connectingTeleporter = null;
    int numOfArrivalPoints = 1;

    bool customFadeLength = false;

    float customFadeOutLength = 1;
    float customFadeInLength = 1;

    bool roomHeadFoldout = false;
    bool musicHeadFoldout = false;
    bool cameraHeadFoldout = false;
    bool teleportHeadFoldout = false;

    [MenuItem("DND World/Create Room")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RoomBuilder));
    }

    void OnGUI()
    {
        roomHeadFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(roomHeadFoldout, "Room Settings");
        if (roomHeadFoldout)
        {
            EditorGUI.indentLevel++;
            roomOrganizer = EditorGUILayout.ObjectField("Room Organizer", roomOrganizer, typeof(GameObject), allowSceneObjects: true) as GameObject;
            roomName = EditorGUILayout.TextField("Room Name", roomName);
            roomPos = EditorGUILayout.Vector3Field("Room Base Position", roomPos);
            if (GUILayout.Button("Align With Camera"))
            {
                roomPos = SceneView.lastActiveSceneView.camera.transform.position;
            }
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        musicHeadFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(musicHeadFoldout, "Music Settings");
        if (musicHeadFoldout)
        {

            EditorGUI.indentLevel++;
            musicFoldout = EditorGUILayout.Foldout(musicFoldout, "Room music");
            if (musicFoldout)
            {
                EditorGUI.indentLevel++;
                roomMusicSize = EditorGUILayout.DelayedIntField("Size", roomMusicSize);
                if (roomMusicSize > 0)
                {
                    if (roomMusicSize != roomMusic.Capacity)
                    {
                        roomMusic = new List<AudioClip>(new AudioClip[roomMusicSize]);
                    }
                    for (int i = 0; i < roomMusicSize; i++)
                    {
                        roomMusic[i] = EditorGUILayout.ObjectField(string.Format("Song {0}", i), roomMusic[i], typeof(AudioClip), allowSceneObjects: false) as AudioClip;
                    }
                }
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Song"))
                {
                    roomMusicSize++;
                }
                if (roomMusicSize > 0)
                {
                    if (GUILayout.Button("Remove Song"))
                    {
                        roomMusicSize--;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
            musicVolume = EditorGUILayout.Slider("Room Music Volume", musicVolume, 0, 1);
            continueMusicWhereLeftOff = EditorGUILayout.ToggleLeft("Continue Music From Last Point?", continueMusicWhereLeftOff);
            loopRoomMusic = EditorGUILayout.ToggleLeft("Loop the room music?", loopRoomMusic);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        cameraHeadFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(cameraHeadFoldout, "Camera Settings");

        if (cameraHeadFoldout)
        {
            EditorGUI.indentLevel++;
            cameraPos = EditorGUILayout.Vector3Field("Camera Position", cameraPos);
            cameraRotQuad = Quaternion.Euler(EditorGUILayout.Vector3Field("Camera Rotation", cameraRotVector));
            if (GUILayout.Button("Align With Camera"))
            {
                cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
                cameraRotVector = SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles;
            }
            isMainCamera = EditorGUILayout.ToggleLeft("Should this be the main camera?", isMainCamera);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        teleportHeadFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(teleportHeadFoldout, "Teleporter Settings");

        if (teleportHeadFoldout)
        {
            EditorGUI.indentLevel++;
            teleporterShape = (TeleporterShape) EditorGUILayout.EnumPopup("Teleporter Shape", teleporterShape);
            teleportPos = EditorGUILayout.Vector3Field("Teleporter Position", teleportPos);
            if (GUILayout.Button("Align With Camera"))
            {
                teleportPos = SceneView.lastActiveSceneView.camera.transform.position;
                RaycastHit hit;
                Physics.Raycast(SceneView.lastActiveSceneView.camera.transform.position, Vector3.down, out hit, 200);
                teleporterHeight = hit.distance;
            }
            connectingTeleporter = EditorGUILayout.ObjectField("Connecting Teleporter", connectingTeleporter, typeof(Teleporter_OLD), true) as Teleporter_OLD;

            teleportSound = EditorGUILayout.ObjectField("Teleport Sound", teleportSound, typeof(AudioClip), allowSceneObjects: false) as AudioClip;
            arriveSound = EditorGUILayout.ObjectField("Arrive Sound", arriveSound, typeof(AudioClip), allowSceneObjects: false) as AudioClip;
            numOfArrivalPoints = EditorGUILayout.IntSlider("# of Arrival Points", numOfArrivalPoints, 1, 6);

            customFadeLength = EditorGUILayout.BeginToggleGroup("Custom Fade Lengths?", customFadeLength);

            if (customFadeLength)
            {
                customFadeOutLength = EditorGUILayout.Slider("Fade Out Length", customFadeOutLength, .3f, 10);
                customFadeInLength = EditorGUILayout.Slider("Fade In Length", customFadeInLength, .3f, 10);
            }
            EditorGUILayout.EndToggleGroup();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        if (GUILayout.Button("Create Room"))
        {
            if (roomOrganizer)
            {
                GameObject roomObj = new GameObject(roomName);
                roomObj.transform.position = roomPos;
                roomObj.transform.SetParent(roomOrganizer.transform);
                Room_OLD room = roomObj.AddComponent<Room_OLD>();
                room.roomMusic = roomMusic;
                room.roomMusicVolume = musicVolume;
                room.continueMusicWhereLeftOff = continueMusicWhereLeftOff;
                room.loopRoomMusic = loopRoomMusic;
                GameObject roomCameraObj = new GameObject(string.Format("{0} Camera", roomName));
                roomCameraObj.transform.SetParent(roomObj.transform);
                Camera roomCamera = roomCameraObj.AddComponent<Camera>();
                int layerMask = 1 << 2;
                layerMask = ~layerMask;
                roomCamera.cullingMask = layerMask;
                roomCamera.transform.position = cameraPos;
                roomCamera.transform.rotation = cameraRotQuad;
                roomCameraObj.AddComponent<AudioListener>();
                roomCamera.gameObject.AddComponent<cakeslice.OutlineEffect>();
                GameObject currentMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                if (isMainCamera)
                {
                    if (currentMainCamera)
                    {
                        currentMainCamera.tag = "Untagged";
                    }
                    roomCameraObj.tag = "MainCamera";
                }
                GameObject teleporterObj;
                switch (teleporterShape)
                {
                    case TeleporterShape.Box:
                        teleporterObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        break;
                    case TeleporterShape.Cylinder:
                        teleporterObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        break;
                    default:
                        teleporterObj = new GameObject("Broke");
                        break;
                }
                teleporterObj.layer = 2;
                teleporterObj.transform.position = teleportPos;
                teleporterObj.transform.SetParent(roomObj.transform);
                teleporterObj.GetComponent<Collider>().isTrigger = true;
                Teleporter_OLD teleporter = teleporterObj.AddComponent<Teleporter_OLD>();

                teleporter.connectingTeleporter = connectingTeleporter;

                if (teleporter.connectingTeleporter)
                {
                    Debug.Log(teleporter.GetComponentInParent<Room_OLD>().gameObject.name);
                    teleporter.gameObject.name = string.Format("{0} To {1}", teleporter.GetComponentInParent<Room_OLD>().gameObject.name, teleporter.arrivalPoints[0].GetComponentInParent<Room_OLD>().gameObject.name);
                    //IF THE TELEPORTER I AM CONNECTING TO WAS THE FIRST ROOM (SO IT DIDN'T HAVE A CONNECTING TELEPORTER) CONNECT IT TO ME
                    if (!teleporter.connectingTeleporter.connectingTeleporter)
                    {
                        teleporter.connectingTeleporter.connectingTeleporter = teleporter;
                        teleporter.connectingTeleporter.gameObject.name = string.Format("{0} To {1}", teleporter.connectingTeleporter.currentRoom.gameObject.name, teleporter.currentRoom.gameObject.name);
                    }
                }

                teleporter.teleportSound = teleportSound;
                teleporter.arriveSound = arriveSound;

                teleporterObj.transform.localScale = new Vector3(1, teleporterHeight * 2, 1);
                Vector3 spawnDir = teleporterObj.transform.TransformPoint(DirectionToSpawn(teleporterObj.transform) * 4);
                Debug.Log(spawnDir);
                GameObject arrivalPointPrefab = Resources.Load(@"Prefabs/ArrivalPoint") as GameObject;
                for (int i = 0; i < numOfArrivalPoints; i++)
                {
                    GameObject arrivalPoint = Instantiate(arrivalPointPrefab, teleporterObj.transform, true);
                    arrivalPoint.transform.position = new Vector3(spawnDir.x + (i * .5f), teleporterObj.GetComponent<Renderer>().bounds.min.y + arrivalPoint.GetComponent<Renderer>().bounds.extents.y, spawnDir.z); 
                }
                teleporter.forceFadeLengths = customFadeLength;
                teleporter.forceFadeOutLength = customFadeOutLength;
                teleporter.forceFadeInLength = customFadeInLength;

                //learAllValues();
            }
            else
            {
                Debug.LogError("All fields must be filled out before a room can be generated.");
            }
        }
    }

    private Vector3 DirectionToSpawn(Transform start)
    {
        RaycastHit hitForward;
        RaycastHit hitLeft;
        RaycastHit hitRight;
        Physics.Raycast(start.position, start.forward, out hitForward, 10);
        Physics.Raycast(start.position, -start.right, out hitLeft, 10);
        Physics.Raycast(start.position, start.right, out hitRight, 10);
        if (hitForward.distance > hitLeft.distance && hitForward.distance > hitRight.distance)
        {
            return start.forward;
        }
        else if (hitLeft.distance > hitForward.distance && hitLeft.distance > hitRight.distance)
        {
            return -start.right;
        }
        else if (hitRight.distance > hitForward.distance && hitRight.distance > hitLeft.distance)
        {
            return start.right;
        }
        else
        {
            return -start.forward;
        }
    }

    private void ClearAllValues()
    {
        roomName = "Room";
        roomPos = Vector3.zero;
        cameraPos = Vector3.zero;
        cameraRotVector = Vector3.zero;
        cameraRotQuad = new Quaternion();

        isMainCamera = false;
        roomOrganizer = null;
        audioClip = null;
        continueMusicWhereLeftOff = false;
        loopRoomMusic = false;

        roomMusic = new List<AudioClip>();
        roomMusicSize = 0;
        musicVolume = .5f;

        musicFoldout = true;
        teleporterShape = TeleporterShape.Box;
        teleportPos = Vector3.zero;
        teleporterHeight = 0;
        teleportSound = null;

        arriveSound = null;
        connectingTeleporter = null;
        numOfArrivalPoints = 1;

        customFadeLength = false;

        customFadeOutLength = 1;
        customFadeInLength = 1;

        roomHeadFoldout = false;
        musicHeadFoldout = false;
        cameraHeadFoldout = false;
    }
}
