using UnityEngine;
using UnityEngine.Events;

namespace Bluepuff.TS
{
    public class Timestate
    {
        public readonly TimestateScriptableObject timestateScriptableObject;
        public readonly UnityEvent onStarted = new UnityEvent();
        public readonly UnityEvent onFinished = new UnityEvent();

        private static AssetBundle timeStateBundle;

        public Timestate(string timeStateName, UnityAction onStartedAction, UnityAction onFinishedAction)
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
            this.onStarted.AddListener(onStartedAction);
            this.onFinished.AddListener(onFinishedAction);
        }
    }
}