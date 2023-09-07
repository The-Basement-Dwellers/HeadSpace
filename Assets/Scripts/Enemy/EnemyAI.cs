using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System.Net;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private GameObject cone;
    private GameObject player;
    [SerializeField] private Vector3 dir;
    [SerializeField] private Transform target;
    private Vector3 spawnPos;
    private Vector3 moveDirection;
    private IAstarAI ai;
    private bool isLost;
    [SerializeField] private bool isWandering;
    private bool sightLine;
    private float delay;
    private AIPath aiPath;
    [SerializeField] private LayerMask rayLayerMask;
    [SerializeField, Range(0f, 360f)] private float dirOffset = 0;

    private void OnEnable()
    {

        if (ai != null) ai.onSearchPath += Update;
        EventController.setMoveDirectionEvent += setMoveDirection;
        CameraEventController.damageEvent += Hurt;

    }

    private void OnDisable()
    {
        EventController.setMoveDirectionEvent -= setMoveDirection;
        CameraEventController.damageEvent -= Hurt;
        if (ai != null) ai.onSearchPath -= Update;
    }

    private void setMoveDirection(Vector3 eventMoveDirection, GameObject targetedGameObject)
    {
        if (gameObject == targetedGameObject) moveDirection = eventMoveDirection;
    }

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
        player = GameObject.Find("Player");
        ai = GetComponent<IAstarAI>();
        spawnPos = transform.position;
        StartCoroutine(Wandering(5));


    }
    void Update()
    {
        StartCoroutine(FindDirection(transform.position));
        float fortnite = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cone.transform.eulerAngles = new Vector3(0, 0, fortnite + 90);

        //if (ai.reachedDestination)
        //{
        //    ai.destination = spawnPos;
        //}


    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, rayLayerMask);
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red, 0.01f);
        
        if (hit.collider != null && hit.collider.transform.name == "Player")
        {
            sightLine = true;
        }

        else
        {
            sightLine = false;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        GraphNode node1 = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(player.transform.position, NNConstraint.Default).node;
        if (collision.gameObject == player && !isLost && sightLine)
        {
            //Debug.Log("SightLine" + sightLine);
            //Debug.Log("isLost" + isLost);
            isWandering = false;
            //aiPath.maxSpeed = s;
            target = player.transform;
           
        }

        if (target != null && ai != null)
        {
            if (PathUtilities.IsPathPossible(node1, node2) && sightLine)
            {
                ai.destination = target.position;
            }
            else if (!PathUtilities.IsPathPossible(node1, node2) && !isLost)
            {
                isLost = true;
                StartCoroutine(Lost(3));
            }
        }


    }

    IEnumerator FindDirection(Vector3 oldPos)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 newPos = transform.position;
        dir = Vector3.Normalize(newPos - oldPos);

        float len = Mathf.Sqrt(2);
        float angle = dirOffset * Mathf.PI / 180;
        float x = Mathf.Cos(angle) * len;
        float y = Mathf.Sin(angle) * len;

        Vector3 offsetVector = new Vector3(x, y, 0);
        if (newPos == oldPos)
        {
            dir += offsetVector;
        }
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
        Debug.Log("Lost has been called");
        aiPath.maxSpeed = 3.5f;
        yield return new WaitForSeconds(delay);
        target = null;
        ai.destination = PickRandomPoint();
        Debug.Log("AI Lost Player. Trying to find");
        isLost = false;
        isWandering = true;
        StartCoroutine(Wandering(5));
    }
    
    IEnumerator Wandering(float delay)
    {
        target = null;
        if (isWandering)
        {
            aiPath.maxSpeed = 2;
            ai.destination = PickRandomPoint();
            yield return new WaitForSeconds(delay);
            StartCoroutine(Wandering(delay));
        }
        else if (!isLost)
        {
            isWandering = false;
        }
        


    }
    
    Vector3 PickRandomPoint()
    {
        var startNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        var nodes = PathUtilities.BFS(startNode, 150);
        List<GraphNode> reachableNodes = PathUtilities.GetReachableNodes(startNode);
        var singleRandomPoint = PathUtilities.GetPointsOnNodes(reachableNodes, 1)[0];
        return singleRandomPoint;


    }

}


