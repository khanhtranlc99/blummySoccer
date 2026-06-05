using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeLevel", menuName = "Tiko/ThemeLevel")]
public class ThemeLevel : SingletonScriptableObject<ThemeLevel>
{
    [Serializable]
    public class ThemeSprite
    {
        public Sprite backgroundSprite;
        public Sprite groundsSprite;
        public Sprite luoiSprite;
        public Texture2D waterTexture;
        public Color top_bot_Color;
        public Color mid_Color;
    }
    public ThemeSprite[] themeSprites;

}

