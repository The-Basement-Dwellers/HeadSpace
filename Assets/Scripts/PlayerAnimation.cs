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
        EventController.setMoveDirectionEvent += setMoveDirection;
    }

    private void OnDisable() {
        EventController.setMoveDirectionEvent -= setMoveDirection;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        if (moveDirection.magnitude > 0)
        {
            isMoving = true;
            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Y", moveDirection.y);
        }

        animator.SetBool("isMoving", isMoving);
    }

     private void setMoveDirection(Vector3 eventMoveDirection) {
        moveDirection = eventMoveDirection;
    }
}
