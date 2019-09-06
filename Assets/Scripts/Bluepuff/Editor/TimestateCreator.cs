using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Bluepuff.TS
{
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
            timeStateScriptableObject.scenePaths = new List<string>();
            timeStateScriptableObject.scenePaths.Add(null);
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
                    for (int i = 0; i < timeStateScriptableObject.scenePaths.Count; i++)
                    {
                        EditorGUI.BeginChangeCheck();
                        string tempPath = AssetDatabase.GetAssetPath(EditorGUILayout.ObjectField((i == 0) ? "Master Scene" : string.Format("Scene {0}", i), AssetDatabase.LoadAssetAtPath<SceneAsset>(timeStateScriptableObject.scenePaths[i]), typeof(SceneAsset), false) as SceneAsset);
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (tempPath == "" && (timeStateScriptableObject.scenePaths[i] != "" && timeStateScriptableObject.scenePaths[i] != null))
                            {
                                Debug.LogWarning("You cannot set a previous scene to none, that could potentially cause issues. Reverting...");
                            }
                            else if (timeStateScriptableObject.scenePaths.Exists((scene) => { return scene == tempPath; }))
                            {
                                Debug.LogWarning("You have already added that scene. Reverting...");

                            }
                            else if (tempPath != "" && tempPath != null)
                            {
                                timeStateScriptableObject.scenePaths[i] = tempPath;
                            }
                        }
                    }
                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginDisabledGroup(timeStateScriptableObject.scenePaths[timeStateScriptableObject.scenePaths.Count - 1] == null);
                    if (GUILayout.Button("Add Scene"))
                    {
                        timeStateScriptableObject.scenePaths.Add(null);
                    }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.BeginDisabledGroup(timeStateScriptableObject.scenePaths.Count == 1);
                    if (GUILayout.Button("Remove Scene"))
                    {
                        timeStateScriptableObject.scenePaths.RemoveAt(timeStateScriptableObject.scenePaths.Count - 1);
                    }
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.BeginDisabledGroup(timeStateName == "" || (!useCurrentSceneSetup && timeStateScriptableObject.scenePaths[0] == null));
            if (GUILayout.Button("Create Timestate"))
            {
                if (useCurrentSceneSetup)
                {
                    timeStateScriptableObject.scenePaths = new List<string>(
                        Array.ConvertAll(EditorSceneManager.GetSceneManagerSetup(),
                        (sceneSetup) => {
                            return sceneSetup.path;
                        }));
                }
                if (!AssetDatabase.IsValidFolder(@"Assets/Timestates"))
                {
                    AssetDatabase.CreateFolder("Assets", "Timestates");
                }
                string timestatePath = AssetDatabase.GenerateUniqueAssetPath(string.Format(@"Assets/Timestates/{0}.asset", timeStateName));
                AssetDatabase.CreateAsset(timeStateScriptableObject, timestatePath);
                AssetImporter.GetAtPath(timestatePath).SetAssetBundleNameAndVariant("timestates", "");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                this.Close();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = timeStateScriptableObject;
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}