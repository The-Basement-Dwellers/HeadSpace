using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour {
    public static event Action<Vector3> setMoveDirectionEvent;
    public static event Action<float, GameObject> setHealthBarPercentEvent;
    public static event Action fire;

    public static void StartMoveDirectionEvent(Vector3 moveDirection) {
        setMoveDirectionEvent?.Invoke(moveDirection);
    }

    public static void StartHealthBarEvent(float percent, GameObject targetedGameObject) {
        setHealthBarPercentEvent?.Invoke(percent, targetedGameObject);
    }

    public static void StartFire() {
        fire?.Invoke();
    }
}
