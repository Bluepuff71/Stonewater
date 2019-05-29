using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    static GameObject currentlySelected;
    static bool isOnObject;
    GameObject contextPanel;

    void Start()
    {
        GetComponent<cakeslice.Outline>().eraseRenderer = true;
    }

    void Update()
    {
        if(Input.GetButtonDown("LeftClick") && !isOnObject && currentlySelected)
        {
            currentlySelected.GetComponent<cakeslice.Outline>().eraseRenderer = true;
            Destroy(GameObject.FindGameObjectWithTag("ContextMenu"));
        }
    }

    void OnMouseEnter()
    {
        isOnObject = true;
    }

    void OnMouseExit()
    {
        isOnObject = false;
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
        CreateButtons();
    }

    void CreateButtons()
    {
        GameObject contextButton;
        foreach(Interactable interactible in GetComponentsInChildren<Interactable>())
        {
            MemberInfo[] MyMemberInfo = interactible.GetType().GetMethods();
            
            List<ContextMenuAttribute> contextMenuAttributes = new List<ContextMenuAttribute>();
            foreach(MemberInfo memberInfo in MyMemberInfo)
            {
                ContextMenuAttribute contextMenuAttribute = (ContextMenuAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(ContextMenuAttribute));
                if (contextMenuAttribute != null)
                {
                    contextButton = Resources.Load<GameObject>(@"Prefabs/Context Menu Button");
                    contextButton.GetComponentInChildren<Text>().text = contextMenuAttribute.CommandName;
                    Instantiate(contextPanel, GameData.ui.transform);
                }
            }   
        }
    }
}
