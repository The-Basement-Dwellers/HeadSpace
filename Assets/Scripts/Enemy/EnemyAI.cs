using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private GameObject cone;
    private GameObject player;
    [SerializeField] private Vector3 dir;
    [SerializeField] private Transform target;
    [SerializeField] private float maxRange;
    [SerializeField] private float lookRad;
    private Vector3 spawnPos;
    private Vector3 moveDirection;
    private IAstarAI ai;
    private bool isLost;
    private float delay;

    private void OnEnable()
    {
        player = GameObject.Find("Player");
        ai = GetComponent<IAstarAI>();
        if (ai != null) ai.onSearchPath += Update;
        EventController.setMoveDirectionEvent += setMoveDirection;
        EventController.damageEvent += Hurt;

    }

    private void OnDisable()
    {
        EventController.setMoveDirectionEvent -= setMoveDirection;
        EventController.damageEvent -= Hurt;
        if (ai != null) ai.onSearchPath -= Update;
    }

    private void setMoveDirection(Vector3 eventMoveDirection, GameObject targetedGameObject)
    {
        if (gameObject == targetedGameObject) moveDirection = eventMoveDirection;
    }

    private void Start()
    {
        spawnPos = transform.position;
    }
    void Update()
    {
        StartCoroutine(FindDirection(transform.position));
        float fortnite = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cone.transform.eulerAngles = new Vector3(0, 0, fortnite + 90);
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GraphNode node1 = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(player.transform.position, NNConstraint.Default).node;
        if (collision.gameObject == player)
        {
            target = player.transform;
            delay = 2f;
        }

        if (target != null && ai != null)
        {
            if (PathUtilities.IsPathPossible(node1, node2))
            {
                ai.destination = target.position;
            }
            else if (!PathUtilities.IsPathPossible(node1, node2) && !isLost)
            {
                Debug.Log("Nein");
                isLost = true;
                StartCoroutine(Lost(delay));
            }
        }


    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision == null) return;
    //    else if (collision.gameObject == player && !isLost)
    //    {
    //        isLost = true;
    //        StartCoroutine(Lost(delay));
    //    }
    //}

    IEnumerator FindDirection(Vector3 oldPos)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 newPos = transform.position;
        dir = Vector3.Normalize(newPos - oldPos);

    }

    private void Hurt(GameObject targetedgameObject, float damageAmount = 0)
    {
        if (targetedgameObject == gameObject)
        {
            ai.destination = player.transform.position;
        }

    }
    IEnumerator Lost(float delay)
    {
        yield return new WaitForSeconds(delay);
        target = null;
        ai.destination = PickRandomPoint();
        ai.SearchPath();
        Debug.Log("Is this being called");
        yield return new WaitForSeconds(delay);
        ai.destination = spawnPos;

        //else {
        //    Debug.Log("No worky");
        //}
        isLost = false;
    }

    Vector3 PickRandomPoint()
    {
        var point = Random.insideUnitSphere * lookRad;
        point.y = 0;
        point += ai.position;
        return point;
    }

}


