using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    private Camera roomCamera;
    //TODO room camera behaivior
    
    public UnityEvent OnEnterRoom;

    private AudioSource audioSource;
    [HideInInspector]
    public Tape music;

    void Awake()
    {
        if (!roomCamera)
        {
            roomCamera = GetComponentInChildren<Camera>();
        }
        if (!audioSource)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Camera.main != roomCamera)
        {
            roomCamera.gameObject.tag = "MainCamera";
            roomCamera.gameObject.SetActive(false);
        }
        else
        {
            if (music.GetTrackAmount() != 0)
            {
                GameData.mainSoundPlayer.Tape = music;
            }
            GameData.mainSoundPlayer.Play();
        }
    }

    public void ChangeRoom(Room toRoom)
    {
        //CAMERA SWITCHING
        Camera.main.gameObject.SetActive(false);
        toRoom.roomCamera.gameObject.SetActive(true);
        GameData.mainSoundPlayer = toRoom.roomCamera.gameObject.GetComponent<SoundPlayer>();

        //MUSIC SWITCHING
        if (music != toRoom.music)
        {
            GameData.mainSoundPlayer.Tape = toRoom.music;
            GameData.mainSoundPlayer.Play();
        }
        //else
        //{
        //    GameData.uiAudioSource.CrossFadeClip(toRoom.music.GetCurrentTrack().volume, .5f, () => Debug.Log("FIX THIS"));
        //}

        toRoom.OnEnterRoom.Invoke();
    }
}
