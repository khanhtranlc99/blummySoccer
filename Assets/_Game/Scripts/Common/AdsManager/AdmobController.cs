using DG.Tweening;
//using GoogleMobileAds.Api;
using System;
using UnityEngine;

namespace MoonlightFramework
{
    public class AdmobController : BaseAds
    {
        //        protected BannerView _AdaptiveBannerView;
        //        protected BannerView _CollapsibleBannerView;
        //        private InterstitialAd _Interstitial;
        //        private RewardedAd _Rewarded;
        //        private AppOpenAd _AppOpen;

        //        private readonly TimeSpan TIMEOUT_APPOPEN = TimeSpan.FromHours(4);
        //        private DateTime _expireTimeAppOpen;
        //        private static string outputMessage = string.Empty;
        //        private int interstitialRetryAttempt;
        //        private int rewardedRetryAttempt;
        //        private int AppOpenRetryAttempt;

        //        Tween DelayAdaptive;
        //        Tween DelayCollapsible;
        //        Tween DelayInter;
        //        Tween DelayRewarded;
        //        Tween DelayAppOpen;

        //        public AdmobController()
        //        {
        //            this.adsNetwork = AdsNetwork.Admob;
        //        }
        //        public override void Init()
        //        {
        //            MobileAds.Initialize(result =>
        //            {
        //                RequestCollapsibleBanner(AdditionAdsType.General);
        //                RequestAdaptiveBanner(AdditionAdsType.General);

        //                ShowAdaptiveBanner();
        //            });
        //        }
        //        #region BANNER
        //        protected internal override void RequestAdaptiveBanner(AdditionAdsType additionAdsType = AdditionAdsType.General)
        //        {
        //            base.RequestAdaptiveBanner(additionAdsType);
        //            // Clean up banner ad before creating a new one.
        //            if (_AdaptiveBannerView != null)
        //            {
        //                _AdaptiveBannerView.Destroy();
        //                _AdaptiveBannerView = null;
        //            }
        //            AdSize adsize = AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //            this._AdaptiveBannerView = new BannerView(this.GetIdAds(AdsType.AdaptiveBanner, additionAdsType), adsize, AdPosition.Bottom);
        //            switch (additionAdsType)
        //            {
        //                case AdditionAdsType.General:
        //                    this._AdaptiveBannerView.OnBannerAdLoaded += () =>
        //                    {
        //                    };
        //                    this._AdaptiveBannerView.OnBannerAdLoadFailed += (error) =>
        //                    {
        //                        LoadDelayAdaptiveBanner();
        //                    };
        //                    this._AdaptiveBannerView.OnAdFullScreenContentOpened += () =>
        //                    {

        //                    };
        //                    this._AdaptiveBannerView.OnAdFullScreenContentClosed += () =>
        //                    {

        //                    };
        //                    this._AdaptiveBannerView.OnAdPaid += (adValue) =>
        //                    {
        //                        FirebaseManager.FirebaseTrackRevenue(adValue, AdsType.AdaptiveBanner, GetIdAds(AdsType.AdaptiveBanner));

        //                    };
        //                    this._AdaptiveBannerView.OnAdClicked += () =>
        //                    {
        //                    };
        //                    this._AdaptiveBannerView.OnAdFullScreenContentOpened += () =>
        //                    {

        //                    };
        //                    this._AdaptiveBannerView.OnAdFullScreenContentClosed += () =>
        //                    {

        //                    };
        //                    break;
        //                default:
        //                    break;
        //            }
        //            AdRequest request = this.CreateAdRequest();
        //            // Load a banner ad.
        //            HideAdaptiveBanner();
        //            this._AdaptiveBannerView.LoadAd(request);
        //        }
        //        protected internal override void RequestCollapsibleBanner(AdditionAdsType additionAdsType = AdditionAdsType.General)
        //        {
        //            base.RequestCollapsibleBanner(additionAdsType);
        //            // Clean up banner ad before creating a new one.
        //            if (_AdaptiveBannerView != null)
        //            {
        //                _AdaptiveBannerView.Destroy();
        //                _AdaptiveBannerView = null;
        //            }
        //            AdSize adsize = AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //            this._AdaptiveBannerView = new BannerView(this.GetIdAds(AdsType.CollapsibleBanner, additionAdsType), adsize, AdPosition.Bottom);
        //            switch (additionAdsType)
        //            {
        //                case AdditionAdsType.General:
        //                    this._AdaptiveBannerView.OnBannerAdLoaded += () =>
        //                    {
        //                        if (!_CollapsibleBannerView.IsCollapsible())
        //                        {
        //                            RequestCollapsibleBanner();
        //                        }
        //                    };
        //                    this._AdaptiveBannerView.OnBannerAdLoadFailed += (error) =>
        //                    {
        //                        LoadDelayCollapsible(3f);
        //                    };
        //                    this._AdaptiveBannerView.OnAdFullScreenContentOpened += () =>
        //                    {

