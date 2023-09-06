using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class cameraPickup : MonoBehaviour
{
    [SerializeField] SpriteLibraryAsset playerWithCamera;
    GameObject player;

    private void Start() {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            player.transform.Find("Head").GetComponent<SpriteLibrary>().spriteLibraryAsset = playerWithCamera;
            player.transform.Find("Canvas").Find("Camera").gameObject.SetActive(true);
            player.transform.Find("Canvas").Find("Camera Bar").gameObject.SetActive(true);
            player.GetComponent<CameraWeapon>().enabled = true;
            Destroy(gameObject);
        }
    }
}
