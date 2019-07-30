using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;

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

    private Text teleporterUIText;

    private void Start()
    {
        currentRoom = GetComponentInParent<Room>();
        connectingRoom = connectedTeleporter.GetComponentInParent<Room>();
        soundPlayer = GetComponent<SoundPlayer>();
        teleporterUIText = GameObject.FindGameObjectWithTag("TeleporterText").GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        numOfPlayersAtTelporter++;
        GameObject playerObj;
        if (other.GetComponent<Player>())
        {
            playerObj = other.gameObject;
            Player player = playerObj.GetComponent<Player>();
            if (!player.readyToTeleport)
            {
                if (!teleporterUIText.enabled)
                {
                    teleporterUIText.text = string.Format("To {0}. Press A to ready up ({1}/{2})", connectingRoom.name, numOfPlayersReady, GameData.players.Count);
                    teleporterUIText.enabled = true;
                }
                player.onPressedConfirm.AddListener(async (ply) => await PlayerReady(ply));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        numOfPlayersAtTelporter--;
        GameObject playerObj;
        if (other.GetComponent<Player>())
        {
            playerObj = other.gameObject;
            Player player = playerObj.GetComponent<Player>();
            player.onPressedConfirm.RemoveListener(async (ply) => await PlayerReady(ply));
        }
        if (numOfPlayersAtTelporter == 0)
        {
            teleporterUIText.enabled = false;
        }
    }

    private async UniTask PlayerReady(Player player)
    {
        player.readyToTeleport = true;
        numOfPlayersReady++;
        if (numOfPlayersReady == GameData.players.Count)
        {
            player.onPressedConfirm.RemoveAllListeners();
            await Teleport();
        }
    }

    [Bluepuff.ContextMenu("Force Teleport")]
    private async UniTask Teleport()
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
            if (currentRoom.soundPlaylist != connectingRoom.soundPlaylist)
            {
                await GameData.mainSoundPlayer.StopAsync(GameData.globalFadeTime);
            }
            await GameUtils.FadeCameraAsync(false, teleportSound.length, true);
            numOfPlayersReady = 0;
            await currentRoom.ChangeRoom(connectingRoom); //call the other room's changeroom function
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
            await GameUtils.FadeCameraAsync(true, arriveSound.length, true);
            GameUtils.PerformOnPlayers((player) =>
            {
                player.enabled = true;
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
