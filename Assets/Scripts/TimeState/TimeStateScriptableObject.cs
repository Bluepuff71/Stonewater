using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Timestate", menuName = "Bluepuff/Create Timestate")]
public class TimestateScriptableObject : ScriptableObject
{
    public List<SceneSetup> sceneSetups;
}
