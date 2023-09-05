using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void QuitGame() {
        Application.Quit();
    }

    public void StartGame() {
        SceneController.StartScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
