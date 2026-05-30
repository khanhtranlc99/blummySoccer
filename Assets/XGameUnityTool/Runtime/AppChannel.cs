using System;

namespace XGame
{
    /// <summary>
    /// app 发布渠道
    /// </summary>
    public enum AppChannel
    {
        
        Unknown = 0,
        
        /// <summary>
        /// MarSdk 渠道(国内各个平台) 以及华为海外
        /// </summary>
        Mar = 1,

        /// <summary>
        /// 谷歌渠道
        /// </summary>
        [Obsolete("已弃用")]
        Google = 2,

        /// <summary>
        /// IOS 国内
        /// </summary>
        [Obsolete("已弃用")]
        IOSInland = 3,

        /// <summary>
        /// IOS 海外
        /// </summary>
        [Obsolete("已弃用")]
        IOSOverseas = 4,

        /// <summary>
        /// 抖音小游戏（Android）
        /// </summary>
        [Obsolete("已弃用")]
        ByteDanceMiniGame = 5,

        /// <summary>
        /// 测试
        /// </summary>
        [Obsolete("已弃用")]
        Test = 6,

        /// <summary>
        /// 微信小游戏
        /// </summary>
        [Obsolete("已弃用")]
        WeChat = 7,

        /// <summary>
        /// 抖音小游戏（IOS）
        /// </summary>
        [Obsolete("已弃用")]
        ByteDanceMiniGameIOS = 8,

        /// <summary>
        /// oppo小游戏
        /// </summary>
        [Obsolete("已弃用")]
        OppoMini = 9,

        /// <summary>
        /// vivo小游戏
        /// </summary>
        [Obsolete("已弃用")]
        VivoMini = 10,

        /// <summary>
        /// xmy google
        /// </summary>
        XMYGoogle = 11,

        /// <summary>
        /// 微信小游戏 ASC SDK 
        /// </summary>
        [Obsolete("已弃用")]
        WeChat_ASC = 12,

        /// <summary>
        /// vivo 小游戏 ASC SDK 
        /// </summary>
        [Obsolete("已弃用")]
        Vivo_ASC = 13,


        /// <summary>
        /// oppo 小游戏 ASC SDK 
        /// </summary>
        [Obsolete("已弃用")]
        Oppo_ASC = 14,

        /// <summary>
        /// 微信小游戏 XSDK SDK
        /// </summary>
        WeChat_XSDK = 15,


        /// <summary>
        /// vivo小游戏 XSDK SDK
        /// </summary>
        Vivo_XSDK = 16,

        /// <summary>
        /// oppo小游戏 XSDK SDK
        /// </summary>
        Oppo_XSDK = 17,

        /// <summary>
        /// 抖音小游戏 XSDK Android
        /// </summary>
        Douyin_XSDK_Android = 18,

        /// <summary>
        /// 抖音小游戏 XSDK IOS
        /// </summary>
        Douyin_XSDK_IOS = 19,

        /// <summary>
        /// IOS XGUG 国内
        /// </summary>
        IOS_XGUG_China = 20,


        /// <summary>
        /// IOS XGUG 海外
        /// </summary>
        IOS_XGUG_Sea = 21,
        /// <summary>
        /// Android Light 国内
        /// </summary>
        Android_Light = 22,
        
        /// <summary>
        /// 快手小游戏 Android
        /// </summary>
        Kuaishou_XSDK_Android = 23,
        
        /// <summary>
        /// 华为小游戏 Android
        /// </summary>
        Huawei_XSDK = 24,
        
        /// <summary>
        /// B站小游戏
        /// </summary>
        Bilibili_XSDK = 25,
        
        /// <summary>
        /// 快手小游戏
        /// </summary>
        Kuaishou_XSDK = 26,
        
        /// <summary>
        /// 鸿蒙
        /// </summary>
        OpenHarmony_Light = 27,
        
        /// <summary>
        /// LOG Google
        /// </summary>
        Google_Log_SDK = 28,
    }
}