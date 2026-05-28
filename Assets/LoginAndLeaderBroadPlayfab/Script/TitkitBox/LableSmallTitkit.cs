using UnityEngine;
using UnityEngine.Purchasing;

public class LableSmallTitkit : LableTitkit
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
        Purchaser.Instance.IAP_Manager.Purchase(Purchaser.Instance.normal_tickit_pack);
    }    
    private void HandleBuy(Product product)
    {
        if (product.definition.id == Purchaser.Instance.normal_tickit_pack)
        {
            UseProfile.Titkit += 10;
            shopTickitBox.HandleShowReward(10);
            LeaderBoardPvPBox.Instance.InitState();
        }
    }
    private void OnDestroy()
    {
        Purchaser.Instance.IAP_Manager.OnPurchaseCompleted -= HandleBuy;
    }

}