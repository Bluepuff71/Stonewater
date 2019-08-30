using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bluepuff
{
    public enum DoorBehaviour
    {
        SWITCH_CAMERAS,
        TELEPORT,
        CUSTOM
    }
    [ExecuteAlways]
    public class Door : MonoBehaviour
    {
        //Camera switcher editor will handle selecting the cameras
        public GameObject[] cameraObjs = new GameObject[2];

        public DoorBehaviour behaviour;

        private Dictionary<PlayerController, Vector3> entryPosDict = new Dictionary<PlayerController, Vector3>();

        // Start is called before the first frame update
        void Start()
        {
            if (Application.isPlaying)
            {
                gameObject.layer = 9;
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
                Debug.Log("Endter");
                PlayerController playerController = other.GetComponentInParent<PlayerController>();
                if (playerController)
                {
                    entryPosDict.Add(playerController, playerController.transform.position);
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
                        // Bit shift the index of the layer (9) to get a bit mask
                        int layerMask = 1 << 9;

                        RaycastHit hit;
                        // Does the ray intersect any objects excluding the player layer
                        if (Physics.Raycast(entryPos, entryPos - exitPos, out hit, Mathf.Infinity, layerMask))
                        {
                            if (hit.transform == this.transform)
                            {
                                Debug.Log("Exited Door");
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
                        Debug.DrawLine(entryPos, exitPos, Color.white, 20);
                        entryPosDict.Remove(playerController);
                    }
                }
            }
        }
    }
}