using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDash : MonoBehaviour
{
	private PlayerInputActions playerControls;
	private InputAction dash;
	private Vector2 moveDirection = Vector2.zero;
	private Vector3 startVelocity;
	private Vector3 endVelocity;
	private float elapsedTime;
	private bool isDashing = false;
	private bool dashOnCooldown = false;
	[SerializeField] private float dashDuration = 0.1f;
	[SerializeField] private float dashSpeed = 3f;
	[SerializeField] private float dashEaseIntensity = 2f;
	[SerializeField] private float dashCooldown = 0.8f;
	private Rigidbody2D rb;

	private void OnEnable()
	{
		if (playerControls == null)
		{
			playerControls = new PlayerInputActions();
			playerControls.Enable();
		}

		dash = playerControls.Player.Dash;
		dash.Enable();
		dash.performed += Dash;

		EventController.setMoveDirectionEvent += setMoveDirection;
		dashOnCooldown = false;
	}

	private void OnDisable()
	{
		dash.Disable();
		EventController.setMoveDirectionEvent -= setMoveDirection;
	}

	private void setMoveDirection(Vector3 eventMoveDirection, GameObject targetedGameObject) {
		if (gameObject == targetedGameObject) moveDirection = eventMoveDirection;
	}

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (isDashing)
		{
			StartCoroutine(DashLerp());
		}
		else
		{
			StopCoroutine(DashLerp());
		}


	}

	// called on dash input
	private void Dash(InputAction.CallbackContext context)
	{
		if (!isDashing && !dashOnCooldown && moveDirection.magnitude > 0.05f)
		{
			StartCoroutine(DashCooldown(dashCooldown));
			startVelocity = Vector3.zero;
			Vector3 offset = dashSpeed * moveDirection;
			endVelocity = dashSpeed * moveDirection;
			elapsedTime = 0;
			isDashing = true;
			EventController.StartIsDashingEvent(true);
		}
	}

	// Makes the dash smooth
	private IEnumerator DashLerp()
	{
		float percentageComplete = elapsedTime / dashDuration;
		elapsedTime += Time.deltaTime;
		if (percentageComplete >= 1)
		{
			isDashing = false;
			EventController.StartIsDashingEvent(false);
		}
		float easedPercentage = IntensifiedEaseInOut(percentageComplete, dashEaseIntensity);

		rb.velocity = Vector3.Lerp(startVelocity, endVelocity, easedPercentage);
		yield return null;
	}

	private IEnumerator DashCooldown(float cooldown)
	{
		dashOnCooldown = true;
		yield return new WaitForSeconds(cooldown);
		dashOnCooldown = false;
	}

	// Ease percentage for slow start/end and fast in middle
	private float IntensifiedEaseInOut(float percent, float intensity)
	{
		percent = Mathf.Clamp01(percent);
		float intensifiedPercent = Mathf.Pow(percent, intensity) / (Mathf.Pow(percent, intensity) + Mathf.Pow(1 - percent, intensity));
		return Mathf.Clamp(intensifiedPercent, 0, 0.8f);
	}
}