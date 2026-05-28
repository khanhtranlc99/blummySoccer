// using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardConfig", menuName = "Configuration/RewardConfig", order = 9)]
public class RewardConfig : ScriptableObject
{
    public List<RewardSprite> ListRewardSprite = new List<RewardSprite>();

    public RewardSprite GetRewardSpriteData(REWARD_TYPE _type) => ListRewardSprite.Find(x => x.REWARD_TYPE == _type);
    public REWARD_RARITY GetRarityByType(REWARD_TYPE _type) => ListRewardSprite.Find(x => x.REWARD_TYPE == _type).REWARD_RARITY;
    public Sprite GetRewardSpriteByType(REWARD_TYPE _type) => ListRewardSprite.Find(x => x.REWARD_TYPE == _type).Sprite;

#if UNITY_EDITOR
    public REWARD_TYPE _type;
    // [Button("ShowIndex")]
    public void ShowIndex()
    {
        int count = 0;
        foreach (var item in ListRewardSprite)
        {
            if (item.REWARD_TYPE == _type)
                Debug.LogError("Index: " + count);
            count++;
        }
    }
#endif
}

[Serializable]
public class RewardSprite
{
    public REWARD_TYPE REWARD_TYPE = REWARD_TYPE.GOLD;
    public Sprite Sprite;
    public REWARD_RARITY REWARD_RARITY = REWARD_RARITY.NORMAL;
}
[Serializable]
public enum REWARD_RARITY
{
    NORMAL = 1,
    UNCOMMON = 2,
    RARE = 3,
    UNIQUE = 4,
    MYTHICAL = 5,
    LEGEND = 6,
    DIVINE = 7
}

[Serializable]
public struct RewardStruct
{
    public REWARD_TYPE REWARD_TYPE;
    public int numberReward;
    public int UIDSpecial;
    public RewardStruct(REWARD_TYPE REWARD_TYPE, int numberReward, int UID = 0)
    {
        this.REWARD_TYPE = REWARD_TYPE;
        this.numberReward = numberReward;
        this.UIDSpecial = UID;
    }
    public RewardStruct(RewardStruct rewardStruct)
    {
        this.REWARD_TYPE = rewardStruct.REWARD_TYPE;
        this.numberReward = rewardStruct.numberReward;
        this.UIDSpecial = rewardStruct.UIDSpecial;
    }
}
[Serializable]
public enum REWARD_TYPE
{
    HAMMER = 1,
    RUNE_DEFAULT = 2,
    DIAMOND = 3,
    GOLD = 4,
    MONSTER = 5,
    EXP = 6,
}
