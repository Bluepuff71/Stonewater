using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TimeState : MonoBehaviour
{
    public List<TimeState> subTimeStates;

    protected abstract void onStateStarted();

    protected abstract void onStateEnded();

    private bool hasFinished;

    public void StartState(bool ignoreHasFinished = false)
    {
        if (!hasFinished || ignoreHasFinished)
        {
            onStateStarted();
        }
        foreach(TimeState timeState in subTimeStates)
        {
            timeState.StartState();
        }
        if (!hasFinished || ignoreHasFinished)
        {
            onStateEnded();
            hasFinished = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
