using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToLastKnownLocation : ModAI
{
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
        StartCoroutine(MoveMarker());
    }

    public override void OnTriggered()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator MoveMarker()
    {
        yield return new WaitForSeconds(.2f);
        if (!GameObject.Find("Chase Marker"))
        {
            Instantiate(Resources.Load(@"Prefabs/Marker"), player.transform.position, player.transform.rotation).name = "Chase Marker";
        }
        else
        {
            GameObject.Find("Chase Marker").transform.position = player.transform.position;
        }
        aiController.MoveToPosition(GameObject.Find("Chase Marker").transform.position);
    }
}
