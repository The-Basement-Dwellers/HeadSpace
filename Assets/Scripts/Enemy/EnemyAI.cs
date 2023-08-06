using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class EnemyAI : MonoBehaviour
{
    public GameObject cone;
    public GameObject player;
    public Vector3 moveDirection;
    public Vector3 dir;
    public Transform target;
    IAstarAI ai;
    private bool hasLOS;

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

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FindDirection(transform.position));
        float fortnite = Mathf.Atan2(dir.y, dir.x) *Mathf.Rad2Deg;
        cone.transform.eulerAngles = new Vector3(0, 0,fortnite+90);
        if (target != null && ai != null) ai.destination = target.position;

    }

    private void setMoveDirection(Vector3 eventMoveDirection, GameObject targetedGameObject)
    {
        if (gameObject == targetedGameObject) moveDirection = eventMoveDirection;
    }
 
    IEnumerator FindDirection(Vector3 oldPos)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 newPos = transform.position;
        dir = Vector3.Normalize(newPos - oldPos);
        //dir = Vector3.Normalize(dir);
    }

    IEnumerator testDelay()
    {
        yield return new WaitForSeconds(3);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        else if (collision.gameObject == player)
        {
            testDelay();
            target = player.transform;
            
        }
    }
}


