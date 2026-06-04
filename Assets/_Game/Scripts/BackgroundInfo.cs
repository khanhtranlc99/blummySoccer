using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundInfo : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundSprite;

    [SerializeField] private SpriteRenderer _groundsSprite;

    [SerializeField] private SpriteRenderer luoiSprite; 
    public void SetBackGround(Sprite backgroundSprite, Sprite groundsSprite, Sprite luoiSpriteParam)
    {
        _backgroundSprite.sprite = backgroundSprite;
        _groundsSprite.sprite = groundsSprite;
        luoiSprite.sprite = luoiSpriteParam;
    }

}
