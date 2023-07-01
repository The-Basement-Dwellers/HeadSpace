using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInteraction;

public class Door : MonoBehaviour, IInteractable
{
    public GameObject player;
    public void Interact()
    {
        if  (gameObject.GetComponent<BoxCollider2D>().isTrigger)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);

        }
    }
}
