using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteraction;
using Pathfinding;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
	private GraphUpdateScene gustavofring;
	[SerializeField] Sprite open, closed;
	[SerializeField] public bool openByDefault = false;
	[SerializeField] public bool lockUntillEnemiesDead = false;
	[SerializeField] public bool endingDoor = false;
	[SerializeField] GameObject linkedDoor;

	void Start()
	{
		gustavofring = gameObject.GetComponent<GraphUpdateScene>();
		if (openByDefault) {
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			gameObject.GetComponent<SpriteRenderer>().sprite = open;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            gameObject.layer = 2;
		}
	}

	private void Update() {
        if (lockUntillEnemiesDead)
        {
            gameObject.tag = "Untagged";
            EventController.ResetInteractables();
            ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
            ParticleSystem.EmissionModule emission = particleSystem.emission;
            emission.enabled = true;
        }

        if (lockUntillEnemiesDead) {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			if (enemies.Length <= 0) {
				lockUntillEnemiesDead = false;
				gameObject.tag = "Interactable";
				EventController.ResetInteractables();
				//Interact();
				ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
				ParticleSystem.EmissionModule emission = particleSystem.emission;
				emission.enabled = false;
			}
		}
	}

    public void Interact()
	{

		if  (gameObject.GetComponent<BoxCollider2D>().isTrigger)
		{
			AudioEventController.DoorClose();
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
			gameObject.GetComponent<SpriteRenderer>().sprite = closed;
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
			gameObject.layer = 4;
		}
		else
		{
			AudioEventController.DoorOpen();
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			gameObject.GetComponent<SpriteRenderer>().sprite = open;
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
			gameObject.layer = 2;
		}
		gustavofring.Apply();
		if (endingDoor) {
			StartCoroutine(LoadNextScene());
		}

        if (linkedDoor != null && linkedDoor.GetComponent<BoxCollider2D>().isTrigger != gameObject.GetComponent<BoxCollider2D>().isTrigger)
        {
            linkedDoor.GetComponent<Door>().Interact();
        }
    }

	private IEnumerator LoadNextScene() {
		yield return new WaitForSeconds(0.3f);
		SceneController.StartScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
