using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public EnemyTemplate eT;
    public PlayerController pC;
    public GameObject player;
    private Vector2 moveDirection = Vector2.zero;

    void Start()
    {
        pC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        moveDirection = gameObject.transform.position;
        EventController.StartEnemyMoveDirectionEvent(gameObject, moveDirection);
    }

    private void OnEnable()
    {
        EventController.damageEvent += enemyHurt;
    }

    private void OnDisable()
    {
        EventController.damageEvent -= enemyHurt;
    }

    private void enemyHurt(GameObject targetedGameObject, float damageAmount)
    {
        if (targetedGameObject == gameObject)
        {
            eT.liveenemyHealth -= damageAmount;
            EventController.StartHealthBarEvent(eT.liveenemyHealth / eT.enemymaxHealth, gameObject);
        }

        if (eT.liveenemyHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void playerHurt(GameObject targetedGameObject, float damageAmount)
    {
        pC.playerHealth -= damageAmount;
        EventController.StartHealthBarEvent(pC.playerHealth / pC.playermaxHealth, player);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            Debug.Log("Player Took Damage");
            playerHurt(player, eT.enemyDamage);
        }
    }
}
