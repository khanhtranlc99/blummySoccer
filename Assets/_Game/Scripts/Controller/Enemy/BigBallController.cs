using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBallController : MonoBehaviour, IAnimPlayable
{
    [SerializeField] protected Animator animator;

    public void PlayAnim()
    {
        animator.SetTrigger("Bounce");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayAnim();
    }

}

