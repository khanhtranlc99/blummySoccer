using UnityEngine;
using XGame;
using System;
public class AdsXGame : MonoBehaviour
{

    

    public void Init()
    {
        Debug.LogError("Start_Call_Init_XGame");
        XGameSdk.Instance.InitSdk(() => {

            Debug.LogError("InitSuccet");
          
            ShowBanner();

        }, () => {
            Debug.LogError("InitFailed");       
        });
        Debug.LogError("End_Call_Init_XGame");
    }    

    public void Login()
    {
        XGameSdk.Instance.Login(() =>
        {
            Debug.Log("Login successful");
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
        Debug.LogError("Start_Call_ShowBanner");
        XGameSdk.Instance.ShowBanner(BannerType.Bottom);
        Debug.LogError("End_Call_ShowBanner");
    }    
    public void HideBanner()
    {
        XGameSdk.Instance.HideBanner();
    }
    #endregion


    #region Interstitial
    public void ShowInterstitial(string name ,Action CallBack )
    {
        Debug.LogError("Start_Call_ShowInterstitial");
        var flag = XGameSdk.Instance.GetIntersFlag();
        if (flag)
        {
            XGameSdk.Instance.ShowInters(name, CallBack);
        }
        Debug.LogError("End_Call_ShowInterstitial");
    }
    #endregion

    #region Video
    public void ShowVideoAds(string name, Action CallBackComplete, Action CallBackFalse)
    {
        Debug.LogError("Start_Call_ShowVideoAds");
        var flag = XGameSdk.Instance.GetVideoFlag();
        if (flag)
        {
            XGameSdk.Instance.ShowVideo("Ad Scene Name", () => {
                CallBackComplete?.Invoke();
            }, () => {
                CallBackFalse?.Invoke();
            });
        }
        Debug.LogError("End_Call_ShowVideoAds");
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
