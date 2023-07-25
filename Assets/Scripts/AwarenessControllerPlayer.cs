using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwarenessControllerPlayer : MonoBehaviour
{
	private SpriteRenderer playerRenderer;
	[SerializeField] private GameObject innerBar;
	[SerializeField] private GameObject player;

	[SerializeField]
	private float max = 0.75f;
	[SerializeField]
	private float min = 0.2f;

	[SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    private Color initalColor;

	void Start()
	{
		initalColor = gameObject.GetComponent<Image>().color;
		playerRenderer = player.GetComponent<SpriteRenderer>();

		innerBar.GetComponent<Image>().color = initalColor;
	}

	private void OnEnable() {
		EventController.setHealthBarPercentEvent += SetAwareness;
	}

	private void OnDisable() {
		EventController.setHealthBarPercentEvent -= SetAwareness;
	}

	void SetAwareness(float percent, GameObject targetedGameObject)
	{
		if (player == targetedGameObject) {
			if (percent <= 0) {
				Destroy(targetedGameObject);
			};
			percent = Mathf.Clamp(percent, 0.075f, 1); 
			innerBar.transform.localScale = new Vector3((percent - 1) * -1, 1, 1);
			float x = -innerBar.transform.localScale.x * 100 / 2 + 50;


			innerBar.transform.localPosition = new Vector3(x, 0, 0);

            float alpha = 1.0f;
			if (percent <= max)
			{
				// normalize 0.75 -> 0.0 to 1.0 -> 0.0 then clamp so cant go below min
				alpha = percent / max;
				alpha = Mathf.Clamp(alpha, min, max);
			}

			playerRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
		}
	}
}