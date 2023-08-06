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
    private Vector3 lookDirection;

    [SerializeField] private GameObject flash;
    [SerializeField] private GameObject sideFlash;
    [SerializeField] private GameObject preFlash;
    [SerializeField] private GameObject range;
    [SerializeField] private GameObject sideRange;
    private bool isMoving;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    int[] withPlayer = new int[] { 0, -529384461, 1088288179, -1277056003, 899026771};
    int[] withoutPlayer = new int[] { 0, -529384461, 1088288179, 899026771};

    private void OnEnable()
    {
        EventController.setMoveDirectionEvent += SetMoveDirection;
        EventController.setLookDirectionEvent += SetLookDirection;
        EventController.setIsShootingEvent += SetIsShooting;

        headAnimator = head.GetComponent<Animator>();
        bodyAnimator = body.GetComponent<Animator>();
    }

    private void OnDisable()
    {
        EventController.setMoveDirectionEvent -= SetMoveDirection;
        EventController.setLookDirectionEvent -= SetLookDirection;
        EventController.setIsShootingEvent -= SetIsShooting;
    }

    private void Update()
    {
        
        isMoving = false;
        if (lookDirection.magnitude > 0.05)
        {
            SetLayers(withPlayer);
            if (lookDirection.x > 0.1) {
                preFlash.transform.localPosition = new Vector3(0.0221f, 0.5826f, 0);
            } else if (lookDirection.x < -0.1) {
                preFlash.transform.localPosition = new Vector3(-0.0221f, 0.5826f, 0);
            } else if (lookDirection.y > 0.1) {
                SetLayers(withoutPlayer);
                preFlash.transform.localPosition = new Vector3(0.0167f, 0.5594f, 0);
            } else {
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

    private void SetLayers(int[] layers) {
        flash.GetComponent<Light2D>().SetLayers(layers);
        sideFlash.GetComponent<Light2D>().SetLayers(layers);
        preFlash.GetComponent<Light2D>().SetLayers(layers);
        range.GetComponent<Light2D>().SetLayers(layers);
        sideRange.GetComponent<Light2D>().SetLayers(layers);
    }

    private void SetMoveDirection(Vector3 eventMoveDirection, GameObject targetedGameObject) {
        if (gameObject == targetedGameObject) moveDirection = eventMoveDirection;
    }

    private void SetLookDirection(Vector3 eventLookDirection) {
        lookDirection = eventLookDirection;
    }

    private void SetIsShooting(bool isShooting) {
        headAnimator.SetBool("isShooting", isShooting);
    }
}

