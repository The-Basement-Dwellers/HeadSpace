using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWeapon : MonoBehaviour
{
    private List<GameObject> colliders = new List<GameObject>();
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private bool showRay = false;
    [SerializeField] private GameObject flash;
    [SerializeField] private GameObject cameraBar;
    [SerializeField] private float damageAmount = 40f;

    private void OnEnable() {
        EventController.fire += Fire;
    }

    private void OnDisable() {
        EventController.fire -= Fire;
    }

    private void Fire() {
        flash.SetActive(true);
        Invoke("DisableFlash", 0.1f);

        cameraBar.transform.localScale = new Vector3(0.25f, 0f, 0f);
        Invoke("RechargeBar", 0.5f);

        foreach (GameObject collider in colliders) {
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, collider.transform.position - player.transform.position, rayDistance, playerLayerMask);
            if (collider.gameObject == hit.collider.gameObject && hit.collider.gameObject.tag == "Enemy") {
                EventController.Damage(hit.collider.gameObject, damageAmount);
            }

            if (showRay) {
                Debug.DrawRay(player.transform.position, collider.transform.position - player.transform.position, Color.red, 2f);
            }
        }     
    }

    private void DisableFlash() {
        flash.SetActive(false);
    }

    private void RechargeBar() {
        cameraBar.transform.localScale = new Vector3(0.25f, 0.95f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        colliders.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        colliders.Remove(collision.gameObject);
    }
}