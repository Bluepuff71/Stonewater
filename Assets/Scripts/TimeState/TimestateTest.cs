using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimestateTest : MonoBehaviour
{
    public void TimestateTesting()
    {
        GameData.timeStateManager.SwitchTo(new Timestate("Test2", () => { Debug.Log("Test2 Started"); }, () => { Debug.Log("Test2 Finished"); }));
    }
}
