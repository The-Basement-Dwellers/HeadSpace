using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;
    private Vector3 moveDirection;
    private bool isMoving;
    private Vector3 dir;

    // Start is called before the first frame update
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        EventController.setEnemyMoveDirectionEvent += setEnemyMoveDirection;
    }

    private void OnDisable() {
        EventController.setEnemyMoveDirectionEvent -= setEnemyMoveDirection;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FindDirection(transform.position));
        if (dir.magnitude > 0.5) {
            isMoving = true;
            animator.SetFloat("X", dir.x);
            animator.SetFloat("Y", dir.y);
        } else isMoving = false;

        animator.SetBool("isMoving", isMoving);
    }

     private void setEnemyMoveDirection(Vector3 eventMoveDirection) {
        moveDirection = eventMoveDirection;

    }
    IEnumerator FindDirection(Vector3 a)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 newPosition = transform.position;
        dir = newPosition - a;
        dir = Vector3.Normalize(dir);
    }
}
