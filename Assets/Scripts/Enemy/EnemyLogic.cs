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
        EventController.StartEnemyMoveDirectionEvent(moveDirection);
    }

    private void OnEnable()
    {
        EventController.damageEvent += Damage;
    }

    private void OnDisable()
    {
        EventController.damageEvent -= Damage;
    }

    private void Damage(GameObject targetedGameObject, float damageAmount)
    {
        if (targetedGameObject == gameObject)
        {
            health -= damageAmount;
            EventController.StartHealthBarEvent(health / maxHealth, gameObject);

        }
        else if (targetedGameObject == player) 
        {   
            playerController.playerHealth -= damageAmount;
            EventController.StartHealthBarEvent(playerController.playerHealth / playerController.playerMaxHealth, player);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Damage(collision.gameObject, damage);
        }
    }

}
