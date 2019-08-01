using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModAI : MonoBehaviour
{
    /// <summary>
    /// The Player gameobject.
    /// </summary>
    protected GameObject player;
    /// <summary>
    /// The AI_Controller Script attached to the AI.
    /// </summary>
    protected AI_Controller aiController;
    /// <summary>
    /// The Head of the AI.
    /// </summary>
    public Transform aiHead;


    /// <summary>
    /// Performs a recursive check on a Transform to see if its base is an AI
    /// </summary>
    /// <param name="transform">The Transform to check</param>
    /// <returns>True if it is the AI, False Otherwise</returns>
    protected bool IsConnectedToAI(Transform transform)
    {
        Transform parentOfObject = transform.parent;
        if (parentOfObject == transform)
        {
            if (transform = gameObject.transform)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return IsConnectedToAI(parentOfObject);
        } 
    }
    /// <summary>
    /// This will be invoked once everytime the Mannequin reaches his current destination.
    /// </summary>
    public abstract void Movement();
    /// <summary>
    /// This is the trigger that will signal OnTriggered or OnLostPlayer.
    /// </summary>
    /// <returns>True when triggered, False when the player is lost</returns>
    public abstract bool? AttackTrigger();
    /// <summary>
    /// Called once when AttackTrigger() returns true.
    /// </summary>
    public abstract void OnTriggered();
    /// <summary>
    /// Called once when AttackTrigger() returns false.
    /// </summary>
    public abstract void OnLostPlayer();


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiController = gameObject.GetComponent<AI_Controller>();
        //aiHead = GameObject.FindGameObjectWithTag("Head").transform;
        Debug.Log(aiHead);
        if (aiHead == null)
        {
            Debug.LogWarning("No Gameobject tagged \"aiHead\" was detected for the model. This may cause issues for AI presets that use the head as a casting point.\nFor now the aiHead has been set to the highest parent of the object");
            aiHead = transform;
        }
    }
}