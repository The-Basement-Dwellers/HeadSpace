using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletTimeController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] float distanceToStart = 0.1f;

    bool isBulletTime = false;

    private void Start() {
        EventController.startIsDashingEvent += Dash;
    }

    private void Update() {
        if (!isBulletTime) {
            Vector3 playerPos = player.transform.position;
            Vector3 enemyPos = enemy.transform.position;
            Vector3 enemyDistance = enemyPos - playerPos;
            float distance = enemyDistance.magnitude;

            if (distance <= distanceToStart && !isBulletTime) {
                Time.timeScale = 0.000000001f;
                isBulletTime = true;
                GameObject.Find("Player").GetComponent<PlayerDash>().enabled = true;
                EventController.IsBulletTime(isBulletTime);
                Debug.Log("Press Space to dash");
            }
        }
    }

    private void Dash(bool isDashing) {
        if (isBulletTime) {
            Time.timeScale = 1;
            Debug.Log("Bullet time ended");
        }
    }
}
