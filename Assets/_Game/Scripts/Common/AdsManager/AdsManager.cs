using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonlightFramework
{
    public enum AdsNetwork
    {
        Admob,
        Max
    }
    public enum AdsType
    {
        AdaptiveBanner,
        Interstitial,
        Rewarded,
        RewardedInterstitial,
        Native,
        AppOpen,
        CollapsibleBanner,
        MREC
    }
    public enum AdditionAdsType
    {
        General,
    }
    public class AdsManager : MonoBehaviour
    {
        //public static AdsManager Instance;
        protected internal float lastTimeShowInter = int.MinValue;
        protected internal float lastTimeShowCollapseBanner;
        protected internal float lastTimeRequestCollapseBanner;

        public Dictionary<AdsNetwork, BaseAds> dictAdsNetwork;
        public bool isRemoveAds = false;
        public float countdownAds;
        protected void Awake()
        {
            //if (Instance != null && Instance != this)
            //{
            //    Destroy(gameObject);
            //    return;
            //}
            //else
            //{
            //    Instance = this;
            //    DontDestroyOnLoad(this.gameObject);
            //}
            dictAdsNetwork = new Dictionary<AdsNetwork, BaseAds>();
            dictAdsNetwork.Add(AdsNetwork.Max, new MaxController());
            //dictAdsNetwork.Add(AdsNetwork.Admob, new AdmobController());
#if UNITY_ANDROID || UNITY_EDITOR
            InitAds(AdsNetwork.Max);
#else
            bool checkAskGDPR = PlayerPrefs.GetInt("IsTrackGDPR", -1) == -1 ? false : true;
            if (checkAskGDPR)
            {
                if (!dictAdsNetwork[AdsNetwork.Max].IsInitialize())
                    InitAds(AdsNetwork.Max);
            }
#endif
            //StartCoroutine(CheckInternetForInitAdmob());
        }
        private void Start()
        {
            dictAdsNetwork[AdsNetwork.Max]._EventRewardInternal = CustomEventInternalRewarded;
            //dictAdsNetwork[AdsNetwork.Admob]._EventRewardInternal = CustomEventInternalRewarded;

            dictAdsNetwork[AdsNetwork.Max]._EventInterstitialInternal = CustomEventInternalInterstitial;
            //dictAdsNetwork[AdsNetwork.Admob]._EventInterstitialInternal = CustomEventInternalInterstitial;
        }
        protected void CustomEventInternalRewarded()
        {

        }
        protected void CustomEventInternalInterstitial()
        {

        }
        public void InitAds(AdsNetwork adsNetwork)
        {
            dictAdsNetwork[adsNetwork].Init();
        }
        protected IEnumerator CheckInternetForInitAdmob()
        {
            WaitForSeconds delay = new WaitForSeconds(3);
            while (true)
            {
                if (this.IsInternetConnection())
                {
                    InitAds(AdsNetwork.Admob);
                    break;
                }
                yield return delay;
            }
        }
        public void RemoveAds()
        {
            this.isRemoveAds = true;
        }
        protected bool IsInternetConnection()
        {
            return !(Application.internetReachability == NetworkReachability.NotReachable);
        }
        public bool ShowAds(AdsNetwork adsNetwork, AdsType adsType, System.Action callbackAction = null, string placement = "", AdditionAdsType additionAdsType = AdditionAdsType.General)
        {
            switch (adsType)
            {
                case AdsType.AdaptiveBanner:
                    if (this.isRemoveAds) return false;
                    return dictAdsNetwork[adsNetwork].ShowAdaptiveBanner(additionAdsType);
                case AdsType.CollapsibleBanner:
                    if (this.isRemoveAds) return false;
                    return dictAdsNetwork[adsNetwork].ShowCollapsibleBanner(additionAdsType);
                case AdsType.Interstitial:
                    if (this.isRemoveAds) return false;
                    return dictAdsNetwork[adsNetwork].ShowInterstitial(additionAdsType, callbackAction);
                case AdsType.Rewarded:
                    return dictAdsNetwork[adsNetwork].ShowRewarded(additionAdsType, callbackAction, placement);
                case AdsType.AppOpen:
                    if (this.isRemoveAds) return false;
                    return dictAdsNetwork[adsNetwork].ShowAppOpen(additionAdsType);
                case AdsType.MREC:
                    if (this.isRemoveAds) return false;
                    return dictAdsNetwork[adsNetwork].ShowMREC(additionAdsType);
                default:
                    return false;
            }
        }
        // protected void OnApplicationPause(bool pause)
        // {

        //     if (!pause)
        //     {
        //         ShowAds(AdsNetwork.Max, AdsType.AppOpen);
        //     }
        // }
        private void Update()
        {
            countdownAds += Time.unscaledDeltaTime;

        }
    }
   
    }

