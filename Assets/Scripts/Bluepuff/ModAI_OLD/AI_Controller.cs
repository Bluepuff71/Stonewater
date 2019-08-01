using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Controller : MonoBehaviour
{
    GameObject player;
    public List<ModAI> movement;
    public List<ModAI> attackTrigger;
    public List<ModAI> onTriggered;
    public List<ModAI> onLostPlayer;
    public bool stopMovementOnTriggered = true;
    public bool stopMovementOnLostPlayer;
    public bool lookAroundAfterCompletingMovement = true;
    bool canCallOnTrigger = true;
    bool canCallMovement = true;
    Coroutine movementUpdate;
    Dictionary<Coroutine,ModAI> repeatTriggers = new Dictionary<Coroutine, ModAI>();
    IEnumerator UpdateMovement()
    {
        while (true)
        {
            yield return null;
            if (gameObject.GetComponent<NavMeshAgent>().remainingDistance == 0 && canCallMovement)
            {
                if (lookAroundAfterCompletingMovement)
                {
                    yield return LookAround(new System.Random().Next(0, 6));
                }
                canCallMovement = false;
                foreach (ModAI move in movement)
                {
                    move.Movement();
                }
            }
            else if(gameObject.GetComponent<NavMeshAgent>().remainingDistance != 0 && !canCallMovement)
            {
                canCallMovement = true;
            }
        }
    }

    IEnumerator RepeatTrigger(ModAI aI)
    {   
        while (true)
        {
            yield return null;
            aI.OnTriggered();
        }
    }


    IEnumerator UpdateAIStates()
    {
        while (true)
        {
            yield return null;
            foreach (ModAI trigger in attackTrigger)
            {
                bool? attackTrig = trigger.AttackTrigger();
                if (attackTrig == true && canCallOnTrigger)
                {
                    canCallOnTrigger = false;
                    if (stopMovementOnTriggered)
                    {
                        StopMovementUpdate();
                    }
                    foreach (ModAI onTrig in onTriggered)
                    {
                        if (ShouldRepeat(onTrig.GetType()))
                        {
                            repeatTriggers.Add(StartCoroutine(RepeatTrigger(onTrig)),onTrig);
                        }
                        else
                        {
                            onTrig.OnTriggered();
                        }
                    }
                }
                else if (attackTrig == false && !canCallOnTrigger) //ONLOST
                {
                    if (repeatTriggers.Count != 0)
                    {
                        foreach (Coroutine coroutine in repeatTriggers.Keys)
                        {
                            StopCoroutine(coroutine);
                        }
                        repeatTriggers.Clear();
                    }
                    canCallOnTrigger = true;
                    if (stopMovementOnLostPlayer)
                    {
                        StopMovementUpdate();
                    }
                    foreach (ModAI onLost in onLostPlayer)
                    {
                        onLost.OnLostPlayer();
                    }
                }
            }
        }
    }

    private bool ShouldRepeat(System.Type aIType)
    {
        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(aIType);
        foreach (System.Attribute attr in attrs)
        {
            if (attr is RepeatOnTrigger)
            {
                return true;
            }
        }
        return false;
    }

    private GameObject head;
    IEnumerator LookAround(int numberOfLooks)
    {
        int rand;
        bool lookDirection = true;
        for (int i = 0; i < numberOfLooks; i++)
        {
            rand = new System.Random().Next(0, 45);
            yield return new WaitForSeconds(1f);
            if (lookDirection)
            {
                for (int x = 0; x < rand; x++)
                {
                    yield return new WaitForSeconds(.0001f);
                    head.transform.Rotate(0, 2, 0, Space.Self);
                }
                lookDirection = false;
            }
            else
            {
                for (int x = 0; x < rand; x++)
                {
                    yield return new WaitForSeconds(.0001f);
                    head.transform.Rotate(0, -2, 0, Space.Self);
                }
                lookDirection = true;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        GetComponent<NavMeshAgent>().SetDestination(position);
        if (GetComponent<NavMeshAgent>().isStopped)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    /// <summary>
    /// Starts or resumes the Mannequin's Movement AI
    /// </summary>
    public void StartMovementUpdate()
    {
        if (GetComponent<NavMeshAgent>().isStopped)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
        }
        movementUpdate = StartCoroutine(UpdateMovement());
    }
    /// <summary>
    /// Pauses the Mannequin's Movement AI
    /// </summary>
    public void StopMovementUpdate()
    {
        StopCoroutine(movementUpdate);
        canCallMovement = true;
        GetComponent<NavMeshAgent>().isStopped = true;
    }
    void Start()
    {
        head = GameObject.FindGameObjectWithTag("MannequinHead");
        player = GameObject.FindGameObjectWithTag("Player");
        StartMovementUpdate();
        StartCoroutine(UpdateAIStates());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
