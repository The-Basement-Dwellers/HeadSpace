using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWeapon : MonoBehaviour
{
    public List<GameObject> colliders = new List<GameObject>();
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private bool showRay = false;
    [SerializeField] private GameObject flash;
    [SerializeField] private GameObject cameraBar;
    [SerializeField] private float damageAmount = 40f;
    [SerializeField] private float cooldown = 1f;

    private float elapsedTime;
    private bool isOnCooldown = false;
    private float lerpScaleY = 0.95f;



    private void OnEnable() {
        EventController.fire += Fire;
    }

    private void OnDisable() {
        EventController.fire -= Fire;
    }

    private void Fire() {
        if (!isOnCooldown)
        {
            isOnCooldown = true;
            elapsedTime = 0;

            flash.SetActive(true);
            Invoke("DisableFlash", 0.1f);

            foreach (GameObject collider in colliders)
            {
                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, collider.transform.position - player.transform.position, rayDistance, playerLayerMask);
                    if (collider.gameObject == hit.collider.gameObject && hit.collider.gameObject.tag == "Enemy")
                    {
                        EventController.Damage(hit.collider.gameObject, damageAmount);
                    }

                    if (showRay)
                    {
                        Debug.DrawRay(player.transform.position, collider.transform.position - player.transform.position, Color.red, 2f);
                    }
                
            }
            
        }
    }

    private void Update()
    {
        if (isOnCooldown) {
            StartCoroutine(BarLerp());
        } else {
            StopCoroutine(BarLerp());
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
}