using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
 using UnityEngine.Experimental.Rendering.Universal; // For Light2D
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator headAnimator;
    private Animator bodyAnimator;
    private Vector3 moveDirection;
    private Vector3 lookDirection = Vector3.zero;

    [SerializeField] private ShadowCaster2D shadows;
    [SerializeField] private GameObject preFlash;
    private bool isMoving;
    private bool canMoveFlash;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    private void OnEnable()
    {
        EventController.setMoveDirectionEvent += SetMoveDirection;
        EventController.setLookDirectionEvent += SetLookDirection;
        EventController.setIsShootingEvent += SetIsShooting;
        EventController.setCanMoveFlash += SetCanMoveFlash;

        headAnimator = head.GetComponent<Animator>();
        bodyAnimator = body.GetComponent<Animator>();
    }

    private void OnDisable()
    {
        EventController.setMoveDirectionEvent -= SetMoveDirection;
        EventController.setLookDirectionEvent -= SetLookDirection;
        EventController.setIsShootingEvent -= SetIsShooting;
        EventController.setCanMoveFlash -= SetCanMoveFlash;
    }

    private void Update()
    {
        isMoving = false;
        if (lookDirection.magnitude > 0.05)
        {
            if (lookDirection.x > 0.1) {
                shadows.enabled = false;
                preFlash.transform.localPosition = new Vector3(0.0221f, 0.5826f, 0);
            } else if (lookDirection.x < -0.1) {
                shadows.enabled = false;
                preFlash.transform.localPosition = new Vector3(-0.0221f, 0.5826f, 0);
            } else if (lookDirection.y > 0.1) {
                shadows.enabled = true;
                preFlash.transform.localPosition = new Vector3(0.0167f, 0.5594f, 0);
            } else if (lookDirection.y < -0.1) {
                shadows.enabled = false;
                preFlash.transform.localPosition = new Vector3(-0.0167f, 0.6047f, 0);
            } 

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
        headAnimator.SetBool("isMoving", isMoving);
    }

    private void SetMoveDirection(Vector3 eventMoveDirection, GameObject targetedGameObject) {
        if (gameObject == targetedGameObject) moveDirection = eventMoveDirection;
    }

    private void SetLookDirection(Vector3 eventLookDirection) {
        lookDirection = eventLookDirection;
    }

    private void SetIsShooting(bool value) {
        headAnimator.SetBool("isShooting", value);
    }

    private void SetCanMoveFlash(bool value) {
        canMoveFlash = value;
    }
}

