using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTutorial : MonoBehaviour
{
    [SerializeField] GameObject cameraPanel;
    [SerializeField] GameObject holdPanel;
    
    private void OnEnable() {
        EventController.tutorialCameraHoldEvent += HoldTutorial;
        EventController.tutorialCameraMissEvent += Miss;
        CameraEventController.fired += Fire;
    }

    private void OnDisable() {
        EventController.tutorialCameraHoldEvent -= HoldTutorial;
        EventController.tutorialCameraMissEvent -= Miss;
        CameraEventController.fired -= Fire;
    }

    private void HoldTutorial() {
        cameraPanel.SetActive(false);
        holdPanel.SetActive(true);
    }

    private void Miss() {
        cameraPanel.SetActive(true);
        holdPanel.SetActive(false);
    }

    private void Fire() {
            holdPanel.SetActive(false);
        Destroy(gameObject);
    }
}
