using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed = 500f;
    
    private Vector2 moveDirection = Vector2.zero;

    private InputAction move;
    private InputAction dash;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float elapsedTime;
    private bool isDashing = false;

    [SerializeField]
    private float dashDuration = 0.1f;
    [SerializeField]
    private float dashDistance = 3f;

    [SerializeField]
    private GameObject pointer;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        dash = playerControls.Player.Dash;
        dash.Enable();
        dash.performed += Dash;
    }

    private void OnDisable()
    {
        move.Disable();
        dash.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();

        float percentageComplete = elapsedTime / dashDuration;
        elapsedTime += Time.deltaTime;
        if (percentageComplete >= 1)
        {
            isDashing = false;
        }

        if (isDashing)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);
        }

        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 offsetPos = mouseWorldPos - pointer.transform.position;
        float rotation = Mathf.Atan2(offsetPos.x, offsetPos.y) * (180/Mathf.PI);
        pointer.transform.eulerAngles = new Vector3(pointer.transform.eulerAngles.x, pointer.transform.eulerAngles.y, -rotation);
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
    }

    private void Dash(InputAction.CallbackContext context)
    {
        startPosition = transform.position;
        Vector3 offset = dashDistance * moveDirection;
        endPosition = transform.position + offset;
        elapsedTime = 0;
        isDashing = true;
    }

}
