using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector3 playermoveDirection;
    private bool isMoving;
    private GameObject self;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        self = gameObject;
        EventController.setMoveDirectionEvent += setMoveDirection;
        
    }

    private void OnDisable() {
        EventController.setMoveDirectionEvent -= setMoveDirection;
    }

    // Update is called once per frame
    void Update()
    {

        isMoving = false;
        if (playermoveDirection.magnitude > 0)
        {
            isMoving = true;
            animator.SetFloat("X", playermoveDirection.x);
            animator.SetFloat("Y", playermoveDirection.y);
        }

        animator.SetBool("isMoving", isMoving);
    }

     private void setMoveDirection(GameObject targetedGameObject, Vector3 eventMoveDirection) {
        playermoveDirection = eventMoveDirection;
        self = targetedGameObject;
    }


}
