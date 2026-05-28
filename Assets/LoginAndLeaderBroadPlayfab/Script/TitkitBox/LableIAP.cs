using UnityEngine;
using UnityEngine.Purchasing;

public class LableIAP : LableTitkit
{

    public override void Init(ShopTickitBox param)
    {
        Purchaser.Instance.IAP_Manager.OnPurchaseCompleted += HandleBuy;



    }
    private void HandleBuy(Product product)
    {
        if(product.definition.id == Purchaser.Instance.normal_tickit_pack)
        {

        }
    }
    private void OnDestroy()
    {
        Purchaser.Instance.IAP_Manager.OnPurchaseCompleted -= HandleBuy;
    }
}