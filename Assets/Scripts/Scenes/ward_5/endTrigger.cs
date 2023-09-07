using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class endTrigger : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float enemyLerpDuration = 0.1f;
    [SerializeField] float cameraLerpDuration = 1f;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Transform marker;
    [SerializeField] GameObject endPanel;
    AudioSource audioSource;
    private Vector2 enemyStartPos;
    private bool enemyLerp = false;
    private bool cameraLerp = false;
	private float elapsedTime;
    private GameObject player;

    private void Start() {
        player = GameObject.Find("Player");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            cameraLerp = true;
            mainCamera.GetComponent<CinemachineBrain>().enabled = false;
            enemyStartPos = enemy.transform.position;
            player.GetComponent<PlayerController>().moveEnabled = false;
        }
    }

    private void Update() {
        if (enemyLerp) {
            StartCoroutine(EnemyLerp());
        }

        if (cameraLerp) {
            StartCoroutine(CameraLerp());
        }
    }

    private IEnumerator EnemyLerp() {
        float percentageComplete = elapsedTime / enemyLerpDuration;
		elapsedTime += Time.deltaTime;
        enemy.transform.position = Vector3.Lerp(enemyStartPos, transform.position, percentageComplete);

        if (percentageComplete >= 1) {
            enemyLerp = false;
        }
        yield return null;
    }

    private IEnumerator CameraLerp() {
        float percentageComplete = elapsedTime / cameraLerpDuration;
        elapsedTime += Time.deltaTime;

        float y = Mathf.Lerp(mainCamera.transform.position.y, marker.position.y, percentageComplete/100);
        float x = Mathf.Lerp(mainCamera.transform.position.x, marker.position.x, percentageComplete/100);
        mainCamera.transform.position = new Vector3(x, y, mainCamera.transform.position.z);

        if (percentageComplete >= 1) {
            enemyLerp = true;
            cameraLerp = false;
            elapsedTime = 0;
            StartCoroutine(EndGame());
        }
        yield return null;
    }

    private IEnumerator EndGame() {
        yield return new WaitForSeconds(enemyLerpDuration / 2);
        endPanel.SetActive(true);
        audioSource.Play();
    }
}
