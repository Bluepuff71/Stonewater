using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public IEnumerator DoTransition(List<SceneSetup> scenes)
    {
        foreach (SceneSetup scene in scenes)
        {
            if (scene.IsLoaded)
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene.Path, LoadSceneMode.Additive);
                while (!asyncOperation.isDone)
                {
                    GetComponentInChildren<Text>().text = string.Format("Loading Scene: {0}\n{1}% Completed", scene.Path, asyncOperation.progress);
                    yield return null;
                }
                if (scene.IsActive)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
                }
            }
        }
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("Transition");
        while (!unloadOperation.isDone)
        {
            yield return null;
        }
    }
}
