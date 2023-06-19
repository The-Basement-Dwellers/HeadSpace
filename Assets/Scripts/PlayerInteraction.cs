using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float influenceSphere;
    public Material myOutline;
    public Material defaultMaterial;
    public GameObject[] allInteractables;
    public GameObject player;
    /// public PlayerController controller;
    public float interactableDist;
    public GameObject closestObject;


    // Start is called before the first frame update
    void Start()
    {
        /// controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        allInteractables = GameObject.FindGameObjectsWithTag("Interactable");
        closestObject = allInteractables[0];
    }

    // Update is called once per frame
    void Update()
    {
        GameObject oldObject = closestObject;
        closestObject = allInteractables[0];
        for (int i = 0; i < allInteractables.Length; i++)
        {

            interactableDist = Vector3.Distance(player.transform.position, allInteractables[i].transform.position);

            if (interactableDist < Vector3.Distance(player.transform.position, closestObject.transform.position))
            {
                closestObject = allInteractables[i];
            }
            
        
        }
        oldObject.GetComponent<Renderer>().material = defaultMaterial;

        if (Vector3.Distance(player.transform.position, closestObject.transform.position) < 3)
        {
            closestObject.GetComponent<Renderer>().material = myOutline;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            collision.gameObject.GetComponent<Renderer>().material = defaultMaterial;
            ///closestObject = null;
        }

    }
}
