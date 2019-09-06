using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bluepuff
{

    public enum Door_TriggerBehaviour
    {
        OnExit,
        OnEnter
    }

    public enum Door_Behaviour
    {
        SWITCH_CAMERAS,
        LOAD_TIMESTATE
    }
    [ExecuteAlways]
    public class Door : MonoBehaviour
    {
        //Camera switcher editor will handle selecting the cameras
        public GameObject[] cameraObjs = new GameObject[2];

        public GameObject[] teleportObjs = new GameObject[2];

        public bool teleportPlayer;

        [Tooltip("When will the door run it's camera/teleport logic?")]
        public Door_TriggerBehaviour triggerBehaviour;

        public Door_Behaviour behaviour;

        private Dictionary<PlayerController, Vector3> entryPosDict = new Dictionary<PlayerController, Vector3>();

        // Start is called before the first frame update
        void Start()
        {
            gameObject.layer = 9;
            gameObject.tag = "EditorOnly";
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            if (meshCollider)
            {
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
                Debug.LogWarning("Mesh Colliders cannot act as triggers. The mesh collider has been converted into a convex collider.");
            }
            else
            {
                GetComponent<Collider>().isTrigger = true;
            }
            if (Application.isPlaying)
            {
                
                //This "bakes" all of the cameras
                Array.ForEach(cameraObjs, (cameraObj) =>
                {
                    GameObject bakedCamera = new GameObject("-- Baked Camera --");
                    bakedCamera.transform.position = cameraObj.transform.position;
                    bakedCamera.transform.rotation = cameraObj.transform.rotation;
                    bakedCamera.transform.localScale = cameraObj.transform.localScale;
                    if (!cameraObj.tag.Equals("MainCamera"))
                    { 
                        Destroy(cameraObj);
                    }
                });
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Application.isPlaying)
            {
                PlayerController playerController = other.GetComponentInParent<PlayerController>();
                if (playerController)
                {
                    entryPosDict.Add(playerController, playerController.transform.position);
                }
                if (triggerBehaviour == Door_TriggerBehaviour.OnEnter)
                {
                    if (teleportPlayer)
                    {
                        Debug.Log("OnEnter Behaviour - Teleport Player");
                    }
                    else
                    {
                        Debug.Log("OnEnter Behaviour - Switch Camera");
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Application.isPlaying)
            {
                PlayerController playerController = other.GetComponentInParent<PlayerController>();
                if (playerController)
                {
                    Vector3 exitPos = playerController.transform.position;
                    if (entryPosDict.TryGetValue(playerController, out Vector3 entryPos))
                    {
                        if (Physics.Raycast(entryPos, exitPos - entryPos, out RaycastHit hit, 500, 1 << 9))
                        {
                            if (hit.transform == this.transform && triggerBehaviour == Door_TriggerBehaviour.OnExit)
                            {
                                if (teleportPlayer)
                                {
                                    Debug.Log("OnExit Behaviour - Teleporting Player");
                                }
                                else //Just switch camera
                                {
                                    Debug.Log("OnExit Behaviour - Walked Through");
                                }
                            }
                            else if (hit.transform != this.transform)
                            {
                                Debug.LogErrorFormat("Door: {0} and Door: {1} are placed too close to eachother.", this.name, hit.transform.name);
                            }
                        }
                        else
                        {
                            if (triggerBehaviour == Door_TriggerBehaviour.OnEnter && !teleportPlayer)
                            {
                                Debug.Log("OnEnter Behaviour - Turned Around");
                            }
                            else if (triggerBehaviour == Door_TriggerBehaviour.OnExit)
                            {
                                Debug.Log("OnExit Behaviour - Turned Around");
                            }
                        }
                        entryPosDict.Remove(playerController);
                    }
                    else
                    {
                        Debug.LogErrorFormat("Unable to get entry pos from key with name: {0}", playerController.name);
                    }
                }
            }
        }
    }
}