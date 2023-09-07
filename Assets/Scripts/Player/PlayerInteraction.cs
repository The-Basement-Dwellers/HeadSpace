using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Material whiteOutline;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private GameObject[] allInteractables;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject closestObject;
    private bool isHighlighted;
    private IInteractable targetedGameObject;


    void OnEnable()
    {
        EventController.interactEvent += Interact;
    }

    private void OnDisable()
    {
        EventController.interactEvent -= Interact;
    }

    // Stores all interactables in an array and sets closestObject to first index
    void Start()
    {
        allInteractables = GameObject.FindGameObjectsWithTag("Interactable");
    }

    // Outlining Logic
    void Update()
    {
        if (allInteractables.Length > 0)
        {
            GameObject oldObject = closestObject;
            closestObject = allInteractables[0];

            if (oldObject != null)
            {
                foreach (GameObject i in allInteractables)
                {
                    float interactableDist = Vector3.Distance(player.transform.position, i.transform.position);

                    if (interactableDist < Vector3.Distance(player.transform.position, closestObject.transform.position))
                    {
                        closestObject = i;
                    }
                }
                oldObject.GetComponent<Renderer>().material = defaultMaterial;
                if (Vector2.Distance(player.transform.position, closestObject.transform.position) <= 1.5)
                {

                    closestObject.GetComponent<Renderer>().material = whiteOutline;
                    targetedGameObject = closestObject.GetComponent<IInteractable>();
                    isHighlighted = true;

                }
                else
                {
                    isHighlighted = false;
                    closestObject = null;
                }
            }
        }
    }
    
    void Interact()
    {
        //Debug.Log(targetedGameObject);
        if (isHighlighted) {
            targetedGameObject.Interact();
        }
        else
        {
            Debug.Log("Not In Range of Interactable");
        }
        
    }
}