        //                    };
        //                    this._AdaptiveBannerView.OnAdFullScreenContentClosed += () =>
        //                    {

        //                    };
        //                    this._AdaptiveBannerView.OnAdPaid += (adValue) =>
        //                    {
        //                        FirebaseManager.FirebaseTrackRevenue(adValue, AdsType.AdaptiveBanner, GetIdAds(AdsType.AdaptiveBanner));

        //                    };
        //                    this._AdaptiveBannerView.OnAdClicked += () =>
        //                    {
        //                    };
        //                    this._AdaptiveBannerView.OnAdFullScreenContentOpened += () =>
        //                    {

        //                    };
        //                    this._AdaptiveBannerView.OnAdFullScreenContentClosed += () =>
        //                    {
        //                        RequestCollapsibleBanner();
        //                        ShowAdaptiveBanner();
        //                    };
        //                    break;
        //                default:
        //                    break;
        //            }
        //            AdRequest request = this.CreateAdRequest();
        //            // Create an extra parameter that aligns the bottom of
        //            // the expanded ad to the bottom of the bannerView.
        //            request.Extras.Add("collapsible", "bottom");
        //            request.Extras.Add("collapsible_request_id", ((int)(Time.realtimeSinceStartup * 1000)).ToString());

        //            HideCollapsibleBanner();
        //            // Load a banner ad.
        //            this._AdaptiveBannerView.LoadAd(request);
        //        }
        //        protected void LoadDelayAdaptiveBanner()
        //        {
        //            this.DelayAdaptive.Kill();
        //            this.DelayAdaptive = DOVirtual.DelayedCall(3f, delegate
        //            {
        //                RequestAdaptiveBanner();
        //            });
        //        }
        //        protected void LoadDelayCollapsible(float TimeDelay = 3f)
        //        {
        //            this.DelayCollapsible.Kill();
        //            this.DelayCollapsible = DOVirtual.DelayedCall(TimeDelay, delegate
        //            {
        //                RequestCollapsibleBanner();
        //            });
        //        }
        //        public override bool ShowAdaptiveBanner(AdditionAdsType additionType = AdditionAdsType.General)
        //        {
        //            base.ShowAdaptiveBanner(additionType);
        //            if (_AdaptiveBannerView == null)
        //                return false;
        //            _AdaptiveBannerView.Show();
        //            HideCollapsibleBanner();
        //            return true;
        //        }
        //        public override bool ShowCollapsibleBanner(AdditionAdsType additionType = AdditionAdsType.General)
        //        {
        //            base.ShowCollapsibleBanner(additionType);
        //            if (_CollapsibleBannerView == null)
        //                return false;
        //            _CollapsibleBannerView.Show();
        //            HideAdaptiveBanner();
        //            return true;
        //        }
        //        public void HideAdaptiveBanner()
        //        {
        //            if (_AdaptiveBannerView == null)
        //                return;
        //            _AdaptiveBannerView.Hide();
        //        }
        //        public void HideCollapsibleBanner()
        //        {
        //            if (_CollapsibleBannerView == null)
        //                return;
        //            _CollapsibleBannerView.Hide();
        //        }
        //        protected internal override bool IsBannerAdaptiveLoaded()
        //        {
        //            return this._AdaptiveBannerView != null;
        //        }
        //        protected internal override bool IsBannerCollapsibleLoaded()
        //        {
        //            return this._CollapsibleBannerView != null;
        //        }
        //        #endregion

