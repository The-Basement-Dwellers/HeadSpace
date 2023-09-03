using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteraction;
using Pathfinding;

public class Door : MonoBehaviour, IInteractable
{
	private GraphUpdateScene gustavofring;

	void Start()
	{
		gustavofring = gameObject.GetComponent<GraphUpdateScene>();
	}
    public void Interact()
	{
		if  (gameObject.GetComponent<BoxCollider2D>().isTrigger)
		{
			AudioEventController.DoorOpen();
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			gameObject.layer = 4;
			

		}
		else
		{
			AudioEventController.DoorClose();
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
			gameObject.layer = 2;
		}
        gustavofring.Apply();
    }
}
