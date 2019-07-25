using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public static void Load(Timestate timestate)
    {
        MonoBehaviour monoBehaviour = Camera.main.GetComponent<MonoBehaviour>();
        GameUtils.CrossFadeCamera(false, 5, () =>
        {
            if (currentTimestate != null)
            {
                currentTimestate.onFinished.Invoke();
            }
            currentTimestate = timestate;
            monoBehaviour.StartCoroutine(LoadTransitionCOR());
        });
    }
    //private static IEnumerator LoadTimestateCOR(Timestate timestate)
    //{
        
    //}

    private static IEnumerator LoadTransitionCOR()
    {
        AsyncOperation loadTransitionOperation = SceneManager.LoadSceneAsync("Transition", LoadSceneMode.Single);
        yield return new WaitUntil(() => loadTransitionOperation.isDone);
    }
}
