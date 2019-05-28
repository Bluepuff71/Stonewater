using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Path))]
public class TeleportToNextMarker : ModAI {

    int currentMarker = 1;
    public bool wrap;
    public override bool? AttackTrigger()
	{
		throw new System.NotImplementedException();
	}

	public override void Movement()
	{
        //Path path = GetComponent<Path>();
        //if (currentMarker + 1 > path.pathMarkers.Count)
        //{
        //    currentMarker = 0;
        //}
        //GetComponent<NavMeshAgent>().Warp(path.pathMarkers[currentMarker].position);
        //if (Vector3.Distance(gameObject.transform.position, path.pathMarkers[currentMarker].position) < 1)
        //{
        //    currentMarker += 1;
        //}
        throw new System.NotImplementedException();
    }

	public override void OnLostPlayer()
	{
        Path path = GetComponent<Path>();
        if (currentMarker + 1 > path.pathMarkers.Count)
        {
            if (wrap)
            {
                currentMarker = 0;
            }
            else
            {
                currentMarker = path.pathMarkers.Count - 1;
            } 
        }
        GetComponent<NavMeshAgent>().Warp(path.pathMarkers[currentMarker].position);
        if (Vector3.Distance(gameObject.transform.position, path.pathMarkers[currentMarker].position) < 1)
        {
            currentMarker += 1;
        }
    }

	public override void OnTriggered()
	{
		throw new System.NotImplementedException();
	}
}
