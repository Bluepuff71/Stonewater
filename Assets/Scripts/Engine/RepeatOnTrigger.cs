using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Continuously runs OnTriggered instead of once.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Class,AllowMultiple = false)]
public class RepeatOnTrigger : System.Attribute { }
