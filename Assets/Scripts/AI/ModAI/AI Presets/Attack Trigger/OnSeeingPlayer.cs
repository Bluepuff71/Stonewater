using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSeeingPlayer : ModAI {
    public float sightDistance;
    public float angleLimit;
    private bool hasSeenPlayer = false;
    public override bool? AttackTrigger()
    {
        Vector3 targetDir = player.transform.position - aiHead.position;
        float angle = Vector3.Angle(targetDir, aiHead.forward);
        RaycastHit hit;
        if (angle < angleLimit)
        {
            Debug.DrawRay(aiHead.position, player.transform.position - aiHead.position);
            if (Physics.Raycast(aiHead.transform.position, player.transform.position - aiHead.position, out hit, sightDistance))
            {
                if (hit.transform == player.transform)
                {
                    hasSeenPlayer = true;
                    return true;
                }
                else if (hasSeenPlayer)
                {
                    return false;
                }
            }
        }
        return null;
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
        throw new System.NotImplementedException();
    }
}
