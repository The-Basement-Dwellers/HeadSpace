using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource
        cameraShoot,
        cameraShootPrint,
        dash,
        doorOpen,
        doorClose,
        hit;

    private void OnEnable()
    {
        AudioEventController.cameraShoot += CameraShoot;
        AudioEventController.cameraShootPrint += CameraShootPrint;
        AudioEventController.dash += Dash;
        AudioEventController.hit += Hit;
        AudioEventController.doorOpen += DoorOpen;
        AudioEventController.doorClose += DoorClose;
    }

    private void OnDisable()
    {
        AudioEventController.cameraShoot -= CameraShoot;
        AudioEventController.cameraShootPrint -= CameraShootPrint;
        AudioEventController.dash -= Dash;
        AudioEventController.hit -= Hit;
        AudioEventController.doorOpen -= DoorOpen;
        AudioEventController.doorClose -= DoorClose;
    }

    private void CameraShoot()
    {
        cameraShoot.Play();
    }

    private void CameraShootPrint() {
        cameraShootPrint.Play();
    }

    private void Dash() {
        dash.Play();
    }

    private void DoorOpen() {
        doorOpen.Play();
    }

    private void DoorClose() {
        doorClose.Play();
    }

    private void Hit() {
        hit.Play();
    }
}
