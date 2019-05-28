using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : ModAI
{
    public override bool? AttackTrigger()
    {
        throw new System.NotImplementedException();
    }
    public override void Movement()
    {
        float range = 10.0f;
        Vector3 randomPoint = gameObject.transform.position + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            Path.PlaceDebugMarker(hit.position);
            Debug.DrawRay(gameObject.transform.position, hit.position - gameObject.transform.position);
            gameObject.GetComponent<NavMeshAgent>().SetDestination(hit.position);
        }
        else
        {
            Movement();
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
