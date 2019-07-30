using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class TimestateManager
{
    private static Timestate currentTimestate = null;

    public static System.Func<UniTask> doTransition;
    public static async UniTask SwitchTo(Timestate timestate)
    {
        await GameUtils.FadeCameraAsync(false, 5, true);  //Fade the scene
        if (currentTimestate != null) //Invoke the finished event
        {
            await currentTimestate.onFinished.OnInvokeAsync(new CancellationToken());
        }
        currentTimestate = timestate; //set the current timestate
        await SceneManager.LoadSceneAsync("Transition"); //load the transition scene
        await GameUtils.RefreshGameData(); //refresh the GameData so that it can be referenced properly
        await GameUtils.FadeCameraAsync(true, 5, true); //fade in the scene
        UniTask<List<AsyncOperation>> loadTimestate = LoadAsync();
        await doTransition();
        await loadTimestate;
        List<AsyncOperation> asyncOperations = loadTimestate.Result;
        await GameUtils.FadeCameraAsync(false, 5, true); //fade out the transition scene
        await FinishLoadingAsync(asyncOperations);
        await SceneManager.UnloadSceneAsync("Transition"); //unload the transition scene
        await GameUtils.RefreshGameData(); //refresh the game data so that it reflects the new scene 
        await GameUtils.FadeCameraAsync(true, 5, true); //fade in the scene
        timestate.onStarted.Invoke(); //invoke the started event
    }

    private static async UniTask FinishLoadingAsync(List<AsyncOperation> asyncOperations)
    {
        foreach (AsyncOperation asyncOperation in asyncOperations)
        {
            asyncOperation.allowSceneActivation = true;
        }
        await UniTask.Yield();
    }

    private static async UniTask<List<AsyncOperation>> LoadAsync()
    {
        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        foreach (SceneSetup scene in currentTimestate.timestateScriptableObject.sceneSetups)
        {
            if (scene.IsLoaded)
            {
                AsyncOperation sceneLoadTask = SceneManager.LoadSceneAsync(scene.Path, LoadSceneMode.Additive);
                sceneLoadTask.allowSceneActivation = false;
                while (!Mathf.Approximately(sceneLoadTask.progress, .9f))
                {
                    await UniTask.Yield();
                }
                asyncOperations.Add(sceneLoadTask);
                //if (scene.IsActive)
                //{
                //    SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
                //}
            }
        }
        return asyncOperations;
    }
}
