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
    [SerializeField] private float damageCooldown = 0.5f;
    private float maxHealth;
    private float damage;
    private bool isColliding = false;
    private bool damageCoroutineIsRunning = false;

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
        EventController.StartMoveDirectionEvent(moveDirection, gameObject);
        
        if (isColliding && !damageCoroutineIsRunning) {
            damageCoroutineIsRunning = true;
            StartCoroutine(DamageCooldown());
        }
    }

    private void OnEnable()
    {
        EventController.damageEvent += Damage;
        EventController.colliderEnter += ColliderEnter;
        EventController.colliderExit += ColliderExit;
    }

    private void OnDisable()
    {
        EventController.damageEvent -= Damage;
        EventController.colliderEnter -= ColliderEnter;
        EventController.colliderExit -= ColliderExit;
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

    public void ColliderEnter(GameObject targetedGameObject)
    {
        if (targetedGameObject == gameObject) isColliding = true;
    }

    private void ColliderExit(GameObject targetedGameObject)
    {
        if (targetedGameObject == gameObject) isColliding = false;
    }

    private IEnumerator DamageCooldown()
    {
        Damage(player, damage);
        yield return new WaitForSeconds(damageCooldown);       
        damageCoroutineIsRunning = false;
    }

}
