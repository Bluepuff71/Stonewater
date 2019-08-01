using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
[CustomEditor(typeof(Path))]
public class PathCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Path path = (Path)target;
        if (GUILayout.Button("New Path Marker"))
        {
            path.NewMarker();
        }
        if (GUILayout.Button("Remove Last Marker"))
        {
            path.RemoveMarker(path.pathMarkers.Count - 1);
        }
        if (GUILayout.Button("Reset Mannequin Location"))
        {
            path.transform.position = path.pathMarkers[0].position;
        }
        if (GUILayout.Button("Reset Marker 0 Location"))
        {
            path.pathMarkers[0].position = path.transform.position;
        }
        DrawDefaultInspector();
    }
}
#endif
