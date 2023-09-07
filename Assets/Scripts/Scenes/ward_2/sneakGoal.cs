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
        StartCoroutine(delayDamage());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            sneakPanel.SetActive(false);
        }
    }

    private IEnumerator delayDamage()
    {
        yield return new WaitForSeconds(0.3f);
        EventController.StartHealthBarEvent(0.2f, player);
        player.GetComponent<PlayerController>().playerHealth = 20f;
    }
}
