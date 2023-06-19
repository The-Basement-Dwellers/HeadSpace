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

    // Update is called once per frame


    public void Doors()
    {
        if (Vector3.Distance(interactscript.player.transform.position, interactscript.closestObject.transform.position) <= 1.5 && interactscript.closestObject.GetComponent<BoxCollider2D>().isTrigger)
        {
            Debug.Log("Frick");
            interactscript.closestObject.GetComponent<BoxCollider2D>().isTrigger = false;
       
        }
        else
        {
            Debug.Log("Interacted");
            interactscript.closestObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
