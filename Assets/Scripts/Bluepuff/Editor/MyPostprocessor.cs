using UnityEngine;
using UnityEditor;
namespace Bluepuff.Utils
{
    public class Postprocessor : AssetPostprocessor
    {
        void OnPostprocessAssetbundleNameChanged(string path,
                string previous, string next)
        {
            Debug.Log("Asset Changed: " + path + " old: " + previous + " new: " + next);
            BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}