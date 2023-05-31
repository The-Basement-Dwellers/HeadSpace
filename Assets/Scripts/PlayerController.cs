using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputAction playerControls;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed = 500f;
    private Vector2 moveDirection = Vector2.zero;

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
    }
}
