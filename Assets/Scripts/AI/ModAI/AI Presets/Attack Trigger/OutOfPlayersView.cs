using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfPlayersView : ModAI {

    public override bool? AttackTrigger()
    {
        RaycastHit hit;
        Vector3 _screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool _onScreen = _screenPoint.z > 0 && _screenPoint.x > 0 && _screenPoint.x < 1 && _screenPoint.y > 0 && _screenPoint.y < 1;
        Debug.DrawRay(player.transform.position, transform.position - player.transform.position, Color.white);
        if (Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit))
        {
            if (!IsConnectedToAI(hit.transform) || !_onScreen)
            {
                return true;
            }
            else if (IsConnectedToAI(hit.transform) && _onScreen)
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
