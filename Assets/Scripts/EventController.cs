using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour {
    public static event Action<GameObject, Vector3> setLookDirectionEvent;
    public static event Action<GameObject, Vector3> setMoveDirectionEvent;
    public static event Action<GameObject, Vector3> setEnemyMoveDirectionEvent;
	public static event Action<float, GameObject> setHealthBarPercentEvent;
	public static event Action<bool> startIsDashingEvent;
	public static event Action<bool> setCanMoveFlash;
	public static event Action dash;
	public static event Action interactEvent;
	public static event Action<GameObject, float> damageEvent;
	public static event Action fire;
	public static event Action fireRelease;
	
	public static void StartLookDirectionEvent(GameObject targetedGameObject, Vector3 lookDirection) {
		setLookDirectionEvent?.Invoke(targetedGameObject, lookDirection);
	}

	public static void StartIsDashingEvent(bool isDashing) {
        startIsDashingEvent?.Invoke(isDashing);
    }

    public static void StartMoveDirectionEvent(GameObject targetedGameObject, Vector3 moveDirection) {
        setMoveDirectionEvent?.Invoke(targetedGameObject, moveDirection);
    }

	public static void StartEnemyMoveDirectionEvent(GameObject targetedGameObject, Vector3 moveDirection)
    {
        setEnemyMoveDirectionEvent?.Invoke(targetedGameObject, moveDirection);
    }

	public static void StartHealthBarEvent(float percent, GameObject targetedGameObject) {
		setHealthBarPercentEvent?.Invoke(percent, targetedGameObject);
	}
	
	public static void StartCanMoveFlashEvent(bool canMoveFlash) {
		setCanMoveFlash?.Invoke(canMoveFlash);
	}

	public static void enemyHurt(GameObject targetedGameObject, float damageAmount) {
        damageEvent?.Invoke(targetedGameObject, damageAmount);
    }

	public static void Fire() {
		fire?.Invoke();
	}
	
	public static void FireRelease() {
		fireRelease?.Invoke();
	}

	public static void Dash() {
		dash?.Invoke();
	}

	public static void InteractEvent()
	{
		interactEvent?.Invoke();
	}
}
