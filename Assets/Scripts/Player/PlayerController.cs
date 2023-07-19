using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rb;

	private Vector2 moveDirection = Vector2.zero;
	private Vector2 lookDirection = Vector2.zero;

	private PlayerInputActions playerControls;

	private InputAction move;
	private InputAction look;
	private InputAction dash;
	private InputAction interact;
	private InputAction restart;

	[SerializeField] private GameObject cameraWeapon;
	[SerializeField] private float moveSpeed = 500f;
	[SerializeField] private float inputBuffer = 0.2f;
	[SerializeField] private bool binaryMove = false;
	[SerializeField] public float playerMaxHealth = 100.0f;
	[SerializeField] public float playerHealth;
	
	private float rotZ;
	private bool isDashing = false;
	private bool canMoveFlash = true;

	private void OnEnable()
	{
		if (playerControls == null)
		{
			playerControls = new PlayerInputActions();
			playerControls.Enable();
		}

		move = playerControls.Player.Move;
		move.Enable();

		look = playerControls.Player.Look;
		look.Enable();

		dash = playerControls.Player.Dash;
		dash.Enable();
		dash.performed += Dash;

		interact = playerControls.Player.Interact;
		interact.Enable();
		interact.performed += InteractAction;
		
		restart = playerControls.Player.Restart;
		restart.Enable();
		restart.performed += Restart;
		
		EventController.setCanMoveFlash += setCanMoveFlash;
		EventController.startIsDashingEvent += SetisDashing;

		AstarPath.active.Scan();
	}

	private void OnDisable()
	{
		move.Disable();
		look.Disable();
		dash.Disable();
		interact.Disable();
		restart.Disable();
		
		EventController.setCanMoveFlash -= setCanMoveFlash;
		EventController.startIsDashingEvent -= SetisDashing;
	}

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		playerHealth = playerMaxHealth;		
	}

	// Update is called once per frame
	void Update()
	{
		
		// read move input
		moveDirection = move.ReadValue<Vector2>();
		lookDirection = look.ReadValue<Vector2>();
		EventController.StartMoveDirectionEvent(moveDirection, gameObject);
		EventController.StartLookDirectionEvent(lookDirection);

		float percent = playerHealth / playerMaxHealth;
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
		} else if (moveDirection.magnitude > 0.05){
			rotZ = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		}
		
		if (canMoveFlash) 
		{
			if (lookDirection.magnitude > 0.05) {
				rotZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
			} else if (moveDirection.magnitude > 0.05){
				rotZ = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			}
			cameraWeapon.transform.eulerAngles = new Vector3(0, 0, rotZ + 90);
		}
	}
	
	// set player velocity
	private void FixedUpdate() {
		if (!isDashing)
		{
			rb.velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
		}
	}
	
	private void setCanMoveFlash(bool canMove) {
		canMoveFlash = canMove;
	}
	
	private void SetisDashing(bool value) {
		isDashing = value;
	}
	
	private void FireRelease(InputAction.CallbackContext context) {
		EventController.FireRelease();
	}

	private void Dash(InputAction.CallbackContext context) {
		EventController.Dash();
	}
	
	private void InteractAction(InputAction.CallbackContext context) {
		EventController.InteractEvent();
	}
	
	private void Restart(InputAction.CallbackContext context) {
		Destroy(gameObject);
	}

	private void OnDestroy() {
		SceneManager.LoadScene("Ward");
	}
}
