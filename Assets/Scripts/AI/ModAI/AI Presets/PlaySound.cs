using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : ModAI {

    public AudioClip audioClip;
    public bool playOnce;
	public override bool? AttackTrigger()
	{
		throw new System.NotImplementedException();
	}

	public override void Movement()
	{
		throw new System.NotImplementedException();
	}

	public override void OnLostPlayer()
	{
        if (playOnce)
        {
            GetComponent<AudioSource>().PlayOneShot(audioClip);
            playOnce = false;
        }
	}

	public override void OnTriggered()
	{
        if (playOnce)
        {
            GetComponent<AudioSource>().PlayOneShot(audioClip);
            playOnce = false;
        }
    }
}
