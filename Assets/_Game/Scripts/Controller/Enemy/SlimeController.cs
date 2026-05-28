// using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IAnimPlayable
{
    [SerializeField] protected Animator animator;

    // [Button("test")]
    // public void Test()
    // {
    //     PlayAnim();
    // }
    // private void OnEnable()
    // {
    //     this.animator.enabled = false;
    // }
    public void PlayAnim()
    {
        //animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0f);
        animator.SetTrigger("Bounce");
    }

    public float speed = 2f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool movingRight = true;

    void Update()
    {
        if (speed == 0) return;

        transform.Translate(Vector2.right * speed * Time.deltaTime * (movingRight ? 1 : -1));

        if (!IsGroundAhead() || IsTouchingWall())
        {
            Flip();
        }
    }


    bool IsGroundAhead()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer);
    }


    bool IsTouchingWall()
    {
        return Physics2D.Raycast(transform.position, movingRight ? Vector2.right : Vector2.left, 0.75f, groundLayer);
    }


    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

}
