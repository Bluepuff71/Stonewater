using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMovementType : ModAI {
    public List<ModAI> movement;
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
        aiController.StopMovementUpdate();
        aiController.movement = movement;
        aiController.StartMovementUpdate();
	}

	public override void OnTriggered()
	{
        aiController.StopMovementUpdate();
        aiController.movement = movement;
        aiController.StartMovementUpdate();
    }
}
