using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
        [SerializeField] private float radius = 20;
        [SerializeField] private float delay;
        [SerializeField] private float switchTime = float.PositiveInfinity;

        IAstarAI ai;

        void Start()
        {
            ai = GetComponent<IAstarAI>();
        }
        Vector3 PickRandomPoint()
        {
            var point = Random.insideUnitSphere * radius;
            point.y = 0;
            point += ai.position;
            return point;
        }
        void Update()
        {
            // Update the destination of the AI if
            // the AI is not already calculating a path and
            // the ai has reached the end of the path or it has no path at all
            if (ai.reachedEndOfPath && !ai.pathPending && float.IsPositiveInfinity(switchTime))
            {
                switchTime = Time.time + delay;
                if (Time.time >= switchTime) {
                Debug.Log("Kil myself");
                ai.destination = PickRandomPoint();
                ai.SearchPath();
            }
           
                Debug.Log("Kil myself 2");
            }
       
    }

}
