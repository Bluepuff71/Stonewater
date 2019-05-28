using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPlayersView : ModAI {
    bool hasSeenPlayer = false;
    public override bool? AttackTrigger()
    {
        RaycastHit hit;
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.white);
        if (Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit))
        {
            if (IsConnectedToAI(hit.transform) && onScreen)
            {
                hasSeenPlayer = true;
                return true;
            }
            else if ((!onScreen && hasSeenPlayer) || !IsConnectedToAI(hit.transform))
            {
                return false;
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
