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
    public float interactableDist;
    public float savedinteractableDist;
    private GameObject closestObject;


    // Start is called before the first frame update
    void Start()
    {
        allInteractables = GameObject.FindGameObjectsWithTag("Interactable");
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
        savedinteractableDist = interactableDist;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {

            collision.gameObject.GetComponent<Renderer>().material = defaultMaterial;


        }

    }
}
