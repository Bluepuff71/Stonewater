using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Camera roomCamera;
    //TODO room camera behaivior
    
    public System.Action OnRoomEnter;

    public AudioSource audioSource;
    [HideInInspector]
    public Tape music;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
