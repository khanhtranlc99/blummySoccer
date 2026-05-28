using DG.Tweening;
using System;
using UnityEngine;

namespace MoonlightFramework
{
    public class MaxController : BaseAds
    {
        private int interstitialRetryAttempt;
        private int rewardedRetryAttempt;

        protected Tween TempTweenLoadInter;
        protected Tween TempTweenLoadRewarded;

        public MaxController()
        {
            this.adsNetwork = AdsNetwork.Max;
        }
        public override bool IsInitialize()
        {
            return MaxSdk.IsInitialized();
        }
        public override void Init()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
            {
                // AppLovin SDK is initialized, start loading ads
                // Banners are automatically sized to 320�50 on phones and 728�90 on tablets
                // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
                RequestAdaptiveBanner();
                RequestInterstitial();
                RequestRewarded();
                RequestAppOpen();
                InitializeMREC();
            };
        //    AdsManager.Instance.countdownAds = 10000;
            //MaxSdk.ShowMediationDebugger();
            MaxSdk.InitializeSdk();
        }
        public override bool ShowInterstitial(AdditionAdsType additionType, Action callbackInterClose)
        {
     //       Debug.LogError("Count_down_" + AdsManager.Instance.countdownAds);
            Debug.LogError("IsInterstitialReady_" + MaxSdk.IsInterstitialReady(GetIdAds(AdsType.Interstitial, additionType)));
            //if (AdsManager.Instance.countdownAds < AdsConfig.Interstitial_Delay)
            //{
            //    callbackInterClose?.Invoke();
            //    return false;
            //}    
         
            if (MaxSdk.IsInterstitialReady(GetIdAds(AdsType.Interstitial, additionType)))
            {
                this.CallbackInterstitialAfterClose = () =>
                {
                    callbackInterClose?.Invoke();
                    FirebaseManager.LogEvent("Inter_Show");
                };
                //AdsManager.Instance.lastTimeShowInter = Time.time;
                MaxSdk.ShowInterstitial(GetIdAds(AdsType.Interstitial, additionType));
             
                return true;
            }
            else
            {
                callbackInterClose?.Invoke();
                LoadInterstitial(additionType);
         
                return false;
            }
        }
        public override bool ShowRewarded(AdditionAdsType additionType, Action callbackrewardDone, string placement = "")
        {
            if (MaxSdk.IsRewardedAdReady(GetIdAds(AdsType.Rewarded, additionType)))
            {
                this.CallbackRewardedAfterClose = () =>
                {
                    callbackrewardDone?.Invoke();
                    FirebaseManager.LogEvent("Reward_Show_" + placement);
                };
                MaxSdk.ShowRewardedAd(GetIdAds(AdsType.Rewarded, additionType));
                //AdsManager.Instance.countdownAds = 0;
                return true;
            }
            else
            {
                this.RequestRewardDelay(additionType);
                return false;
            }
        }
        public override bool ShowAdaptiveBanner(AdditionAdsType additionType)
        {
            // if (IsBannerAdaptiveLoaded())
            // {
            MaxSdk.ShowBanner(GetIdAds(AdsType.AdaptiveBanner, additionType));
            return true;
            // }
            // return false;
        }
        public override void HideAdaptiveBanner(AdditionAdsType additionType)
        {
            base.HideAdaptiveBanner(additionType);
            MaxSdk.HideBanner(GetIdAds(AdsType.AdaptiveBanner, additionType));
        }
        private string GetIdAds(AdsType adsType, AdditionAdsType additionAdsType = AdditionAdsType.General)
        {
            return AdsConfig.AdsKey[(adsNetwork, adsType, additionAdsType)]; ;
        }
        protected internal override void RequestAdaptiveBanner(AdditionAdsType additionType = AdditionAdsType.General)
        {
            base.RequestAdaptiveBanner(additionType);
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += (id, adinfo) =>
            {
                if (id.Equals(GetIdAds(AdsType.AdaptiveBanner, additionType)))
                {
                    Debug.Log("Banner ad loaded");
                }
            };
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += (id, errorInfo) =>
            {
                if (id.Equals(GetIdAds(AdsType.AdaptiveBanner, additionType)))
                {
                    Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
                }
            };
            MaxSdkCallbacks.Banner.OnAdClickedEvent += (id, adinfo) =>
            {
                if (id.Equals(GetIdAds(AdsType.AdaptiveBanner, additionType)))
                {
                    Debug.Log("Banner ad clicked");
                }
            };
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (id, adInfo) =>
            {
                if (id.Equals(GetIdAds(AdsType.AdaptiveBanner, additionType)))
                {
                    Debug.Log("Banner ad revenue paid");

                    // Ad revenue
                    double revenue = adInfo.Revenue;
                    // Miscellaneous data
                    string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
                    string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
                    string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
                    string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
                    FirebaseManager.FirebaseTrackRevenue(adInfo);
             
                }
            };
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += (id, adinfo) =>
            {

            };
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += (id, adinfo) =>
            {

            };

            // Adaptive banners are sized based on device width for positions that stretch full width (TopCenter and BottomCenter).
            // You may use the utility method `MaxSdkUtils.GetAdaptiveBannerHeight()` to help with view sizing adjustments
            MaxSdk.CreateBanner(GetIdAds(AdsType.AdaptiveBanner, additionType), MaxSdkBase.BannerPosition.BottomCenter);
            MaxSdk.SetBannerExtraParameter(GetIdAds(AdsType.AdaptiveBanner, additionType), "adaptive_banner", "true");
            MaxSdk.SetBannerBackgroundColor(GetIdAds(AdsType.AdaptiveBanner, additionType), Color.black);

            ShowAdaptiveBanner(AdditionAdsType.General);
        }
        protected internal override void RequestInterstitial(AdditionAdsType additionType = AdditionAdsType.General)
        {
            base.RequestInterstitial(additionType);
            switch (additionType)
            {
                case AdditionAdsType.General:
                    MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (id, adinfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Interstitial, additionType)))
                        {
                            Debug.Log("Interstitial loaded");

                            // Reset retry attempt
                            interstitialRetryAttempt = 0;
                        }
                    };
                    MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (id, errorInfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Interstitial, additionType)))
                        {
                            interstitialRetryAttempt++;
                            Debug.Log("Interstitial failed to load with error code: " + errorInfo.Code);
                            RequestInterstitialDelay(additionType);
                        }
                    };
                    MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (id, errorInfo, adInfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Interstitial, additionType)))
                        {
                            LoadInterstitial(additionType);
                        }
                    };
                    MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (id, adinfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Interstitial, additionType)))
                        {
                            // MainCanvas.Instance.HiddenAds();
                            this.CallbackInterstitialAfterClose?.Invoke();
                            this.CallbackInterstitialAfterClose = null;
                            this._EventInterstitialInternal?.Invoke();
                            LoadInterstitial(additionType);
                            //AdsManager.Instance.lastTimeShowInter = Time.time;
                            //AdsManager.Instance.countdownAds = 0;
                        }
                    };
                    MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += (id, adInfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Interstitial, additionType)))
                        {
                            Debug.Log("Interstitial revenue paid");

                            // Ad revenue
                            double revenue = adInfo.Revenue;

                            // Miscellaneous data
                            string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
                            string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
                            string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
                            string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
                            FirebaseManager.FirebaseTrackRevenue(adInfo);
                        }
                    };
                    MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += (id, adInfo) =>
                    {
                    };
                    break;

                default:
                    break;
            }

            MaxSdk.LoadInterstitial(GetIdAds(AdsType.Interstitial, additionType));
        }
        protected void RequestInterstitialDelay(AdditionAdsType additionType = AdditionAdsType.General)
        {
            double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
            this.TempTweenLoadInter.Kill();
            this.TempTweenLoadInter = DOVirtual.DelayedCall((float)retryDelay, () => LoadInterstitial(additionType), false);
        }
        protected void RequestRewardDelay(AdditionAdsType additionType = AdditionAdsType.General)
        {
            double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
            this.TempTweenLoadRewarded.Kill();
            this.TempTweenLoadRewarded = DOVirtual.DelayedCall((float)retryDelay, () => LoadRewardedAd(additionType), false);
        }
        protected internal override void RequestRewarded(AdditionAdsType additionType = AdditionAdsType.General)
        {
            base.RequestRewarded(additionType);
            switch (additionType)
            {
                case AdditionAdsType.General:
                    MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += (id, adinfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Rewarded, additionType)))
                        {
                            Debug.Log("Rewarded ad loaded");

                            // Reset retry attempt
                            rewardedRetryAttempt = 0;
                        }
                    };
                    MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (id, errorInfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Rewarded, additionType)))
                        {
                            rewardedRetryAttempt++;
                            this.RequestRewardDelay(additionType);
                        }
                    };
                    MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (id, errorInfo, adinfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Rewarded, additionType)))
                        {
                            Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
                            LoadRewardedAd(additionType);
                        }
                    };
                    MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += (id, adinfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Rewarded, additionType)))
                        {

                        }
                        //GameMgr.instance.generalVariables.isAdsShowOverlay = true;
                    };
                    MaxSdkCallbacks.Rewarded.OnAdClickedEvent += (id, adinfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Rewarded, additionType)))
                        {

                        }
                    };
                    MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (id, adinfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Rewarded, additionType)))
                        {
                            Debug.Log("Rewarded ad dismissed");
                            LoadRewardedAd(additionType);
                        }
                    };
                    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (id, reward, adinfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Rewarded, additionType)))
                        {
                            this.CallbackRewardedAfterClose?.Invoke();
                            this.CallbackRewardedAfterClose = null;
                            this._EventRewardInternal?.Invoke();
                            Debug.Log("Rewarded ad received reward");
                        }
                    };
                    MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (id, adInfo) =>
                    {
                        if (id.Equals(GetIdAds(AdsType.Rewarded, additionType)))
                        {
                            Debug.Log("Rewarded ad revenue paid");

                            // Ad revenue
                            double revenue = adInfo.Revenue;

                            // Miscellaneous data
                            string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
                            string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
                            string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
                            string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
                            FirebaseManager.FirebaseTrackRevenue(adInfo);
                            //AdsManager.Instance.countdownAds = 0;
                        }
                    };
                    break;
                default:
                    break;
            }
            MaxSdk.LoadRewardedAd(GetIdAds(AdsType.Rewarded, additionType));
        }
        protected internal override void RequestAppOpen(AdditionAdsType additionType = AdditionAdsType.General)
        {
            base.RequestAppOpen(additionType);
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += (id, info) =>
            {
                // MainCanvas.Instance.HiddenAds();
                MaxSdk.LoadAppOpenAd(GetIdAds(AdsType.AppOpen));
            };
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += (id, adInfo) =>
            {
                if (id.Equals(GetIdAds(AdsType.AppOpen, additionType)))
                {
                    Debug.Log("Open ad revenue paid");

                    // Ad revenue
                    double revenue = adInfo.Revenue;

                    // Miscellaneous data
                    string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
                    string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
                    string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
                    string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
                    FirebaseManager.FirebaseTrackRevenue(adInfo);
                }
            };
            MaxSdk.LoadAppOpenAd(GetIdAds(AdsType.AppOpen));
        }

        protected void InitializeMREC(AdditionAdsType additionType = AdditionAdsType.General)
        {
            DestroyMREC(additionType);
            MaxSdk.CreateMRec(GetIdAds(AdsType.MREC, additionType), MaxSdkBase.AdViewPosition.Centered);
            MaxSdkCallbacks.MRec.OnAdLoadedEvent += (id, info) =>
            {

            };
            MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += (id, info) =>
            {

            };
            MaxSdkCallbacks.MRec.OnAdClickedEvent += (id, info) =>
            {

            };
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += (id, adInfo) =>
            {
                if (id.Equals(GetIdAds(AdsType.MREC, additionType)))
                {
                    Debug.Log("Open ad revenue paid");

                    // Ad revenue
                    double revenue = adInfo.Revenue;

                    // Miscellaneous data
                    string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
                    string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
                    string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
                    string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
                    FirebaseManager.FirebaseTrackRevenue(adInfo);
                }
            };
            MaxSdkCallbacks.MRec.OnAdExpandedEvent += (id, info) =>
            {

            };
            MaxSdkCallbacks.MRec.OnAdCollapsedEvent += (id, info) =>
            {

            };
        }

        private void LoadInterstitial(AdditionAdsType additionType)
        {
            MaxSdk.LoadInterstitial(GetIdAds(AdsType.Interstitial, additionType));
        }
        private void LoadRewardedAd(AdditionAdsType additionType)
        {
            MaxSdk.LoadRewardedAd(GetIdAds(AdsType.Rewarded, additionType));
        }

        protected void UpdateMRecAds(AdditionAdsType additionType)
        {
            float bannerHeight = MaxSdk.GetBannerLayout(GetIdAds(AdsType.MREC, additionType)).height;
            if (bannerHeight == 0)
            {
                if (MaxSdkUtils.IsTablet())
                {
                    bannerHeight = 100;
                }
                else
                {
                    bannerHeight = 70;
                }
            }

            Rect mrecRect = MaxSdk.GetMRecLayout(GetIdAds(AdsType.MREC, additionType));
            if (mrecRect == Rect.zero)
            {
                mrecRect.width = 300;
                mrecRect.height = 250;
            }
            float density = MaxSdkUtils.GetScreenDensity();

            float height = Screen.safeArea.height;
            if (height == 0) height = Screen.height;

            float dpX = Screen.width / (density * 2);
            float dpY = height / density;

            MaxSdk.UpdateMRecPosition(GetIdAds(AdsType.MREC, additionType), dpX - mrecRect.width / 2, dpY - mrecRect.height - bannerHeight);
        }
        public override void DestroyMREC(AdditionAdsType additionType)
        {
            MaxSdk.DestroyMRec(GetIdAds(AdsType.MREC, additionType));
        }
        public override bool ShowMREC(AdditionAdsType additionType)
        {
            base.ShowMREC(additionType);
            MaxSdk.ShowMRec(GetIdAds(AdsType.MREC, additionType));
            return true;
        }
        public override void HideMREC(AdditionAdsType additionType)
        {
            base.HideMREC(additionType);
            MaxSdk.HideMRec(GetIdAds(AdsType.MREC, additionType));
        }
        
    }
}
