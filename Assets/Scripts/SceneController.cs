using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private bool LoadOnEnemyDeath = true;
    public static event Action<int> loadScene;
    [SerializeField] private GameObject loadingScreen;
    public Slider loadingBar;
    private AsyncOperation loadingOperation;
	private bool loading = false;


    private void OnEnable() {
        loadScene += LoadScene;
    }

    private void OnDisable() {
        loadScene -= LoadScene;
    }

    private void Update() {
        if (loadingOperation != null) {
            loadingBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        }

        if (LoadOnEnemyDeath) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length <= 0) {
                int index = SceneManager.GetActiveScene().buildIndex + 1;
                if (!loading) {
                    loading = true;
                    SceneController.StartScene(index);
                }
            }
        }
    }

    public static void StartScene(int index) {
        loadScene?.Invoke(index);
    }

    private void LoadScene(int index) {
        StartCoroutine(LoadAsync(index));
    }

    IEnumerator LoadAsync(int sceneIndex) {
        loadingScreen.SetActive(true);
        loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone) {
            if (loadingOperation.progress >= 0.9f) loadingOperation.allowSceneActivation = true;
            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}
