namespace XGame
{
    /// <summary>
    /// App配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 上架渠道
        /// </summary>
        public static AppChannel CHANNEL => AppConfigAsset.Instance.CHANNEL;

        
        /// <summary>
        /// 抖音小游戏 视频广告id
        /// </summary>
        // public static string BYTE_DANCE_VIDEO_AD_ID => AppConfigAsset.Instance.BYTE_DANCE_VIDEO_AD_ID;

        /// <summary>
        /// 抖音小游戏 插页广告id
        /// </summary>
        // public static string BYTE_DANCE_INTERS_AD_ID => AppConfigAsset.Instance.BYTE_DANCE_INTERS_AD_ID;


        /// <summary>
        /// 版本号
        /// </summary>
        public static string VERSION => AppConfigAsset.Instance.VERSION;

        /// <summary>
        /// 构建类型
        /// </summary>
        public static BuildType BUILD_TYPE => AppConfigAsset.Instance.BUILD_TYPE;


        /// <summary>
        /// 保存时间
        /// </summary>
        public static string BUILD_DATE_TIME => AppConfigAsset.Instance.BUILD_DATE_TIME;


        /// <summary>
        /// 抖音小游戏 Channel Id
        /// </summary>
        public static int BYTE_DANCE_ANDROID_CHANNEL_ID => AppConfigAsset.Instance.BYTE_DANCE_ANDROID_CHANNEL_ID;

        /// <summary>
        /// 抖音小游戏 XSDK Channel Id
        /// </summary>
        public static int DOUYIN_XSDK_CHANNEL_ID => AppConfigAsset.Instance.DOUYIN_XSDK_CHANNEL_ID;

        /// <summary>
        /// 快手小游戏 XSDK Channel Id
        /// </summary>
        public static int KUAISHOU_XSDK_CHANNEL_ID => AppConfigAsset.Instance.KUAISHOU_XSDK_CHANNEL_ID;
        
        /// <summary>
        /// 快手小游戏 appid
        /// </summary>
        public static string KUAISHOU_APP_ID => AppConfigAsset.Instance.KUAISHOU_APP_ID;
        
        /// <summary>
        /// 快手小游戏，引力引擎开关
        /// </summary>
        public static bool KUAISHOU_GE_IS_ON => AppConfigAsset.Instance.KUAISHOU_GE_IS_ON;


        /// <summary>
        /// 快手小游戏，引力引擎 access token
        /// </summary>
        public static string KUAISHOU_GE_ACCESS_TOKEN => AppConfigAsset.Instance.KUAISHOU_GE_ACCESS_TOKEN;
        
        /// <summary>
        /// 快手小游戏，引力引擎 版本号
        /// </summary>
        public static int KUAISHOU_GE_VERSION => AppConfigAsset.Instance.KUAISHOU_GE_VERSION;
        
        /// <summary>
        /// 抖音小游戏，引力引擎开关
        /// </summary>
        public static bool BYTE_DANCE_GE_IS_ON => AppConfigAsset.Instance.BYTE_DANCE_GE_IS_ON;


        /// <summary>
        /// 抖音小游戏，引力引擎 access token
        /// </summary>
        public static string BYTE_DANCE_GE_ACCESS_TOKEN => AppConfigAsset.Instance.BYTE_DANCE_GE_ACCESS_TOKEN;


        /// <summary>
        /// 抖音小游戏，引力引擎 aes key
        /// </summary>
        public static string BYTE_DANCE_GE_AES_KEY => AppConfigAsset.Instance.BYTE_DANCE_GE_AES_KEY;


        /// <summary>
        /// 抖音小游戏，引力引擎 版本号
        /// </summary>
        public static int BYTE_DANCE_GE_VERSION => AppConfigAsset.Instance.BYTE_DANCE_GE_VERSION;
        
        /// <summary>
        /// 华为快游戏 XSDK Channel Id
        /// </summary>
        public static int HUAWEI_XSDK_CHANNEL_ID => AppConfigAsset.Instance.HUAWEI_XSDK_CHANNEL_ID;
        
        /// <summary>
        /// 华为快游戏 XSDK 版本号 预上线功能
        /// </summary>
        public static string HUAWEI_XSDK_VERSION_NAME => AppConfigAsset.Instance.HUAWEI_XSDK_VERSION_NAME;
        
        
        
        /// <summary>
        /// 华为快游戏 appid
        /// </summary>
        public static string HUAWEI_APP_ID => AppConfigAsset.Instance.HUAWEI_APP_ID;
        
        /// <summary>
        /// 华为快游戏，引力引擎开关
        /// </summary>
        public static bool HUAWEI_KUAIGAME_GE_IS_ON => AppConfigAsset.Instance.HUAWEI_KUAIGAME_GE_IS_ON;
        
        /// <summary>
        /// 华为快游戏 引力引擎access token
        /// </summary>
        public static string HUAWEI_KUAIGAME_GE_ACCESS_TOKEN => AppConfigAsset.Instance.HUAWEI_KUAIGAME_GE_ACCESS_TOKEN;
        
        /// <summary>
        /// 华为快游戏，引力引擎 版本号
        /// </summary>
        public static int HUAWEI_KUAIGAME_GE_VERSION => AppConfigAsset.Instance.HUAWEI_KUAIGAME_GE_VERSION;
        
        
        /// <summary>
        /// oppo小游戏 XSDK Channel Id
        /// </summary>
        public static int OPPO_XSDK_CHANNEL_ID => AppConfigAsset.Instance.OPPO_XSDK_CHANNEL_ID;
        
        /// <summary>
        /// vivo小游戏 XSDK Channel Id
        /// </summary>
        public static int VIVO_XSDK_CHANNEL_ID => AppConfigAsset.Instance.VIVO_XSDK_CHANNEL_ID;
        
        /// <summary>
        /// 微信小游戏 XSDK Channel Id
        /// </summary>
        public static int WX_XSDK_CHANNEL_ID => AppConfigAsset.Instance.WX_XSDK_CHANNEL_ID;
        
        
        /// <summary>
        /// B站小游戏 XSDK Channel Id
        /// </summary>
        public static int BILIBILI_XSDK_CHANNEL_ID => AppConfigAsset.Instance.BILIBILI_XSDK_CHANNEL_ID;
        
        /// <summary>
        /// B站小游戏 引力引擎access token
        /// </summary>
        public static string BILIBILI_GE_ACCESS_TOKEN => AppConfigAsset.Instance.BILIBILI_GE_ACCESS_TOKEN;
        
        /// <summary>
        /// B站小游戏 引力引擎 版本号
        /// </summary>
        public static int BILIBILI_GE_VERSION => AppConfigAsset.Instance.BILIBILI_GE_VERSION;
        
        /// <summary>
        /// B站小游戏，引力引擎开关
        /// </summary>
        public static bool BILIBILI_GE_IS_ON => AppConfigAsset.Instance.BILIBILI_GE_IS_ON;
        
        /// <summary>
        /// 微信小游戏 引力引擎access token
        /// </summary>
        public static string WX_GE_ACCESS_TOKEN => AppConfigAsset.Instance.WX_GE_ACCESS_TOKEN;
        
        /// <summary>
        /// 微信小游戏 引力引擎 版本号
        /// </summary>
        public static int WX_GE_VERSION => AppConfigAsset.Instance.WX_GE_VERSION;
        
        /// <summary>
        /// 微信小游戏，引力引擎开关
        /// </summary>
        public static bool WX_GE_IS_ON => AppConfigAsset.Instance.WX_GE_IS_ON;
        
    }
}