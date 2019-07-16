﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData
{
    public static TimeState headTimeState;
    public static void LoadCurrentTimeState()
    {
        headTimeState.StartState();
    }

    public static List<Player> players = new List<Player>();

    public static Player GetPlayerByNumber(int playerNum)
    {
        return players.Find((player) =>
        {
            return player.controllerNumber == playerNum;
        });
    }

    public static void PerformOnPlayers(System.Action<Player> action)
    {
        players.ForEach(action);
    }

    public static List<int> GetUnAvaliableControllers()
    {
        List<int> unavaliableNumbers = new List<int>();
        foreach(Player player in players)
        {
            if(player.controllerNumber != -1)
            {
                unavaliableNumbers.Add(player.controllerNumber);
            }
        }
        return unavaliableNumbers;
    }

    public static GameObject ui = GameObject.FindGameObjectWithTag("UI");

    public static SoundPlayer mainSoundPlayer = ui.GetComponent<SoundPlayer>();

    [System.Obsolete]
    public static AudioSource uiAudioSource = ui.GetComponent<AudioSource>();

    public static float globalFadeInTime = .5f;
    public static float globalFadeOutTime = .5f;

}
