using UnityEngine;

public class AutoCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
