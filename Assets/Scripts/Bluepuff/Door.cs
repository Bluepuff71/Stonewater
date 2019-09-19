using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bluepuff
{
    [ExecuteAlways]
    public class Door : MonoBehaviour
    {
        //Camera switcher editor will handle selecting the cameras
        public GameObject[] cameraObjs = new GameObject[2];

        //which index of cameraObjs the main camera is located
        private static GameObject mainCameraLocation;

        public GameObject[] teleportObjs = new GameObject[2];

        public bool TeleportPlayer { get; set; }

        public Tape[] tapes = new Tape[2];

        public enum TriggerBehaviour
        {
            OnExit,
            OnEnter
        }

        public enum Behaviour
        {
            SWITCH_CAMERAS,
            LOAD_TIMESTATE
        }

        [Tooltip("When will the door run it's camera/teleport logic?")]
        public TriggerBehaviour triggerBehaviour;

        public Behaviour behaviour;

        private Dictionary<PlayerController, Vector3> entryPosDict = new Dictionary<PlayerController, Vector3>();

        private static Dictionary<GameObject, GameObject> cameraBakedDict = new Dictionary<GameObject, GameObject>();

        public event EventHandler OnTriggered; //People can subscribe to this event but only this class can invoke it.

        public event EventHandler OnTurnedAround;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                //If we are switching cameras, we need to subscribe to the OnTriggered event
                if (behaviour == Behaviour.SWITCH_CAMERAS)
                {
                    OnTriggered += SwitchCamera;
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            gameObject.layer = 9;
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
                GetComponent<MeshRenderer>().enabled = false;
                //This "bakes" all of the cameras
                for (byte i = 0; i < cameraObjs.Length; i++)
                {
                    //try to get the baked camera
                    if (cameraBakedDict.TryGetValue(cameraObjs[i], out GameObject bakedCamera))
                    {
                        //if we are able to get it then that means the camera has already been baked so we should
                        //just set the cameraObj = the baked camera
                        cameraObjs[i] = bakedCamera;
                    }
                    else
                    {
                        //if the camera isn't found then it needs to be baked
                        bakedCamera = new GameObject(string.Format("Baked: {0}", cameraObjs[i].name));
                        bakedCamera.transform.position = cameraObjs[i].transform.position;
                        bakedCamera.transform.rotation = cameraObjs[i].transform.rotation;
                        bakedCamera.transform.localScale = cameraObjs[i].transform.localScale;
                        cameraBakedDict.Add(cameraObjs[i], bakedCamera);
                        if (!cameraObjs[i].tag.Equals("MainCamera"))
                        {
                            Destroy(cameraObjs[i]);
                        }
                        else
                        {
                            mainCameraLocation = bakedCamera;
                        }
                        cameraObjs[i] = bakedCamera;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            cameraBakedDict.Clear(); //We do this so that there won't be a problem when switching timestates
        }

        private void OnTriggerEnter(Collider other)
        {
            //Only if we are playing
            if (Application.isPlaying)
            {
                PlayerController playerController = other.GetComponentInParent<PlayerController>();
                if (playerController)
                {
                    entryPosDict.Add(playerController, playerController.transform.position);
                }
                if (triggerBehaviour == TriggerBehaviour.OnEnter)
                {
                    if(OnTriggered != null)
                    {
                        OnTriggered.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //Only if we are playing
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
                            if (hit.transform == this.transform && triggerBehaviour == TriggerBehaviour.OnExit)
                            {
                                if(OnTriggered != null)
                                {
                                    OnTriggered.Invoke(this, EventArgs.Empty);
                                }
                            }
                            else if (hit.transform != this.transform)
                            {
                                Debug.LogErrorFormat("Door: {0} and Door: {1} are placed too close to eachother.", this.name, hit.transform.name);
                            }
                        }
                        else
                        {
                            if(OnTurnedAround != null)
                            {
                                OnTurnedAround.Invoke(this, EventArgs.Empty);
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

        //Switch camera behaviour
        //TODO Implement teleport behaviour
        private void SwitchCamera(object sender, EventArgs e)
        {
            byte cameraNum = 0;
            if(cameraObjs[0] == mainCameraLocation)
            {
                cameraNum = 1;
            }
            Camera.main.transform.position = cameraObjs[cameraNum].transform.position;
            Camera.main.transform.rotation = cameraObjs[cameraNum].transform.rotation;
            Camera.main.transform.localScale = cameraObjs[cameraNum].transform.localScale;
            mainCameraLocation = cameraObjs[cameraNum];
        }
    }
}