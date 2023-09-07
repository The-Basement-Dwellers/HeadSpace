using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraWeapon : MonoBehaviour
{
	public List<GameObject> colliders = new List<GameObject>();
	[SerializeField] private LayerMask rayLayerMask;
	[SerializeField] private GameObject player;
	[SerializeField] private GameObject flash;
	[SerializeField] private GameObject flashSpill;
	[SerializeField] private GameObject rangeFlashSpill;
	[SerializeField] private GameObject preFlash;
	[SerializeField] private GameObject rangeFlash;
	[SerializeField] private GameObject cameraBar;
	[SerializeField] private float rayDistance = 10f;
	[SerializeField] private float damageAmount = 100f;
	[SerializeField] private float cooldown = 1f;
	[SerializeField] private bool showRay = false;

	[SerializeField] private GameObject collision;
	[SerializeField] private float rangePeriod = 2;
	[SerializeField] private float flashDuration = 0.1f;

	private bool loading = false;
	private float range = 0;
	private float elapsedTimeCooldown;
	private float rangePercent;
	private bool isShooting = false;
	[SerializeField] private float shootThreshhold = 0.2f;
	private bool isPastThreshhold = false;

	private float elapsedTime;
	private float lerpScaleY = 0.95f;
	private bool isOnCooldown = false;

	private void OnEnable() {
		CameraEventController.fireRelease += StopFire;
		EventController.setLookDirectionEvent += SetLookDirectionEvent;
	}

	private void OnDisable() {
		CameraEventController.fireRelease -= StopFire;
		EventController.setLookDirectionEvent -= SetLookDirectionEvent;
	}
	
	private void SetLookDirectionEvent(Vector3 lookDirection) {
		if (lookDirection.magnitude > 0) {
			if (!isOnCooldown) {
				isShooting = true;
				preFlash.SetActive(true);
			}
		} else StopFire();
	}
	
	private void Update() {
		if (isOnCooldown) {
			StartCoroutine(BarLerp());
		} else {
			StopCoroutine(BarLerp());
		}
		
		if (isShooting) {
			CameraEventController.StartIsShootingEvent(true);
			StartCoroutine(RangeLerp());
		} else {
			CameraEventController.StartIsShootingEvent(false);
			StopCoroutine(RangeLerp());
		}
	}

	private void Fire(float range = -1) {
		if (range == -1) range = collision.transform.localScale.y;
		
		if (!isOnCooldown)
		{
			isOnCooldown = true;
			elapsedTimeCooldown = 0;

			flash.SetActive(true);
			flash.GetComponent<Light2D>().pointLightInnerRadius = range - 0.5f;
			flash.GetComponent<Light2D>().pointLightOuterRadius = range;
			flashSpill.GetComponent<Light2D>().pointLightInnerRadius = range;
			flashSpill.GetComponent<Light2D>().pointLightOuterRadius = range * 2;
			Invoke("DisableFlash", flashDuration);

			bool playAudio = true;

			List<GameObject> collidersCopy = new List<GameObject>(colliders);

			List<GameObject> damagedColliders = new List<GameObject>();
			foreach (GameObject collider in collidersCopy) {
				if (collider != null) {
					if (collider.gameObject.tag == "Enemy") {
						rayDistance = Vector3.Distance(player.transform.position, collider.transform.position);
						RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, collider.transform.position - player.transform.position, rayDistance, rayLayerMask);
						bool hasLOS = checkLOS(collider, hits);
						foreach (RaycastHit2D hit in hits) {
							bool hasCollider = hit.collider != null;
							bool isEnemy = hit.collider.gameObject.tag == "Enemy";
							bool isDamaged = damagedColliders.Contains(hit.collider.gameObject);
							if (hasCollider && isEnemy && !isDamaged) {
								if (showRay) {
									Debug.DrawRay(player.transform.position, (hit.point - (Vector2)player.transform.position), Color.red, 1f);
									Debug.DrawLine(player.transform.position, player.transform.position + (collider.transform.position - player.transform.position).normalized * range, Color.green, 1f);
								}

								bool withinRange = Vector3.Distance(player.transform.position, hit.collider.gameObject.transform.position) <= range;
								if (withinRange && hasLOS) {
									playAudio = false;
									Vector3 dif = hit.transform.position - transform.position;
									float chargeDamageAmount = damageAmount * rangePercent;
									CameraEventController.Damage(hit.collider.gameObject, chargeDamageAmount);
									damagedColliders.Add(hit.collider.gameObject);
                                    CameraEventController.Fired();
                                    StartCoroutine("CheckEnemys");
								}
							}
						}
					}
				} else {
					colliders.Remove(collider);
				}
			}
			range = 0;
			preFlash.SetActive(false);

			if (playAudio) AudioEventController.CameraShoot();
        }
	}
	
	private void DisableFlash() {
		flash.SetActive(false);
		CameraEventController.StartCanMoveFlashEvent(true);
    }

    private void DisablePreFlash() {
		preFlash.SetActive(false);
	}
	
	private void EnablePreFlash() {
		preFlash.SetActive(true);
		Invoke("DisablePreFlash", flashDuration / 4);
	}
	
	private void DelayedFire() {
			rangeFlash.SetActive(false);
			elapsedTime = 0;
			isShooting = false;
			Fire(range);
	}
	
	private bool checkLOS(GameObject collider, RaycastHit2D[] hits) 
	{	
		bool hasLOS = true;
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider.gameObject.tag != "Enemy") {
				hasLOS = false;
			}
		}
		return hasLOS;
	}
	
	private IEnumerator BarLerp()
	{
		float percentageComplete = elapsedTimeCooldown / (cooldown * rangePercent);

		elapsedTimeCooldown += Time.deltaTime;
		if (percentageComplete >= 1)
		{
			isOnCooldown = false;
		}

		lerpScaleY = Mathf.Lerp(0, 1f, percentageComplete);
		cameraBar.transform.localScale = new Vector3(1, lerpScaleY, 1f);

		float barHeight = cameraBar.GetComponent<RectTransform>().rect.height;
		cameraBar.transform.localPosition = new Vector3(0, -barHeight + (barHeight/2 * lerpScaleY) + 5f, 0);
		yield return null;
	}
	
	private IEnumerator RangeLerp()
	{
		float percentageComplete = elapsedTime / rangePeriod;
		elapsedTime += Time.deltaTime;

		float maxRange = collision.transform.localScale.y;
		rangePercent = percentageComplete;
		percentageComplete = Mathf.Clamp(percentageComplete, 0, 1);
		if (percentageComplete < shootThreshhold) rangePercent = 0;
		else rangePercent = (percentageComplete - shootThreshhold);
		rangePercent = (rangePercent / (1 - shootThreshhold));
		rangePercent = Mathf.Clamp(rangePercent, 0, 1);
		range = Mathf.Lerp(0, maxRange, rangePercent);
		rangeFlash.GetComponent<Light2D>().pointLightInnerRadius = range - 0.75f;
		rangeFlash.GetComponent<Light2D>().pointLightOuterRadius = range;
		rangeFlash.SetActive(true);
		rangeFlashSpill.GetComponent<Light2D>().pointLightInnerRadius = range;

		if (percentageComplete >= 1) {
			elapsedTime = rangePeriod;
		}

		if (percentageComplete < shootThreshhold) isPastThreshhold = false;
		else isPastThreshhold = true;

		yield return null;
	}

	private IEnumerator CheckEnemys() {
		yield return new WaitForSeconds(0.1f);
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) {
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            if (!loading) {
				loading = true;
				SceneController.StartScene(index);
			}
        }
		yield return null;
	}
	
	private void StopFire() {
        if (isShooting && isPastThreshhold) {
			CameraEventController.StartCanMoveFlashEvent(false);
			DisablePreFlash();
            Invoke("EnablePreFlash", flashDuration / 2);
			Invoke("EnablePreFlash", (flashDuration / 2) * 3);
			Invoke("DelayedFire", flashDuration * 2);
		} else if (!isPastThreshhold) {
			rangeFlashSpill.SetActive(false);
			rangeFlash.SetActive(false);
			preFlash.SetActive(false);
			elapsedTime = 0;
		}
        isShooting = false;
    }
}