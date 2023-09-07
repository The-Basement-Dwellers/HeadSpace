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
    [SerializeField] GameObject cameraPanel;
    private GameObject player;

    void Start() {
        player = GameObject.Find("Player");
    }

    public void Interact() {
        cameraPanel.SetActive(true);
        player.transform.Find("Head").GetComponent<SpriteLibrary>().spriteLibraryAsset = playerWithCamera;
        player.transform.Find("Canvas").Find("Awarness Bar").gameObject.SetActive(true);
        player.transform.Find("Canvas").Find("Camera").gameObject.SetActive(true);
        player.transform.Find("Canvas").Find("Camera Bar").gameObject.SetActive(true);
        player.GetComponent<CameraWeapon>().enabled = true;
        pedestoolLight.SetActive(false);
        player.GetComponent<PlayerController>().playerMaxHealth = 100f;
        player.GetComponent<PlayerController>().playerHealth = 100f;
        Destroy(gameObject);
		EventController.ResetInteractables();
    }
}
