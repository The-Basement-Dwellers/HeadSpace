using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;

    private void OnEnable() {
        EventController.damageEvent += Damage;
    }

    private void OnDisable() {
        EventController.damageEvent -= Damage;
    }

    private void Damage(GameObject targetedGameObject, float damageAmount) {
        if (targetedGameObject == gameObject) {
            health -= damageAmount;
            EventController.StartHealthBarEvent(health / maxHealth, gameObject);
        }

        if (health <= 0f) Destroy(gameObject);
    }
}
