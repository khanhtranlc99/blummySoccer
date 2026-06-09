//using com.adjust.sdk;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
// using static MaxSdkBase;
using Newtonsoft.Json.Linq;
//using com.adjust.sdk;

public class AdmobAds : MonoBehaviour
{
    public bool offBanner;

    public float countdownAds;
    public float countdownAdsOpenAppAds;
    public bool IsMRecReady;
    public bool wasShowMer;
    private bool _isInited;
    public bool canShowOpenAppAds;
    public bool wasShowOpenAppAdsInGame;
    public bool lockShowOpenAppAds;
    public int coutOpenAdsLoad;
 
    public bool showingMREC;


    public AdsXGame AdsXGame;
 

 
    public void Init()
    {
        AdsXGame.Init();
    }

    #region Interstitial

    public UnityAction actionInterstitialClose;

    public int amountInterClick
    {
        get
        {
            return PlayerPrefs.GetInt("Amount_Inter_Click", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Amount_Inter_Click", value);
        }
    }

    public int amountLoadFailInter
    {
        get
        {
            return PlayerPrefs.GetInt("Amount_Load_Fail_Inter", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Amount_Load_Fail_Inter", value);
        }
    }

    public DateTime timeLoadFailInter
    {
        get
        {
            var timeLoad = DateTime.Now.AddSeconds(0);
            if (PlayerPrefs.HasKey("Time_Load_Fail_Inter"))
            {
                var binaryDateTime = long.Parse(PlayerPrefs.GetString("Time_Load_Fail_Inter"));
                timeLoad = DateTime.FromBinary(binaryDateTime);
            }

            return timeLoad;
        }
        set
        {
            PlayerPrefs.SetString("Time_Load_Fail_Inter", DateTime.Now.ToBinary().ToString());
        }
    }

    private bool _isLoading;
    private int errorCodeLoadFail_Inter;

    public bool IsLoadedInterstitial()
    {
       // return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
       return true;
    }

    private void InitInterstitial()
    {
        // MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        // MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        // MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
        // MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        // MaxSdkCallbacks.Interstitial.OnAdClickedEvent += MaxSdkCallbacks_OnInterstitialClickedEvent;
        // MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += MaxSdkCallbacks_OnInterstitialDisplayedEvent;

        // RequestInterstitial();

        // MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        // // MaxSdkCallbacks.

    }

    public void ShowInterstitial(bool isShowImmediatly = false, string actionWatchLog = "other", Action actionIniterClose = null, string level = null)
    {

        AdsXGame.ShowInterstitial(actionWatchLog, actionIniterClose); 

    }

    private void RequestInterstitial()
    {
        // if (_isLoading) return;

        // MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        // GameController.Instance.AnalyticsController.LogInterLoad();
        // _isLoading = true;
    }

    #endregion

    #region Video Reward
    private UnityAction _actionClose;
    private UnityAction _actionRewardVideo;
    private UnityAction _actionNotLoadedVideo;
    private ActionWatchVideo actionWatchVideo;

    public int amountVideoRewardClick
    {
        get
        {
            return PlayerPrefs.GetInt("Amount_VideoReward_Click", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Amount_VideoReward_Click", value);
        }
    }
    private int numRequestedInScene_Video;

    private bool isVideoDone;

    private void InitRewardVideo()
    {
        InitializeRewardedAds();
    }

    // public bool IsLoadedVideoReward()
    // {
        // var result = MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
        // if (!result)
        // {
        //     RequestInterstitial();
        // }
        // return result;
  //  }

    // public bool IsLoadedAds()
    // {
    //     var result = IsLoadedVideoReward();
    //     return !result ? IsLoadedInterstitial() : result;
    // }

    public bool ShowVideoReward(Action actionReward, Action actionNotLoadedVideo, Action actionClose, ActionWatchVideo actionType, string level)
    {
        //actionClose?.Invoke();
        //actionReward?.Invoke();
        AdsXGame.ShowVideoAds(actionType.ToString(), actionReward, actionNotLoadedVideo);
        return true;
    }

    #endregion

    #region Applovin Rewards Ads
    private void InitializeRewardedAds()
    {
        // Attach callbacks
   
        // MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        // MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        // MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        // MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        // // Load the first RewardedAd
        // LoadRewardedAd();
    }

//     private void LoadRewardedAd()
//     {
//      //   MaxSdk.LoadRewardedAd(RewardedAdUnitId);
//     }

//     private void OnRewardedAdLoadedEvent(string adUnitId, AdInfo info)
//     {
//     //    GameController.Instance.AnalyticsController.LogVideoRewardReady();
//     }

//     private void OnRewardedAdFailedEvent(string adUnitId, ErrorInfo errorCode)
//     {
//     //     Debug.Log("Rewarded ad failed to load with error code: " + errorCode);
//     //     Invoke("LoadRewardedAd", 10);
//     //     GameController.Instance.AnalyticsController.LogVideoRewardLoadFail(actionWatchVideo.ToString(), errorCode.ToString());
//     // }

//     private void OnRewardedAdFailedToDisplayEvent(string adUnitId, ErrorInfo errorCode, AdInfo adInfo)
//     {
//         Debug.Log("Rewarded ad failed to display with error code: " + errorCode);
//         isVideoDone = false;

//         //if (IsLoadedInterstitial())
//         //{
//         //    ShowInterstitial(isShowImmediatly: true);
//         //}
//         //else
//         //{
//         //    //ConfirmBox.Setup().AddMessageYes(Localization.Get("s_noti"), Localization.Get("s_TryAgain"), () => { });
//         //}
//   //      LoadRewardedAd();
//     }

    // private void OnRewardedAdDisplayedEvent(string adUnitId, AdInfo info)
    // {
    //     Debug.Log("Rewarded ad displayed " + isVideoDone);
    //     GameController.Instance.AnalyticsController.HandleFireEvent_Total_Reward_Count();
    //     isVideoDone = false;
    // }

    // private void OnRewardedAdClickedEvent(string adUnitId, AdInfo info)
    // {
    //     amountVideoRewardClick++;
    //     Debug.Log("Rewarded ad clicked");
    //     isVideoDone = true;
    //     GameController.Instance.AnalyticsController.LogClickToVideoReward(actionWatchVideo.ToString());
    // }

    // private void OnRewardedAdDismissedEvent(string adUnitId, AdInfo info)
    // {
    //     // Rewarded ad is hidden. Pre-load the next ad
    //     lockShowOpenAppAds = false;
    //     Debug.Log("Rewarded ad dismissed");
    //     _actionClose?.Invoke();
    //     _actionClose = null;
    //     LoadRewardedAd();
    // }

    // private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, AdInfo  info)
    // {
    //     // Rewarded ad was displayed and user should receive the reward
    //     Debug.Log("Rewarded ad received reward");
    //     isVideoDone = true;
    //     _actionRewardVideo?.Invoke();
    //     _actionRewardVideo = null;
    //     countdownAds = 0;
    //     GameController.Instance.AnalyticsController.LogVideoRewardShowDone(actionWatchVideo.ToString());
    // }
    #endregion

    #region Applovin Interstitial
    // private void OnInterstitialLoadedEvent(string adUnitId, AdInfo info)
    // {
    //     _isLoading = true;
    //     GameController.Instance.AnalyticsController.LogInterReady();
    // }

    // private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorCode)
    // {
    //     _isLoading = false;
    //     actionInterstitialClose?.Invoke();
    //     actionInterstitialClose = null;
    //     Invoke("RequestInterstitial", 3);

       
    //     GameController.Instance.AnalyticsController.LogInterLoadFail(errorCode.AdLoadFailureInfo);
    // }

    // private void InterstitialFailedToDisplayEvent(string adUnitId, ErrorInfo errorCode, AdInfo info)
    // {
    //     _isLoading = false;
    //     actionInterstitialClose?.Invoke();
    //     actionInterstitialClose = null;
    //     RequestInterstitial();
    // }

    // private void OnInterstitialHiddenEvent(string adUnitId, AdInfo info)
    // {
    //     _isLoading = false;
    //     Debug.Log("InterstitialAdClosedEvent");
    //     Time.timeScale = 1;

    //     _actionRewardVideo?.Invoke();
    //     _actionRewardVideo = null;

    //     _actionClose?.Invoke();
    //     _actionClose = null;

    //     actionInterstitialClose?.Invoke();
    //     actionInterstitialClose = null;
    //     lockShowOpenAppAds = false;
    //     RequestInterstitial();
    // }
    // private void MaxSdkCallbacks_OnInterstitialDisplayedEvent(string adUnitId, AdInfo info)
    // {
    //     //if (UseProfile.RetentionD <= 1)
    //     //{
    //     //    UseProfile.NumberOfDisplayedInterstitialD0_D1++;
    //     //}
    //     //GameController.Instance.AnalyticsController.LogDisplayedInterstitialDay01();
    //     Debug.Log("InterstitialAdOpenedEvent");
    //     GameController.Instance.AnalyticsController.HandleFireEvent_Total_Inter_Count();
    //     _isLoading = false;
    //     Time.timeScale = 0;
    // }

    // private void MaxSdkCallbacks_OnInterstitialClickedEvent(string adUnitId, AdInfo info)
    // {
    //     amountInterClick++;
    //     GameController.Instance.AnalyticsController.LogInterClick();
    //     _isLoading = false;
    // }
    #endregion

    #region Applovin Baner


    public int amountBanerClick
    {
        get
        {
            return PlayerPrefs.GetInt("Amount_Baner_Click", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Amount_Baner_Click", value);
        }
    }

    public int amountLoadFailBaner
    {
        get
        {
            return PlayerPrefs.GetInt("Amount_Load_Fail_Baner", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Amount_Load_Fail_Baner", value);
        }
    }

    public DateTime timeLoadFailBaner
    {
        get
        {
            var timeLoad = DateTime.Now.AddSeconds(0);
            if (PlayerPrefs.HasKey("Time_Load_Fail_Baner"))
            {
                var binaryDateTime = long.Parse(PlayerPrefs.GetString("Time_Load_Fail_Baner"));
                timeLoad = DateTime.FromBinary(binaryDateTime);
            }

            return timeLoad;
        }
        set
        {
            PlayerPrefs.SetString("Time_Load_Fail_Baner", DateTime.Now.ToBinary().ToString());
        }
    }

    private IEnumerator reloadBannerCoru;

    public void InitializeBannerAds()
    {
        // MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        // MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        // MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        // MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;

        // MaxSdk.CreateBanner(BanerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        // MaxSdk.SetBannerExtraParameter(BanerAdUnitId, "adaptive_banner", "true");
        // MaxSdk.SetBannerBackgroundColor(BanerAdUnitId, Color.black);
        // MaxSdk.SetBannerWidth(BanerAdUnitId, 520);

        
        // ShowBanner();
    }
   
    // private void OnBannerAdClickedEvent(string obj, AdInfo info)
    // {
    //     //inter click
    //     Debug.Log("Click Baner !!!");
    //     amountBanerClick++;

    // }

    // private void OnBannerAdLoadFailedEvent(string arg1, ErrorInfo arg2)
    // {
    //     // if (reloadBannerCoru != null)
    //     // {
    //     //     StopCoroutine(reloadBannerCoru);
    //     //     reloadBannerCoru = null;
    //     // }
    //     // reloadBannerCoru = Helper.StartAction(() => { ShowBanner(); }, 0.3f);
    //     // StartCoroutine(reloadBannerCoru);
    // }

    // private void OnBannerAdLoadedEvent(string obj, AdInfo info)
    // {
    //     // Debug.Log("Request success");
    //     // if (reloadBannerCoru != null)
    //     // {
    //     //     StopCoroutine(reloadBannerCoru);
    //     //     reloadBannerCoru = null;
    //     // }
       
    // }

    public void DestroyBanner()
    {
        // if (reloadBannerCoru != null)
        // {
        //     StopCoroutine(reloadBannerCoru);
        //     reloadBannerCoru = null;
        // }
        // MaxSdk.HideBanner(BanerAdUnitId);
        AdsXGame.HideBanner();
    }

    public void ShowBanner()
    { 

        AdsXGame.ShowBanner();



        // MaxSdk.ShowBanner(BanerAdUnitId);
    }


    #endregion

    #region Limit Click
    public DateTime ToDayAds
    {
        get
        {
            if (!PlayerPrefs.HasKey("TODAY_ADS"))
                PlayerPrefs.SetString("TODAY_ADS", DateTime.Now.AddDays(-1).ToString());
            return DateTime.Parse(PlayerPrefs.GetString("TODAY_ADS"));
        }
        set
        {
            PlayerPrefs.SetString("TODAY_ADS", value.ToString());
        }
    }

    public void CheckResetCaping()
    {
        // bool isPassday = TimeManager.IsPassTheDay(ToDayAds, DateTime.Now);
        // if (isPassday)
        {
            amountLoadFailInter = 0;
            amountLoadFailBaner = 0;
            amountInterClick = 0;
            amountBanerClick = 0;
            amountVideoRewardClick = 0;
            ToDayAds = DateTime.Now;
        }
    }
    #endregion

//     private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
//     {
    

//         double revenue = impressionData.Revenue;
//         var impressionParameters = new[] {
//     new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
//     new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
//     new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
//     new Firebase.Analytics.Parameter("value", revenue),
//     new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
// };
//         Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_max", impressionParameters);
//         Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    

 
//     }

//     private void OnLevelWasLoaded(int level)
//     {
//         _actionRewardVideo = null;
//         _actionClose = null;
//         actionInterstitialClose = null;
//     }

//     private void Update()
//     {
//         countdownAds += Time.unscaledDeltaTime;
//         countdownAdsOpenAppAds += Time.unscaledTime;
//     }

    //public bool IsOpenAdsReady
    //{
    //    get
    //    {
    //        return MaxSdk.IsAppOpenAdReady(AppOpenId);
    //    }

    //}
    //public void InitializeOpenAppAds()
    //{
    //    MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += delegate { };
    //    MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += delegate { };
    //    MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += delegate { MaxSdk.LoadAppOpenAd(AppOpenId); };
    //    MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    //    MaxSdk.LoadAppOpenAd(AppOpenId);

    //}

    //public void LoadOpenAdsIfFalse()
    //{
    //    if (!IsOpenAdsReady)
    //    {
    //        MaxSdk.LoadAppOpenAd(AppOpenId);

    //    }

    //}
    //public void ShowOpenAppAdsReady()
    //{
    //    if (UseProfile.IsRemoveAds)
    //    {
    //        return;
    //    }

    //    //if (!UseProfile.FirstShowOpenAds)
    //    //{

    //    //    UseProfile.FirstShowOpenAds = true;
    //    //}
    //    //else
    //    //{
    //        //if (RemoteConfigController.GetBoolConfig(FirebaseConfig.SHOW_OPEN_ADS, true))
    //        //{
    //            if (MaxSdk.IsAppOpenAdReady(AppOpenId))
    //            {
    //                MaxSdk.ShowAppOpenAd(AppOpenId);
    //                countdownAdsOpenAppAds = 0;
    //                Debug.LogError("SHOW_OPEN_ADS");
    //            }
    //            else
    //            {
    //                MaxSdk.LoadAppOpenAd(AppOpenId);
    //            }
    //     //   }
    //    //    Debug.LogError("FirstShowOpenAds_2");
    //    //}



    //}

    //public void ShowOpenAppAdsInGame()
    //{
    //    if (wasShowOpenAppAdsInGame == false)
    //    {
    //        ShowOpenAppAdsReady();
    //        wasShowOpenAppAdsInGame = true;
    //    }

    //}
    //public void OnApplicationPause(bool pause)
    //{

    //    if (!pause)
    //    {

    //        if (canShowOpenAppAds)
    //        {

    //            if (lockShowOpenAppAds == false)
    //            {
    //                ShowOpenAppAdsReady();

    //            }

    //        }
    //    }

    //}
    // public void InitializeMRecAds()
    // {
    // //    // MRECs are sized to 300x250 on phones and tablets
    //     MaxSdk.CreateMRec(MREC_Id, MaxSdkBase.AdViewPosition.Centered);

    //     MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
    //     MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdLoadFailedEvent;
    //     MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
    //     MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    //     MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnMRecAdExpandedEvent;
    //    MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnMRecAdCollapsedEvent;


    // }

    // public void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    // {
    //     IsMRecReady = true;

    // }

    // public void OnMRecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error)
    // {

    //     IsMRecReady = false;


    // }

    // public void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    // public void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    // public void OnMRecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    // public void OnMRecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }


    // public void HandleShowMerec()
    // {
    //     //if (UseProfile.IsRemoveAds)
    //     //{
    //     //    return;
    //     //}

    //     DestroyBanner();
    //     MaxSdk.ShowMRec(MREC_Id);
    //     showingMREC = true;
    // }
    // public void HandleHideMerec()
    // {
    //     if (showingMREC)
    //     {
    //         MaxSdk.HideMRec(MREC_Id);
    //         ShowBanner();
    //         showingMREC = false;
    //    }

    // }

}
