using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;

public class TimestateTest : Interactable
{
    private void OnTriggerEnter(Collider other)
    {
        test().Forget();
    }

    [Bluepuff.ContextMenu("Switch Scenes")]
    private async UniTaskVoid test()
    {
        await TimestateManager.SwitchTo(new Timestate("test2", () => { Debug.Log("Test2 Started"); }, () => { Debug.Log("Test2 Finished"); }));
    }
}
