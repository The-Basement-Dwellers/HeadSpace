using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = GetComponent<PlayerController>().moveDirection;

        if (moveDirection.magnitude > 0)
        {
            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Y", moveDirection.y);
        }
    }
}
