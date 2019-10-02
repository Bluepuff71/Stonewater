﻿using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Bluepuff.Utils
{
    public static class GameUtils
    {
        

        public static List<int> GetUnAvaliableControllers()
        {
            List<int> unavaliableNumbers = new List<int>();
            foreach (PlayerController player in PlayerController.players)
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
            if (!fadeImageObj)
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.dataPath, "AssetBundles/prefabs/ui/fade"));
                GameObject fadePrefab = bundle.LoadAsset("Fade Image") as GameObject;
                fadeImageObj = GameObject.Instantiate(fadePrefab, GameData.ui.transform);
                bundle.Unload(false);
            }
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
                    SoundPlayer.Main.PlayAsync(duration).Forget();
                }
                else
                {
                    sound = SoundPlayer.Main.StopAsync(duration);
                }
            }
            await UniTask.WhenAll(crossfade, sound);
        }

        public static async UniTask RefreshGameData()
        {
            GameData.ui = GameObject.FindGameObjectWithTag("UI");
            await UniTask.Yield();
        }
    }
}