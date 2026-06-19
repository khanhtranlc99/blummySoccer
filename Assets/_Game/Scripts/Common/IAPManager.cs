using System;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Purchasing;
// using UnityEngine.Purchasing.Extension;
// using UnityEngine.Purchasing.Security;
using XGame;

public class IAPManager : MonoSingleton<IAPManager> //IDetailedStoreListener
{

 
    public void HandleBuy(int price, string productId)
    {
       XGameSdk.Instance.Pay(price, productId, productId, "");
    }



 
    




    
}
