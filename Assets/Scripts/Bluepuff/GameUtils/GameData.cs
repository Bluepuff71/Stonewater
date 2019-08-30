using System.Collections.Generic;
using UnityEngine;

namespace Bluepuff.Utils
{
    public class GameData
    {
        public static List<PlayerController> players = new List<PlayerController>();

        public static GameObject ui = GameObject.FindGameObjectWithTag("UI");

        [System.Obsolete("This is deprecated and will be removed in the future use SoundPlayer.Main")]
        public static SoundPlayer mainSoundPlayer = ui.GetComponent<SoundPlayer>();

        [System.Obsolete]
        public static AudioSource uiAudioSource = ui.GetComponent<AudioSource>();

        public static float globalFadeTime = .5f;
    }
}