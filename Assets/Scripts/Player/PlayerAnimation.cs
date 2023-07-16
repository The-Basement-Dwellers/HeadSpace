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

    private void OnEnable()
    {
        EventController.setMoveDirectionEvent += SetMoveDirection;
        EventController.setLookDirectionEvent += SetLookDirection;

        headAnimator = head.GetComponent<Animator>();
        bodyAnimator = body.GetComponent<Animator>();
    }

    private void OnDisable()
    {
        EventController.setMoveDirectionEvent -= SetMoveDirection;
        EventController.setLookDirectionEvent -= SetLookDirection;
    }

    private void Update()
    {
        isMoving = false;
        if (lookDirection.magnitude > 0.05)
        {
            headAnimator.SetFloat("X", lookDirection.x);
            headAnimator.SetFloat("Y", lookDirection.y);
        }
        else if (moveDirection.magnitude > 0.05)
        {
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

    private void SetMoveDirection(Vector3 eventMoveDirection, GameObject targetedGameObject) {
        if (gameObject == targetedGameObject) moveDirection = eventMoveDirection;
    }

    private void SetLookDirection(Vector3 eventLookDirection) {
        lookDirection = eventLookDirection;
    }
}

