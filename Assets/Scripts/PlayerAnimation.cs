using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator headAnimator;
    private Animator bodyAnimator;
    private Vector3 moveDirection;
    private Vector3 lookDirection;
    private bool isMoving;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    // Start is called before the first frame update
    void Start()
    {
        headAnimator = head.GetComponent<Animator>();
        bodyAnimator = body.GetComponent<Animator>();
        EventController.setMoveDirectionEvent += setMoveDirection;
        EventController.setLookDirectionEvent += setLookDirection;
    }

    private void OnDisable() {
        EventController.setMoveDirectionEvent -= setMoveDirection;
        EventController.setLookDirectionEvent -= setLookDirection;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        if (lookDirection.magnitude > 0.05)
        {
            headAnimator.SetFloat("X", lookDirection.x);
            headAnimator.SetFloat("Y", lookDirection.y);
        } else if (moveDirection.magnitude > 0.05) {
            headAnimator.SetFloat("X", moveDirection.x);
            headAnimator.SetFloat("Y", moveDirection.y);
        }

        if (moveDirection.magnitude > 0.05)
        {
            isMoving = true;
            bodyAnimator.SetFloat("X", moveDirection.x);
            bodyAnimator.SetFloat("Y", moveDirection.y);
        }

        bodyAnimator.SetBool("isMoving", isMoving);
    }

    private void setMoveDirection(Vector3 eventMoveDirection) {
        moveDirection = eventMoveDirection;
    }

    private void setLookDirection(Vector3 eventLookDirection)
    {
        lookDirection = eventLookDirection;
    }
}
