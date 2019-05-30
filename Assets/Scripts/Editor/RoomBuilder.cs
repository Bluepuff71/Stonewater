using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RoomBuilder : EditorWindow
{
    string roomName = "Room";
    Vector3 cameraPos = Vector3.zero;
    Vector3 cameraRotVector = Vector3.zero;
    Quaternion cameraRotQuad;
    GameObject roomOrganizer;
    AudioClip audioClip = null;


    List<AudioClip> roomMusic = new List<AudioClip>();
    int roomMusicSize = 0;
    float musicVolume = .5f;

    bool musicFoldout;


    [MenuItem("DND World/Create Room")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RoomBuilder));
    }

    void OnGUI()
    {
        GUILayout.Label("Room Settings", EditorStyles.boldLabel);
        roomOrganizer = EditorGUILayout.ObjectField("Room Organizer Object", roomOrganizer, typeof(GameObject), allowSceneObjects: true) as GameObject;
        roomName = EditorGUILayout.TextField("Room Name", roomName);
        GUILayout.Label("Music Settings", EditorStyles.boldLabel);

        musicFoldout = EditorGUILayout.Foldout(musicFoldout, "Room music");

        if (musicFoldout)
        {
            EditorGUI.indentLevel = 2;
            roomMusicSize = EditorGUILayout.DelayedIntField("Size", roomMusicSize);
            if(roomMusicSize > 0)
            {
                if(roomMusicSize != roomMusic.Capacity)
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
            if(roomMusicSize > 0)
            {
                if (GUILayout.Button("Remove Song"))
                {
                    roomMusicSize--;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel = 0;
        musicVolume = EditorGUILayout.Slider("Room Music Volume", musicVolume, 0, 1);

        GUILayout.Label("Camera", EditorStyles.boldLabel);
        cameraPos = EditorGUILayout.Vector3Field("Camera Position", cameraPos);
        cameraRotQuad = Quaternion.Euler(EditorGUILayout.Vector3Field("Camera Rotation", cameraRotVector));
        if (GUILayout.Button("Align With Camera"))
        {
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
            cameraRotVector = SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles;
        }
        GUILayout.Label("Teleporter", EditorStyles.boldLabel);
        if (GUILayout.Button("Create Room"))
        {
            if (roomOrganizer)
            {
                //GameObject room = new GameObject(roomName);
                //room.AddComponent<Room>()
                ////Instantiate(, roomOrganizer.transform);
                //Camera roomCamera = new Camera();
                ////roomCamera.
                ////Instantiate<Camera>()
            }
            else
            {
                Debug.LogError("All fields must be filled out before a room can be generated.");
            }
        }
    }
}
