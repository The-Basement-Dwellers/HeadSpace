using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private PlayerInputActions playerControls;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;

    [SerializeField]
    private float moveSpeed = 500f;

        [SerializeField]
    private GameObject cameraWeapon;

    private InputAction move;
    private InputAction look;
    private InputAction fire;
    private InputAction dash;
    private InputAction interact;
    private InteractablesManager interManager;
    
    [SerializeField]
    private float inputBuffer = 0.2f;

    [SerializeField]
    private bool binaryMove = false;

    [SerializeField]
    private float maxHealth = 100.0f;
    [SerializeField]
    private float health;

    private float rotZ;

    private bool isDashing = false;

    private void OnEnable()
    {
        EventController.startIsDashingEvent += SetisDashing;
        interManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<InteractablesManager>();

        if (playerControls == null)
        {
            playerControls = new PlayerInputActions();
            playerControls.Enable();
        }

        move = playerControls.Player.Move;
        move.Enable();

        look = playerControls.Player.Look;
        look.Enable();

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
        EventController.startIsDashingEvent -= SetisDashing;

        move.Disable();
        look.Disable();
        fire.Disable();
        dash.Disable();
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
        lookDirection = look.ReadValue<Vector2>();
        EventController.StartMoveDirectionEvent(moveDirection);
        EventController.StartLookDirectionEvent(lookDirection);

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

        if (lookDirection.magnitude > 0.05) {
            rotZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        } else {
            rotZ = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        }

        cameraWeapon.transform.eulerAngles = new Vector3(0, 0, rotZ + 90);
    }

    private void FixedUpdate()
    {
        // set player velocity
        if (!isDashing)
        {
            rb.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
        }
    }

    private void Fire(InputAction.CallbackContext context) {
        EventController.Fire();
    }

    private void Dash(InputAction.CallbackContext context) {
        EventController.Dash();
    }
    
    private void Interact(InputAction.CallbackContext context)
    {
        interManager.Doors();
    }

    private void SetisDashing(bool value) {
        isDashing = value;
    }
}
