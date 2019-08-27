using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx.Async;
using Bluepuff.Utils;

namespace Bluepuff
{
    [RequireComponent(typeof(Tape))]
    public class Room : MonoBehaviour
    {
        public static Room Main
        {
            get
            {
                return Main;
            }
            private set
            {
                Main = value;
            }
        }

        private Camera roomCamera;
        //TODO room camera behaivior

        public UnityEvent OnEnterRoom;

        [HideInInspector]
        public Tape tape;

        // Start is called before the first frame update
        void Start()
        {
            if (!roomCamera)
            {
                roomCamera = GetComponentInChildren<Camera>();
            }
            if (!tape)
            {
                tape = GetComponent<Tape>();
            }
            if (Camera.main != roomCamera)
            {
                roomCamera.gameObject.tag = "MainCamera";
                roomCamera.gameObject.SetActive(false);
            }
            else
            {
                Main = gameObject.GetComponent<Room>(); 
                if (tape.GetTrackAmount() != 0)
                {
                    GameData.mainSoundPlayer.SwitchTape(tape, playWhenSwitched: false).Forget();
                }
            }
        }

        public async UniTask ChangeRoom(Room toRoom)
        {
            //CAMERA SWITCHING
            Camera.main.gameObject.SetActive(false);
            toRoom.roomCamera.gameObject.SetActive(true);

            //MUSIC SWITCHING
            if (tape != toRoom.tape)
            {
                await GameData.mainSoundPlayer.SwitchTape(toRoom.tape, playWhenSwitched: false);
            }
            //else
            //{
            //    GameData.uiAudioSource.CrossFadeClip(toRoom.music.GetCurrentTrack().volume, .5f, () => Debug.Log("FIX THIS"));
            //}

            toRoom.OnEnterRoom.Invoke();
        }
    }
}