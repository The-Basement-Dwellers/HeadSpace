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
    [SerializeField] float animationTime = 0.2f;

    IEnumerator streamCoroutine;

    public void Hover() {
        animator.SetBool("IsHover", true);
            
        if (stream != null) stream.Play();
        streamCoroutine = StopParticals(animationTime);
        StartCoroutine(streamCoroutine);
    }
    public void NoHover() {
        animator.SetBool("IsHover", false);
        if (stream != null) stream.Stop();
        StopCoroutine(streamCoroutine);
    }

    private IEnumerator StopParticals(float time)
    {
        yield return new WaitForSeconds(time);
        stream.Stop();
    }
}
