using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimestateTest : Interactable
{
    private void OnTriggerEnter(Collider other)
    {
        test();
    }

    [Bluepuff.ContextMenu("Switch Scenes")]
    private void test()
    {
        TimestateManager.Load(new Timestate("test2", () => { Debug.Log("Test2 Started"); }, () => { Debug.Log("Test2 Finished"); }));
    }
}
