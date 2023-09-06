using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteraction;
using Pathfinding;

public class Door : MonoBehaviour, IInteractable
{
	private GraphUpdateScene gustavofring;
	[SerializeField] Sprite open, closed;

	void Start()
	{
		gustavofring = gameObject.GetComponent<GraphUpdateScene>();
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
    }
}
