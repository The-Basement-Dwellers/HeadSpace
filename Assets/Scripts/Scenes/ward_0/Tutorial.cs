using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject WASDPanel;
    [SerializeField] GameObject interactPanel;
    [SerializeField] float hideDelay = 1f;
    [SerializeField] float triggerDistance = 2f;

    private GameObject player;
    private GameObject bed;

    private void OnEnable()
    {
        EventController.isMoving += IsMoving;
    }

    private void OnDisable()
    {
        EventController.isMoving -= IsMoving;
    }

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
        if (distanceToBed.magnitude < triggerDistance && !WASDPanel.activeSelf)
        {
            interactPanel.SetActive(true);
        }
    }

    private void IsMoving(bool isMoving)
    {
        if (isMoving) StartCoroutine(HideText(WASDPanel, hideDelay));
    }

    private IEnumerator HideText(GameObject text, float time)
    {
        yield return new WaitForSeconds(time);
        text.SetActive(false);
    }
}
