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
    void Update()
    {
        
    }

    public void Doors()
    {
        if (Vector3.Distance(interactscript.player.transform.position, interactscript.closestObject.transform.position) < 3 && interactscript.closestObject.GetComponent<BoxCollider2D>() == true)
        {
            Debug.Log("Interacted");
            Destroy(interactscript.closestObject.GetComponent<BoxCollider2D>());

        }
        else
        {
            Debug.Log("fuck");
            interactscript.closestObject.AddComponent<BoxCollider2D>();
        }
    }
}
