using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessController : MonoBehaviour
{
    private GameObject parent;
    private SpriteRenderer parentRenderer;
    [SerializeField] private GameObject innerBar;

    [SerializeField]
    private float max = 0.75f;
    [SerializeField]
    private float min = 0.2f;

    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        parentRenderer = parent.GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        EventController.setHealthBarPercentEvent += SetAwareness;
    }

    private void OnDisable() {
        EventController.setHealthBarPercentEvent -= SetAwareness;
    }

    void SetAwareness(float percent, GameObject targetedGameObject)
    {
        if (parent == targetedGameObject) {
            if (percent <= 0) {
                Destroy(targetedGameObject);
            };
            percent = Mathf.Clamp(percent, 0, 1); 
            innerBar.transform.localScale = new Vector3(percent, 1, 1);

            float alpha = 1.0f;
            if (percent <= max)
            {
                // normalize 0.75 -> 0.0 to 1.0 -> 0.0 then clamp so cant go below min
                alpha = percent / max;
                alpha = Mathf.Clamp(alpha, min, max);
            }

            parentRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }
}
