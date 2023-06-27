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

	[SerializeField] private GameObject pointer;
	[SerializeField] private GameObject bar;
	[SerializeField] private float pointerPeriod;
	private float elapsedTimeCooldown;

	private float halfBarWidth;
	private Vector3 startPos;
	private Vector3 endPos;
	private bool isShooting = false;
	
	[SerializeField] private bool enableTimingBar= true;

	private float elapsedTime;
	private float lerpScaleY = 0.95f;
	private bool isOnCooldown = false;

	private void OnEnable() {
		EventController.fire += StartFire;
	}

	private void OnDisable() {
		EventController.fire -= StartFire;
	}
	
	private void Update() {
		if (isOnCooldown) {
			StartCoroutine(BarLerp());
		} else {
			StopCoroutine(BarLerp());
		}

		if (isShooting) {
			StartCoroutine(PointerLerp());
		} else {
			StopCoroutine(PointerLerp());
		}
	}

	private void Fire(float modifier) {
		if (!isOnCooldown)
		{
			isOnCooldown = true;
			elapsedTimeCooldown = 0;

			flash.SetActive(true);
			Invoke("DisableFlash", 0.1f);

			List<GameObject> collidersCopy = new List<GameObject>(colliders);

			List<GameObject> damagedColliders = new List<GameObject>();
			foreach (GameObject collider in collidersCopy) {
				if (collider.gameObject.tag == "Enemy") {
					RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, collider.transform.position - player.transform.position, rayDistance, rayLayerMask);
					
					bool hasLOS = checkLOS(collider, hits);
					foreach (RaycastHit2D hit in hits) {
						if (hit.collider != null && hit.collider.gameObject.tag == "Enemy" && !damagedColliders.Contains(hit.collider.gameObject) && hasLOS && colliders.Contains(hit.collider.gameObject)) {
							if (showRay) {
								Debug.DrawRay(player.transform.position, (hit.point - (Vector2)player.transform.position), Color.red, 1f);
							}
							EventController.Damage(hit.collider.gameObject, damageAmount * modifier);
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
		float percentageComplete = elapsedTimeCooldown / cooldown;

		elapsedTimeCooldown += Time.deltaTime;
		if (percentageComplete >= 1)
		{
			isOnCooldown = false;
		}

		lerpScaleY = Mathf.Lerp(0, 0.95f, percentageComplete);
		cameraBar.transform.localScale = new Vector3(0.25f, lerpScaleY, 0f);
		yield return null;
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

	private void StartFire() {
		if (!isOnCooldown) {
			if (enableTimingBar) {
				if (isShooting && !isOnCooldown) {
					isShooting = false;
					float pointerDistance = Mathf.Abs(pointer.transform.localPosition.x) * 2;
					float dmgModifier = 1 - pointerDistance;
					bar.SetActive(false);
					Fire(dmgModifier);
				} else {
					elapsedTime = 0;
					isShooting = false;
					bar.SetActive(true);
					isShooting = true;
				}
			} else {
				Fire(1);
			}
		}
	}

	private IEnumerator PointerLerp()
	{
		float percentageComplete = elapsedTime / pointerPeriod;
		elapsedTime += Time.deltaTime;

		halfBarWidth = bar.transform.localScale.x / 2;
		startPos = new Vector3(bar.transform.position.x - halfBarWidth, bar.transform.position.y, 0);
		endPos = new Vector3(bar.transform.position.x + halfBarWidth, bar.transform.position.y, 0);
		
		pointer.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

		if (percentageComplete >= 1) {
			if (bar.activeSelf) {
				EventController.Fire();
			}
		}
		yield return null;
	}
}