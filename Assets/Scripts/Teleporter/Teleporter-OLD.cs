using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteAlways]
[RequireComponent(typeof(AudioSource))]
public class Teleporter_OLD : MonoBehaviour
{

    public AudioClip teleportSound;

    public AudioClip arriveSound;

    [HideInInspector]
    public Room currentRoom;

    [HideInInspector]
    public Room connectingRoom;

    public Teleporter_OLD connectingTeleporter;

    private int numOfPlayersAtTeleporter;

    private AudioSource teleporterAudioSource;

    List<GameObject> arrivalPoints = new List<GameObject>();

    [Range(.3f, 5f)]
    public float forceFadeOutLength = .3f;

    private float fadeOutLength = .3f;

    [Range(.3f, 5f)]
    public float forceFadeInLength = .3f;

    private float fadeInLength = .3f;

    public bool forceFadeLengths;


    // Start is called before the first frame update
    void Start()
    {
        //currentRoom = GetComponentInParent<Room>();
        //connectingRoom = arrivalPoints[0].GetComponentInParent<Room>();
        teleporterAudioSource = GetComponent<AudioSource>();
        if (!forceFadeLengths)
        {
            if (teleportSound)
            {
                fadeOutLength = teleportSound.length;
            }
            if (arriveSound)
            {
                fadeInLength = arriveSound.length;
            }
        }
        else
        {
            fadeOutLength = forceFadeOutLength;
            fadeInLength = forceFadeInLength;
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        //Show UI element to press a button
        if (Input.GetButtonDown("Confirm"))
        {
            numOfPlayersAtTeleporter++;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!GameData.ui.GetComponentInChildren<Text>().enabled && GameData.players.Count > 1)
        {
            GameData.ui.GetComponentInChildren<Text>().text = string.Format("To {0} ({1}/{2})", connectingRoom.name, numOfPlayersAtTeleporter, GameData.players.Count);
            GameData.ui.GetComponentInChildren<Text>().enabled = true;
        }
        if (numOfPlayersAtTeleporter == GameData.players.Count)
        {
            numOfPlayersAtTeleporter--;
            other.enabled = false;

            //AUDIO STUFF
            teleporterAudioSource.clip = teleportSound;
            teleporterAudioSource.Play();
            if (currentRoom.roomMusic[currentRoom.currentSongIndex] != connectingRoom.roomMusic[currentRoom.currentSongIndex])
            {
                GameData.uiMusic.CrossFadeClip(0, fadeOutLength, () => Debug.Log("FIX THIS"));
            }

            GameData.ui.GetComponentInChildren<Image>().CrossFadeAlphaWithCallBack(1, fadeOutLength, delegate
            {
                GetComponentInParent<Room>().ChangeRoom(connectingRoom); //call the other room's changeroom function
                foreach(Player player in GameData.players)
                {
                    player.transform.position = arrivalPoints[player.playerNumber].transform.position;
                }

                //PLAY THE ENTER ROOM NOISE
                if (arriveSound)
                {
                    connectingTeleporter.teleporterAudioSource.clip = arriveSound;
                    connectingTeleporter.teleporterAudioSource.Play();
                }
                if (currentRoom.roomMusic != connectingRoom.roomMusic)
                {
                    GameData.ui.GetComponent<AudioSource>().CrossFadeClip(connectingRoom.roomMusicVolume, fadeInLength, () => Debug.Log("FIX THIS"));
                }

                //FADE IN
                GameData.ui.GetComponentInChildren<Image>().CrossFadeAlphaWithCallBack(0, fadeInLength, delegate
                {
                    other.enabled = true;
                });
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        numOfPlayersAtTeleporter--;
        if(numOfPlayersAtTeleporter == 0)
        {
            GameData.ui.GetComponentInChildren<Text>().enabled = false;
        }
    }
}
