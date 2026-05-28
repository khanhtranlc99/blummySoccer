using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GioController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] BoxCollider2D boxCollider2D;
    Material material;
    private void Start()
    {
        material = spriteRenderer.material;
        boxCollider2D.size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y);
    }
    private void Update()
    {
        float valueOffsetY = material.GetTextureOffset("_MainTex").y;
        valueOffsetY -= Time.deltaTime * 2f;
        if (valueOffsetY < -10) valueOffsetY = 10;
        material.SetTextureOffset("_MainTex", new Vector2(0, valueOffsetY));
    }
}
