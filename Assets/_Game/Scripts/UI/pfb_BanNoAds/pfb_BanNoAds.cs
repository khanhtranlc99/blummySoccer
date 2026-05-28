using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pfb_BanNoAds : UIBehavior
{
    [SerializeField] protected Button btnOke;

    private void Start()
    {
        this.btnOke.onClick.AddListener(GetWifiSettings);
    }
    public void GetWifiSettings()
    {
        // Nếu có mạng thì chỉ tắt popup
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
       //     ActiveNormalPopup(false);
            Debug.Log("1111");
        }
        else
        {
             ActiveNormalPopup(false);
               Debug.Log("2222");
        }
      
     


    }
}
