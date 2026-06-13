using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAdjustColliderGround : MonoBehaviour
{
    public SpriteRenderer spriteRendererTop;
     public SpriteRenderer spriteRendererMid;
       public SpriteRenderer spriteRendererBot;

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

         int idTheme = GameManager.Instance.CurrentMap.theme - 1;
        if (idTheme >= ThemeLevel.i.themeSprites.Length)
        {
            idTheme = ThemeLevel.i.themeSprites.Length - 1;
        }
        if (idTheme < 0)
        {
            idTheme = 0;
        }
        spriteRendererTop.color = ThemeLevel.i.themeSprites[idTheme].top_bot_Color;
        spriteRendererMid.color = ThemeLevel.i.themeSprites[idTheme].mid_Color;
        spriteRendererBot.color = ThemeLevel.i.themeSprites[idTheme].top_bot_Color;

        spriteRendererTop.sortingOrder = -3;
        spriteRendererMid.sortingOrder =    -2;
        spriteRendererBot.sortingOrder = -1;

        // spriteRendererTop.transform.localPosition = new Vector3(0, 0.6f, 0);
        // spriteRendererMid.transform.localPosition = new Vector3(0,  0.4f, 0);
        // spriteRendererBot.transform.localPosition = new Vector3(0,  0.2f, 0);

        spriteRendererTop.transform.position = new Vector3(transform.position.x,transform.position.y + 0.6f , 0);
        spriteRendererMid.transform.position = new Vector3(transform.position.x,transform.position.y + 0.4f, 0);
        spriteRendererBot.transform.position = new Vector3(transform.position.x,transform.position.y + 0.2f  , 0);
        spriteRendererTop.size = this.GetComponent<SpriteRenderer>().size;
        spriteRendererMid.size = this.GetComponent<SpriteRenderer>().size;
        spriteRendererBot.size = this.GetComponent<SpriteRenderer>().size;
        //Debug.LogError("spriteRendererTop.size: " );


    }
}
