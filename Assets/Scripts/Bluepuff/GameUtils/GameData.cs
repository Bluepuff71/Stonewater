using System.Collections.Generic;
using UnityEngine;

namespace Bluepuff.Utils
{
    public class GameData
    {
        public static List<Player> players = new List<Player>();

        public static GameObject ui = GameObject.FindGameObjectWithTag("UI");

        public static SoundPlayer mainSoundPlayer = ui.GetComponent<SoundPlayer>();

        [System.Obsolete]
        public static AudioSource uiAudioSource = ui.GetComponent<AudioSource>();

        public static float globalFadeTime = .5f;
    }
}