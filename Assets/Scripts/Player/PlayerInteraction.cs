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
    private IInteractable targetedGameObject;


    void OnEnable()
    {
        EventController.interactEvent += InteractTest;
    }

    private void OnDisable()
    {
        EventController.interactEvent -= InteractTest;
    }

    // Stores all interactables in an array and sets closestObject to first index
    void Start()
    {
        allInteractables = GameObject.FindGameObjectsWithTag("Interactable");
        closestObject = allInteractables[0];
    }

    // Outlining Logic
    void Update()
    {
        GameObject oldObject = closestObject;
        closestObject = allInteractables[0];
        for (int i = 0; i < allInteractables.Length; i++)
        {
            float interactableDist = Vector3.Distance(player.transform.position, allInteractables[i].transform.position);

            if (interactableDist < Vector3.Distance(player.transform.position, closestObject.transform.position))
            {
                closestObject = allInteractables[i];
            }
        }
        targetedGameObject = closestObject.GetComponent<IInteractable>();
        oldObject.GetComponent<Renderer>().material = defaultMaterial;

        if (Vector3.Distance(player.transform.position, closestObject.transform.position) <= 1.5)
        {
            closestObject.GetComponent<Renderer>().material = whiteOutline;
        }
        
    }
    
    void InteractTest()
    {
        //Debug.Log(targetedGameObject);
        targetedGameObject.Interact();
    }
}
