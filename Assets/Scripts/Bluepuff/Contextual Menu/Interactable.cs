using Bluepuff.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bluepuff.Contextual
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(cakeslice.Outline))]
    public abstract class Interactable : MonoBehaviour
    {
        static GameObject currentlySelected;
        static bool isOnObject;
        GameObject contextPanel;
        static GameObject contextPanelPrefab;
        public string buttonTrigger = "LeftClick";
        bool trackMouseClicks;
        static GameObject contextButtonPrefab;
        GameObject contextButton;
        static AssetBundle contextBundle;

        void Awake()
        {
            //one line if statement
            trackMouseClicks = buttonTrigger.ToLower().Contains("click") ? true : false;
        }

        void Start()
        {
            GetComponent<cakeslice.Outline>().eraseRenderer = true;
            if (!contextBundle)
            {
                contextBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.dataPath, "AssetBundles/prefabs/ui/contextmenu"));
                contextButtonPrefab = contextBundle.LoadAsset<GameObject>("Context Menu Button");
                contextPanelPrefab = contextBundle.LoadAsset<GameObject>("Context Menu Panel");
            }
        }

        void Update()
        {
            if (trackMouseClicks)
            {
                if ((Input.GetButtonDown(buttonTrigger) && !EventSystem.current.IsPointerOverGameObject() && !isOnObject) || (currentlySelected && !isOnScreen()))
                {
                    DestroyCurrentButtons();
                }
            }
            else if (Input.GetButtonDown(buttonTrigger))
            {
                if (currentlySelected == gameObject)
                {
                    DestroyCurrentButtons();
                }
                else
                {
                    CreateButtons();
                }
            }
        }

        void FixedUpdate()
        {

        }

        void OnMouseEnter()
        {
            isOnObject = true;
        }
        void OnMouseDown()
        {
            if (trackMouseClicks && !EventSystem.current.IsPointerOverGameObject() && gameObject != currentlySelected)
            {
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

        void DestroyCurrentButtons()
        {
            if (currentlySelected)
            {
                currentlySelected.GetComponent<cakeslice.Outline>().eraseRenderer = true;
                Destroy(GameObject.FindGameObjectWithTag("ContextMenu"));
                currentlySelected = null;
            }
        }

        void CreateButtons()
        {
            DestroyCurrentButtons();
            currentlySelected = gameObject;
            contextPanel = Instantiate(contextPanelPrefab, GameData.ui.transform) as GameObject;
            contextPanel.transform.SetAsFirstSibling();
            contextPanel.GetComponentInChildren<Text>().text = gameObject.name;
            GetComponent<cakeslice.Outline>().eraseRenderer = false;

            int i = 0;
            foreach (Interactable interactable in GetComponentsInChildren<Interactable>())
            {
                //buckle up, here is a crazy lambda function
                Array.ForEach(
                    //First Parameter
                    Array.FindAll(interactable.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public),
                        (member) =>
                        {
                            return member.GetCustomAttribute<ContextButtonAttribute>() != null;
                        }),
                    //Second Parameter
                    (memberWithContext) =>
                    {
                        contextButton = Instantiate(contextButtonPrefab, contextPanel.transform);
                        contextButton.GetComponentInChildren<Text>().text = memberWithContext.GetCustomAttribute<ContextButtonAttribute>().ButtonLabel;
                        contextButton.transform.localPosition = new Vector3(0, (-30f * i) - 35, 0);
                        contextButton.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            DestroyCurrentButtons();
                            interactable.Invoke(memberWithContext.Name, 0);
                        });
                        i++;
                    });
            }
            contextPanel.GetComponent<RectTransform>().sizeDelta = new Vector3(250, (32 * i) + 25);
        }
    }
}