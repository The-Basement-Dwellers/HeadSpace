using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    public GameObject cone;
    void Update()
    {
        if (gameObject.transform.position.x > 0)
        {
            cone.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            cone.transform.eulerAngles = new Vector3(0, 0, -90);
        }
    }

}


