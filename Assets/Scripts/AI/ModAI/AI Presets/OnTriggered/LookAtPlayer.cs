using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RepeatOnTrigger]
public class LookAtPlayer : ModAI {

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
		throw new System.NotImplementedException();
	}

	public override void OnTriggered()
	{
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, player.transform.position - transform.position, 1,0));
	}
}
