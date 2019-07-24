using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "Timestate", menuName = "Bluepuff/Create Timestate")]
public class TimestateScriptableObject : ScriptableObject
{
    public List<SceneSetup> sceneSetups;
}
