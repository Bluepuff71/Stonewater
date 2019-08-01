using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Path))]
public class PredefinedPath : ModAI
{
    int currentMarker = 0;
    public bool teleportToBeginningWhenFinished = false;
    public bool stopWhenFinished = false;
    private bool isDone = false;
    public override bool? AttackTrigger()
    {
        throw new System.NotImplementedException();
    }
    public override void Movement()
    {
        Path path = GetComponent<Path>();
        if (currentMarker + 1 == path.pathMarkers.Count)
        {
            if (stopWhenFinished)
            {
                isDone = true;
            }
            else
            {
                if (teleportToBeginningWhenFinished)
                {
                    GetComponent<NavMeshAgent>().gameObject.transform.position = path.pathMarkers[0].position;
                    currentMarker = 1;
                }
                else
                {
                    currentMarker = 0;
                }
            }
        }
        else
        {
            currentMarker += 1;
        }
        if (!isDone)
        {
            GetComponent<NavMeshAgent>().SetDestination(path.pathMarkers[currentMarker].position);
        }
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
