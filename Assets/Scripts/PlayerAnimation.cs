using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector3 moveDirection;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = GetComponent<PlayerController>().moveDirection;

        isMoving = false;
        if (moveDirection.magnitude > 0)
        {
            isMoving = true;
            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Y", moveDirection.y);
        }

        animator.SetBool("isMoving", isMoving);
    }
}
