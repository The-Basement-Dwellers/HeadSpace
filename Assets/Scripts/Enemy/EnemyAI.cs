using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class EnemyAI : MonoBehaviour
{
    IAstarAI ai;
    [SerializeField] private GameObject cone;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 dir;
    [SerializeField] private Transform target;
    [SerializeField] private float maxRange;

    private void OnEnable()
    {
        EventController.setMoveDirectionEvent += setMoveDirection;
        ai = GetComponent<IAstarAI>();
        if (ai != null) ai.onSearchPath += Update;
    }

    private void OnDisable()
    {
        EventController.setMoveDirectionEvent -= setMoveDirection;
        if (ai != null) ai.onSearchPath -= Update;
    }

    private void setMoveDirection(Vector3 eventMoveDirection, GameObject targetedGameObject)
    {
        if (gameObject == targetedGameObject) moveDirection = eventMoveDirection;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FindDirection(transform.position));
        float fortnite = Mathf.Atan2(dir.y, dir.x) *Mathf.Rad2Deg;
        cone.transform.eulerAngles = new Vector3(0, 0,fortnite+90);
        Chase();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
        }

    }

    private void Chase()
    {
        if (target != null && ai != null) ai.destination = target.position;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;
        else if (collision.gameObject == player)
        {
            target = player.transform;
            Debug.Log(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;
        else if (collision.gameObject == player)
        {
            StartCoroutine(TestDelay(2));
        }
    }
    IEnumerator FindDirection(Vector3 oldPos)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 newPos = transform.position;
        dir = Vector3.Normalize(newPos - oldPos);

    }

    IEnumerator TestDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        target = null;
    }



}


