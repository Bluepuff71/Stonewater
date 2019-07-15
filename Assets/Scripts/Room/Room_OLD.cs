using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Room_OLD : MonoBehaviour
{
    //CAN BE TOP DOWN VIEW

    public List<AudioClip> roomMusic;

    [HideInInspector]
    public int currentSongIndex;

    [Range(0, 1)]
    public float roomMusicVolume = 1;

    public Camera roomCamera;

    public UnityEvent OnEnterRoom;

    public bool continueMusicWhereLeftOff;

    public bool loopRoomMusic;

    private float musicLocation;

    // Start is called before the first frame update

    void Awake()
    {
        GameData.ui = GameObject.FindGameObjectWithTag("UI");

        GameData.uiMusic.volume = 0;
        musicLocation = 0;
        if (!roomCamera)
        {
            roomCamera = GetComponentInChildren<Camera>();
        }
    }


    void Start()
    {
        if (Camera.main != roomCamera)
        {
            roomCamera.gameObject.tag = "MainCamera";
            roomCamera.gameObject.SetActive(false);
        }
        else
        {
            if (roomMusic.Count != 0)
            {
                GameData.uiMusic.clip = roomMusic[0];
            }
            GameData.uiMusic.volume = roomMusicVolume;
            GameData.uiMusic.PlayDelayed(0);
        }
    }

    private void Update()
    {
        if (roomMusic.Count != 0 && GameData.uiMusic.time == roomMusic[currentSongIndex].length && roomCamera == Camera.main)
        {
            GameData.uiMusic.Stop();
            if (currentSongIndex + 1 == roomMusic.Count)
            {
                if (loopRoomMusic)
                {
                    currentSongIndex = 0;
                    GameData.uiMusic.clip = roomMusic[0];
                }
                else
                {
                    return;
                }
            }
            else
            {
                currentSongIndex++;
                GameData.uiMusic.clip = roomMusic[currentSongIndex];
            }
            GameData.uiMusic.Play();
        }
    }

    public void ChangeRoom(Room_OLD toRoom)
    {
        //CAMERA SWITCHING
        Camera.main.gameObject.SetActive(false);

        //MUSIC SWITCHING
        if (roomMusic[currentSongIndex] != toRoom.roomMusic[currentSongIndex])
        {
            if (continueMusicWhereLeftOff)
            {
                musicLocation = GameData.uiMusic.time;
                currentSongIndex = roomMusic.IndexOf(GameData.uiMusic.clip);
            }
            else
            {
                musicLocation = 0;
                currentSongIndex = 0;
            }
            GameData.uiMusic.Stop();
            GameData.uiMusic.clip = toRoom.roomMusic[toRoom.currentSongIndex];
            GameData.uiMusic.time = toRoom.musicLocation;
            GameData.uiMusic.Play();
        } else
        {
            GameData.uiMusic.CrossFadeClip(toRoom.roomMusicVolume, .5f, () => Debug.Log("FIX THIS"));
        }
        
        //CAMERA SWITCHING
        toRoom.roomCamera.gameObject.SetActive(true);
        OnEnterRoom.Invoke();
    }
}
