using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameUtils
{
    public static Player GetPlayerByNumber(int playerNum)
    {
        return GameData.players.Find((player) =>
        {
            return player.controllerNumber == playerNum;
        });
    }

    public static void PerformOnPlayers(System.Action<Player> action)
    {
        GameData.players.ForEach(action);
    }

    public static List<int> GetUnAvaliableControllers()
    {
        List<int> unavaliableNumbers = new List<int>();
        foreach (Player player in GameData.players)
        {
            if (player.controllerNumber != -1)
            {
                unavaliableNumbers.Add(player.controllerNumber);
            }
        }
        return unavaliableNumbers;
    }

    public static void CrossFadeCamera(bool isFadingIn, float duration, System.Action callback) {
        GameObject fadeImage = GameObject.FindGameObjectWithTag("Fade");
        if (fadeImage)
        {
            fadeImage.GetComponent<Image>().CrossFadeAlphaWithCallBack(isFadingIn ? 0 : 1, duration, callback);
        }
        else
        {
            Debug.LogError("Attempted to start fade but no image was found (Make sure it's tagged)");
        }
    }
}
