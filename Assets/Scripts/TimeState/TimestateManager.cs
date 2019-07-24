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
        GameUtils.CrossFadeCamera(false, 5, () =>
        {
            //TODO switch the audio to the new scene
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
                SceneManager.LoadSceneAsync(scene.Path,LoadSceneMode.Additive);
                if(!scene.IsLoaded)
                {
                    SceneManager.UnloadSceneAsync(scene.Path);
                }
                else if(scene.IsActive)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneUtility.GetBuildIndexByScenePath(scene.Path)));
                }
            });
            GameUtils.CrossFadeCamera(true, 5, () =>
            {
                currentTimestate.onStarted.Invoke();
            });
        });
    }
}
