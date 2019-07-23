using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "Timestate", menuName = "Bluepuff/Create Timestate")]
public class TimeStateScriptableObject : ScriptableObject
{
    public List<SceneSetup> sceneSetups;
}
