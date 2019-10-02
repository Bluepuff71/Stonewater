using Bluepuff.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bluepuff
{
    public class OnConfirmPressedEvent : UnityEvent<PlayerController> { }
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// List of all players
        /// </summary>
        public static List<PlayerController> players = new List<PlayerController>();

        private CharacterController characterController;

        public int controllerNumber = -1;
        public bool readyToTeleport;

        public float speed = 2.0f;
        public float rotationSpeed = 100.0f;
        public float gravity = 80.0F;

        public UnityEvent<PlayerController> onPressedConfirm;

        private void Awake()
        {
            if (onPressedConfirm == null)
            {
                onPressedConfirm = new OnConfirmPressedEvent();
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            players.Add(GetComponent<PlayerController>());
            characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            //Move all of this out of here and into a character select screen
            if (controllerNumber == -1)
            {
                List<int> unavaliableControllers = GameUtils.GetUnAvaliableControllers();
                if (Input.GetButton("START_1") && !unavaliableControllers.Contains(1))
                {
                    controllerNumber = 1;
                }
                if (Input.GetButton("START_2") && !unavaliableControllers.Contains(2))
                {
                    controllerNumber = 2;
                }
                if (Input.GetButton("START_3") && !unavaliableControllers.Contains(3))
                {
                    controllerNumber = 3;
                }
                if (Input.GetButton("START_4") && !unavaliableControllers.Contains(4))
                {
                    controllerNumber = 4;
                }
                if (Input.GetButton("START_5") && !unavaliableControllers.Contains(5))
                {
                    controllerNumber = 5;
                }
                if (Input.GetButton("START_6") && !unavaliableControllers.Contains(6))
                {
                    controllerNumber = 6;
                }
            }
            else
            {

                // Get the horizontal and vertical axis.
                // By default they are mapped to the arrow keys.
                // The value is in the range -1 to 1
                float up_down_translation = Input.GetAxis(string.Format("UP_DOWN_{0}", controllerNumber));
                float left_right_translation = Input.GetAxis(string.Format("LEFT_RIGHT_{0}", controllerNumber));

                //Vector3 relativeMovement = Camera.main.transform.TransformVector(left_right_translation, 0, up_down_translation);
                //camera forward and right vectors:
                Vector3 forward = Camera.main.transform.forward;
                Vector3 right = Camera.main.transform.right;
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();
                if (forward == Vector3.zero)
                {
                    forward = Camera.main.transform.up;
                    forward.y = 0;
                }


                //this is the direction in the world space we want to move:
                Vector3 relativeMovement = forward * up_down_translation + right * left_right_translation;

                //relativeMovement.y -= gravity;

                if (characterController.enabled)
                {
                    characterController.Move(relativeMovement * (Time.deltaTime * speed));
                }
                // Rotate around our y-axis
                //float heading = Mathf.Atan2(joyVector.x, joyVector.y);
                //transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);
                if (Input.GetButtonDown(string.Format("CONFIRM_{0}", controllerNumber)))
                {
                    onPressedConfirm.Invoke(this);
                }
            }

        }

        public static PlayerController GetPlayerByNumber(int playerNum)
        {
            return players.Find((player) =>
            {
                return player.controllerNumber == playerNum;
            });
        }

        public static void PerformOnPlayers(System.Action<PlayerController> action)
        {
            players.ForEach(action);
        }
    }
}