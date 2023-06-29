using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class CameraWeapon : MonoBehaviour
{
	public List<GameObject> colliders = new List<GameObject>();
	[SerializeField] private LayerMask rayLayerMask;
	[SerializeField] private GameObject player;
	[SerializeField] private GameObject flash;
	[SerializeField] private GameObject preFlash;
	[SerializeField] private GameObject rangeFlash;
	[SerializeField] private GameObject cameraBar;
	[SerializeField] private float rayDistance = 10f;
	[SerializeField] private float damageAmount = 40f;
	[SerializeField] private float cooldown = 1f;
	[SerializeField] private bool showRay = false;

	[SerializeField] private GameObject timingPointer;
	[SerializeField] private GameObject timingBar;
	[SerializeField] private GameObject collision;
	[SerializeField] private float timingPointerPeriod = 2;
	[SerializeField] private float rangePeriod = 2;
	[SerializeField] private float flashDuration = 0.1f;
	private float range = 0;
	private float elapsedTimeCooldown;
	private float halftimingBarWidth;
	private Vector3 startPos;
	private Vector3 endPos;
	private bool isShooting = false;
	
	[SerializeField] private bool enableTimingBar = true;
	[SerializeField] private bool enableRangeBar = true;
	private bool isRangeShooting = false;

	private float elapsedTime;
	private float lerpScaleY = 0.95f;
	private bool isOnCooldown = false;

	private void OnEnable() {
		EventController.fire += StartFire;
		EventController.fireRelease += StopFire;
		
		flash.GetComponent<Light2D>().pointLightInnerRadius = collision.transform.localScale.y - 0.5f;
		flash.GetComponent<Light2D>().pointLightOuterRadius = collision.transform.localScale.y ;
	}

	private void OnDisable() {
		EventController.fire -= StartFire;
		EventController.fireRelease -= StopFire;
	}
	
	private void Update() {
		if (isOnCooldown) {
			StartCoroutine(BarLerp());
		} else {
			StopCoroutine(BarLerp());
		}

		if (isShooting) {
			StartCoroutine(timingPointerLerp());
		} else {
			StopCoroutine(timingPointerLerp());
		}
		
		if (isRangeShooting) {
			StartCoroutine(RangeLerp());
		} else {
			StopCoroutine(RangeLerp());
		}
	}
	
	private void StartFire() {
		if (!isOnCooldown) {
			if (enableTimingBar) {
				if (isShooting && !isOnCooldown) {
					DisablePreFlash();
					Invoke("EnablePreFlash", flashDuration / 2);
					isShooting = false;
					float timingPointerDistance = Mathf.Abs(timingPointer.transform.localPosition.x) * 2;
					float dmgModifier = 1 - timingPointerDistance;
					timingBar.SetActive(false);
					Fire(modifier: dmgModifier);
				} else {
					elapsedTime = 0;
					timingBar.SetActive(true);
					isShooting = true;
					preFlash.SetActive(true);
				}
			} else if (enableRangeBar) {
				rangeFlash.SetActive(true);
				range = 0;
				isRangeShooting = true;
				preFlash.SetActive(true);
			} else {
				Fire();
			}
		}
	}

	private void Fire(float modifier = 1, float range = -1) {
		if (range == -1) range = collision.transform.localScale.y;
		
		if (!isOnCooldown)
		{
			isOnCooldown = true;
			elapsedTimeCooldown = 0;

			flash.SetActive(true);
			flash.GetComponent<Light2D>().pointLightInnerRadius = range - 0.5f;
			flash.GetComponent<Light2D>().pointLightOuterRadius = range;
			EventController.StartCanMoveFlashEvent(false);
			Invoke("DisableFlash", flashDuration);

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
								Debug.DrawLine(player.transform.position, player.transform.position + (collider.transform.position - player.transform.position).normalized * range, Color.green, 1f);
							}						
							
							if (Vector3.Distance(player.transform.position, hit.collider.gameObject.transform.position) <= range) {
								EventController.Damage(hit.collider.gameObject, damageAmount * modifier);
								damagedColliders.Add(hit.collider.gameObject);
							}
							
						}
					}
				}
			}
		}
	}
	
	private void DisableFlash() {
		flash.SetActive(false);
		flash.GetComponent<Light2D>().pointLightInnerRadius = collision.transform.localScale.y - 0.5f;
		flash.GetComponent<Light2D>().pointLightOuterRadius = collision.transform.localScale.y ;
		EventController.StartCanMoveFlashEvent(true);
	}
	
	private void DisablePreFlash() {
		preFlash.SetActive(false);
	}
	
	private void EnablePreFlash() {
		preFlash.SetActive(true);
		Invoke("DisablePreFlash", flashDuration / 2);
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

	private IEnumerator timingPointerLerp()
	{
		float percentageComplete = elapsedTime / timingPointerPeriod;
		elapsedTime += Time.deltaTime;

		halftimingBarWidth = timingBar.transform.localScale.x / 2;
		startPos = new Vector3(timingBar.transform.position.x - halftimingBarWidth, timingBar.transform.position.y, 0);
		endPos = new Vector3(timingBar.transform.position.x + halftimingBarWidth, timingBar.transform.position.y, 0);
		
		timingPointer.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

		if (percentageComplete >= 1) {
			if (timingBar.activeSelf) {
				EventController.Fire();
			}
		}
		yield return null;
	}
	
	private IEnumerator RangeLerp()
	{
		float percentageComplete = elapsedTime / rangePeriod;
		elapsedTime += Time.deltaTime;

		float maxRange = collision.transform.localScale.y;
		
		range = Mathf.Lerp(0, maxRange, percentageComplete);
		rangeFlash.GetComponent<Light2D>().pointLightInnerRadius = range - 0.75f;
		rangeFlash.GetComponent<Light2D>().pointLightOuterRadius = range;

		if (percentageComplete >= 1) {
			StopFire();
		}
		yield return null;
	}
	
	private void StopFire() {
		if (isRangeShooting && enableRangeBar) {
			DisablePreFlash();
			Invoke("EnablePreFlash", flashDuration / 2);
			//Invoke("EnablePreFlash", (flashDuration / 2) * 3);
			rangeFlash.SetActive(false);
			elapsedTime = 0;
			isRangeShooting = false;
			Fire(range: range);
		}
	}
}