using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private AudioClip
        cameraShootClip,
        cameraShootPrintClip,
        dash,
        doorOpen,
        doorClose,
        hit;

    private void OnEnable()
    {
        AudioEventController.cameraShoot += cameraShoot;
        AudioEventController.cameraShootPrint += cameraShootPrint;
        AudioEventController.dash += Dash;
        AudioEventController.hit += Hit;
    }

    private void OnDisable()
    {
        AudioEventController.cameraShoot -= cameraShoot;
        AudioEventController.cameraShootPrint -= cameraShootPrint;
        AudioEventController.dash -= Dash;
        AudioEventController.hit -= Hit;
    }

    private void cameraShoot()
    {
        audioSource.clip = cameraShootClip;
        audioSource.Play();
    }

    private void cameraShootPrint() {
        audioSource.clip = cameraShootPrintClip;
        audioSource.Play();
    }

    private void Dash() {
        audioSource.clip = dash;
        audioSource.Play();
    }

    private void DoorOpen() {
        audioSource.clip = doorOpen;
        audioSource.Play();
    }

    private void DoorClose() {
        audioSource.clip = doorClose;
        audioSource.Play();
    }

    private void Hit() {
        audioSource.clip = hit;
        audioSource.Play();
    }
}
