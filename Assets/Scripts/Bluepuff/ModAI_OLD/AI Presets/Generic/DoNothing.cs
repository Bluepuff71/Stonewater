using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothing : ModAI
{
    public override bool? AttackTrigger()
    {
        return null;
    }

    public override void Movement() { }

    public override void OnLostPlayer() { }

    public override void OnTriggered() { }
}
