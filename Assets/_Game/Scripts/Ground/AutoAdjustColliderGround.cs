using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAdjustColliderGround : MonoBehaviour
{
    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 spriteSize = spriteRenderer.size;

        float edgeRadius = 0.25f;
        float maxEdgeRadius = Mathf.Min(spriteSize.x, spriteSize.y) / 2f;
        edgeRadius = Mathf.Clamp(edgeRadius, 0, maxEdgeRadius);

        Vector2 adjustedSize = spriteSize - new Vector2(edgeRadius * 2, edgeRadius * 2);

        collider.size = adjustedSize;
        collider.edgeRadius = edgeRadius;
    }
}
