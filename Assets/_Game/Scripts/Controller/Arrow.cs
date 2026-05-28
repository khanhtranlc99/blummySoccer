using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{

    Vector3 post;
    void Start()
    {
        post = this.transform.position;
        Move();
    }
    private void Move()
    {
        this.transform.DOMoveY(this.transform.position.y + 0.3f, 0.5f).OnComplete(delegate
        {
            this.transform.DOMoveY(post.y, 0.5f).OnComplete(delegate
            {
                Move();
            });
        });

    }

    private void OnDisable()
    {
        this.transform.DOKill();
    }
    private void OnDestroy()
    {
        this.transform.DOKill();
    }

}
