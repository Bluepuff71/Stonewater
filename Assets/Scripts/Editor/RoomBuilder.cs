using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoomBuilder : EditorWindow
{
    string roomName = "Room";
    Vector3 cameraPos = Vector3.zero;
    Vector3 cameraRotVector = Vector3.zero;
    Quaternion cameraRotQuad;

    bool isMainCamera = false;
    GameObject roomOrganizer;
    AudioClip audioClip = null;
    bool continueMusicWhereLeftOff = false;
    bool loopRoomMusic = false;

    List<AudioClip> roomMusic = new List<AudioClip>();
    int roomMusicSize = 0;
    float musicVolume = .5f;

    bool musicFoldout;
    enum TeleporterShape
    {
        Box = 0,
        Cylinder = 1,
    }
    TeleporterShape teleporterShape = TeleporterShape.Box;
    Vector3 teleportPos = Vector3.zero;
    AudioClip teleportSound;

    AudioClip arriveSound;

    int numOfArrivalPoints;

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
            continueMusicWhereLeftOff = EditorGUILayout.ToggleLeft("Restart music when entering room?", continueMusicWhereLeftOff);
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
                //Cast a ray to the gound to get the height
            }


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
                roomObj.transform.SetParent(roomOrganizer.transform);
                Room room = roomObj.AddComponent<Room>();
                room.roomMusic = roomMusic;
                room.roomMusicVolume = musicVolume;
                room.continueMusicWhereLeftOff = continueMusicWhereLeftOff;
                room.loopRoomMusic = loopRoomMusic;
                GameObject roomCameraObj = new GameObject(string.Format("{0} Camera", roomName));
                roomCameraObj.transform.SetParent(roomObj.transform);
                Camera roomCamera = roomCameraObj.AddComponent<Camera>();
                roomCamera.transform.position = cameraPos;
                roomCamera.transform.rotation = cameraRotQuad;
                roomCameraObj.AddComponent<AudioListener>();
                roomCamera.gameObject.AddComponent<cakeslice.OutlineEffect>();
                GameObject currentMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                if (isMainCamera)
                {
                    if (currentMainCamera)
                    {
                        currentMainCamera.tag = "";
                    }
                    roomCameraObj.tag = "MainCamera";
                }
                GameObject teleportObj = new GameObject("Teleporter");

            }
            else
            {
                Debug.LogError("All fields must be filled out before a room can be generated.");
            }
        }
    }
}
