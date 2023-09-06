using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour {
    public static event Action<Vector3> setLookDirectionEvent;
    public static event Action<Vector3, GameObject> setMoveDirectionEvent;
	public static event Action<float, GameObject> setHealthBarPercentEvent;

	public static event Action<bool> startIsDashingEvent;

	public static event Action dash;
	public static event Action interactEvent;

	public static event Action<GameObject> colliderEnter;
	public static event Action<GameObject> colliderExit;

	public static event Action<bool> isBulletTime;

	public static void StartColliderEnterEvent(GameObject targetedGameObject) {
		colliderEnter?.Invoke(targetedGameObject);
	}

	public static void StartColliderExitEvent(GameObject targetedGameObject) {
		colliderExit?.Invoke(targetedGameObject);
	}
	
	public static void StartLookDirectionEvent(Vector3 lookDirection) {
		setLookDirectionEvent?.Invoke(lookDirection);
	}

	public static void StartIsDashingEvent(bool isDashing) {
        startIsDashingEvent?.Invoke(isDashing);
    }

    public static void StartMoveDirectionEvent(Vector3 moveDirection, GameObject targetedGameObject) {
        setMoveDirectionEvent?.Invoke(moveDirection, targetedGameObject);
    }

	public static void StartHealthBarEvent(float percent, GameObject targetedGameObject) {
		setHealthBarPercentEvent?.Invoke(percent, targetedGameObject);
	}

	public static void Dash() {
		dash?.Invoke();
	}

	public static void InteractEvent()
	{
		interactEvent?.Invoke();
	}

	public static void IsBulletTime(bool bulletTime) {
		isBulletTime?.Invoke(bulletTime);
	}
}
