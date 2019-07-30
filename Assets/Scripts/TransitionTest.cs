using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;

public class TransitionTest : MonoBehaviour
{
    private void Start()
    {
        GameData.mainSoundPlayer.SwitchTape(GetComponent<SoundPlaylist>().tape, false).Forget();
        TimestateManager.doTransition = async () => await Transition();
    }
    async UniTask Transition()
    {
        Debug.Log("Transition Start");
        await UniTask.Delay(5000);
        Debug.Log("Transition End");
    }
}
