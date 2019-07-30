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
    public static async UniTask SwitchTo(Timestate timestate, System.Action transition)
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
        UniTask invokeTransition = UniTask.Run(transition);//invoke the transition event and begin loading the timestate
        UniTask loadTimestate = LoadAsync();
        await UniTask.WhenAll(invokeTransition, loadTimestate); //Wait until they are both done
        await GameUtils.FadeCameraAsync(false, 3, true); //fade out the transition scene
        await SceneManager.UnloadSceneAsync("Transition"); //unload the transition scene
        await GameUtils.RefreshGameData(); //refresh the game data so that it reflects the new scene 
        await GameUtils.FadeCameraAsync(true, 5, true); //fade in the scene
        timestate.onStarted.Invoke(); //invoke the started event
    }

    private static async UniTask LoadAsync()
    {
        foreach (SceneSetup scene in currentTimestate.timestateScriptableObject.sceneSetups)
        {
            if (scene.IsLoaded)
            {
                AsyncOperation sceneLoadTask = SceneManager.LoadSceneAsync(scene.Path, LoadSceneMode.Additive);
                while (!sceneLoadTask.isDone)
                {
                    await UniTask.Yield();
                }
                if (scene.IsActive)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
                }
            }
        }
    }
}
