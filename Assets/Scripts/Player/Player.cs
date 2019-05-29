using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Interactable
{
    private CharacterController characterController;

    public int playerNumber = -1;

    public float speed = 2.0f;
    public float rotationSpeed = 100.0f;
    public float gravity = 80.0F;
    private Vector3 moveDirection = Vector3.zero;
    private Transform mainCameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        GameData.players.Add(GetComponent<Player>());
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerNumber == -1)
        {
            List<int> unavaliableControllers = GameData.GetUnAvaliableControllers();
            if (Input.GetButton("START_1") && !unavaliableControllers.Contains(1))
            {
                playerNumber = 1;
            }
            if (Input.GetButton("START_2") && !unavaliableControllers.Contains(2))
            {
                playerNumber = 2;
            }
            if (Input.GetButton("START_3") && !unavaliableControllers.Contains(3))
            {
                playerNumber = 3;
            }
            if (Input.GetButton("START_4") && !unavaliableControllers.Contains(4))
            {
                playerNumber = 4;
            }
            if (Input.GetButton("START_5") && !unavaliableControllers.Contains(5))
            {
                playerNumber = 5;
            }
            if (Input.GetButton("START_6") && !unavaliableControllers.Contains(6))
            {
                playerNumber = 6;
            }
        }
        else
        {
            // Get the horizontal and vertical axis.
            // By default they are mapped to the arrow keys.
            // The value is in the range -1 to 1
            float up_down_translation = Input.GetAxis(string.Format("UP_DOWN_{0}", playerNumber));
            float left_right_translation = Input.GetAxis(string.Format("LEFT_RIGHT_{0}", playerNumber));

            Vector3 relativeMovement = Camera.main.transform.TransformVector(left_right_translation, 0, up_down_translation);

            relativeMovement.y -= gravity;

            //moveDirection = left_right_translation * mainCameraTransform.worldToLocalMatrix.MultiplyVector(transform.right).normalized + up_down_translation * -mainCameraTransform.worldToLocalMatrix.MultiplyVector(transform.forward).normalized; ;
            //moveDirection = transform.TransformDirection(moveDirection);
            //moveDirection *= speed;

            //moveDirection.y -= gravity * Time.deltaTime;

            if (characterController.enabled)
            {
                characterController.Move(relativeMovement * (Time.deltaTime * speed));
            }
            // Rotate around our y-axis
            //float heading = Mathf.Atan2(joyVector.x, joyVector.y);
            //transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);
        }
    }
}
