using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
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
        //if(Input.GetButtonDown("LeftClick") && !isOnObject && currentlySelected)
        //{
        //    currentlySelected.GetComponent<cakeslice.Outline>().eraseRenderer = true;
        //    Destroy(GameObject.FindGameObjectWithTag("ContextMenu"));
        //}
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
        contextPanel = Instantiate(Resources.Load<GameObject>(@"Prefabs/Context Menu Panel"), GameData.ui.transform);
        contextPanel.GetComponentInChildren<Text>().text = gameObject.name;
        GetComponent<cakeslice.Outline>().eraseRenderer = false;
        CreateButtons();
    }

    void CreateButtons()
    {
        

        foreach (Interactable interactible in GetComponentsInChildren<Interactable>())
        {
            //interactible.Invoke("Test", 0);
            //contextButton.onClick.AddListener();
            MemberInfo[] MyMemberInfo = interactible.GetType().GetMethods();
            List<ContextMenuAttribute> contextMenuAttributes = new List<ContextMenuAttribute>();
            foreach(MemberInfo memberInfo in MyMemberInfo)
            {
                ContextMenuAttribute contextMenuAttribute = (ContextMenuAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(ContextMenuAttribute));
                if (contextMenuAttribute != null)
                {
                    Button contextButton = Instantiate(Resources.Load<Button>(@"Prefabs/Context Menu Button"), contextPanel.transform);
                    contextButton.GetComponentInChildren<Text>().text = contextMenuAttribute.CommandName;
                    //interactible.Invoke(memberInfo.Name, 0);
                    contextButton.onClick.AddListener(() => interactible.Invoke(memberInfo.Name, 0));
                    
                }
            }   
        }
    }
}
