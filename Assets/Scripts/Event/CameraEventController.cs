using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEventController : MonoBehaviour
{
    public static event Action<bool> setIsShootingEvent;
    public static event Action fire;
    public static event Action fireRelease;
    public static event Action<GameObject, float> damageEvent;
    public static event Action<bool> setCanMoveFlash;
    public static event Action fired;


    public static void StartIsShootingEvent(bool isShooting)
    {
        setIsShootingEvent?.Invoke(isShooting);
    }

    public static void Damage(GameObject targetedGameObject, float damageAmount)
    {
        damageEvent?.Invoke(targetedGameObject, damageAmount);
    }

    public static void Fire()
    {
        fire?.Invoke();
    }

    public static void FireRelease()
    {
        fireRelease?.Invoke();
    }

    public static void StartCanMoveFlashEvent(bool canMoveFlash)
    {
        setCanMoveFlash?.Invoke(canMoveFlash);
    }

    public static void Fired()
    {
        fired?.Invoke();
    }
}
