using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip cameraShootClip;
    [SerializeField] private AudioClip cameraShootPrintClip;
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AudioEventController.cameraShoot += cameraShoot;
        AudioEventController.cameraShootPrint += cameraShootPrint;
        AudioEventController.dash += Dash;
    }

    private void OnDisable()
    {
        AudioEventController.cameraShoot -= cameraShoot;
        AudioEventController.cameraShootPrint -= cameraShootPrint;
        AudioEventController.dash += Dash;
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
}
