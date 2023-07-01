using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public EnemyTemplate enemyTemplate;
    public PlayerController playerController;
    public GameObject player;
    private Vector2 moveDirection = Vector2.zero;
    [SerializeField] private float health;
    private float maxHealth;
    private float damage;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        health = enemyTemplate.maxHealth;
        maxHealth = enemyTemplate.maxHealth;
        damage = enemyTemplate.damage;
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
            health -= damageAmount;

            EventController.StartHealthBarEvent(health / maxHealth , gameObject);
        }

        // if (health <= 0f)
        // {
        //     Destroy(gameObject);
        // }
    }

    private void playerHurt(GameObject targetedGameObject, float damageAmount)
    {
        playerController.playerHealth -= damageAmount;

        EventController.StartHealthBarEvent(playerController.playerHealth / playerController.playerMaxHealth, player);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            playerHurt(player, damage);
        }
    }
}
