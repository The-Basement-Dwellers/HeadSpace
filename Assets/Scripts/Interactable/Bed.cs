using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bed : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneController.StartScene(sceneIndex);
    }
}