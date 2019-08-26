using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputTest : ModAI {

	public override bool? AttackTrigger()
	{
        Debug.Log("Waiting For Trigger");
        return null;
    }

	public override void Movement()
	{
        Debug.Log("Movement");
    }

	public override void OnLostPlayer()
	{
        Debug.Log("Lost");
    }

	public override void OnTriggered()
	{
        Debug.Log("Triggered");
	}
}
