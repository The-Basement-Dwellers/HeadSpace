using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static event Action<int> loadScene;
    [SerializeField] private GameObject loadingScreen;
    public Slider loadingBar;
    private AsyncOperation loadingOperation;

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
