using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepController : MonoBehaviour
{
    public bool playSound;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.enabled = playSound;
    }
}
