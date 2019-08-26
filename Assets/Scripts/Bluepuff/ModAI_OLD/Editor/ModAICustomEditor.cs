using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
[CustomEditor(typeof(AI_Controller))]
public class ModAICustomEditor : Editor{
    int? movementCount, attackTrigCount, onTrigCount, onLostCount;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AI_Controller aiController = (AI_Controller)target;
        if (GUILayout.Button("Clear all Components"))
        {
            foreach (ModAI movement in aiController.movement)
            {
                DestroyImmediate(movement, false);
            }
            aiController.movement.Clear();
            foreach (ModAI attackTrigger in aiController.attackTrigger)
            {
                DestroyImmediate(attackTrigger, false);
            }
            aiController.attackTrigger.Clear();
            foreach (ModAI onTrigger in aiController.onTriggered)
            {
                DestroyImmediate(onTrigger, false);
            }
            aiController.onTriggered.Clear();
            foreach (ModAI onLost in aiController.onLostPlayer)
            {
                DestroyImmediate(onLost, false);
            }
            aiController.onLostPlayer.Clear();
            foreach (ModAI script in aiController.GetComponents<ModAI>())
            {
                DestroyImmediate(script, false);
            }
        }
    }
}
#endif
