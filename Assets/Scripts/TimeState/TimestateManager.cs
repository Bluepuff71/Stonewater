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
        AsyncOperation loadTransition = SceneManager.LoadSceneAsync("Transition"); //Begin loading the transition scene
        loadTransition.allowSceneActivation = false;
        await GameUtils.FadeCameraAsync(false, 5, true);  //Fade the scene
        if (currentTimestate != null) //Invoke the finished event
        {
            await currentTimestate.onFinished.OnInvokeAsync(new CancellationToken());
        }
        currentTimestate = timestate; //set the current timestate

        //Finishing up the transition scene
        await UniTask.WaitUntil(() => Mathf.Approximately(loadTransition.progress, .9f));
        loadTransition.allowSceneActivation = true;
        await loadTransition;


        await GameUtils.RefreshGameData(); //refresh the GameData so that it can be referenced properly
        UniTask<List<AsyncOperation>> loadTimestate = LoadAsync(); //start loading the scene
        await GameUtils.FadeCameraAsync(true, 5, true); //fade in the transition
        await doTransition(); //invoke transition
        await loadTimestate;
        List<AsyncOperation> asyncOperations = loadTimestate.Result;
        await GameUtils.FadeCameraAsync(false, 5, true); //fade out the transition scene
        await FinishLoadingAsync(asyncOperations);
        await GameUtils.RefreshGameData(); //refresh the game data so that it reflects the new scene 
        await GameUtils.FadeCameraAsync(true, 5, true); //fade in the scene
        timestate.onStarted.Invoke(); //invoke the started event
    }

    private static async UniTask FinishLoadingAsync(List<AsyncOperation> asyncOperations)
    {
        foreach (AsyncOperation asyncOperation in asyncOperations)
        {
            asyncOperation.allowSceneActivation = true;
            await asyncOperation;
        }
    }

    private static async UniTask<List<AsyncOperation>> LoadAsync()
    {
        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        List<string> scenePaths = currentTimestate.timestateScriptableObject.scenePaths;
        for (int i = 0; i < scenePaths.Count; i++)
        {
            AsyncOperation sceneLoadTask;
            if (i == 0)
            {
                sceneLoadTask = SceneManager.LoadSceneAsync(scenePaths[0], LoadSceneMode.Single);
            }
            else
            {
                sceneLoadTask = SceneManager.LoadSceneAsync(scenePaths[i], LoadSceneMode.Additive);
            }
            sceneLoadTask.allowSceneActivation = false;
            await UniTask.WaitUntil(() => Mathf.Approximately(sceneLoadTask.progress, .9f));
            asyncOperations.Add(sceneLoadTask);
        }
        return asyncOperations;
    }
}
