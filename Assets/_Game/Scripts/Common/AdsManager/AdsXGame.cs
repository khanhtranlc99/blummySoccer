using UnityEngine;
using XGame;
using System;
public class AdsXGame : MonoBehaviour
{

    

    public void Init()
    {
    
        XGameSdk.Instance.InitSdk(() => {

            Debug.LogError("InitSuccet");
            Login();
          

        }, () => {
            Debug.LogError("InitFailed");       
        });
      
    }    

    public void Login()
    {
        XGameSdk.Instance.Login(() =>
        {
            Debug.Log("Login successful");
            ShowBanner();
        }, () =>
        {
            Debug.Log("Login failed");
        });
    }    

    public void ShowPolicy()
    {
        var flag = XGameSdk.Instance.IsSupportPrivacyBtn();
        XGameSdk.Instance.ShowPrivacy();
    }


    #region Banner
    public void ShowBanner()
    {
        if (RemoteConfigController.GetBoolConfig("Show_Ads_XGame", false) == true)
        {
            XGameSdk.Instance.ShowBanner(BannerType.Bottom);
        }
      
     
    }    
    public void HideBanner()
    {
        XGameSdk.Instance.HideBanner();
    }
    #endregion


    #region Interstitial
    public void ShowInterstitial(string name ,Action CallBack )
    {
        
        var flag = XGameSdk.Instance.GetIntersFlag();
        if (flag)
        {
            XGameSdk.Instance.ShowInters(name, CallBack);
        }
     
    }
    #endregion

    #region Video
    public void ShowVideoAds(string name, Action CallBackComplete, Action CallBackFalse)
    {
       
        var flag = XGameSdk.Instance.GetVideoFlag();
        if (flag)
        {
            XGameSdk.Instance.ShowVideo("Ad Scene Name", () => {
                CallBackComplete?.Invoke();
            }, () => {
                CallBackFalse?.Invoke();
            });
        }
     
    }
    #endregion

    #region Native advertising
    public void ShowNativeAdvertising(string name)
    {
        var flag = XGameSdk.Instance.GetNativeFlag();
        if (flag)
        {
            XGameSdk.Instance.ShowNativeAd("name");
        }
    }    


    #endregion



}
