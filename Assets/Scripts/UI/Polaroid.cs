using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Polaroid : MonoBehaviour
{
    [SerializeField] GameObject polaroidCamera;
    [SerializeField] GameObject polaroidFrame;
    RectTransform polaroidFrameRect;

    private float elapsedTime = 0;
    private bool isLerping = false;

    [SerializeField] Vector3 topPos;
    [SerializeField] Vector3 bottomPos;

    private IEnumerator turnOffCamera;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 initalPos;

    private Vector3 lookDirection;

    private void OnEnable()
    {
        CameraEventController.fired += Fired;
        EventController.setLookDirectionEvent += SetLookDirection;
    }

    private void OnDisable()
    {
        CameraEventController.fired -= Fired;
        EventController.setLookDirectionEvent -= SetLookDirection;
    }

    private void Start()
    {
        polaroidFrameRect = polaroidFrame.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isLerping) { StartCoroutine(PoloroidLerp()); }

        polaroidCamera.transform.localPosition = lookDirection * 3.5f + new Vector3(0, 0, -20);
    }

    void Fired()
    {
        startPos = bottomPos;
        endPos = topPos;
        initalPos = transform.localPosition;
        isLerping = true;
        elapsedTime = 0;
        if (turnOffCamera != null) StopCoroutine(turnOffCamera); 
        polaroidCamera.SetActive(true);
        turnOffCamera = TurnOffCamera(3);
        StartCoroutine(turnOffCamera);
    }

    void SetLookDirection(Vector3 direction)
    {
        lookDirection = direction;
    }

    IEnumerator TurnOffCamera(float seconds)
    {
        yield return new WaitForSeconds(0.01f);
        polaroidCamera.SetActive(false);
        yield return new WaitForSeconds(seconds);
        polaroidCamera.SetActive(true);
    }

    private IEnumerator PoloroidLerp()
    {
        float percentageComplete = elapsedTime;

        elapsedTime += Time.deltaTime;

        float lerpPosition = Mathf.Lerp(startPos.y, endPos.y, percentageComplete);
        polaroidFrameRect.localPosition = new Vector3(startPos.x, lerpPosition, 0f);

        if (percentageComplete >= 1.0f && isLerping && startPos == bottomPos)
        {
            StartCoroutine(Reverse(1));
        }

        yield return null;
    }

    IEnumerator Reverse(float seconds)
    {
        isLerping = false;
        yield return new WaitForSeconds(seconds);
        startPos = topPos;
        endPos = bottomPos;
        elapsedTime = 0;
        isLerping = true;
    }
}
