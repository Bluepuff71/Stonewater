using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public static TimeState headTimeState;
    public static void LoadCurrentTimeState()
    {
        headTimeState.StartState();
    }

    public static List<Player> players = new List<Player>();
    public static List<int> GetUnAvaliableControllers()
    {
        List<int> unavaliableNumbers = new List<int>();
        foreach(Player player in players)
        {
            if(player.playerNumber != -1)
            {
                unavaliableNumbers.Add(player.playerNumber);
            }
        }
        return unavaliableNumbers;
    }

    public static GameObject ui = GameObject.FindGameObjectWithTag("UI");

    public static AudioSource uiMusic = ui.GetComponent<AudioSource>();
}
