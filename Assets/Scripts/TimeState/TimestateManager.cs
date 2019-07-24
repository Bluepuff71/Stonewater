using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TimestateManager
{
    private Timestate currentTimestate = null;
    public void SwitchTo(Timestate to)
    {
        GameUtils.CrossFade(false, 2, () =>
        {
            if(currentTimestate != null)
            {
                currentTimestate.onFinished.Invoke();
            }
            //load in the Transition scene (it is always there, just unloaded) and play the scene transition
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
            currentTimestate = to;
            currentTimestate.timestateScriptableObject.sceneSetups.ForEach((scene) =>
            {
                SceneManager.LoadSceneAsync(scene.path,LoadSceneMode.Additive);
                if(!scene.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(scene.path);
                }
                else if(scene.isActive)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneUtility.GetBuildIndexByScenePath(scene.path)));
                }
            });
            GameUtils.CrossFade(true, 2, () =>
            {
                currentTimestate.onStarted.Invoke();
            });
        });
    }
}