        //        #region INTERSTITIAL
        //        public override bool ShowInterstitial(AdditionAdsType additionAdsType, Action callbackInterClose)
        //        {
        //            if ((Time.time - AdsManager.Instance.lastTimeShowInter) < AdsConfig.Interstitial_Delay)
        //                return false;
        //            if (IsInterstitialLoaded(additionAdsType))
        //            {
        //                this._Interstitial.Show();
        //                this.CallbackInterstitialAfterClose = callbackInterClose;
        //                return true;
        //            }
        //            else
        //            {
        //                RequestDelayInterstitial(additionAdsType);
        //                return false;
        //            }
        //        }
        //        protected internal override void RequestInterstitial(AdditionAdsType additionType = AdditionAdsType.General)
        //        {
        //            base.RequestInterstitial(additionType);
        //            if (this._Interstitial == null)
        //                return;
        //            _Interstitial.Destroy();
        //            _Interstitial = null;

        //            AdRequest adRequest = CreateAdRequest();

        //            InterstitialAd.Load(GetIdAds(AdsType.Interstitial, additionType), adRequest,
        //            (InterstitialAd ad, LoadAdError error) =>
        //            {
        //                // if error is not null, the load request failed.
        //                if (error != null || ad == null)
        //                {
        //                    Debug.LogError("interstitial ad failed to load an ad " +
        //                                   "with error : " + error);
        //                    this.interstitialRetryAttempt++;
        //                    RequestDelayInterstitial(additionType);
        //                    return;
        //                }

        //                Debug.Log("Interstitial ad loaded with response : "
        //                + ad.GetResponseInfo());

        //                this._Interstitial = ad;
        //                this.interstitialRetryAttempt = 0;

        //                // Raised when the ad is estimated to have earned money.
        //                this._Interstitial.OnAdPaid += (AdValue adValue) =>
        //                {
        //                    Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
        //                        adValue.Value,
        //                        adValue.CurrencyCode));
        //                    FirebaseManager.FirebaseTrackRevenue(adValue, AdsType.Interstitial, GetIdAds(AdsType.Interstitial, additionType));
        //                };
        //                // Raised when an impression is recorded for an ad.
        //                this._Interstitial.OnAdImpressionRecorded += () =>
        //                {
        //                    Debug.Log("Interstitial ad recorded an impression.");
        //                };
        //                // Raised when a click is recorded for an ad.
        //                this._Interstitial.OnAdClicked += () =>
        //                {
        //                    Debug.Log("Interstitial ad was clicked.");
        //                };
        //                // Raised when an ad opened full screen content.
        //                this._Interstitial.OnAdFullScreenContentOpened += () =>
        //                {
        //                    // FirebaseManager.LogEvent(FirebaseManager.m_inter_show);
        //                    Debug.Log("Interstitial ad full screen content opened.");
        //                };
        //                // Raised when the ad closed full screen content.
        //                this._Interstitial.OnAdFullScreenContentClosed += () =>
        //                {
        //                    Debug.Log("Interstitial Ad full screen content closed.");

        //                    this.CallbackInterstitialAfterClose();
        //                    this.CallbackInterstitialAfterClose = null;
        //                    this._EventInterstitialInternal?.Invoke();
        //                    // Reload the ad so that we can show another as soon as possible.
        //                    RequestInterstitial(additionType);
        //                };
        //                // Raised when the ad failed to open full screen content.
        //                this._Interstitial.OnAdFullScreenContentFailed += (AdError error) =>
        //                {
        //                    Debug.LogError("Interstitial ad failed to open full screen content " +
        //                                   "with error : " + error);

        //                    // Reload the ad so that we can show another as soon as possible.
        //                    RequestDelayInterstitial(additionType);
        //                };
        //            });
        //        }
        //        protected void RequestDelayInterstitial(AdditionAdsType additionAdsType)
        //        {
        //            double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        //            this.DelayInter.Kill();
        //            this.DelayInter = DOVirtual.DelayedCall((float)retryDelay, delegate
        //            {
        //                RequestInterstitial(additionAdsType);
        //            }, false);
        //        }

        //        protected internal override bool IsInterstitialLoaded(AdditionAdsType additionAdsType)
        //        {
        //            if (this._Interstitial != null)
        //                if (this._Interstitial.CanShowAd())
        //                    return true;
        //            return false;
        //        }
        //        #endregion

