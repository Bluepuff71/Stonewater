using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    GameObject contextPanel;
    void OnMouseDown()
    {
        Destroy(GameObject.FindGameObjectWithTag("ContextMenu"));
        contextPanel = Resources.Load<GameObject>(@"Prefabs/Context Menu Panel");
        Instantiate(contextPanel, GameData.ui.transform);
    }
}
