using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    static GameObject currentlySelected;
    static bool isOnObject;
    GameObject contextPanel;
    GameObject contextPanelPrefab;
    Button contextButtonPrefab;
    Button contextButton;

    void Awake()
    {
        contextPanelPrefab = Resources.Load<GameObject>(@"Prefabs/Context Menu Panel");
        contextButtonPrefab = Resources.Load<Button>(@"Prefabs/Context Menu Button");
    }
    void Start()
    {

        GetComponent<cakeslice.Outline>().eraseRenderer = true;
    }

    void Update()
    {
        if (Input.GetButtonDown("LeftClick") && !EventSystem.current.IsPointerOverGameObject() && !isOnObject && currentlySelected)
        {
            currentlySelected.GetComponent<cakeslice.Outline>().eraseRenderer = true;
            Destroy(GameObject.FindGameObjectWithTag("ContextMenu"));
        }
    }

    void OnMouseEnter()
    {
        isOnObject = true;

    }
    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (currentlySelected)
            {
                currentlySelected.GetComponent<cakeslice.Outline>().eraseRenderer = true;
                Destroy(GameObject.FindGameObjectWithTag("ContextMenu"));
            }
            currentlySelected = gameObject;
            contextPanel = Instantiate(contextPanelPrefab, GameData.ui.transform) as GameObject;
            contextPanel.GetComponentInChildren<Text>().text = gameObject.name;
            GetComponent<cakeslice.Outline>().eraseRenderer = false;
            CreateButtons();

        }
    }

    void OnMouseExit()
    {
        isOnObject = false;
    }

    void CreateButtons()
    {
        int i = 0;
        foreach (Interactable interactible in GetComponentsInChildren<Interactable>())
        {
            MemberInfo[] MyMemberInfo = interactible.GetType().GetMethods();
            List<ContextMenuAttribute> contextMenuAttributes = new List<ContextMenuAttribute>();

            foreach (MemberInfo memberInfo in MyMemberInfo)
            {

                ContextMenuAttribute contextMenuAttribute = (ContextMenuAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(ContextMenuAttribute));
                if (contextMenuAttribute != null)
                {
                    contextButton = Instantiate(contextButtonPrefab, contextPanel.transform);
                    contextButton.GetComponentInChildren<Text>().text = contextMenuAttribute.CommandName;
                    Debug.Log(contextButton.transform.localPosition);
                    contextButton.transform.localPosition = new Vector3(0, (-30f * i) - 35, 0);
                    contextButton.onClick.AddListener(() => interactible.Invoke(memberInfo.Name, 0));
                    i++;
                }
            }
        }
        contextPanel.GetComponent<RectTransform>().sizeDelta = new Vector3(98.9f, (32 * i) + 20);
    }
}
