using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public sealed class AtlasManager : MonoBehaviour
{
    [SerializeField] private SpriteAtlas AtlasUICommon;
    [SerializeField] private SpriteAtlas AtlasUISettings;
    

    public Sprite GetSpriteUICommon(string key){
        return this.AtlasUICommon.GetSprite(key);
    }

    public Sprite GetSpriteUISettings(string key)
    {
        return this.AtlasUISettings.GetSprite(key);
    }
}
