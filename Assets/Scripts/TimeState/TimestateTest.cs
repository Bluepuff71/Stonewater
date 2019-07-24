using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimestateTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameData.timeStateManager.SwitchTo(new Timestate("test2", () => { Debug.Log("Test2 Started"); }, () => { Debug.Log("Test2 Finished"); }));
    }
}
