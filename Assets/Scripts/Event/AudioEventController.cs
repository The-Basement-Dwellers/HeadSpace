using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventController : MonoBehaviour
{
    public static event Action cameraShoot;
    public static event Action cameraShootPrint;
    public static event Action dash;
    public static event Action doorOpen;
    public static event Action doorClose;

    public static void CameraShoot() {
        cameraShoot?.Invoke();
    }

    public static void CameraShootPrint() {
        cameraShootPrint?.Invoke();
    }

    public static void Dash() {
        dash?.Invoke();
    }

    public static void DoorOpen() {
        doorOpen?.Invoke();
    }

    public static void DoorClose() {
        doorClose?.Invoke();
    }
}
