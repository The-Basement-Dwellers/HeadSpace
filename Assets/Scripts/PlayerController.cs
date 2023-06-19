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

    private InputAction move;
    private InputAction fire;
    private InputAction dash;
    private InputAction interact;
    private InteractablesManager interManager;
    


    private void Awake()
    {
        interManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<InteractablesManager>();

    }

    [SerializeField]
    private float inputBuffer = 0.2f;

    [SerializeField]
    private bool binaryMove = false;

    [SerializeField]
    private float maxHealth = 100.0f;
    [SerializeField]
    private float health;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerInputActions();
            playerControls.Enable();
        }

        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        dash = playerControls.Player.Dash;
        dash.Enable();
        dash.performed += Dash;

        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        interact.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // read move input
        moveDirection = move.ReadValue<Vector2>();
        EventController.StartMoveDirectionEvent(moveDirection);

        float percent = health / maxHealth;
        EventController.StartHealthBarEvent(percent, gameObject);

        if (binaryMove)
        {
            float binaryMoveDirectionX = 0;
            float binaryMoveDirectionY = 0;
            if (moveDirection.x > inputBuffer) binaryMoveDirectionX = 1;
            if (moveDirection.x < -inputBuffer) binaryMoveDirectionX = -1;

            if (moveDirection.y > inputBuffer) binaryMoveDirectionY = 1;
            if (moveDirection.y < -inputBuffer) binaryMoveDirectionY = -1;

            moveDirection = new Vector2(binaryMoveDirectionX, binaryMoveDirectionY);
        }

        // Rotates Pointer
        // Vector3 mousePos = Input.mousePosition;
        // Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        // Vector3 offsetPos = mouseWorldPos - pointer.transform.position;
        // float rotation = Mathf.Atan2(offsetPos.x, offsetPos.y) * (180/Mathf.PI);
        // pointer.transform.eulerAngles = new Vector3(pointer.transform.eulerAngles.x, pointer.transform.eulerAngles.y, -rotation);

        // Perspective mouse follow
        Plane spritePlane = new Plane(Vector3.forward, transform.position);
        Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDist;
        spritePlane.Raycast(cursorRay, out rayDist);
        Vector3 mouseRayPos = cursorRay.GetPoint(rayDist);
        Vector3 offsetPos = mouseRayPos - pointer.transform.position;
        float rotation = Mathf.Atan2(offsetPos.x, offsetPos.y) * (180/Mathf.PI);
        pointer.transform.eulerAngles = new Vector3(pointer.transform.eulerAngles.x, pointer.transform.eulerAngles.y, -rotation);
    }

    private void FixedUpdate()
    {
        // set player velocity
        rb.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
    }
    
    private void Interact(InputAction.CallbackContext context)
    {
        interManager.Doors();
    }
}
