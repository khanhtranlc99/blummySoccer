using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform topLeft;
    public Transform bottomRight;
    public int theme;

    [SerializeField] private BackgroundInfo _backGround;

    public void SetTheme(int theme)
    {
        this.theme = theme;
        var themeLevel = ThemeLevel.i.themeSprites[theme];
        _backGround.SetBackGround(themeLevel.backgroundSprite, themeLevel.groundsSprite);
    }
}