        //        #region REWARDED
        //        public override bool ShowRewarded(AdditionAdsType additionAdsType, Action callbackRewarded)
        //        {
        //#if UNITY_EDITOR
        //            this.CallbackRewardedAfterClose = callbackRewarded;
        //            this.CallbackRewardedAfterClose?.Invoke();
        //            this._EventRewardInternal?.Invoke();
        //            return true;
        //#endif
        //            if (IsRewardedLoaded(additionAdsType))
        //            {
        //                this.CallbackRewardedAfterClose = callbackRewarded;
        //                this._Rewarded.Show((Reward reward) =>
        //                {
        //                    // TODO: Reward the user.
        //                    // Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
        //                    this.CallbackRewardedAfterClose?.Invoke();
        //                    this._EventRewardInternal?.Invoke();
        //                });
        //                return true;
        //            }
        //            else
        //            {
        //                this.CallbackInterstitialAfterClose = null;
        //                RequestDelayRewarded(additionAdsType);
        //                return false;
        //            }
        //        }
        //        protected internal override void RequestRewarded(AdditionAdsType additionType = AdditionAdsType.General)
        //        {
        //            base.RequestRewarded(additionType);
        //            // Clean up the old ad before loading a new one.
        //            if (this._Rewarded != null)
        //            {
        //                _Rewarded.Destroy();
        //                _Rewarded = null;
        //            }
        //            AdRequest adRequest = CreateAdRequest();
        //            // send the request to load the ad.
        //            RewardedAd.Load(GetIdAds(AdsType.Rewarded, additionType), adRequest,
        //                (RewardedAd ad, LoadAdError error) =>
        //                {
        //                    // if error is not null, the load request failed.
        //                    if (error != null || ad == null)
        //                    {
        //                        Debug.LogError("Rewarded ad failed to load an ad " +
        //                                       "with error : " + error);
        //                        this.rewardedRetryAttempt++;
        //                        RequestRewarded();
        //                        return;
        //                    }

        //                    Debug.Log("Rewarded ad loaded with response : "
        //                              + ad.GetResponseInfo());

        //                    this._Rewarded = ad;
        //                    this.rewardedRetryAttempt = 0;

        //                    // Raised when the ad is estimated to have earned money.
        //                    this._Rewarded.OnAdPaid += (AdValue adValue) =>
        //                    {
        //                        Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
        //                            adValue.Value,
        //                            adValue.CurrencyCode));
        //                        FirebaseManager.FirebaseTrackRevenue(adValue, AdsType.Rewarded, GetIdAds(AdsType.Rewarded, additionType));
        //                    };
        //                    this._Rewarded.OnAdImpressionRecorded += () =>
        //                    {
        //                    };
        //                    this._Rewarded.OnAdClicked += () =>
        //                    {
        //                    };
        //                    this._Rewarded.OnAdFullScreenContentOpened += () =>
        //                    {
        //                        Debug.Log("Rewarded ad full screen content opened.");
        //                    };
        //                    this._Rewarded.OnAdFullScreenContentClosed += () =>
        //                    {
        //                        RequestRewarded(additionType);
        //                    };
        //                    this._Rewarded.OnAdFullScreenContentFailed += (AdError error) =>
        //                    {
        //                        RequestDelayRewarded(additionType);
        //                    };
        //                });
        //        }
        //        protected void RequestDelayRewarded(AdditionAdsType additionAdsType)
        //        {
        //            double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        //            this.DelayRewarded.Kill();
        //            this.DelayRewarded = DOVirtual.DelayedCall((float)retryDelay, delegate
        //            {
        //                RequestRewarded(additionAdsType);
        //            }, false);
        //        }
        //        protected internal override bool IsRewardedLoaded(AdditionAdsType additionAdsType)
        //        {
        //            if (this._Rewarded != null)
        //                if (this._Rewarded.CanShowAd())
        //                    return true;
        //            return false;
        //        }
        //        #endregion

