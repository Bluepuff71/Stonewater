using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using Bluepuff.TS;
using Bluepuff;

public class TransitionTest : MonoBehaviour
{
    private void Start()
    {
        SoundPlayer.Main.SwitchTape(GetComponent<Tape>(), false).Forget();
        TimestateManager.doTransition = async () => await Transition();
    }
    async UniTask Transition()
    {
        Debug.Log("Transition Start");
        await UniTask.Delay(5000);
        Debug.Log("Transition End");
    }
}
