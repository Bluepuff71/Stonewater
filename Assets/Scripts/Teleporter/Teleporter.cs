using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : Interactable
{

    private int numOfPlayersReady;

    private int numOfPlayersAtTelporter;

    public Teleporter connectedTeleporter;

    private Room currentRoom;

    private Room connectingRoom;

    public AudioClip teleportSound;

    public AudioClip arriveSound;

    private SoundPlayer soundPlayer;

    private void Awake()
    {
        currentRoom = GetComponentInParent<Room>();
        connectingRoom = connectedTeleporter.GetComponentInParent<Room>();
        soundPlayer = GetComponent<SoundPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        numOfPlayersAtTelporter++;
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject playerObj;
        if (other.GetComponent<Player>())
        {
            playerObj = other.gameObject;
            Player player = playerObj.GetComponent<Player>();
            if (!playerObj.GetComponent<Player>().readyToTeleport)
            {
                if (!GameData.ui.GetComponentInChildren<Text>().enabled)
                {
                    GameData.ui.GetComponentInChildren<Text>().text = string.Format("To {0}. Press A to ready up ({1}/{2})", connectingRoom.name, numOfPlayersReady, GameData.players.Count);
                    GameData.ui.GetComponentInChildren<Text>().enabled = true;
                }
                if (Input.GetButtonDown(string.Format("CONFIRM_{0}", player.controllerNumber)))
                {
                    player.readyToTeleport = true;
                    numOfPlayersReady++;
                    TeleportIfReady();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        numOfPlayersAtTelporter--;
        if(numOfPlayersAtTelporter == 0)
        {
            GameData.ui.GetComponentInChildren<Text>().enabled = false;
        }
    }

    private void TeleportIfReady()
    {
        if (numOfPlayersReady == GameData.players.Count)
        {
            Teleport();
        }
    }

    [Bluepuff.ContextMenu("Force Teleport")]
    private void Teleport()
    {
        if (connectedTeleporter)
        {
            GameUtils.PerformOnPlayers((player) =>
            {
                player.enabled = false;
            });
            if (teleportSound)
            {
                soundPlayer.QuickPlay(teleportSound);
            }
            if (currentRoom.music != connectingRoom.music)
            {
                GameData.mainSoundPlayer.StopAsync(GameData.globalFadeTime);
            }
            GameUtils.CrossFadeCamera(false, GameData.globalFadeTime, () =>
            {
                numOfPlayersReady = 0;
                currentRoom.ChangeRoom(connectingRoom); //call the other room's changeroom function
                GameData.players.ForEach((player) =>
                {
                    player.readyToTeleport = false;
                    player.transform.position = connectedTeleporter.transform.position;
                });
                if (arriveSound)
                {
                    connectedTeleporter.soundPlayer.QuickPlay(arriveSound);
                }
                //if (currentRoom.music != connectingRoom.music)
                //{
                //    GameData.mainSoundPlayer.Play(GameData.globalFadeInTime);
                //}

                //FADE IN
                GameUtils.CrossFadeCamera(true, GameData.globalFadeTime, () =>
                {
                    GameUtils.PerformOnPlayers((player) =>
                    {
                        player.enabled = true;
                    });
                });
                    
            });
        }
        else
        {
            Debug.LogWarning("No connected teleporter exists!");
        }
    }

    [Bluepuff.ContextMenu("TEst")]
    public void Test()
    {
        Debug.Log("TEst");
    }
}
