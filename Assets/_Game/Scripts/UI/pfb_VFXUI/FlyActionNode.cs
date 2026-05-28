using UnityEngine;
using UnityEngine.UI;
public class FlyActionNode : MonoBehaviour
{
    [SerializeField] protected Image imgNode;
    public void Init(Sprite spr)
    {
        this.imgNode.sprite = spr;
    }
    //public void Init(RewardData RewardData)
    //{
    //    this.imgNode.sprite = Facade.Instance.ConfigController.RewardConfig.GetRewardSpriteByType(RewardData.REWARD_TYPE);
    //}
}
