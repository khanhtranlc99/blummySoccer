namespace MoonlightFramework
{
    public abstract class BaseAds
    {
        protected internal System.Action _EventRewardInternal, _EventInterstitialInternal;
        protected System.Action CallbackRewardedAfterClose, CallbackInterstitialAfterClose;
        protected AdsNetwork adsNetwork;
        public abstract void Init();
        public virtual bool IsInitialize()
        {
            return false;
        }

        protected internal virtual void RequestAdaptiveBanner(AdditionAdsType additionType)
        {
        }
        protected internal virtual void RequestCollapsibleBanner(AdditionAdsType additionType)
        {
        }
        protected internal virtual void RequestInterstitial(AdditionAdsType additionType)
        {
        }
        protected internal virtual void RequestRewarded(AdditionAdsType additionType)
        {
        }
        protected internal virtual void RequestAppOpen(AdditionAdsType additionType)
        {
        }
        public virtual bool ShowAdaptiveBanner(AdditionAdsType additionType)
        {
            return false;
        }
        public virtual void HideAdaptiveBanner(AdditionAdsType additionType)
        {
        }
        public virtual bool ShowCollapsibleBanner(AdditionAdsType additionType)
        {
            return false;
        }
        public virtual void HideCollapBanner(AdditionAdsType additionType)
        {
        }
        public abstract bool ShowInterstitial(AdditionAdsType additionType, System.Action callbackInterClose);
        public abstract bool ShowRewarded(AdditionAdsType additionType, System.Action callbackrewardDone, string logEvent = "");
        public virtual bool ShowAppOpen(AdditionAdsType additionType)
        {
            return false;
        }
        public virtual bool ShowMREC(AdditionAdsType additionType)
        {
            return false;
        }
        public virtual void HideMREC(AdditionAdsType additionType)
        {

        }
        public virtual void DestroyMREC(AdditionAdsType additionType)
        {

        }
        protected internal virtual bool IsBannerAdaptiveLoaded()
        {
            return false;
        }
        protected internal virtual bool IsBannerCollapsibleLoaded()
        {
            return false;
        }
        protected internal virtual bool IsInterstitialLoaded(AdditionAdsType additionType)
        {
            return false;
        }
        protected internal virtual bool IsRewardedLoaded(AdditionAdsType additionType)
        {
            return false;
        }
        protected internal virtual bool IsOpenAppLoaded(AdditionAdsType additionType)
        {
            return false;
        }
    }
}
