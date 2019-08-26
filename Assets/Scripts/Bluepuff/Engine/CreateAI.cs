using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.IO;
#if UNITY_EDITOR
class MyWindow : EditorWindow
{
    [MenuItem("Assets/Create/AIScript", priority = 3)]
    public static void ShowWindow()
    {
        GetWindow(typeof(MyWindow));
    }
    string myString = "NewAIScript";
    private string path;
    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Name: ", myString);
        if (GUILayout.Button("Create"))
        {
            Object obj = Selection.activeObject;
            if (obj == null)
            {
                path = "Assets";
            }
            else
            {
                path = AssetDatabase.GetAssetPath(obj.GetInstanceID());
            }
            if (path.Length > 0)
            {
                if (Directory.Exists(path))
                {
                    Debug.Log("Folder");
                }
                else
                {
                    Debug.Log("File");
                }
            }
            Write(myString);
            AssetDatabase.ImportAsset(path+ "/" + myString + ".cs");
        }
    }
    public void Write(string name)
    {


        File.WriteAllText(path + "/" + name + ".cs",
                       "using System.Collections;\n" +
                       "using System.Collections.Generic;\n" +
                       "using UnityEngine;\n" +
                       "\n" +
                       "public class " + name.Replace(" ", "") + " : MannequinAI {\n" +
                       "\n" +
                       "\tpublic override bool? AttackTrigger()\n" +
                       "\t{\n" +
                       "\t\tthrow new System.NotImplementedException();\n" +
                       "\t}\n" +
                       "\n" +
                       "\tpublic override void Movement()\n" +
                       "\t{\n" +
                       "\t\tthrow new System.NotImplementedException();\n" +
                       "\t}\n" +
                       "\n" +
                       "\tpublic override void OnLostPlayer()\n" +
                       "\t{\n" +
                       "\t\tthrow new System.NotImplementedException();\n" +
                       "\t}\n" +
                       "\n" +
                       "\tpublic override void OnTriggered()\n" +
                       "\t{\n" +
                       "\t\tthrow new System.NotImplementedException();\n" +
                       "\t}\n" +
                       "}\n");
    }
}
#endif