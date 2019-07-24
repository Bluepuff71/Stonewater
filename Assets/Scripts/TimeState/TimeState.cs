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

    private static AssetBundle timeStateBundle;

    public Timestate(string timeStateName, Action onStarted, Action onFinished)
    {
        if (!timeStateBundle)
        {
            timeStateBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.dataPath, "AssetBundles/timestates"));
        }
        this.timestateScriptableObject = timeStateBundle.LoadAsset<TimestateScriptableObject>(timeStateName);
        if (!timestateScriptableObject)
        {
            Debug.LogWarning("The timestate was not loaded in from the timestate assetbundle. Please insure that it is spelled correctly and is included in the assetbundle.");
        }
        this.onStarted = onStarted;
        this.onFinished = onFinished;
    }
}
