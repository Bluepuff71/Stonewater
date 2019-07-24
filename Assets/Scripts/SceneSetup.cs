using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneSetup
{
    [SerializeField]
    private string path;

    [SerializeField]
    private bool isActive;

    [SerializeField]
    private bool isLoaded;

    public string Path { get => path; set => path = value; }
    public bool IsActive { get => isActive; set => isActive = value; }
    public bool IsLoaded { get => isLoaded; set => isLoaded = value; }

    public SceneSetup()
    {
        this.Path = null;
        this.IsActive = false;
        this.IsLoaded = true;
    }

    public SceneSetup(string path, bool isActive, bool isLoaded)
    {
        this.Path = path;
        this.IsActive = isActive;
        this.IsLoaded = isLoaded;
    }
}
