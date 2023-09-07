using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && door.GetComponent<SpriteRenderer>().sprite.name == "door open") {
            door.GetComponent<Door>().Interact();
        }
    }
}
