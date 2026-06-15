using System;
using UnityEngine.Scripting;

namespace XGame
{
    [Preserve]
    [Serializable]
    public class XMYAdResult
    {
        public const string AD_TYPE_SPLASH = "splash"; //splash
        public const string AD_TYPE_BANNER = "banner"; //Banner
        public const string AD_TYPE_INTERS = "inters"; //插页
        public const string AD_TYPE_REWARD_INTERS = "rewardInters"; //激励插页
        public const string AD_TYPE_VIDEO = "reward"; //视频
        public const string AD_TYPE_NATIVE = "native"; //原生
        public const string AD_TYPE_TEMPLATE = "template"; //模板广告
        public const string AD_TYPE_NATIVE_BANNER = "nativeBanner"; //原生banner广告

        public const string AD_EVENT_FAILED = "onAdFailed"; //广告失败回调
        public const string AD_EVENT_LOADED = "onAdReady"; //广告加载成功回调
        public const string AD_EVENT_SHOWED = "onAdShow"; //广告展示回调
        public const string AD_EVENT_CLOSED = "onAdClose"; //广告结束/关闭回调
        public const string AD_EVENT_REWARDED = "onAdReward"; //激励视频广告发放奖励回调
        public const string AD_EVENT_CLICK = "onAdClick"; //点击回调
        
        //广告类型： 对应AD_TYPE_XX
        public string adType;

        //事件类型：对应AD_EVENT_XX 
        public string eventType;

        //广告位ID
        public string adId;
    }
}