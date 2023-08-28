using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonAnimator : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] Animator animator;
    [SerializeField] Image image;
    [SerializeField] ParticleSystem stream;

    IEnumerator streamCoroutine;

    public void Hover() {
        animator.SetBool("IsHover", true);
        stream.Play();
        streamCoroutine = StopParticals(1);
        StartCoroutine(streamCoroutine);
    }
    public void NoHover() {
        animator.SetBool("IsHover", false);
        stream.Stop();
        StopCoroutine(streamCoroutine);
    }

    private IEnumerator StopParticals(float time)
    {
        yield return new WaitForSeconds(time);
        stream.Stop();
    }
}
