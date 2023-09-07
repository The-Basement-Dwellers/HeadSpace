using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletTimeController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject dashPanel;
    [SerializeField] float hideDelay = 1f;
    [SerializeField] float distanceToStart = 0.1f;
    [SerializeField] float bulletTime = 0.1f;

    bool isBulletTime = false;

    private void OnEnable() {
        EventController.startIsDashingEvent += Dash;
    }

    private void OnDisable() {
        EventController.startIsDashingEvent -= Dash;
    }

    private void Update() {
        if (!isBulletTime) {
            Vector3 playerPos = player.transform.position;
            Vector3 enemyPos = enemy.transform.position;
            Vector3 enemyDistance = enemyPos - playerPos;
            float distance = enemyDistance.magnitude;

            if (distance <= distanceToStart && !isBulletTime) {
                Time.timeScale = bulletTime;
                isBulletTime = true;
                GameObject.Find("Player").GetComponent<PlayerDash>().enabled = true;
                EventController.IsBulletTime(isBulletTime);
                dashPanel.SetActive(true);
            }
        }
    }

    private void Dash(bool isDashing) {
        if (isBulletTime) {
            Time.timeScale = 1;
            StartCoroutine(HideText(dashPanel, hideDelay));
        }
    }

    private IEnumerator HideText(GameObject text, float time)
    {
        yield return new WaitForSeconds(time);
        text.SetActive(false);
    }
}
