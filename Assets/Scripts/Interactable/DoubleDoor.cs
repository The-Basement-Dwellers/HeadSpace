using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    [SerializeField] bool openByDefault = false;
    [SerializeField] bool lockUntillEnemiesDead = false;
    [SerializeField] bool endingDoor = false;
    private Door left, right;

    // Start is called before the first frame update
    void Start()
    {
        left = transform.GetChild(0).GetComponent<Door>();
        right = transform.GetChild(1).GetComponent<Door>();

        setVars(left, true);
        setVars(right, false);
    }

    private void setVars(Door door, bool setEnd)
    {
        door.openByDefault = openByDefault;
        door.lockUntillEnemiesDead = lockUntillEnemiesDead;
        if (setEnd) door.endingDoor = endingDoor;
    }
}
