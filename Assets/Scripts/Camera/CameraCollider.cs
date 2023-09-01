using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
	[SerializeField] private CameraWeapon cameraWeapon;
	
	private void OnTriggerEnter2D(Collider2D collision) {
		cameraWeapon.colliders.Add(collision.gameObject);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		cameraWeapon.colliders.Remove(collision.gameObject);
	}
}