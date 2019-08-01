using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MannequinAnimationController : MonoBehaviour
{
    public Animator animator;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("velocity", GetComponent<NavMeshAgent>().velocity.magnitude);
    }
}

