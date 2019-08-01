using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Bluepuff.Utils
{
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


        public static async UniTask FadeCameraAsync(bool isFadingIn, float duration, bool andSound)
        {
            GameObject fadeImageObj = GameObject.FindGameObjectWithTag("Fade");
            if (fadeImageObj)
            {
                Image fadeImage = fadeImageObj.GetComponent<Image>();
                if (isFadingIn)
                {
                    fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
                }
                else
                {
                    fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
                }
                UniTask crossfade = fadeImage.CrossFadeAlphaAsync(isFadingIn ? 0 : 1, duration);
                UniTask sound;
                if (andSound)
                {
                    if (isFadingIn)
                    {
                        GameData.mainSoundPlayer.PlayAsync(duration).Forget();
                    }
                    else
                    {
                        sound = GameData.mainSoundPlayer.StopAsync(duration);
                    }
                }
                await UniTask.WhenAll(crossfade, sound);
            }
            else
            {
                Debug.LogError("Attempted to start fade but no image was found (Make sure it's tagged)");
            }
        }

        public static async UniTask RefreshGameData()
        {
            GameData.ui = GameObject.FindGameObjectWithTag("UI");
            GameData.mainSoundPlayer = GameData.ui.GetComponent<SoundPlayer>();
            await UniTask.Yield();
        }
    }
}