using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject WASDPanel;
    [SerializeField] GameObject interactPanel;
    [SerializeField] float triggerDistance = 2f;

    private GameObject player;
    private GameObject bed;

    private void Start()
    {
        player = GameObject.Find("Player");
        bed = GameObject.Find("Objects").transform.Find("Bed").gameObject;
    }

    private void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 bedPos = bed.transform.position;

        Vector3 distanceToBed = playerPos - bedPos;
        if (distanceToBed.magnitude < triggerDistance)
        {
            WASDPanel.SetActive(false);
            interactPanel.SetActive(true);
        }
    }
}
