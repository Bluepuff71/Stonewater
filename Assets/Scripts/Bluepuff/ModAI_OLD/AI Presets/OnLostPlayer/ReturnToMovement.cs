using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMovement : ModAI {

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
        aiController.StartMovementUpdate();
    }

    public override void OnTriggered()
    {
        throw new System.NotImplementedException();
    }
}
