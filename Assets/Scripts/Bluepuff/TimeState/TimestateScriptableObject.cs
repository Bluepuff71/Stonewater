using System.Collections.Generic;
using UnityEngine;

namespace Bluepuff.TS
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Timestate", menuName = "Bluepuff/Create Timestate")]
    public class TimestateScriptableObject : ScriptableObject
    {
        public List<string> scenePaths;
    }
}
