using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class TimestateManager
{
    private static Timestate currentTimestate = null;
    //public static void SwitchTo(Timestate to)
    //{
    //    GameUtils.CrossFadeCamera(false, 5, () =>
    //    {
    //        //TODO switch the audio to the new scene
    //        if(currentTimestate != null)
    //        {
    //            currentTimestate.onFinished.Invoke();
    //        }
    //        currentTimestate = to;

    //        Transition transition = GameObject.Find("Canvas").GetComponentInChildren<Transition>();
    //        transition.StartCoroutine(transition.DoTransition(currentTimestate.timestateScriptableObject.sceneSetups));
    //        GameUtils.CrossFadeCamera(true, 5, () =>
    //        {
    //            currentTimestate.onStarted.Invoke();
    //        });
    //    });
    //}

    //private static IEnumerator LoadTransitionScene()
    //{
    //    AsyncOperation loadTransitionOperation = SceneManager.LoadSceneAsync("Transition", LoadSceneMode.Single);
    //    yield return WaitUntil(loadTransitionOperation.isDone);
    //}


        //TODO Refresh gamedata on new scene loaded
    public static async UniTask LoadAsync(Timestate timestate)
    {
        await GameUtils.CrossFadeCameraAsync(false, 5, true);
        if (currentTimestate != null)
        {
            currentTimestate.onFinished.Invoke();
        }
        currentTimestate = timestate;
        await SceneManager.LoadSceneAsync("Transition");
        await GameUtils.RefreshGameData();
        await GameUtils.CrossFadeCameraAsync(true, 3, true);
        foreach (SceneSetup scene in currentTimestate.timestateScriptableObject.sceneSetups)
        {
            if (scene.IsLoaded)
            {
                AsyncOperation sceneLoadTask = SceneManager.LoadSceneAsync(scene.Path, LoadSceneMode.Additive);
                while (!sceneLoadTask.isDone)
                {
                    GameData.ui.GetComponentInChildren<Text>().text = string.Format("Loading Scene: {0}\n{1}% Completed", scene.Path, sceneLoadTask.progress * 100);
                    await UniTask.Yield();
                }
                if (scene.IsActive)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
                }
            }
        }
        await GameUtils.CrossFadeCameraAsync(false, 3, true);
        await SceneManager.UnloadSceneAsync("Transition");
        await GameUtils.RefreshGameData();
        await GameUtils.CrossFadeCameraAsync(true, 5, true);
    }
}
