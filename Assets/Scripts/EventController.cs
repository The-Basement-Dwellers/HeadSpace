using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour {
	public static event Action<Vector3> setLookDirectionEvent;
	public static event Action<Vector3> setMoveDirectionEvent;
	public static event Action<float, GameObject> setHealthBarPercentEvent;
	public static event Action dash;
	public static event Action<GameObject, float> damageEvent;
	public static event Action fire;
	
	public static void StartLookDirectionEvent(Vector3 lookDirection) {
		setLookDirectionEvent?.Invoke(lookDirection);
	}

	public static void StartMoveDirectionEvent(Vector3 moveDirection) {
		setMoveDirectionEvent?.Invoke(moveDirection);
	}

	public static void StartHealthBarEvent(float percent, GameObject targetedGameObject) {
		setHealthBarPercentEvent?.Invoke(percent, targetedGameObject);
	}

	public static void Damage(GameObject targetedGameObject, float damageAmount) {
		damageEvent?.Invoke(targetedGameObject, damageAmount);
	}

	public static void Fire() {
		fire?.Invoke();
	}

	public static void Dash() {
		dash?.Invoke();
	}
}
