using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBlinkController : MonoBehaviour
{
    [SerializeField] private Material original, hitBlink;
    private GameObject player;
    private GameObject head, body;

    [SerializeField] private float hitBlinkTime = 0.1f;

    private void Start()
    {
        player = GameObject.Find("Player");
        head = GameObject.Find("Head");
        body = GameObject.Find("Body");
    }

    private void OnEnable()
    {
        EventController.setHealthBarPercentEvent += CallDamage;
    }

    private void OnDestroy()
    {
        EventController.setHealthBarPercentEvent -= CallDamage;
    }

    private void CallDamage(float damage, GameObject target)
    {
        StartCoroutine(Damage(target));
    }

    private IEnumerator Damage(GameObject target)
    {
        if (target != player)
        {
            target.GetComponent<SpriteRenderer>().material = hitBlink;
            yield return new WaitForSecondsRealtime(hitBlinkTime);
            if (target != null) target.GetComponent<SpriteRenderer>().material = original;
        } else {
            AudioEventController.Hit();
            head.GetComponent<SpriteRenderer>().material = hitBlink;
            body.GetComponent<SpriteRenderer>().material = hitBlink;

            float alpha = head.GetComponent<SpriteRenderer>().color.a;
            head.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            body.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSecondsRealtime(hitBlinkTime);
            if (target != null)
            {
                head.GetComponent<SpriteRenderer>().material = original;
                body.GetComponent<SpriteRenderer>().material = original;

                head.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
                body.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
            }
        }
    }
}
