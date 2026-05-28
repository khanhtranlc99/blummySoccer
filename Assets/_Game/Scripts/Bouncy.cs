using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncySlime : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float activePushForce = 1.5f;
    // public float minimumForce = 5f;
    // public float maximumForce = 20f;
    private Rigidbody2D rb;

    void OnCollisionEnter2D(Collision2D collision)
    {
        rb = collision.rigidbody;

        if (rb != null)
        {
            Vector2 collisionNormal = collision.contacts[0].normal;

            Vector2 reactionDirection = -collisionNormal;

            Vector2 force = reactionDirection * activePushForce;

            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
