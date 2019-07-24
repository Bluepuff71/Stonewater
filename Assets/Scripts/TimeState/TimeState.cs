using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Timestate
{
    public readonly TimestateScriptableObject timestateScriptableObject;
    public readonly Action onStarted;
    public readonly Action onFinished;

    public Timestate(string timeStateName, Action onStarted, Action onFinished)
    {
        this.timestateScriptableObject = Resources.Load<TimestateScriptableObject>(string.Format(@"Timestates\{0}.asset", timeStateName));
        this.onStarted = onStarted;
        this.onFinished = onFinished;
    }
}
