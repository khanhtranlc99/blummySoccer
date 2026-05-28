using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxClouds : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 limitX;
    void Update()
    {
        float delta = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta, 0, 0);
        CheckReset();
    }
    void CheckReset()
    {
        if (transform.localPosition.x < limitX.x) transform.localPosition = new Vector3(limitX.y, 0, 0);
        if (transform.localPosition.x > limitX.y) transform.localPosition = new Vector3(limitX.x, 0, 0);
    }
}
