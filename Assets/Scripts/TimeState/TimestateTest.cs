using UniRx.Async;
using Bluepuff.Timestate;
using Bluepuff.Contextual;
using UnityEngine;

public class TimestateTest : Interactable
{
    private void OnTriggerEnter(Collider other)
    {
        test().Forget();
    }

    [Bluepuff.Contextual.ContextMenu("Switch Scenes")]
    private async UniTaskVoid test()
    {
        await TimestateManager.SwitchTo(new Timestate("test2", () => { Debug.Log("Test2 Started"); }, () => { Debug.Log("Test2 Finished"); }));
    }
}
