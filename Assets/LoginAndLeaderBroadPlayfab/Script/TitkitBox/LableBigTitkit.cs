using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class LableBigTitkit : LableTitkit
{
    public override void Init(ShopTickitBox param)
    {
        shopTickitBox = param;
        Purchaser.Instance.IAP_Manager.OnPurchaseCompleted += HandleBuy;
        btnBuy.onClick.AddListener(HandleOnclick);


    }
    public void HandleOnclick()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
        Purchaser.Instance.IAP_Manager.Purchase(Purchaser.Instance.big_tickit_pack);
    }
    private void HandleBuy(Product product)
    {
        if (product.definition.id == Purchaser.Instance.big_tickit_pack)
        {
            UseProfile.Titkit += 30;
            shopTickitBox.HandleShowReward(30);
            LeaderBoardPvPBox.Instance.InitState();
        }
    }
    private void OnDestroy()
    {
        Purchaser.Instance.IAP_Manager.OnPurchaseCompleted -= HandleBuy;
    }
}
