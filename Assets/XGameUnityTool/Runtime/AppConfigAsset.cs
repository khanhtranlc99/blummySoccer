using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace XGame
{
    public class AppConfigAsset : ScriptableObject
    {
        public static AppConfigAsset _instance;

        public static AppConfigAsset Instance
        {
            get
            {
                _instance = Resources.Load<AppConfigAsset>("XGameUnityTool_AppConfig");
                if (_instance == null)
                {
#if UNITY_EDITOR
                    var path = $"Assets/XGameUnityTool_Gen/Resources/XGameUnityTool_AppConfig.asset";
                    _instance = CreateInstance<AppConfigAsset>();
                    var dir = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        AssetDatabase.Refresh();
                    }

                    AssetDatabase.CreateAsset(_instance, path);
#endif
                }

                return _instance;
            }
        }

        /// <summary>
        /// 上架渠道
        /// </summary>
        public AppChannel CHANNEL = AppChannel.Mar;
        
        /// <summary>
        /// 版本号
        /// </summary>
        public string VERSION = "1.0.0";

        /// <summary>
        /// 构建类型
        /// </summary>
        public BuildType BUILD_TYPE = BuildType.Release;


        /// <summary>
        /// 保存时间
        /// </summary>
        public string BUILD_DATE_TIME = "230830";


        /// <summary>
        /// 抖音小游戏 XGP Channel Id
        /// </summary>
        public int BYTE_DANCE_ANDROID_CHANNEL_ID = -1;

        /// <summary>
        /// 抖音小游戏 XSDK Channel Id
        /// </summary>
        public int DOUYIN_XSDK_CHANNEL_ID = -1;

        /// <summary>
        /// 快手小游戏 XSDK Channel Id
        /// </summary>
        public int KUAISHOU_XSDK_CHANNEL_ID = -1;
        
        /// <summary>
        /// 快手小游戏 appId
        /// </summary>
        public string KUAISHOU_APP_ID = "";
        
        /// <summary>
        /// 快手小游戏，引力引擎开关
        /// </summary>
        public bool KUAISHOU_GE_IS_ON = true;
        
        /// <summary>
        /// 快手小游戏，引力引擎 access token
        /// </summary>
        public string KUAISHOU_GE_ACCESS_TOKEN = "";

        /// <summary>
        /// 快手小游戏，引力引擎 版本号
        /// </summary>
        public int KUAISHOU_GE_VERSION = 1;
        
        
        /// <summary>
        /// 抖音小游戏，引力引擎开关
        /// </summary>
        public bool BYTE_DANCE_GE_IS_ON = true;
        
        /// <summary>
        /// 抖音小游戏，引力引擎 access token
        /// </summary>
        public string BYTE_DANCE_GE_ACCESS_TOKEN = "";


        /// <summary>
        /// 抖音小游戏，引力引擎 aes key
        /// </summary>
        public string BYTE_DANCE_GE_AES_KEY = "";


        /// <summary>
        /// 抖音小游戏，引力引擎 版本号
        /// </summary>
        public int BYTE_DANCE_GE_VERSION = 0;
        
        
        /// <summary>
        /// 华为快游戏 XSDK Channel Id
        /// </summary>
        public int HUAWEI_XSDK_CHANNEL_ID = -1;
        /// <summary>
        /// 华为快游戏 XSDK versionName (预上线功能)
        /// </summary>
        public string HUAWEI_XSDK_VERSION_NAME = "";
        
        /// <summary>
        /// 华为快游戏 appId
        /// </summary>
        public string HUAWEI_APP_ID = "";
        
        
        /// <summary>
        /// 华为快游戏 引力引擎access token
        /// </summary>
        public string HUAWEI_KUAIGAME_GE_ACCESS_TOKEN = "";
        
        /// <summary>
        /// 华为快游戏 引力引擎 版本号
        /// </summary>
        public int HUAWEI_KUAIGAME_GE_VERSION = 1;
        
        /// <summary>
        /// 华为快游戏，引力引擎开关
        /// </summary>
        public bool HUAWEI_KUAIGAME_GE_IS_ON = false;
        
        /// <summary>
        /// 微信小游戏 XSDK Channel Id
        /// </summary>
        public int WX_XSDK_CHANNEL_ID = -1;
        
        /// <summary>
        /// VIVO小游戏 XSDK Channel Id
        /// </summary>
        public int VIVO_XSDK_CHANNEL_ID = -1;
        
        /// <summary>
        /// OPPO小游戏 XSDK Channel Id
        /// </summary>
        public int OPPO_XSDK_CHANNEL_ID = -1;
        
        /// <summary>
        /// B站小游戏 XSDK Channel Id
        /// </summary>
        public int BILIBILI_XSDK_CHANNEL_ID = -1;
        
        /// <summary>
        /// B站小游戏 引力引擎access token
        /// </summary>
        public string BILIBILI_GE_ACCESS_TOKEN = "";
        
        /// <summary>
        /// B站小游戏 引力引擎 版本号
        /// </summary>
        public int BILIBILI_GE_VERSION = 1;
        
        /// <summary>
        /// B站小游戏，引力引擎开关
        /// </summary>
        public bool BILIBILI_GE_IS_ON = false;
        
        
        /// <summary>
        /// 微信小游戏 引力引擎access token
        /// </summary>
        public string WX_GE_ACCESS_TOKEN = "";
        
        /// <summary>
        /// 微信小游戏 引力引擎 版本号
        /// </summary>
        public int WX_GE_VERSION = 1;
        
        /// <summary>
        /// 微信小游戏，引力引擎开关
        /// </summary>
        public bool WX_GE_IS_ON = false;
        
    }
}