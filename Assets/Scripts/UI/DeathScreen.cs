using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] GameObject saveButton;

    public void Save()
    {
        saveButton.SetActive(false); 
    }

    public void GiveIn() {
        SceneController.StartScene(0);
    }

    public void FightBack() {
        SceneController.StartScene(SceneManager.GetActiveScene().buildIndex);
    }
}
