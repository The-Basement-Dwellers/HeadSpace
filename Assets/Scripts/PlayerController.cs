using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private PlayerInputActions playerControls;
    private InputAction move;

    [HideInInspector]
    public Vector2 moveDirection = Vector2.zero;

    [SerializeField]
    private GameObject pointer;

    [SerializeField]
    private float moveSpeed = 500f;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerInputActions();
            playerControls.Enable();
        }

        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // read move input
        moveDirection = move.ReadValue<Vector2>();

        // Rotates Pointer
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 offsetPos = mouseWorldPos - pointer.transform.position;
        float rotation = Mathf.Atan2(offsetPos.x, offsetPos.y) * (180/Mathf.PI);
        pointer.transform.eulerAngles = new Vector3(pointer.transform.eulerAngles.x, pointer.transform.eulerAngles.y, -rotation);
    }

    private void FixedUpdate()
    {
        // set player velocity
        rb.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
    }
}