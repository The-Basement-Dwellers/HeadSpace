using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endTrigger : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float lerpDuration = 0.1f;
    private Vector2 enemyStartPos;
    private bool enemyLerp = false;
	private float elapsedTime;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            enemyLerp = true;
            enemyStartPos = enemy.transform.position;
            
        }
    }

    private void Update() {
        if (enemyLerp) {
            StartCoroutine(EnemyLerp());
        }
    }

    private IEnumerator EnemyLerp() {
        float percentageComplete = elapsedTime / lerpDuration;
		elapsedTime += Time.deltaTime;
        enemy.transform.position = Vector3.Lerp(enemyStartPos, transform.position, percentageComplete);
        yield return null;
    }
}
