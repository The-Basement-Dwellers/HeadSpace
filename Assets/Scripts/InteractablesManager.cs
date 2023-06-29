using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    private PlayerInteraction interactscript;
    // Start is called before the first frame update
    void Start()
    {
        interactscript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
    }

    public void Doors()
    {
        Debug.Log("Interacted");
        if (Vector3.Distance(interactscript.player.transform.position, interactscript.closestObject.transform.position) < 3 && interactscript.closestObject.GetComponent<BoxCollider2D>() == true) {
            Destroy(interactscript.closestObject.GetComponent<BoxCollider2D>());
        } else {
            interactscript.closestObject.AddComponent<BoxCollider2D>();
        }
    }
}
