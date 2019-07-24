using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public string path { get; set; }
    public bool isActive { get; set; }
    public bool isLoaded { get; set; }

    public SceneSetup()
    {
        this.path = null;
        this.isActive = false;
        this.isLoaded = true;
    }

    public SceneSetup(string path, bool isActive, bool isLoaded)
    {
        this.path = path;
        this.isActive = isActive;
        this.isLoaded = isLoaded;
    }
}
