using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteraction;
using Pathfinding;
using UnityEngine.U2D.Animation;

public class Camera : MonoBehaviour, IInteractable
{
    [SerializeField] SpriteLibraryAsset playerWithCamera;
    [SerializeField] GameObject pedestoolLight;
    GameObject player;

    void Start() {
        player = GameObject.Find("Player");
    }

    public void Interact() {
        player.transform.Find("Head").GetComponent<SpriteLibrary>().spriteLibraryAsset = playerWithCamera;
        player.transform.Find("Canvas").Find("Camera").gameObject.SetActive(true);
        player.transform.Find("Canvas").Find("Camera Bar").gameObject.SetActive(true);
        player.GetComponent<CameraWeapon>().enabled = true;
        Destroy(gameObject);
		EventController.ResetInteractables();
        pedestoolLight.SetActive(false);
        EventController.StartHealthBarEvent(1f, player);
        player.GetComponent<PlayerController>().playerHealth = 100f;
    }
}
