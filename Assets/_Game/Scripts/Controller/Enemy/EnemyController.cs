using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IAnimPlayable
{
    [SerializeField] protected Animator animator;
    public Rigidbody2D rb;

    public void PlayAnim()
    {
        animator.Play("GetHit", 0, 0f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.relativeVelocity.magnitude > 5)
        {
            PlayAnim();
        }

    }
}

