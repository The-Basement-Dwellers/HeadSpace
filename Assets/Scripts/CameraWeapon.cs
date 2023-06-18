using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWeapon : MonoBehaviour
{
    private void OnEnable() {
        EventController.fire += Fire;
    }

    private void OnDisable() {
        EventController.fire -= Fire;
    }

    private void Fire() {
        Debug.Log("Fired");
    }
}