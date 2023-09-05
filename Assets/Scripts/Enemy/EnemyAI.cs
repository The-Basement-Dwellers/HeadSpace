using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;
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
    private bool sightLine;
    private float delay;
    private AIPath aiPath;
    [SerializeField] private LayerMask rayLayerMask;


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
        aiPath = GetComponent<AIPath>();
        spawnPos = transform.position;

    }
    void Update()
    {
        StartCoroutine(FindDirection(transform.position));
        float fortnite = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cone.transform.eulerAngles = new Vector3(0, 0, fortnite + 90);

        if (ai.reachedDestination)
        {
            ai.destination = spawnPos;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, rayLayerMask);
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red, 1);
        
        if (hit.collider != null)
        {
            sightLine = true;
            Debug.Log(hit.collider.name);
        }
        else
        {
            sightLine = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        GraphNode node1 = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(player.transform.position, NNConstraint.Default).node;
        if (collision.gameObject == player && sightLine && !isLost)
        {
            Debug.Log("I LOVE MEN");
            aiPath.maxSpeed = 5;
            target = player.transform;
            delay = 2f;
        }

        if (target != null && ai != null)
        {
            if (PathUtilities.IsPathPossible(node1, node2) && sightLine)
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
        Debug.Log("Lost has been called");
        aiPath.maxSpeed = 2;
        yield return new WaitForSeconds(delay);
        target = null;
        ai.destination = PickRandomPoint();
        //ai.SearchPath();
        Debug.Log("Wandering");
        isLost = false;

    }
    
    
    Vector3 PickRandomPoint()
    {
        var startNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
        var nodes = PathUtilities.BFS(startNode, 100);
        List<GraphNode> reachableNodes = PathUtilities.GetReachableNodes(startNode);
        var singleRandomPoint = PathUtilities.GetPointsOnNodes(reachableNodes, 1)[0];
        return singleRandomPoint;


    }

}


