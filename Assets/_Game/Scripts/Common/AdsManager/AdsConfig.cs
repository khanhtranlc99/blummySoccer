using System.Collections.Generic;

namespace MoonlightFramework
{
    public class AdsConfig
    {
        public const string MaxSdkKey = "izbW4oEiJA_cdTh6wc0r6Cqyel80b8VaLe1pL0pAKx7TvV9BoLk4F29V4R3OUqiynDPwowsUIsszEb66mbssOZ";

#if UNITY_ANDROID
        public static readonly Dictionary<(AdsNetwork, AdsType, AdditionAdsType), string> AdsKey = new Dictionary<(AdsNetwork, AdsType, AdditionAdsType), string>()
        {
            {(AdsNetwork.Max,AdsType.AdaptiveBanner, AdditionAdsType.General),"738fc4ac1c0644ee" },
            {(AdsNetwork.Max,AdsType.Interstitial, AdditionAdsType.General),"606422c9fa3ddfdf" },
            {(AdsNetwork.Max,AdsType.Rewarded, AdditionAdsType.General),"87b7ac618f73006f" },
            {(AdsNetwork.Max,AdsType.AppOpen, AdditionAdsType.General),"f134fb77cadd5d81" },
            {(AdsNetwork.Max,AdsType.MREC, AdditionAdsType.General),"3975eee6b214276b" },

            {(AdsNetwork.Admob,AdsType.CollapsibleBanner, AdditionAdsType.General),"ca-app-pub-8048589936179473/2504720006" },
            {(AdsNetwork.Admob,AdsType.AdaptiveBanner, AdditionAdsType.General),"ca-app-pub-8048589936179473/2504720006" },
            {(AdsNetwork.Admob,AdsType.Interstitial, AdditionAdsType.General),"ca-app-pub-3940256099942544/1033173712" },
            {(AdsNetwork.Admob,AdsType.Rewarded, AdditionAdsType.General),"ca-app-pub-3940256099942544/5224354917" },
        };
#endif

#if UNITY_IOS
        public static readonly Dictionary<(AdsNetwork, AdsType, AdditionAdsType), string> AdsKey = new Dictionary<(AdsNetwork, AdsType, AdditionAdsType), string>()
        {
            {(AdsNetwork.Max,AdsType.AdaptiveBanner, AdditionAdsType.General),"64cc9083cef8d84f" },
            {(AdsNetwork.Max,AdsType.Interstitial, AdditionAdsType.General),"84ff57fdea84ab74" },
            {(AdsNetwork.Max,AdsType.Rewarded, AdditionAdsType.General),"3f5d4d7eb77cf447" },
            {(AdsNetwork.Max,AdsType.AppOpen, AdditionAdsType.General),"f134fb77cadd5d81" },
            {(AdsNetwork.Max,AdsType.MREC, AdditionAdsType.General),"99e97fc7cc821d0b" },

            {(AdsNetwork.Admob,AdsType.CollapsibleBanner, AdditionAdsType.General),"ca-app-pub-8048589936179473/2504720006" },
            {(AdsNetwork.Admob,AdsType.AdaptiveBanner, AdditionAdsType.General),"ca-app-pub-8048589936179473/2504720006" },
            {(AdsNetwork.Admob,AdsType.Interstitial, AdditionAdsType.General),"ca-app-pub-3940256099942544/1033173712" },
            {(AdsNetwork.Admob,AdsType.Rewarded, AdditionAdsType.General),"ca-app-pub-3940256099942544/5224354917" },
        };
#endif

        //ID FOR TEST
        //public static readonly Dictionary<(AdsNetwork, AdsType, AdditionAdsType), string> AdsKey = new Dictionary<(AdsNetwork, AdsType, AdditionAdsType), string>()
        //{
        //    {(AdsNetwork.Max,AdsType.AdaptiveBanner, AdditionAdsType.General),"ENTER_BANNER_AD_UNIT_ID_HERE" },
        //    {(AdsNetwork.Max,AdsType.Interstitial, AdditionAdsType.General),"ENTER_INTERSTITIAL_AD_UNIT_ID_HERE" },
        //    {(AdsNetwork.Max,AdsType.Rewarded, AdditionAdsType.General),"ENTER_REWARD_AD_UNIT_ID_HERE" },

        //    {(AdsNetwork.Admob,AdsType.AppOpen, AdditionAdsType.General),"ca-app-pub-3940256099942544/9257395921" },
        //    {(AdsNetwork.Admob,AdsType.AdaptiveBanner, AdditionAdsType.General),"ca-app-pub-3940256099942544/6300978111" },
        //    {(AdsNetwork.Admob,AdsType.Interstitial, AdditionAdsType.General),"ca-app-pub-3940256099942544/1033173712" },
        //    {(AdsNetwork.Admob,AdsType.Rewarded, AdditionAdsType.General),"ca-app-pub-3940256099942544/5224354917" },
        //    {(AdsNetwork.Admob,AdsType.RewardedInterstitial, AdditionAdsType.General),"ca-app-pub-3940256099942544/5354046379" },
        //    {(AdsNetwork.Admob,AdsType.Native, AdditionAdsType.General),"ca-app-pub-3940256099942544/2247696110" },
        //    {(AdsNetwork.Admob,AdsType.CollapsibleBanner, AdditionAdsType.General),"ca-app-pub-3940256099942544/2014213617" },
        //};

        public static float Interstitial_Delay = 60;
    }
}
