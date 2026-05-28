using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BirdRiderController : MonoBehaviour, IAnimPlayable
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected EnemyController enemy;
    [SerializeField] DOTweenAnimation tween;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D birdCollider;
    [SerializeField] BoxCollider2D riderCollider;
    bool isDead = false;
    public void PlayAnim()
    {
        if (isDead) return;
        isDead = true;

        rb.bodyType = RigidbodyType2D.Dynamic;
        tween.DOKill();

        animator.Play("GetHit", 0, 0f);

        birdCollider.enabled = true;
        riderCollider.enabled = false;
        var e = Instantiate(enemy, transform.position, Quaternion.identity, transform.parent);
        e.rb.AddForce(new Vector2(1, 1) * 5, ForceMode2D.Impulse);
        rb.AddForce(new Vector2(-1, 1) * 5, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.relativeVelocity.magnitude > 5)
        {
            PlayAnim();
        }
    }
}

