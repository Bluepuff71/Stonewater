using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx.Async;

[RequireComponent(typeof(SoundPlaylist))]
public class Room : MonoBehaviour
{
    private Camera roomCamera;
    //TODO room camera behaivior
    
    public UnityEvent OnEnterRoom;

    [HideInInspector]
    public SoundPlaylist soundPlaylist;

    // Start is called before the first frame update
    void Start()
    {
        if (!roomCamera)
        {
            roomCamera = GetComponentInChildren<Camera>();
        }
        if (!soundPlaylist)
        {
            soundPlaylist = GetComponent<SoundPlaylist>();
        }
        if (Camera.main != roomCamera)
        {
            roomCamera.gameObject.tag = "MainCamera";
            roomCamera.gameObject.SetActive(false);
        }
        else
        {
            if (soundPlaylist.tape.GetTrackAmount() != 0)
            {
                GameData.mainSoundPlayer.SwitchTape(soundPlaylist.tape).Forget();

            }
        }
    }

    public async UniTask ChangeRoom(Room toRoom)
    {
        //CAMERA SWITCHING
        Camera.main.gameObject.SetActive(false);
        toRoom.roomCamera.gameObject.SetActive(true);

        //MUSIC SWITCHING
        if (soundPlaylist != toRoom.soundPlaylist)
        {
            await GameData.mainSoundPlayer.SwitchTape(toRoom.soundPlaylist.tape, playWhenSwitched: false);
        }
        //else
        //{
        //    GameData.uiAudioSource.CrossFadeClip(toRoom.music.GetCurrentTrack().volume, .5f, () => Debug.Log("FIX THIS"));
        //}

        toRoom.OnEnterRoom.Invoke();
    }
}
