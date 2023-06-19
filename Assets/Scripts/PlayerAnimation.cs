using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector3 lookDirection;
    private Vector3 moveDirection;
    private bool isLooking;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        EventController.setLookDirectionEvent += setLookDirection;
        EventController.setMoveDirectionEvent += setMoveDirection;
    }

    private void OnDisable() {
        EventController.setLookDirectionEvent -= setLookDirection;
        EventController.setMoveDirectionEvent -= setMoveDirection;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        isLooking = false;
        if (lookDirection.magnitude > 0)
        {
            isLooking = true;
            animator.SetFloat("X", lookDirection.x);
            animator.SetFloat("Y", lookDirection.y);
        } else if (moveDirection.magnitude > 0) {
            isMoving = true;
            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Y", moveDirection.y);
        }


        animator.SetBool("isLooking", isLooking);
        animator.SetBool("isMoving", isMoving);
    }

    private void setLookDirection(Vector3 eventLookDirection) {
        lookDirection = eventLookDirection;
    }
    private void setMoveDirection(Vector3 eventMoveDirection) {
        moveDirection = eventMoveDirection;
    }
}
