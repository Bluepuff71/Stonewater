using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicControl : MonoBehaviour
{

    public CharacterController controller;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontialMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        if (controller.enabled)
        {
            controller.Move((new Vector3(horizontialMovement, 0, verticalMovement) * (Time.deltaTime * speed)));
        }
    }
}
