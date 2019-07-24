using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TimestateCreator : EditorWindow
{
    string timeStateName = "";
    bool useCurrentSceneSetup;
    TimestateScriptableObject timeStateScriptableObject;
    bool sceneFoldout;

    [MenuItem("Bluepuff/Create Timestate")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TimestateCreator));
    }

    private void OnEnable()
    {
        timeStateScriptableObject = CreateInstance<TimestateScriptableObject>();
        timeStateScriptableObject.sceneSetups = new List<SceneSetup>();
        timeStateScriptableObject.sceneSetups.Add(new SceneSetup());
    }
    void OnGUI()
    {
        timeStateName = EditorGUILayout.TextField("Timestate Name", timeStateName);
        useCurrentSceneSetup = EditorGUILayout.ToggleLeft("Use the current scene setup?", useCurrentSceneSetup);
        if (!useCurrentSceneSetup)
        {
            sceneFoldout = EditorGUILayout.Foldout(sceneFoldout, "Scene Setup");
            if (sceneFoldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < timeStateScriptableObject.sceneSetups.Count; i++)
                {
                    EditorGUI.BeginChangeCheck();
                    string tempPath = AssetDatabase.GetAssetPath(EditorGUILayout.ObjectField(string.Format("Scene {0}", i), AssetDatabase.LoadAssetAtPath<SceneAsset>(timeStateScriptableObject.sceneSetups[i].path), typeof(SceneAsset), false) as SceneAsset);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (tempPath == "" && (timeStateScriptableObject.sceneSetups[i].path != "" && timeStateScriptableObject.sceneSetups[i].path != null))
                        {
                            Debug.LogWarning("You cannot set a previous scene to none, that could potentially cause issues. Reverting...");
                        }
                        else if (timeStateScriptableObject.sceneSetups.Exists((scene) => { return scene.path == tempPath; }))
                        {
                            Debug.LogWarning("You have already added that scene. Reverting...");

                        }
                        else if (tempPath != "" && tempPath != null)
                        {
                            timeStateScriptableObject.sceneSetups[i].path = tempPath;
                        }
                    }
                    EditorGUI.BeginDisabledGroup(timeStateScriptableObject.sceneSetups[i].path == null);
                    timeStateScriptableObject.sceneSetups[i].isLoaded = EditorGUILayout.ToggleLeft("Is Loaded", timeStateScriptableObject.sceneSetups[i].isLoaded);
                    EditorGUI.BeginChangeCheck();
                    timeStateScriptableObject.sceneSetups[i].isActive = EditorGUILayout.ToggleLeft("Is Master", timeStateScriptableObject.sceneSetups[i].isActive);
                    if (EditorGUI.EndChangeCheck())
                    {
                        timeStateScriptableObject.sceneSetups.ForEach((scene) =>
                        {
                            if(scene != timeStateScriptableObject.sceneSetups[i])
                            {
                                scene.isActive = false;
                            }
                        });
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(timeStateScriptableObject.sceneSetups[timeStateScriptableObject.sceneSetups.Count - 1].path == null);
                if (GUILayout.Button("Add Scene"))
                {
                    timeStateScriptableObject.sceneSetups.Add(new SceneSetup());
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(timeStateScriptableObject.sceneSetups.Count == 1);
                if (GUILayout.Button("Remove Scene"))
                {
                    timeStateScriptableObject.sceneSetups.RemoveAt(timeStateScriptableObject.sceneSetups.Count - 1);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }
        EditorGUI.BeginDisabledGroup(timeStateName == "" || (!useCurrentSceneSetup && timeStateScriptableObject.sceneSetups[0].path == null));
        if (GUILayout.Button("Create Timestate"))
        {
            if (useCurrentSceneSetup)
            {
                timeStateScriptableObject.sceneSetups = new List<SceneSetup>(
                    Array.ConvertAll(EditorSceneManager.GetSceneManagerSetup(), 
                    (sceneSetup) => {
                        return new SceneSetup(sceneSetup.path, sceneSetup.isActive, sceneSetup.isLoaded); ;
                    }));
            }
            if (!AssetDatabase.IsValidFolder(@"Assets/Timestates"))
            {
                AssetDatabase.CreateFolder("Assets", "Timestates");
            }
            AssetDatabase.CreateAsset(timeStateScriptableObject, AssetDatabase.GenerateUniqueAssetPath(string.Format(@"Assets/Timestates/{0}.asset", timeStateName)));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            this.Close();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = timeStateScriptableObject;
        }
        EditorGUI.EndDisabledGroup();
    }
}
