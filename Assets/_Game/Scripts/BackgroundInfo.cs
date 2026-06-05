using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BackgroundInfo : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundSprite;

    [SerializeField] private SpriteRenderer _groundsSprite;

    [SerializeField] private SpriteRenderer luoiSprite; 
    [SerializeField] private Canvas canvasBG;
    public Image _backgroundImage;
    public void SetBackGround(Sprite backgroundSprite, Sprite groundsSprite, Sprite luoiSpriteParam)
    {
        _backgroundSprite.sprite = backgroundSprite;
        _groundsSprite.sprite = groundsSprite;
        luoiSprite.sprite = luoiSpriteParam;
        _backgroundImage.sprite = backgroundSprite;
    }
     public void FitSpriteToCamera()
     {
           Camera cam = Camera.main;
              if (cam == null || !cam.orthographic) return;
               RectTransform rect = canvasBG.GetComponent<RectTransform>();
                   float worldHeight = cam.orthographicSize * 2f;
        float worldWidth = worldHeight * cam.aspect;
                Vector3 parentScale = rect.parent != null ? rect.parent.lossyScale : Vector3.one;
        rect.localScale = new Vector3(
            1f / parentScale.x,
            1f / parentScale.y,
            1f / parentScale.z);
        rect.sizeDelta = new Vector2(worldWidth, worldHeight);
              rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        Vector3 pos = cam.transform.position;
        pos.z = rect.position.z;
        rect.position = pos;
        canvasBG.worldCamera = cam;
     }


}
