//using com.adjust.sdk;
// using static MaxSdkBase;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Newtonsoft.Json;
 
//using com.adjust.sdk;
using XGame;
using static Dreamteck.Splines.ParticleController;
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

 
string gameName = "Penalty Master"; // hỏi team analytics tên chính xác
 string GetEventAds (string param )
 {
      var temp =   JsonConvert.SerializeObject(new
    {
          ad_scene = param,
          game_name = gameName,
     });
        return temp;
 } 

 
    public void Init()
    {
        Debug.LogError("AdmobAds");
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

       
            AdsXGame.ShowInterstitial(GetEventAds(actionWatchLog), actionIniterClose);
    
        
      














    }

    private void RequestInterstitial()
    {
        // if (_isLoading) return;

        // MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        // GameController.Instance.AnalyticsController.LogInterLoad();
        // _isLoading = true;
        
        //  XGameSdk.Instance.Track("InterShow" + Facade.Instance.PlayerPrefManager.CurrentLevel, new KVItems()
        //         {
        //            {"InterShow", actionWatchLog},
        //         });
    }

    const string GameName = "blummy_soccer"; // hỏi team analytics tên chính xác
string BuildAdParams(string adScene)
{
    return JsonConvert.SerializeObject(new
    {
        ad_scene = adScene,
        game_name = GameName,
    });
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
     


            AdsXGame.ShowVideoAds(GetEventAds(actionType.ToString()), actionReward, actionNotLoadedVideo);
   
       


          
        // }
        // else
        // {
        //     actionReward?.Invoke();
        // }
         
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
    
    }
   
 

    public void DestroyBanner()
    {

        AdsXGame.HideBanner();
    }

    public void ShowBanner()
    {

            AdsXGame.ShowBanner();
    
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

 

    private void Update()
    {
        countdownAds += Time.unscaledDeltaTime;
       // countdownAdsOpenAppAds += Time.unscaledTime;
    }

 

}
