using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goal : MonoBehaviour
{
    GameObject player;

    private void Start() {
        player = GameObject.Find("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject == player) {
            SceneController.StartScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
