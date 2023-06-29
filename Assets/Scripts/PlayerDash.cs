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

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float elapsedTime;
    private bool isDashing = false;
    private bool dashOnCooldown = false;

    [SerializeField]
    private float dashDuration = 0.1f;
    [SerializeField]
    private float dashDistance = 3f;
    [SerializeField]
    private float dashEaseIntensity = 2f;
    [SerializeField]
    private float dashCooldown = 0.8f;

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
    }

    private void OnDisable()
    {
        dash.Disable();
        EventController.setMoveDirectionEvent -= setMoveDirection;
    }

    private void setMoveDirection(GameObject targetedGameObject, Vector3 eventMoveDirection) {
        moveDirection = eventMoveDirection;

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
        if (!isDashing && !dashOnCooldown)
        {
            StartCoroutine(DashCooldown(dashCooldown));
            startPosition = transform.position;
            Vector3 offset = dashDistance * moveDirection;
            endPosition = transform.position + offset;
            elapsedTime = 0;
            isDashing = true;

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
        }
        float easedPercentage = IntensifiedEaseInOut(percentageComplete, dashEaseIntensity);

        transform.position = Vector3.Lerp(startPosition, endPosition, easedPercentage);
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
        return Mathf.Pow(percent, intensity) / (Mathf.Pow(percent, intensity) + Mathf.Pow(1 - percent, intensity));
    }
}
