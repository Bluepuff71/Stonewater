using Bluepuff;
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
    void Start()
    {
        contextPanelPrefab = Resources.Load<GameObject>(@"Prefabs/Context Menu Panel");
        contextButtonPrefab = Resources.Load<Button>(@"Prefabs/Context Menu Button");
        GetComponent<cakeslice.Outline>().eraseRenderer = true;
    }

    void Update()
    {
        if ((Input.GetButtonDown("LeftClick") && !EventSystem.current.IsPointerOverGameObject() && !isOnObject && currentlySelected) || (currentlySelected && !isOnScreen()))
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

    bool isOnScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(currentlySelected.transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    void CreateButtons()
    {
        int i = 0;
        foreach (Interactable interactible in GetComponents<Interactable>())
        {
            //buckle up, here is a crazy lambda function
            Array.ForEach(
                //First Parameter
                Array.FindAll(interactible.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public),
                    (member) =>
                    {
                        return member.GetCustomAttribute<ContextMenuAttribute>() != null;
                    }),
                //Second Parameter
                (memberWithContext) =>
                {
                    contextButton = Instantiate(contextButtonPrefab, contextPanel.transform);
                    contextButton.GetComponentInChildren<Text>().text = memberWithContext.GetCustomAttribute<ContextMenuAttribute>().CommandName;
                    Debug.Log(contextButton.transform.localPosition);
                    contextButton.transform.localPosition = new Vector3(0, (-30f * i) - 35, 0);
                    contextButton.onClick.AddListener(() => interactible.Invoke(memberWithContext.Name, 0));
                    i++;
                });
        }
        contextPanel.GetComponent<RectTransform>().sizeDelta = new Vector3(250, (32 * i) + 25);
    }
}
