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
    [SerializeField] Sprite hover;
    [SerializeField] Sprite noHover;



    public void Hover() { animator.SetBool("IsHover", true);}
    public void NoHover() { animator.SetBool("IsHover", false);}
}
