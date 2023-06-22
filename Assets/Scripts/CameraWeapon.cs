using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWeapon : MonoBehaviour
{
	public List<GameObject> colliders = new List<GameObject>();
	[SerializeField] private LayerMask rayLayerMask;
	[SerializeField] private GameObject player;
	[SerializeField] private GameObject flash;
	[SerializeField] private GameObject cameraBar;
	[SerializeField] private float rayDistance = 10f;
	[SerializeField] private float damageAmount = 40f;
	[SerializeField] private float cooldown = 1f;
	[SerializeField] private bool showRay = false;

	private float elapsedTime;
	private float lerpScaleY = 0.95f;
	private bool isOnCooldown = false;

	private void OnEnable() {
		EventController.fire += Fire;
	}

	private void OnDisable() {
		EventController.fire -= Fire;
	}
	
	private void Update() {
		if (isOnCooldown) {
			StartCoroutine(BarLerp());
		} else {
			StopCoroutine(BarLerp());
		}
	}

	private void Fire() {
		if (!isOnCooldown)
		{
			isOnCooldown = true;
			elapsedTime = 0;

			flash.SetActive(true);
			Invoke("DisableFlash", 0.1f);

			List<GameObject> collidersCopy = new List<GameObject>(colliders);

			List<GameObject> damagedColliders = new List<GameObject>();
			foreach (GameObject collider in collidersCopy) {
				if (collider.gameObject.tag == "Enemy") {
					RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, collider.transform.position - player.transform.position, rayDistance, rayLayerMask);
					
					bool hasLOS = checkLOS(collider, hits);
					foreach (RaycastHit2D hit in hits) {
						if (hit.collider != null && hit.collider.gameObject.tag == "Enemy" && !damagedColliders.Contains(hit.collider.gameObject) && hasLOS) {
							if (showRay) {
								Debug.DrawRay(player.transform.position, (hit.point - (Vector2)player.transform.position), Color.red, 1f);
							}
							EventController.Damage(hit.collider.gameObject, damageAmount);
							damagedColliders.Add(hit.collider.gameObject);
						}
					}
				}
			}
		}
	}

	private void DisableFlash() {
		flash.SetActive(false);
	}

	private IEnumerator BarLerp()
	{
		float percentageComplete = elapsedTime / cooldown;

		elapsedTime += Time.deltaTime;
		if (percentageComplete >= 1)
		{
			isOnCooldown = false;
		}

		lerpScaleY = Mathf.Lerp(0, 0.95f, percentageComplete);
		cameraBar.transform.localScale = new Vector3(0.25f, lerpScaleY, 0f);
		yield return null;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		colliders.Add(collision.gameObject);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		colliders.Remove(collision.gameObject);
	}
	
	private bool checkLOS(GameObject collider, RaycastHit2D[] hits) 
	{	
		bool hasLOS = true;
		float colliderDistance = 0;
		float nonColliderDistance = 0;
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider.gameObject == collider) 
			{
				colliderDistance = hit.fraction;
			}
			else if (hit.collider.gameObject.tag != "Enemy")
			{
				nonColliderDistance= hit.fraction;
			}
		}
		
		if (colliderDistance > nonColliderDistance) hasLOS = false;
		return hasLOS;
	}
}