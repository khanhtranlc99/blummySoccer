using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundInfo : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundSprite;

    [SerializeField] private SpriteRenderer _groundsSprite;
    public void SetBackGround(Sprite backgroundSprite, Sprite groundsSprite)
    {
        _backgroundSprite.sprite = backgroundSprite;
        _groundsSprite.sprite = groundsSprite;
    }

}
