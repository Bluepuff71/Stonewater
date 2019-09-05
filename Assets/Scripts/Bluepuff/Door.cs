using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bluepuff
{

    public enum TriggerBehaviour
    {
        OnExit,
        OnEnter
    }
    [ExecuteAlways]
    public class Door : MonoBehaviour
    {
        //Camera switcher editor will handle selecting the cameras
        public GameObject[] cameraObjs = new GameObject[2];

        public GameObject[] teleportObjs = new GameObject[2];

        public bool teleportPlayer;

        [Tooltip("When will the door run it's camera/teleport logic?")]
        public TriggerBehaviour triggerBehaviour;

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
                    if (!cameraObj.tag.Equals("MainCamera"))
                    {
                        GameObject bakedCamera = Instantiate(new GameObject("--Baked Camera--"), cameraObj.transform);
                        bakedCamera.transform.position = cameraObj.transform.position;
                        bakedCamera.transform.rotation = cameraObj.transform.rotation;
                        bakedCamera.transform.localScale = cameraObj.transform.localScale;
                        Destroy(cameraObj);
                    }
                });
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Application.isPlaying)
            {
                if(triggerBehaviour == TriggerBehaviour.OnExit)
                {
                    PlayerController playerController = other.GetComponentInParent<PlayerController>();
                    if (playerController)
                    {
                        entryPosDict.Add(playerController, playerController.transform.position);
                    }
                }
                else
                {
                    Debug.Log("Trigger OnEnter - Entered Trigger");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Application.isPlaying)
            {
                if(triggerBehaviour == TriggerBehaviour.OnExit)
                {
                    PlayerController playerController = other.GetComponentInParent<PlayerController>();
                    if (playerController)
                    {
                        Vector3 exitPos = playerController.transform.position;
                        if (entryPosDict.TryGetValue(playerController, out Vector3 entryPos))
                        {
                            if (Physics.Raycast(entryPos, exitPos - entryPos, out RaycastHit hit, 500, 1 << 9))
                            {
                                if (hit.transform == this.transform)
                                {
                                    
                                    if (teleportPlayer)
                                    {
                                        Debug.Log("Teleporting Player");
                                    }
                                    else //Just switch camera
                                    {
                                        Debug.Log("Switching Camera");
                                    }
                                }
                                else
                                {
                                    Debug.LogErrorFormat("Door: {0} and Door: {1} are placed too close to eachother.", this.name, hit.transform.name);
                                }
                            }
                            else
                            {
                                Debug.Log("Turned Around");
                            }
                            entryPosDict.Remove(playerController);
                        }
                        else
                        {
                            Debug.LogErrorFormat("Unable to get entry pos from key with name: {0}", playerController.name);
                        }
                    }
                }
                else
                {
                    Debug.Log("Trigger OnEnter - Exited Trigger");
                }
            }
        }
    }
}