using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    static GameObject currentlySelected;
    GameObject contextPanel;

    void Start()
    {
        GetComponent<cakeslice.Outline>().eraseRenderer = true;
    } 
    void OnMouseDown()
    {
        if (currentlySelected)
        {
            currentlySelected.GetComponent<cakeslice.Outline>().eraseRenderer = true;
            Destroy(GameObject.FindGameObjectWithTag("ContextMenu"));
        }
        currentlySelected = gameObject;
        contextPanel = Resources.Load<GameObject>(@"Prefabs/Context Menu Panel");
        contextPanel.GetComponentInChildren<Text>().text = gameObject.name;
        GetComponent<cakeslice.Outline>().eraseRenderer = false;
        Instantiate(contextPanel, GameData.ui.transform);
    }
}
