using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sneakGoal : MonoBehaviour
{
    [SerializeField] GameObject sneakPanel;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            sneakPanel.SetActive(false);
        }
    }
}