        //        #region APPOPEN
        //        public override bool ShowAppOpen(AdditionAdsType additionType)
        //        {
        //            base.ShowAppOpen(additionType);
        //            if (this.IsOpenAppLoaded(additionType) && DateTime.Now < this._expireTimeAppOpen)
        //            {
        //                this._AppOpen.Show();
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        protected internal override void RequestAppOpen(AdditionAdsType additionType = AdditionAdsType.General)
        //        {
        //            base.RequestAppOpen(additionType);
        //            if (this._AppOpen != null)
        //            {
        //                _AppOpen.Destroy();
        //                _AppOpen = null;
        //            }

        //            AdRequest adRequest = new AdRequest();
        //            AppOpenAd.Load(GetIdAds(AdsType.AppOpen), adRequest, (AppOpenAd ad, LoadAdError error) =>
        //            {
        //                if (error != null || ad == null)
        //                {
        //                    Debug.LogError("App open ad failed to load an ad " +
        //                                   "with error : " + error);
        //                    this.AppOpenRetryAttempt++;
        //                    RequestDelayAppOpen(additionType);
        //                    return;
        //                }
        //                // The operation completed successfully.
        //                Debug.Log("App open ad loaded with response : " + ad.GetResponseInfo());
        //                this._AppOpen = ad;
        //                this.AppOpenRetryAttempt = 0;

        //                // App open ads can be preloaded for up to 4 hours.
        //                this._expireTimeAppOpen = DateTime.Now + TIMEOUT_APPOPEN;

        //                // Register to ad events to extend functionality.
        //                // Raised when the ad is estimated to have earned money.
        //                ad.OnAdPaid += (AdValue adValue) =>
        //                {
        //                    FirebaseManager.FirebaseTrackRevenue(adValue, AdsType.AppOpen, GetIdAds(AdsType.AppOpen));
        //                };
        //                // Raised when an impression is recorded for an ad.
        //                ad.OnAdImpressionRecorded += () =>
        //                {
        //                    Debug.Log("App open ad recorded an impression.");
        //                };
        //                // Raised when a click is recorded for an ad.
        //                ad.OnAdClicked += () =>
        //                {
        //                    Debug.Log("App open ad was clicked.");
        //                };
        //                // Raised when an ad opened full screen content.
        //                ad.OnAdFullScreenContentOpened += () =>
        //                {
        //                    Debug.Log("App open ad full screen content opened.");

        //                    // Inform the UI that the ad is consumed and not ready.
        //                };
        //                // Raised when the ad closed full screen content.
        //                ad.OnAdFullScreenContentClosed += () =>
        //                {
        //                    Debug.Log("App open ad full screen content closed.");

        //                    // It may be useful to load a new ad when the current one is complete.
        //                    RequestAppOpen(additionType);
        //                };
        //                // Raised when the ad failed to open full screen content.
        //                ad.OnAdFullScreenContentFailed += (AdError error) =>
        //                {
        //                    Debug.LogError("App open ad failed to open full screen content with error : "
        //                                    + error);
        //                };

        //                // Inform the UI that the ad is ready.
        //            });
        //        }
        //        protected void RequestDelayAppOpen(AdditionAdsType additionAdsType)
        //        {
        //            double retryDelay = Math.Pow(2, Math.Min(6, AppOpenRetryAttempt));
        //            this.DelayAppOpen.Kill();
        //            this.DelayAppOpen = DOVirtual.DelayedCall((float)retryDelay, delegate
        //            {
        //                RequestAppOpen(additionAdsType);
        //            }, false);
        //        }

        //        protected internal override bool IsOpenAppLoaded(AdditionAdsType additionType)
        //        {
        //            if (this._AppOpen != null)
        //                if (this._AppOpen.CanShowAd())
        //                    return true;
        //            return false;
        //        }
        //        #endregion

        //        private AdRequest CreateAdRequest()
        //        {
        //            return new AdRequest();
        //        }
        //        private string GetIdAds(AdsType adsType, AdditionAdsType additionAdsType = AdditionAdsType.General)
        //        {
        //            return AdsConfig.AdsKey[(adsNetwork, adsType, additionAdsType)];
        //        }
        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override bool ShowInterstitial(AdditionAdsType additionType, Action callbackInterClose)
        {
            throw new NotImplementedException();
        }

        public override bool ShowRewarded(AdditionAdsType additionType, Action callbackrewardDone, string logEvent = "")
        {
            throw new NotImplementedException();
        }
    }

}
