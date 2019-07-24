using UnityEngine;
using UnityEditor;

public class MyPostprocessor : AssetPostprocessor
{
    void OnPostprocessAssetbundleNameChanged(string path,
            string previous, string next)
    {
        Debug.Log("Asset Changed" + path + " old: " + previous + " new: " + next);
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
