using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dioorClose : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Sprite closed;
    void Start()
    {
        StartCoroutine(closeDoor(0.3f));
    }

    private IEnumerator closeDoor(float delay) {
        yield return new WaitForSeconds(delay);
        AudioEventController.DoorClose();
        door.GetComponent<SpriteRenderer>().sprite = closed;
    }
}
