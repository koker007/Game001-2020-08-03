using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerAnimationController : MonoBehaviour
{
    private Animator animator;
    public bool canPlayInAnim = false;
    public bool canPlayOutAnim = false;
    public bool isMarkerPlayingAnimation = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayInAnim();
        PlayOutAnim();
    }

    private void PlayInAnim()
    {
        if (canPlayInAnim)
        {
            animator.Play("MarkerFrameAnimIn");
            canPlayInAnim = false;
        }
    }

    private void PlayOutAnim()
    {
        if (canPlayOutAnim)
        {
            animator.Play("MarkerFrameAnimOut");
            canPlayOutAnim = false;
        }
    }

    private void PlayIdleAnim()
    {
        animator.Play("MarkerFrameAnimIdle");      
    }
}
