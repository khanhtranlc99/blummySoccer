using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XGame
{
    /// <summary>
    /// 代理，请求订阅或者非消耗商品信息回调
    /// </summary>
    /// <param name="success">是否成功</param>
    /// <param name="states">商品情况</param>
    public delegate void RequestSubscribeStatesResult(bool success, SubscribeState[] states);

    /// <summary>
    /// 代理，请求商品信息回调
    /// </summary>
    /// <param name="success">是否成功</param>
    /// <param name="infos">商品信息</param>
    public delegate void RequestProductInfoResult(bool success, ProductInfo[] infos);

    /// <summary>
    /// xgame sdk 管理器封装类
    /// </summary>
    [Preserve]
    public class XGameSdk : MonoBehaviour
    {
        //SDK绑定数据
        [Preserve]
        private class SDKBindingItem
        {
            public string ClassFullName; //实例类名
            public bool IsSupportCloudArchive; //是否支持云存档
            public string Abbreviation; //缩写，用于组合版本号

            public SDKBindingItem(string classFullName, bool isSupportCloudArchive, string abbreviation)
            {
                ClassFullName = classFullName;
                IsSupportCloudArchive = isSupportCloudArchive;
                Abbreviation = abbreviation;
            }
        }

        //SDK绑定
        private static Dictionary<AppChannel, SDKBindingItem> _SdkBindings =
            new Dictionary<AppChannel, SDKBindingItem>()
            {
                //微信
                // { AppChannel.WeChat, new SDKBindingItem("XGame.WXSdk", true, "w") },
                // { AppChannel.WeChat_ASC, new SDKBindingItem("XGame.WXASCSdk", false, "w2") },
                { AppChannel.WeChat_XSDK, new SDKBindingItem("XGame.WXXSDK", true, "w3") },
                //VIVO
                // { AppChannel.VivoMini, new SDKBindingItem("XGame.VivoMiniSdk", true, "v") },
                // { AppChannel.Vivo_ASC, new SDKBindingItem("XGame.VivoAscSdk", false, "v2") },
                { AppChannel.Vivo_XSDK, new SDKBindingItem("XGame.VivoXSDK", true, "v3") },
                //OPPO
                // { AppChannel.OppoMini, new SDKBindingItem("XGame.OppoMiniSdk", true, "o") },
                // { AppChannel.Oppo_ASC, new SDKBindingItem("XGame.OppoAscSdk", false, "o2") },
                { AppChannel.Oppo_XSDK, new SDKBindingItem("XGame.OppoXSDK", true, "o3") },
                //GOOGLE
                // { AppChannel.Google, new SDKBindingItem("XGame.XAGoogleSdk", false, "g") },
                { AppChannel.XMYGoogle, new SDKBindingItem("XGame.XMYGoogleSdk", false, "g2") },
                { AppChannel.Google_Log_SDK, new SDKBindingItem("XGame.GoogleLogSdk", false, "g3") },

                //国内硬核
                { AppChannel.Mar, new SDKBindingItem("XGame.MarSdk", false, "a") },
                { AppChannel.Android_Light, new SDKBindingItem("XGame.LightSdk", false, "a2") },
                //抖音
                // { AppChannel.ByteDanceMiniGame, new SDKBindingItem("XGame.ByteDanceSdk", true, "d") },
                // { AppChannel.ByteDanceMiniGameIOS, new SDKBindingItem("XGame.ByteDanceSdk", true, "di") },
                { AppChannel.Douyin_XSDK_Android, new SDKBindingItem("XGame.DouyinXSDK", true, "d2") },
                { AppChannel.Douyin_XSDK_IOS, new SDKBindingItem("XGame.DouyinXSDK", true, "di2") },
                //IOS
                // { AppChannel.IOSInland, new SDKBindingItem("XGame.IOSSdkInstance", false, "ii") },
                // { AppChannel.IOSOverseas, new SDKBindingItem("XGame.IOSSdkInstance", false, "io") },
                { AppChannel.IOS_XGUG_China, new SDKBindingItem("XGame.IOSXGUGSdk", false, "ixc") },
                { AppChannel.IOS_XGUG_Sea, new SDKBindingItem("XGame.IOSXGUGSdk", false, "ixs") },

                //快手
                { AppChannel.Kuaishou_XSDK_Android, new SDKBindingItem("XGame.KuaishouXSDK", true, "k") },
                { AppChannel.Kuaishou_XSDK, new SDKBindingItem("XGame.KuaishouWebXSDK", true, "k2") },

                //华为
                { AppChannel.Huawei_XSDK, new SDKBindingItem("XGame.HuaweiXSDK", true, "h") },
                { AppChannel.OpenHarmony_Light, new SDKBindingItem("XGame.HarmonyLightSdk", false, "h2") },


                //b站
                { AppChannel.Bilibili_XSDK, new SDKBindingItem("XGame.BilibiliXSDK", true, "b") },

                //测试
                // { AppChannel.Test, new SDKBindingItem("XGame.XGameTestSdk", false, "t") },
            };


        #region Sdk Class 名

        //支付回调类
        private const string PAY_RESULT_CLASS_NAME = "XGame.XGameSdkPayResult";

        //兑换回调类
        private const string GIFT_RESULT_CLASS_NAME = "XGame.XGameSdkGiftResult";

        #endregion

        #region 静态事件

        /// <summary>
        /// 激励视频奖励成功时触发
        /// </summary>
        public static event Action OnVideoRewardSuccessEvent;

        /// <summary>
        /// 当IOS端口暂停游戏时触发
        /// true ：暂停
        /// false ：继续
        /// </summary>
        public static event Action<bool> OnIOSPauseGameEvent;

        // /// <summary>
        // ///  字节sdk录制完成时触发
        // /// </summary>
        // public static event Action OnRecordCompleteEvent;


        /// <summary>
        /// 云存档开始同步
        /// </summary>
        public static event Action OnCloudArchiveSyncBeginEvent;

        /// <summary>
        /// 云存档结束同步
        /// </summary>
        public static event Action OnCloudArchiveSyncEndEvent;


        /// <summary>
        /// 触发事件-云存档开始同步
        /// </summary>
        public static void InvokeOnCloudArchiveSyncBegin()
        {
            OnCloudArchiveSyncBeginEvent?.Invoke();
        }

        /// <summary>
        /// 触发事件-云存档结束同步
        /// </summary>
        public static void InvokeOnCloudArchiveSyncEnd()
        {
            OnCloudArchiveSyncEndEvent?.Invoke();
        }

        #endregion

        #region 日志

        //日志模式
#if UNITY_EDITOR
        public static bool LOG_MODE = true;
#else
        public static bool LOG_MODE;
#endif

        /// <summary>
        /// 打印日志
        /// </summary>
        public static void Log(string conent)
        {
            if (LOG_MODE)
            {
                Debug.Log($"[XGAME_UNITY] {conent}");
            }
        }

        #endregion

        private static XGameSdk _instance;
        private static object _lock = new object();


        /// <summary>
        /// 单例对象实例
        /// </summary>
        public static XGameSdk Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            GameObject clone = new GameObject("XgameSdk");
                            _instance = clone.AddComponent<XGameSdk>();
                            _instance.Init();
                        }
                    }
                }

                return _instance;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        ///  本机测试参数
        /// </summary>
        private SdkPreference _sdkPreference;
#endif
        /// <summary>
        /// 设置用户属性功能开关
        /// </summary>
        public static bool UserPropertyIsEnable = true;

        //视频成功代理
        private Action _videoSuccess;

        //视频失败代理
        private Action _videoFail;

        //插页关闭
        private Action _intersClose;

        //插屏展示
        private Action _interOnShow;

        //sdk 实例
        private BaseSdk _sdk;

        private BaskSdkListener _sdkListener;

        //本地云存档模拟器
        private LocalHostCloudArchiveServer _localHostCloudArchiveServer;

        //是否有缓存的ip地址
        // private bool _hasCacheIp = false;

        //ip地址
        // private string _ip;

        /// <summary>
        /// 当前渠道
        /// </summary>
        public static AppChannel Channel => AppConfig.CHANNEL;

        private XHttpClient _httpClient;

        private XHttpClient XHttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = XHttpClient.CreateInstance();
                    DontDestroyOnLoad(_httpClient.gameObject);
                }

                return _httpClient;
            }
        }

        //是否处理开发阶段
        private bool IsDevelopment
        {
            get
            {
#if UNITY_EDITOR
                return true;
#endif
                return false;
            }
        }

        #region 构建

        // /// <summary>
        // ///  构建实例
        // /// </summary>
        // public void Build()
        // {
        // }

        private void Init()
        {
            DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
            //加载测试参数
            _sdkPreference = SdkPreference.Global;
            if (_sdkPreference == null)
            {
                throw new Exception("找不到 SdkPreference 设置，请点击 \"XGame/设置\" 进行部署");
            }
#endif
            //初始化本地云存档服务器
            InitializeLocalHostCloudArchiveServer();
            Debug.Log($"[XGameSdk] Channel:{AppConfig.CHANNEL}");
            TryOpenDebugMode();
            BuildSdkInstance();
        }


        private void TryOpenDebugMode()
        {
#if UNITY_EDITOR
            return;
#endif
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            //开启动态debug
            var dynamicDebug = false;
            try
            {
                var dir = new DirectoryInfo(Application.persistentDataPath);
                var root = dir.Parent.Parent.Parent.Parent;
                var path = $"{root.FullName}/xgamesdk_debug.bin";
                dynamicDebug = File.Exists(path);
            }
            finally
            {
            }

            if (File.Exists($"{Application.persistentDataPath}/xgamesdk_debug.bin") || dynamicDebug)
            {
                Debug.Log("发现xgamesdk_debug.bin or xgamesdk_debug.txt 开启动态调试模式");
                LOG_MODE = true;
            }
        }

        #endregion

        #region 获取Type

        private Type GetType(string typeName)
        {
            Type result = Type.GetType(typeName);
            if (result != null)
            {
                return result;
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 是否有实例
        /// </summary>
        public static bool HasInstance => _instance != null;

        /// <summary>
        /// 置空实例
        /// </summary>
        public static void SetInstanceNull()
        {
            _instance = null;
        }

        //触发ios暂停游戏事件
        public static void InvokeIOSPauseEvent(bool isPause)
        {
            OnIOSPauseGameEvent?.Invoke(isPause);
        }

        #endregion

        #region 反射构建实例

        private T CreateInstance<T>(string className) where T : class
        {
            Type type = GetType(className);
            //带一个参数的构造函数
            var instance = Activator.CreateInstance(type);
            var t = instance as T;
            if (t == null)
            {
                throw new Exception($"{className} 不可转为 {typeof(T).Name},请检查是否引入{className}");
            }

            return t;
        }


        //构建SDK实例
        private void BuildSdkInstance()
        {
#if UNITY_EDITOR
            var payRest = CreateInstance<IOnPayResult>(PAY_RESULT_CLASS_NAME); //创建支付回调
            var giftRes = CreateInstance<IOnGiftResult>(GIFT_RESULT_CLASS_NAME); //创建兑换回调
            _sdkListener = new BaskSdkListener(payRest, giftRes);
            return;
#endif
            if (_SdkBindings.TryGetValue(Channel, out var match))
            {
                var className = match.ClassFullName;
                var payResult = CreateInstance<IOnPayResult>(PAY_RESULT_CLASS_NAME); //创建支付回调
                var giftResult = CreateInstance<IOnGiftResult>(GIFT_RESULT_CLASS_NAME); //创建兑换回调
                _sdkListener = new BaskSdkListener(payResult, giftResult);
                var type = GetTypeByFullName(className);
                Debug.Log($"[XGAME_UNITY_TOOL] 构建sdk实例 {type}");
                _sdk = Activator.CreateInstance(type) as BaseSdk;
                //绑定回调
                _sdk.OnCreate(_sdkListener);
            }
            else
            {
                Debug.LogError($"未绑定SDK Class Channel:{Channel} 采用默认实现：DemoSdk");
                _sdk = new DemoSdk();
                //绑定回调
                _sdk.OnCreate(_sdkListener);
            }
        }


        private static Type GetTypeByFullName(string fullName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(fullName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        #endregion

        #region 初始化sdk

        /// <summary>
        /// 初始化sdk
        /// </summary>
        public void InitSdk(Action success, Action fail = null)
        {
#if UNITY_EDITOR
            Debug.Log($"[SDK调用成功] InitSdk 编辑器下直接触发成功");
            success?.Invoke();
            return;
#endif
            _sdk?.InitSdk(success, fail);
        }

        #endregion

        #region 登录

        /// <summary>
        /// 谷歌登录
        /// </summary>
        /// <param name="success">成功回调，第一个参数是用户ID，第二个参数是用户名</param>
        /// <param name="fail">失败回调</param>
        public void LoginGoogle(Action<string /*用户id*/, string /*用户名*/> success = null, Action fail = null)
        {
#if UNITY_EDITOR
            Debug.Log($"[SDK调用成功] LoginGoogle 编辑器下直接触发成功");
            var userid = "12345353w4";
            var username = "sufjbsjf";
            success?.Invoke(userid, username);
            return;
#endif
            _sdk?.LoginGoogle(success, fail);
        }

        /// <summary>
        /// 登录,必接
        /// </summary>
        /// <param name="success">成功回调</param>
        /// <param name="fail">失败回调</param>
        public void Login(Action success = null, Action fail = null)
        {
#if UNITY_EDITOR
            Debug.Log($"[SDK调用成功] Login  编辑器下直接触发成功");
            success?.Invoke();
            return;
#endif
            _sdk?.Login(success, fail);
        }

        #endregion

        #region 视频广告

        /// <summary>
        /// 播放视频广告
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XA),Google(XMY),Mar</font><para/>
        /// <font color="#4dd276">IOS(国内),IOS(海外),IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),vivo小游戏(XGP),vivo小游戏(ASC),oppo小游戏(XGP),oppo小游戏(ASC)</font><para/>
        /// </summary>
        /// <param name="sceneName">广告场景</param>
        /// <param name="success">成功回调</param>
        /// <param name="fail">失败回调</param>
        public void ShowVideo(string sceneName, Action success, Action fail = null)
        {
            _videoSuccess = success;
            _videoFail = fail;
#if UNITY_EDITOR

            //本地测试
            if (_sdkPreference.VideoWatchSuccess)
            {
                Debug.Log(
                    $"[SDK调用成功] ShowVideo  sceneName:{sceneName} （XGameUnityTool/Channel Config->Test params）VideoWatchSuccess={_sdkPreference.VideoWatchSuccess} 模拟成功！");
                InvokeVideoSuccess();
            }
            else
            {
                Debug.Log(
                    $"[SDK调用成功] ShowVideo  sceneName:{sceneName} （XGameUnityTool/Channel Config->Test params）VideoWatchSuccess={_sdkPreference.VideoWatchSuccess} 模拟触发失败！");
                InvokeVideoFail();
            }

            return;
#endif
            _sdk?.ShowVideo(sceneName, success, fail);
        }

        [Obsolete("已过时，请改用GetVideoFlag")]
        public bool HasVideo()
        {
            Debug.Log($"[SDK调用成功] HasVideo 已过时 请改用 GetVideoFlag");
            return GetVideoFlag();
        }

        /// <summary>
        /// 激励广告是否加载完毕
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XA),Google(XMY),Mar</font><para/>
        /// <font color="#4dd276">IOS(国内),IOS(海外),IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),vivo小游戏(XGP),vivo小游戏(ASC),oppo小游戏(XGP),oppo小游戏(ASC)</font><para/>
        /// </summary>
        public bool GetVideoFlag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetVideoFlag 返回Test params（XGameUnityTool/Channel Config） GetVideoFlag={_sdkPreference.GetVideoFlag}");
            return _sdkPreference.GetVideoFlag;
#endif
            return _sdk?.HasVideo() ?? false;
        }

        public bool IsClickTriggerVideo()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] IsClickTriggerVideo");
            return false;
#endif
            return _sdk?.IsClickTriggerVideo() ?? false;
        }

        //视频回调
        private void OnRewardVideoCallBack(bool success)
        {
            if (success)
            {
                InvokeVideoSuccess();
            }
            else
            {
                InvokeVideoFail();
            }
        }

        //触发视频成功回调
        private void InvokeVideoSuccess()
        {
            _videoSuccess?.Invoke();
            OnVideoRewardSuccessEvent?.Invoke();
        }

        //触发视频失败回调
        private void InvokeVideoFail()
        {
            _videoFail?.Invoke();
        }

        #endregion

        #region 插页广告

        [Obsolete("已过时，请改用GetIntersFlag")]
        public bool HasInters()
        {
            Debug.Log($"[SDK调用成功] HasInters 已过时 请改用 GetIntersFlag");
            return GetIntersFlag();
        }

        /// <summary>
        /// 插页广告是否加载完毕
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XA),Google(XMY),Mar</font><para/>
        /// <font color="#4dd276">IOS(国内),IOS(海外),IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),vivo小游戏(XGP),vivo小游戏(ASC),oppo小游戏(XGP),oppo小游戏(ASC)</font><para/>
        /// </summary>
        public bool GetIntersFlag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetIntersFlag 返回Test params（XGameUnityTool/Channel Config）GetIntersFlag={_sdkPreference.GetIntersFlag}");
            return _sdkPreference.GetIntersFlag;
#endif
            return _sdk?.HasInters() ?? false;
        }

        /// <summary>
        /// 插页广告当前是否允许展示（频控/策略等，与是否加载完成不同，请对比 <see cref="GetIntersFlag"/>）
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// </summary>
        public bool CanShowInters()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] CanShowInters 返回Test params（XGameUnityTool/Channel Config）CanShowInters={_sdkPreference.CanShowInters}");
            return _sdkPreference.CanShowInters;
#endif
            return _sdk?.CanShowInters() ?? true;
        }

        /// <summary>
        /// 显示插页广告
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XA),Google(XMY),Mar</font><para/>
        /// <font color="#4dd276">IOS(国内),IOS(海外),IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),vivo小游戏(XGP),vivo小游戏(ASC),oppo小游戏(XGP),oppo小游戏(ASC)</font><para/>
        /// </summary>
        /// <param name="sceneName">广告场景名</param>
        /// <param name="onClose">关闭回调,支持Google(XA),Google(XMY)</param>
        public void ShowInters(string sceneName = "unknown", Action onClose = null)
        {
            //设置展示回调
            _interOnShow = null;
            //设置关闭回调
            _intersClose = onClose;
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowInters sceneName:{sceneName} ");
            _intersClose?.Invoke();
            return;
#endif
            _sdk?.ShowInters(sceneName, onClose);
        }

        public void ShowInterstitial(string sceneName, Action onShow, Action onClose = null)
        {
            //设置展示回调
            _interOnShow = onShow;
            //设置关闭回调
            _intersClose = onClose;
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowInterstitial sceneName:{sceneName} ");
            _interOnShow?.Invoke();
            _intersClose?.Invoke();
            return;
#endif
            _sdk?.ShowInterstitial(sceneName, onShow, onClose);
        }


        /// <summary>
        /// 拓展回调
        /// </summary>
        /// <param name="code">回调码</param>
        /// <param name="data">回调数据</param>
        private void OnExtraCallBack(CallBackCode code, string data)
        {
            switch (code)
            {
                case CallBackCode.IntersOnClose: //插页关闭
                    _intersClose?.Invoke();
                    break;
            }
        }

        #endregion

        #region Banner

        /// <summary>
        /// 展示banner<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XA),Google(XMY),Mar</font><para/>
        /// <font color="#4dd276">IOS(国内),IOS(海外),IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),vivo小游戏(XGP),vivo小游戏(ASC),oppo小游戏(XGP),oppo小游戏(ASC)</font><para/>
        /// </summary>
        /// <param name="bannerType">banner条样式</param>
        public void ShowBanner(BannerType bannerType)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowBanner bannerType:{bannerType}");
            return;
#endif
            _sdk?.ShowBanner(bannerType);
        }

        /// <summary>
        /// 隐藏banner<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XA),Google(XMY),Mar</font><para/>
        /// <font color="#4dd276">IOS(国内),IOS(海外),IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),vivo小游戏(XGP),vivo小游戏(ASC),oppo小游戏(XGP),oppo小游戏(ASC)</font><para/>
        /// </summary>
        public void HideBanner()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HideBanner");
            return;
#endif
            _sdk?.HideBanner();
        }

        #endregion

        #region 手机震动

        public void PhoneVibrate(string type)
        {
#if UNITY_EDITOR
            Log("[本地模拟] PhoneVibrate 调用成功！");
            return;
#endif
            _sdk?.PhoneVibrate(type);
        }

        #endregion

        #region 原生广告

        public bool GetNativeFlag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetNativeFlag 返回Test params（XGameUnityTool/Channel Config）GetNativeFlag=={_sdkPreference.GetNativeFlag}");
            return _sdkPreference.GetNativeFlag;
#endif
            return _sdk?.HasNative() ?? false;
        }

        public void ShowNativeAd(string scene, Action onClose = null)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowNativeAd scene:{scene}");
            onClose?.Invoke();
            return;
#endif
            if (onClose == null)
            {
                _sdk?.ShowNativeAd(scene);
            }
            else
            {
                _sdk?.ShowNativeAd(scene, onClose);
            }
        }

        /// <summary>
        /// 隐藏原生广告
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY),Mar</font><para/>
        /// </summary>
        public void HideNative()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HideNative");
            return;
#endif
            _sdk?.HideNative();
        }

        #endregion

        #region 原生大图

        [Obsolete("已过时,请改用GetBigNativeFlag")]
        public bool HasBigNative()
        {
            Debug.Log($"[SDK调用成功] HasBigNative 已过时,请改用GetBigNativeFlag");
            return GetBigNativeFlag();
        }


        /// <summary>
        /// 原生大图是否加载flag<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY),Mar</font><para/>
        /// </summary>
        public bool GetBigNativeFlag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetBigNativeFlag 返回Test params（XGameUnityTool/Channel Config）GetBigNativeFlag=={_sdkPreference.GetBigNativeFlag}");
            return _sdkPreference.GetBigNativeFlag;
#endif
            return _sdk?.HasBigNative() ?? false;
        }


        /// <summary>
        /// 展示原生大图
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY),Mar</font><para/>
        /// <font color="red">已过时</font><para/>
        /// </summary>
        [Obsolete("已过时,请使用ShowBigNative(string scene)")]
        public void ShowBigNative()
        {
#if UNITY_EDITOR
            Debug.Log($"[SDK调用成功] ShowBigNative() 已过时,请改用 ShowBigNative(string scene)");
            return;
#endif
            ShowBigNative("unknown");
        }

        /// <summary>
        /// 隐藏原生大图
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY),Mar</font><para/>
        /// </summary>
        public void HideBigNative()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HideBigNative");
            return;
#endif
            _sdk?.HideBigNative();
        }

        /// <summary>
        /// 展示原生大图
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY),Mar</font><para/>
        /// </summary>
        /// <param name="scene">广告场景</param>
        public void ShowBigNative(string scene)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowBigNative scene:{scene}");
            return;
#endif
            _sdk?.ShowBigNative(scene);
        }

        #endregion

        #region 悬浮窗广告

        /// <summary>
        /// 显示悬浮ICON广告
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Mar</font><para/>
        /// </summary>
        /// <param name="posX">左上角x坐标</param>
        /// <param name="posY">左上角y坐标</param>
        public void ShowFloatIconAd(float posX, float posY)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowFloatIconAd posX:{posX} posY:{posY}");
            return;
#endif
            _sdk?.ShowFloatIconAd(posX, posY);
        }

        /// <summary>
        /// 隐藏悬浮ICON广告
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Mar</font><para/>
        /// </summary>
        public void HideFloatIcon()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HideFloatIcon ");
            return;
#endif
            _sdk?.HideFloatIcon();
        }

        #endregion

//         #region 自定义广告
//
//         /// <summary>
//         ///  显示自定义广告v1<para/>
//         /// x,y代表的百分比位置，（0，0）代表左上角
//         /// 广告第一次展示确定位置后，后续显示切换位置不会生效
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// <font color="red">逐步废弃</font><para/> 
//         /// </summary>
//         /// <param name="type">广告类型（问运营组同学要）</param>
//         /// <param name="x">x轴百分比位置，取值[0~1]</param>
//         /// <param name="y">y轴百分比位置，取值[0~1]</param>
//         [Obsolete("逐步废弃，请改用ShowCustomAdPos")]
//         public void ShowCustomAd(int type, float x, float y)
//         {
// #if UNITY_EDITOR
//             Debug.Log($"[SDK调用成功] ShowCustomAd 逐步废弃，请改用ShowCustomAdPos");
//             return;
// #endif
//             _sdk?.ShowCustomAd(type, x, y);
//         }
//
//         /// <summary>
//         /// 隐藏自定义广告v1<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// <font color="red">逐步废弃</font><para/> 
//         /// </summary>
//         /// <param name="type">广告类型（问运营组同学要）</param>
//         [Obsolete("逐步废弃，请改用HideCustomAdPos")]
//         public void HideCustomAd(int type)
//         {
// #if UNITY_EDITOR
//             Debug.Log($"[SDK调用成功] HideCustomAd 逐步废弃，请改用HideCustomAdPos");
//             return;
// #endif
//             _sdk?.HideCustomAd(type);
//         }
//
//         /// <summary>
//         /// 显示自定义广告v2<para/>
//         /// 第一次显示位置会锁定，后续无法更改
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// </summary>
//         /// <param name="sceneId">广告位ID,具体由后台配置(问运营同学要)</param>
//         /// <param name="pivotX">广告描点X：取值范围[0~1]</param>
//         /// <param name="pivotY">广告描点Y：取值范围[0~1]</param>
//         /// <param name="x">屏幕位置X（百分比）取值范围[0~1]</param>
//         /// <param name="y">屏幕位置Y（百分比）取值范围[0~1]</param>
//         public void ShowCustomAdPos(string sceneId, float pivotX, float pivotY, float x, float y)
//         {
// #if UNITY_EDITOR
//             Debug.Log($"[SDK调用成功] ShowCustomAdPos sceneId:{sceneId} pivotX:{pivotX} pivotY：{pivotY} x：{x} y:{y}");
//             return;
// #endif
//             _sdk?.ShowCustomAdPos(sceneId, pivotX, pivotY, x, y);
//         }
//
//         /// <summary>
//         /// 显示自定义广告v2<para/>
//         /// 第一次显示位置会锁定，后续无法更改
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// </summary>
//         /// <param name="sceneId">广告位ID,具体由后台配置(问运营同学要)</param>
//         /// <param name="pivot">常用广告描点枚举</param>
//         /// <param name="x">屏幕位置X（百分比）取值范围[0~1]</param>
//         /// <param name="y">屏幕位置Y（百分比）取值范围[0~1]</param>
//         public void ShowCustomAdPos(string sceneId, PivotType pivot, float x, float y)
//         {
//             var pivotX = 0f;
//             var pivotY = 0f;
//             switch (pivot)
//             {
//                 case PivotType.UpperLeft: //左上
//                     pivotX = 0;
//                     pivotY = 0;
//                     break;
//                 case PivotType.UpperCenter: //中上
//                     pivotX = 0.5f;
//                     pivotY = 0;
//                     break;
//                 case PivotType.UpperRight: //右上
//                     pivotX = 1f;
//                     pivotY = 0;
//                     break;
//                 case PivotType.MiddleLeft: //左中
//                     pivotX = 0f;
//                     pivotY = 0.5f;
//                     break;
//                 case PivotType.MiddleCenter: //中
//                     pivotX = 0.5f;
//                     pivotY = 0.5f;
//                     break;
//                 case PivotType.MiddleRight: //右中
//                     pivotX = 1f;
//                     pivotY = 0.5f;
//                     break;
//                 case PivotType.BottomLeft: //左下
//                     pivotX = 0f;
//                     pivotY = 1f;
//                     break;
//                 case PivotType.BottomCenter: //中下
//                     pivotX = 0.5f;
//                     pivotY = 1f;
//                     break;
//                 case PivotType.BottomRight: //右下
//                     pivotX = 1f;
//                     pivotY = 1f;
//                     break;
//                 default:
//                     throw new Exception($"未处理的类型:{pivot}");
//             }
//
//             ShowCustomAdPos(sceneId, pivotX, pivotY, x, y);
//         }
//
//         /// <summary>
//         ///  隐藏自定义广告v2<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// </summary>
//         /// <param name="sceneId">广告位ID,具体由后台配置(问运营同学要)</param>
//         public void HideCustomAdPos(string sceneId)
//         {
// #if UNITY_EDITOR
//             Debug.Log($"[SDK调用成功] HideCustomAdPos sceneId:{sceneId} ");
//             return;
// #endif
//             _sdk?.HideCustomAdPos(sceneId);
//         }
//
//         #endregion

        #region 模板广告

        /// <summary>
        /// 模板广告是否加载完毕(可展示)<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// </summary>
        public bool GetTemplateAdFlag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetTemplateAdFlag 返回Test params（XGameUnityTool/Channel Config） GetTemplateAdFlag={_sdkPreference.GetTemplateAdFlag}");
            return _sdkPreference.GetTemplateAdFlag;
#endif
            return _sdk?.GetTemplateAdFlag() ?? false;
        }


        /// <summary>
        /// 展示模板广告<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// </summary>
        /// <param name="scene">广告场景</param>
        public void ShowTemplateAd(string scene)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowTemplateAd scene：{scene}");
            return;
#endif
            _sdk?.ShowTemplateAd(scene);
        }

        /// <summary>
        /// 隐藏模板广告<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// </summary>
        public void HideTemplateAd()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HideTemplateAd");
            return;
#endif
            _sdk?.HideTemplateAd();
        }

        #endregion

        #region 单格子广告

        /// <summary>
        /// 单格子广告flag<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        public bool GetNativeIconFlag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetNativeIconFlag 返回Test params（XGameUnityTool/Channel Config）GetNativeIconFlag={_sdkPreference.GetNativeIconFlag} ");
            return _sdkPreference.GetNativeIconFlag;
#endif
            return _sdk?.GetNativeIconFlag() ?? false;
        }

        /// <summary>
        /// 展示单格子广告<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="scene">广告场景</param>
        /// <param name="spx">屏幕距离左上角x轴百分比位置取值范围：[0,1]</param>
        /// <param name="spy">屏幕距离左上角y轴百分比位置取值范围：[0,1]</param>
        public void ShowNativeIcon(string scene, float spx, float spy)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowNativeIcon scene:{scene} spx:{spx} spy:{spy} ");
            return;
#endif
            _sdk?.ShowNativeIcon(scene, spx, spy);
        }

        /// <summary>
        /// 隐藏单格子广告<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        public void HideNativeIcon()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HideNativeIcon ");
            return;
#endif
            _sdk?.HideNativeIcon();
        }

        #endregion

        #region 单格子广告2

        /// <summary>
        /// 单格子广告2 flag<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        public bool GetNativeIcon2Flag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetNativeIcon2Flag 返回Test params（XGameUnityTool/Channel Config）GetNativeIconFlag={_sdkPreference.GetNativeIconFlag} ");
            return _sdkPreference.GetNativeIconFlag;
#endif
            return _sdk?.GetNativeIcon2Flag() ?? false;
        }

        /// <summary>
        /// 展示单格子广告2<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="scene">广告场景</param>
        /// <param name="spx">屏幕距离左上角x轴百分比位置取值范围：[0,1]</param>
        /// <param name="spy">屏幕距离左上角y轴百分比位置取值范围：[0,1]</param>
        public void ShowNativeIcon2(string scene, float spx, float spy)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowNativeIcon2 scene:{scene} spx:{spx} spy:{spy} ");
            return;
#endif
            _sdk?.ShowNativeIcon2(scene, spx, spy);
        }

        /// <summary>
        /// 隐藏单格子广告2<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        public void HideNativeIcon2()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HideNativeIcon2 ");
            return;
#endif
            _sdk?.HideNativeIcon2();
        }

        #endregion

        #region 多格子广告

        /// <summary>
        /// 多格子广告flag<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        public bool GetBlockFlag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetBlockFlag 返回Test params（XGameUnityTool/Channel Config）GetBlockFlag={_sdkPreference.GetBlockFlag} ");
            return _sdkPreference.GetBlockFlag;
#endif
            return _sdk?.GetBlockFlag() ?? false;
        }

        /// <summary>
        /// 展示多格子广告<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="scene">广告场景</param>
        public void ShowBlock(string scene)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ShowBlock scene:{scene}");
            return;
#endif
            _sdk?.ShowBlock(scene);
        }

        /// <summary>
        /// 隐藏多格子广告<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        public void HideBlock()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HideBlock");
            return;
#endif
            _sdk?.HideBlock();
        }

        #endregion

//         #region 互推盒子Banner
//
//         /// <summary>
//         /// 互推盒子Banner flag<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(ASC),vivo小游戏(ASC),oppo小游戏(ASC)</font><para/>
//         /// </summary>
//         public bool GetNavigateBoxBannerFlag()
//         {
// #if UNITY_EDITOR
//             Log(
//                 $"[SDK调用成功] GetNavigateBoxBannerFlag 返回Test params（XGameUnityTool/Channel Config）GetNavigateBoxBannerFlag={_sdkPreference.GetNavigateBoxBannerFlag} ");
//             return _sdkPreference.GetNavigateBoxBannerFlag;
// #endif
//             return _sdk?.GetNavigateBoxBannerFlag() ?? false;
//         }
//
//         /// <summary>
//         /// 展示互推盒子Banner<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(ASC),vivo小游戏(ASC),oppo小游戏(ASC)</font><para/>
//         /// </summary>
//         /// <param name="scene">广告场景</param>
//         public void ShowNavigateBoxBanner(string scene)
//         {
// #if UNITY_EDITOR
//             Log(
//                 $"[SDK调用成功] ShowNavigateBoxBanner scene:{scene} ");
//             return;
// #endif
//             _sdk?.ShowNavigateBoxBanner(scene);
//         }
//
//         /// <summary>
//         /// 隐藏互推盒子Banner<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(ASC),vivo小游戏(ASC),oppo小游戏(ASC)</font><para/>
//         /// </summary>
//         public void HideNavigateBoxBanner()
//         {
// #if UNITY_EDITOR
//             Log(
//                 $"[SDK调用成功] HideNavigateBoxBanner ");
//             return;
// #endif
//             _sdk?.HideNavigateBoxBanner();
//         }
//
//         #endregion

        #region 贴片广告

        /// <summary>
        /// 贴片广告加载状态(可展示)<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// <font color="#4dd276">IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// </summary>
        public bool GetPatchAdFlag()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetPatchAdFlag 返回Test params（XGameUnityTool/Channel Config->Test params）GetPatchAdFlag={_sdkPreference.GetPatchAdFlag} ");
            return _sdkPreference.GetPatchAdFlag;
#endif
            return _sdk?.GetPatchAdFlag() ?? false;
        }

        /// <summary>
        /// 展示贴片广告<para/>
        /// 左上角为(0,0),右下角(1,1)<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// <font color="#4dd276">IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// </summary>
        /// <param name="scene">场景名</param>
        /// <param name="type">贴片类型 问运营同学</param>
        /// <param name="xNormalize">左上角x,范围：[0,1]</param>
        /// <param name="yNormalize">左上角y,范围：[0,1]</param>
        /// <param name="widthNormalize">宽度,范围:[0,1]</param>
        /// <param name="heightNormalize">高度,范围:[0,1]</param>
        public void ShowPatchAd(string scene, PatchAdType type, float xNormalize, float yNormalize,
            float widthNormalize,
            float heightNormalize)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] ShowPatchAd scene：{scene} type:{type} xNormalize:{xNormalize} yNormalize:{yNormalize} widthNormalize:{widthNormalize} heightNormalize:{heightNormalize}");
            return;
#endif
            _sdk?.ShowPatchAd(scene, type, xNormalize, yNormalize, widthNormalize, heightNormalize);
        }

        /// <summary>
        /// 隐藏贴片广告
        /// </summary>
        public void HidePatchAd()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] HidePatchAd ");
            return;
#endif
            _sdk?.HidePatchAd();
        }

        #endregion

        // #region 设置广告加载状态
        // /// <summary>
        // /// 设置广告加载状态<para/>
        // /// isOn:true：加载，false：不加载<para/>
        // /// 默认为开启状态，sdk会加载广告<para/>
        // /// <font color="red">已弃用</font><para/> 
        // /// </summary>
        // /// <param name="isOn"></param>
//         [Obsolete("已弃用")]
//         public void SetADLoadingState(bool isOn)
//         {
// #if UNITY_EDITOR
//             return;
// #endif
//             _sdk?.SetADLoadingState(isOn);
//         }
//
//         #endregion

        #region 退出

        /// <summary>
        /// 是否支持退出游戏
        /// </summary>
        public bool HasExit()
        {
#if UNITY_EDITOR
            return false;
#endif
            return _sdk?.HasExit() ?? false;
        }

        /// <summary>
        /// 退出游戏<para/>
        /// </summary>
        public void Exit()
        {
#if UNITY_EDITOR
            return;
#endif
            _sdk?.Exit();
        }

        #endregion

        #region 退出挽留

        /// <summary>
        /// 是否支持退出挽留<para/>
        /// </summary>
        public bool IsSupportExitRetention()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] IsSupportExitRetention 返回false ");
            return false;
#endif
            return _sdk?.IsSupportExitRetention() ?? false;
        }

        /// <summary>
        /// 弹出退出挽留界面<para/>
        /// </summary>
        public void PopUpExitRetention()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] PopUpExitRetention  ");
            return;
#endif
            _sdk?.PopUpExitRetention();
        }

        #endregion

        #region 渠道细分

        /// <summary>
        /// 获取子渠道名，主要细分国内各家渠道<para/>
        /// </summary>
        public ChannelName GetChannelName()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetChannelName 返回Test params（XGameUnityTool/Channel Config->Test params） ChannelName={_sdkPreference.ChannelName}");
            return _sdkPreference.ChannelName;
#endif
            if (_sdk != null)
            {
                return _sdk.GetChannelName();
            }

            return ChannelName.Unknown;
        }

        #endregion

        #region 其它

        /// <summary>
        /// 是否有游戏忠告<para/>
        /// </summary>
        public bool HasGameCounsel()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] HasGameCounsel 返回Test params（XGameUnityTool/Channel Config->Test params） HasGameCounsel={_sdkPreference.HasGameCounsel}");
            return _sdkPreference.HasGameCounsel;
#endif
            return _sdk?.HasGameCounsel() ?? false;
        }


        /// <summary>
        /// 跳转评价页面<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// <font color="#4dd276">IOS_XGUG(国内),IOS_XGUG(海外)</font><para/>
        /// </summary>
        public void OpenReview()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] OpenReview");
            return;
#endif
            _sdk?.OpenReview();
        }


        /// <summary>
        /// 调起原生评价页面<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// </summary>
        public void OpenNativeReview()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] OpenNativeReview");
            return;
#endif
            _sdk?.OpenNativeReview();
        }


        /// <summary>
        /// 跳转商店<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// </summary>
        /// <param name="pkm">游戏包名</param>
        public void OpenStore(string pkm)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] OpenStore 包名：{pkm}");
            return;
#endif
            _sdk?.OpenStore(pkm);
        }

        /// <summary>
        /// 获取版本号<para/>
        /// 主要用于区分内部开发版本，仅做显示作用<para/>
        /// </summary>
        public string GetVersionCode()
        {
            var abbreviation = "unknown";
            if (_SdkBindings.TryGetValue(Channel, out var match))
            {
                abbreviation = match.Abbreviation;
            }

            var buildTypeString = AppConfig.BUILD_TYPE.ToString().ToLower();
            var result = $"{AppConfig.VERSION}.{AppConfig.BUILD_DATE_TIME}_{abbreviation}_{buildTypeString}";
            Log($"[SDK调用成功] GetVersionCode result：{result}");
            return result;
        }


        /// <summary>
        /// 根据网络获取国家代号<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// <font color="#4dd276">IOS(海外)</font><para/>
        /// 默认返回空字符
        /// </summary>
        public string GetNetWorkCountryIso()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetNetWorkCountryIso 返回Test params（XGameUnityTool/Channel Config->Test params） CountryIso={_sdkPreference.CountryIso}");
            return _sdkPreference.CountryIso;
#endif
            return _sdk?.GetNetWorkCountryIso() ?? "";
        }

        /// <summary>
        /// 获取设备类型<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信</font><para/>
        /// </summary>
        public DeviceType GetDeviceType()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetDeviceType 返回Test params（XGameUnityTool/Channel Config->Test params） DeviceType={_sdkPreference.DeviceType}");
            return _sdkPreference.DeviceType;
#endif
            return _sdk?.GetDeviceType() ?? DeviceType.UnKnown;
        }


        /// <summary>
        /// 获取网络类型<para/>
        /// </summary>
        public NetworkType GetNetworkType()
        {
#if UNITY_EDITOR
            //返回Test params
            if (_sdkPreference.UseUnityInternetReachability)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    Log(
                        $"[SDK调用成功] GetDeviceType 返回Test params（XGameUnityTool/Channel Config->Test params） UseUnityInternetReachability={_sdkPreference.UseUnityInternetReachability} Application.internetReachability={Application.internetReachability}");
                    return NetworkType.NO;
                }
            }

            Log(
                $"[SDK调用成功] GetDeviceType 返回Test params（XGameUnityTool/Channel Config->Test params） NetworkType={_sdkPreference.NetworkType} ");
            return _sdkPreference.NetworkType;
#endif
            if (_sdk != null)
            {
                return _sdk.GetNetworkType();
            }

            return NetworkType.UnKnown;
        }


        /// <summary>
        /// 获取sdk 用户ID,请登录成功后调用<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        public string GetSDKUserID()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetSDKUserID 返回Test params（XGameUnityTool/Channel Config->Test params） SDKUserID={_sdkPreference.SDKUserID}");
            return _sdkPreference.SDKUserID;
#endif
            return _sdk?.GetSDKUserID() ?? SystemInfo.deviceUniqueIdentifier;
        }

        public string GetSdkUID()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetSdkUID 返回Test params（XGameUnityTool/Channel Config->Test params） SdkUID={_sdkPreference.SdkUID}");
            return _sdkPreference.SdkUID;
#endif
            return _sdk?.GetSdkUID() ?? SystemInfo.deviceUniqueIdentifier;
        }

        /// <summary>
        /// 用户的头像URL，请登录成功后调用，注意是否为empty<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">XMY</font><para/>
        /// </summary>
        public string GetSDKUserPicUrl()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetSDKUserPicUrl 返回Test params（XGameUnityTool/Channel Config->Test params） SDKUserPicUrl={_sdkPreference.SDKUserPicUrl}");
            return _sdkPreference.SDKUserPicUrl;
#endif
            return _sdk?.GetSDKUserPicUrl();
        }


        /// <summary>
        /// 获取设备唯一码，请在登录成功后调用<para/>
        /// </summary>
        public string GetDeviceUniqueID()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetSDKUserID 默认返回(SystemInfo.deviceUniqueIdentifier): {SystemInfo.deviceUniqueIdentifier}");
            return SystemInfo.deviceUniqueIdentifier;
#endif
            return _sdk?.GetDeviceUniqueID() ?? SystemInfo.deviceUniqueIdentifier;
        }


        /// <summary>
        /// 获取小游戏屏幕大小<para/>
        /// </summary>
        public Vector2 GetMiniGameScreenSize()
        {
            var result = new Vector2(Screen.width, Screen.height);
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetMiniGameScreenSize 默认返回(Screen.width, Screen.height): {result}");
            return result;
#endif
            return _sdk?.GetMiniGameScreenSize() ?? new Vector2(Screen.width, Screen.height);
        }

        #endregion

        #region 录屏分享（抖音小游戏）

        /// <summary>
        /// 是否可录屏<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音Android(XSDK)</font><para/>
        /// </summary>
        public bool CanRecord()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] 返回Test params（XGameUnityTool/Channel Config->Test params） CanRecord： {_sdkPreference.CanRecord}");
            return _sdkPreference.CanRecord;
#endif
            return _sdk?.CanRecord() ?? false;
        }

        /// <summary>
        /// 开始录屏<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音Android(XSDK)</font><para/>
        /// </summary>
        public bool StartRecord()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] StartRecord");
            return true;
#endif
            return _sdk?.StartRecord() ?? false;
        }


        /// <summary>
        /// 结束录制<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音Android(XSDK)</font><para/>
        /// </summary>
        public bool StopRecord()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] StopRecord");
            return true;
#endif
            return _sdk?.StopRecord() ?? true;
        }


        /// <summary>
        /// 获取录屏时长<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音Android(XSDK)</font><para/>
        /// </summary>
        public float GetRecordDuration()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] GetRecordDuration 模拟返回 ：0");
            return 0;
#endif
            return _sdk?.GetRecordDuration() ?? 0;
        }

        /// <summary>
        /// 是否可分享录屏<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音Android(XSDK)</font><para/>
        /// </summary>
        public bool CanShareRecord()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] CanShareRecord 模拟返回 true");
            return true;
#endif
            return _sdk?.CanShareRecord() ?? false;
        }


        /// <summary>
        /// 分享录屏<para/>
        /// 注意;调用前请进行判断是否可以分享,API：XGameSdk.Instance.CanShareRecord()<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音Android(XSDK)</font><para/>
        /// </summary>
        /// <param name="success">成功回调</param>
        /// <param name="fail">失败回调</param>
        /// <param name="cancel">取消回调</param>
        /// <param name="title">主标题</param>
        /// <param name="topics">话题</param>
        public void ShareRecord(Action success, Action<string> fail, Action cancel, string title,
            params string[] topics)
        {
#if UNITY_EDITOR
            if (_sdkPreference.ShareRecordResult)
            {
                Log(
                    $"[SDK调用成功] ShareRecord  ShareRecordResult=={_sdkPreference.ShareRecordResult} 模拟触发成功回调 （XGameUnityTool/Channel Config->Test params）可修改");
                success?.Invoke();
            }
            else
            {
                Log(
                    $"[SDK调用成功] ShareRecord  ShareRecordResult=={_sdkPreference.ShareRecordResult} 模拟触发失败回调 （XGameUnityTool/Channel Config->Test params）可修改");
                fail?.Invoke("preference test result false");
            }

            return;
#endif
            _sdk?.ShareRecord(success, fail, cancel, title, topics);
        }


        /// <summary>
        ///清理录屏内容,标记缓存的录屏失效<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音Android(XSDK)</font><para/>
        /// </summary>
        public void ClearRecord()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] ClearRecord ");
            return;
#endif
            _sdk?.ClearRecord();
        }

        #endregion

        #region IM客服（抖音小游戏）

        /// <summary>
        /// 打开IM客服页<para/>
        /// 默认触发失败回调&amp;lt;para/&amp;gt;
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        /// <param name="success">成功回调</param>
        /// <param name="fail">失败回调</param>
        public void OpenCustomerServiceConversation(Action success = null, Action fail = null)
        {
#if UNITY_EDITOR
            if (_sdkPreference.OpenCustomerServiceResult)
            {
                Log(
                    $"[SDK调用成功] OpenCustomerServiceConversation  OpenCustomerServiceResult=={_sdkPreference.OpenCustomerServiceResult} 模拟触发成功回调 （XGameUnityTool/Channel Config->Test params）可修改");
                success?.Invoke();
            }
            else
            {
                Log(
                    $"[SDK调用成功] OpenCustomerServiceConversation  OpenCustomerServiceResult=={_sdkPreference.OpenCustomerServiceResult} 模拟触发失败回调 （XGameUnityTool/Channel Config->Test params）可修改");
                fail?.Invoke();
            }

            return;
#endif
            if (_sdk != null)
            {
                _sdk.OpenCustomerServiceConversation(success, fail);
            }
            else
            {
                fail?.Invoke();
            }
        }

        /// <summary>
        /// 是否有IM客服页功能<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音IOS(XGP),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        /// <returns></returns>
        public bool HasCustomerServiceConversation()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] HasCustomerService 模拟返回 HasCustomerService={_sdkPreference.HasCustomerService}  （XGameUnityTool/Channel Config->Test params）可修改");
            return _sdkPreference.HasCustomerService;
#endif
            return _sdk?.HasCustomerServiceConversation() ?? false;
        }

        #endregion

        #region 客服（抖音小游戏）

        /// <summary>
        /// 打开客服页<para/>
        /// 默认触发失败回调&amp;lt;para/&amp;gt;
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音IOS(XGP),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        /// <param name="success">成功回调</param>
        /// <param name="fail">失败回调</param>
        public void OpenCustomerService(Action success = null, Action fail = null)
        {
#if UNITY_EDITOR
            if (_sdkPreference.OpenCustomerServiceResult)
            {
                Log(
                    $"[SDK调用成功] OpenCustomerService  OpenCustomerServiceResult=={_sdkPreference.OpenCustomerServiceResult} 模拟触发成功回调 （XGameUnityTool/Channel Config->Test params）可修改");
                success?.Invoke();
            }
            else
            {
                Log(
                    $"[SDK调用成功] OpenCustomerService  OpenCustomerServiceResult=={_sdkPreference.OpenCustomerServiceResult} 模拟触发失败回调 （XGameUnityTool/Channel Config->Test params）可修改");
                fail?.Invoke();
            }

            return;
#endif
            if (_sdk != null)
            {
                _sdk.OpenCustomerService(success, fail);
            }
            else
            {
                fail?.Invoke();
            }
        }

        /// <summary>
        /// 是否有客服页功能<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音IOS(XGP),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        /// <returns></returns>
        public bool HasCustomerService()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] HasCustomerService 模拟返回 HasCustomerService={_sdkPreference.HasCustomerService}  （XGameUnityTool/Channel Config->Test params）可修改");
            return _sdkPreference.HasCustomerService;
#endif
            return _sdk?.HasCustomerService() ?? false;
        }

        #endregion

        #region 创建快捷方式

        /// <summary>
        /// 是否支持创建快捷方式<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">抖音Android(XGP),快手，B站</font><para/>
        /// </summary>
        public bool IsSupportShortcut()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] IsSupportShortcut 模拟返回  IsSupportShortcut={_sdkPreference.IsSupportShortcut}  （XGameUnityTool/Channel Config->Test params）可修改");
            return _sdkPreference.IsSupportShortcut;
#endif
            return _sdk?.IsSupportShortcut() ?? false;
        }


        /// <summary>
        /// 创建快捷方式<para/>
        /// <font color="#4dd276">抖音Android(XGP),抖音Android(XSDK)，快手，B站</font><para/>
        /// </summary>
        public void CreateShortcut(Action success, Action fail = null)
        {
#if UNITY_EDITOR
            if (_sdkPreference.CreateShortcutReturnSuccess)
            {
                Log(
                    $"[SDK调用成功] CreateShortcut 模拟触发成功回调  CreateShortcutReturnSuccess={_sdkPreference.CreateShortcutReturnSuccess}  （XGameUnityTool/Channel Config->Test params）可修改");
                success?.Invoke();
            }
            else
            {
                Log(
                    $"[SDK调用成功] CreateShortcut 模拟触发失败回调  CreateShortcutReturnSuccess={_sdkPreference.CreateShortcutReturnSuccess}  （XGameUnityTool/Channel Config->Test params）可修改");
                fail?.Invoke();
            }

            return;
#endif
            _sdk?.CreateShortcut(success, fail);
        }

        #endregion

        #region 常用

        public bool IsSupportCommonUse()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] IsSupportCommonUse 模拟返回 true");
            return true;
#endif
            return _sdk?.IsSupportCommonUse() ?? false;
        }

        public void AddCommonUse(Action success, Action fail = null)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] AddCommonUse 模拟触发成功回调");
            success?.Invoke();
            return;
#endif
            _sdk?.AddCommonUse(success, fail);
        }

        #endregion

        #region 内购

        /// <summary>
        /// 支付接口<para/>
        /// <font color="#4dd276">Mar：【price:价格(元)】【productId：商品id】【productName：商品名】【productDesc：商品描述】</font><para/>
        /// <font color="#4dd276">Google(XMY)：【price:任意值】【productId：商品id】【productName：任意值】【productDesc：任意值】【offerToken：谷歌的优惠token】</font><para/>
        /// <font color="#4dd276">抖音Android(XSDK)：【price:任意值】【productId：商品id】【productName：任意值】【productDesc：任意值】</font><para/>
        /// <font color="#4dd276">抖音IOS(XSDK)：【price:任意值】【productId：商品id】【productName：任意值】【productDesc：任意值】</font><para/>
        /// <font color="#4dd276">IOS_XGUG(国内)：【price:任意值】【productId：商品id】【productName：任意值】【productDesc：任意值】</font><para/>
        /// <font color="#4dd276">IOS_XGUG(海外)：【price:任意值】【productId：商品id】【productName：任意值】【productDesc：任意值】</font><para/>
        /// <font color="#4dd276">微信小游戏(XSDK)：【price:任意值】【productId：商品id】【productName：任意值】【productDesc：任意值】</font><para/>
        /// <font color="#4dd276">OPPO小游戏(XSDK)：【price:任意值】【productId：商品id】【productName：任意值】【productDesc：任意值】</font><para/>
        /// <font color="#4dd276">VIVO小游戏(XSDK)：【price:任意值】【productId：商品id】【productName：任意值】【productDesc：任意值】</font><para/>
        /// </summary>
        public void Pay(int price, string productId, string productName, string productDesc, string offerToken = null)
        {
#if UNITY_EDITOR
            var fakeOrder = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds + Guid.NewGuid().ToString();
            Log(
                $"[SDK调用成功] Pay price：{price} productId:{productId} productName:{productName} productDesc:{productDesc} offerToken:{offerToken}");
            Log(
                $"[SDK调用成功] Pay 模拟触发回调  延迟返回（PayDelayReturn）={_sdkPreference.PayDelayReturn}  成功（PaySuccess）={_sdkPreference.PaySuccess} productId={productId} 模拟订单号：{fakeOrder}（XGameUnityTool/Channel Config->Test params）可修改");

            //如果延迟返回
            if (_sdkPreference.PayDelayReturn)
            {
                WaitTo(_sdkPreference.PayDelayReturnTime,
                    () => { _sdkListener.InvokeOnPayResult(_sdkPreference.PaySuccess, productId, fakeOrder); });
            }
            else
            {
                _sdkListener.InvokeOnPayResult(_sdkPreference.PaySuccess, productId, fakeOrder);
            }

            return;
#endif
            if (string.IsNullOrEmpty(offerToken))
            {
                _sdk?.Pay(price, productId, productName, productDesc);
            }
            else
            {
                _sdk?.Pay(price, productId, productName, productDesc, offerToken);
            }
        }

        /// <summary>
        /// 请求内购商品列表<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// <font color="#4dd276">IOS_XGUG</font><para/>
        /// </summary>
        /// <param name="complete">完成回调，参数1（bool）:成功/失败，参数2（ProductInfo[]）商品列表</param>
        public void RequestProductInfo(RequestProductInfoResult complete)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] RequestProductInfo 模拟触发回调  成功（RequestProductInfoSuccess）={_sdkPreference.RequestProductInfoSuccess}  商品清单（ProductInfos）={_sdkPreference.ProductInfos.ToXJson()} （XGameUnityTool/Channel Config->Test params）可修改");
            WaitTo(_sdkPreference.RequestProductInfoReturnDelay, () =>
            {
                //触发测试用数据
                if (_sdkPreference.RequestProductInfoSuccess)
                {
                    complete?.Invoke(true, _sdkPreference.ProductInfos);
                }
                else
                {
                    complete?.Invoke(false, _sdkPreference.ProductInfos);
                }
            });
            return;
#endif
            if (_sdk != null)
            {
                _sdk.RequestProductInfo(complete);
                return;
            }

            complete?.Invoke(true, Array.Empty<ProductInfo>());
        }

        /// <summary>
        /// 请求订阅或者非消耗类型商品情况<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// <font color="#4dd276">IOS_XGUG(海外)</font><para/>
        /// </summary>
        /// <param name="complete">完成回调，参数1（bool）:成功/失败，参数2（SubscribeState[]）订阅情况列表</param>
        /// <param name="requestNew">获取最新数据,当为false优先获取缓存里的数据</param>
        public void RequestSubscribeStates(RequestSubscribeStatesResult complete, bool requestNew = true)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] RequestSubscribeStates 模拟触发回调  成功（RequestSubscribeStatesSuccess）={_sdkPreference.RequestSubscribeStatesSuccess}  订阅状态（SubscribeStates）={_sdkPreference.SubscribeStates.ToXJson()} （XGameUnityTool/Channel Config->Test params）可修改");
            WaitTo(_sdkPreference.RequestSubscribeDelayReturn, () =>
            {
                //触发测试用数据
                if (_sdkPreference.RequestSubscribeStatesSuccess)
                {
                    complete?.Invoke(true, _sdkPreference.SubscribeStates);
                }
                else
                {
                    complete?.Invoke(false, _sdkPreference.SubscribeStates);
                }
            });
            return;
#endif
            if (_sdk != null)
            {
                _sdk.RequestSubscribeStates(complete, requestNew);
                return;
            }

            //其它渠道,默认返回true,空订阅
            complete?.Invoke(true, Array.Empty<SubscribeState>());
        }


        /// <summary>
        /// 恢复购买过的非消耗类型和自动订阅类型的订阅情况（IOS必接）<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">IOS_XGUG</font><para/>
        /// </summary>
        /// <param name="success">响应成功时触发</param>
        /// <param name="fail">响应失败/异常时触发</param>
        public virtual void RestoreCompletedPayInfo(Action success = null, Action<string> fail = null)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] RestoreCompletedPayInfo 模拟触发回调  成功（RestoreCompletePayInfoCallResult）={_sdkPreference.RestoreCompletePayInfoCallResult}  恢复的商品清单（RestoreCompletePayInfoDataResult）={_sdkPreference.RestoreCompletePayInfoDataResult.ToXJson()} （XGameUnityTool/Channel Config->Test params）可修改");
            WaitTo(_sdkPreference.RestoreCompletePayInfoDelayTime, () =>
            {
                if (_sdkPreference.RestoreCompletePayInfoCallResult)
                {
                    var products = _sdkPreference.RestoreCompletePayInfoDataResult;
                    foreach (var product in products)
                    {
                        var fakeOrder =
                            $"fake_order_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Random.Range(100000, 99999)}";

                        _sdkListener.InvokeOnPayResult(true, product, fakeOrder);
                    }

                    //触发成功
                    success?.Invoke();
                }
                else
                {
                    //失败
                    fail?.Invoke("测试环境下模拟返回失败,可从（XGameUnityTool/Channel Config->Test params）进行修改");
                }
            });
            return;
#endif
            if (_sdk != null)
            {
                _sdk.RestoreCompletedPayInfo(success, fail);
                return;
            }

            //默认触发失败回调
            fail?.Invoke("no support");
        }

        #endregion

        #region 兑换

        /// <summary>
        /// 兑换礼物<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Mar</font><para/>
        /// <font color="#4dd276">IOS_XGUG(国内)</font><para/>
        /// </summary>
        /// <param name="giftCode">礼包码</param>
        public void Gift(string giftCode)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] Gift 模拟触发回调  是否成功（GiftSuccess）={_sdkPreference.GiftSuccess}  道具数量（GiftProductCount）={_sdkPreference.GiftProductCount} 道具名称（GiftProductName）={_sdkPreference.GiftProductName}（XGameUnityTool/Channel Config->Test params）可修改");
            _sdkListener.InvokeOnGiftResult(_sdkPreference.GiftSuccess, _sdkPreference.GiftProductCount,
                _sdkPreference.GiftProductName);
            return;
#endif
            _sdk?.Gift(giftCode);
        }

        /// <summary>
        /// 是否有兑换礼物功能<para/>
        /// </summary>
        public bool CanGift()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] CanGift 模拟返回 CanGift = {_sdkPreference.CanGift} XGameUnityTool/Channel Config->Test params）可修改");
            return _sdkPreference.CanGift;
#endif
            return _sdk?.CanGift() ?? false;
        }

        #endregion

        #region 事件上报(Version1)(旧接口)

        /// <summary>
        /// 打点上报 字符串数据<para/>
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">值</param>
        /// <param name="level">事件等级</param>
        [Obsolete(
            "已过时，请改用 Track(string eventName, string field, object value = null, EventLevel level = EventLevel.Level_0)")]
        public void DotEvent(string eventName, string field, string value = "", EventLevel level = EventLevel.Level_0)
        {
            Track(eventName, field, value, level);
        }

        /// <summary>
        /// 打点上报 整型数据<para/>
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">值</param>
        /// <param name="level">事件等级</param>
        [Obsolete(
            "已过时，请改用 Track(string eventName, string field, object value = null, EventLevel level = EventLevel.Level_0)")]
        public void DotEvent(string eventName, string field, int value, EventLevel level = EventLevel.Level_0)
        {
            Track(eventName, field, value, level);
        }

        /// <summary>
        /// 打点上报 浮点数据<para/>
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">值</param>
        /// <param name="level">事件等级</param>
        [Obsolete(
            "已过时，请改用 Track(string eventName, string field, object value = null, EventLevel level = EventLevel.Level_0)")]
        public void DotEvent(string eventName, string field, float value, EventLevel level = EventLevel.Level_0)
        {
            Track(eventName, field, value, level);
        }

        /// <summary>
        /// 打点上报 多项数据 （带事件等级）<para/>
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="level">事件等级</param>
        /// <param name="keyValues">按格式：字段1，值1，字段2，值2，...格式传递</param>
        [Obsolete("已过时，请改用 Track(string eventName, KVItems items, EventLevel level = EventLevel.Level_0)")]
        public void DoEventMultipleWithLevel(string eventName, EventLevel level, params object[] keyValues)
        {
            Track(eventName, ObjectsToKvItems(keyValues), level);
        }

        /// <summary>
        /// 打点上报 多项数据<para/>
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="keyValues">按格式：字段1，值1，字段2，值2，...格式传递</param>
        [Obsolete("已过时，请使用 Track(string eventName, KVItems items, EventLevel level = EventLevel.Level_0)")]
        public void DoEventMultiple(string eventName, params object[] keyValues)
        {
            Track(eventName, ObjectsToKvItems(keyValues));
        }

        /// <summary>
        /// 打点上报 多项数据 （带事件等级）<para/>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        /// <param name="level"></param>
        [Obsolete("已过时，请改用 Track(string eventName, KVItems items, EventLevel level = EventLevel.Level_0)")]
        public void DoEventMultiple(string eventName, KVItems items, EventLevel level = EventLevel.Level_0)
        {
            Track(eventName, items, level);
        }

        #endregion

        #region 用户属性上报(Version1)(旧接口)

        /// <summary>
        /// 设置用户属性，以key-value的形式传递数据，满足2的整数倍<para/>
        /// </summary>
        [Obsolete("已过时，请改用 TrackUserProperty(KVItems)")]
        public void SetUserProperty(params object[] data)
        {
#if UNITY_EDITOR
            return;
#endif
            if (!UserPropertyIsEnable)
            {
                return;
            }

            TrackUserProperty(ObjectsToKvItems(data));
        }


        /// <summary>
        /// 设置用户属性，以key-value的形式传递数据<para/>
        /// </summary>
        [Obsolete("已过时，请改用 TrackUserProperty(KVItems)")]
        public void SetUserProperty(KVItems items)
        {
            TrackUserProperty(items);
        }

        #endregion

        #region 用户属性上报(Verson2)

        //obejcts转kvitems
        private KVItems ObjectsToKvItems(object[] keyValues)
        {
            var items = new KVItems();
            if (keyValues != null)
            {
                for (int i = 0; i < keyValues.Length && i + 1 < keyValues.Length; i += 2)
                {
                    var k = keyValues[i]?.ToString();
                    var v = keyValues[i + 1];
                    if (!string.IsNullOrEmpty(k))
                    {
                        items.Add(k, v);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// 设置用户属性-单项<para/>
        /// </summary>
        public void TrackUserProperty(string property, object value)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] TrackUserProperty property:{property}  value:{value} ");
            return;
#endif
            //统一处理
            TrackUserProperty(new KVItems()
            {
                { property, value }
            });
        }

        /// <summary>
        /// 设置用户属性-多项属性<para/>
        /// </summary>
        public void TrackUserProperty(KVItems items)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] TrackUserProperty items:{items?.ToXJson()} ");
            return;
#endif
            //用户属性是否开启
            if (!UserPropertyIsEnable)
            {
                Log("[XGameSdk] 用户属性开关已关闭，略过上报");
                return;
            }

            _sdk?.TrackUserProperty(items);
        }

        #endregion

        #region 事件上报(Verson2)

        /// <summary>
        /// 事件上报，多项上报<para/>
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="items">事件属性[key-value]形式</param>
        /// <param name="level">事件等级不传取默认等级</param>
        public void Track(string eventName, KVItems items, EventLevel level = EventLevel.Level_0)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] Track eventName:{eventName}  items:{items?.ToXJson()} level:{level}");
            return;
#endif
            _sdk?.Track(eventName, items, level);
        }

        /// <summary>
        /// 事件上报，单项上报<para/>
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">值（可选，默认数值1）</param>
        /// <param name="level">上报等级（可选，默认0）</param>
        public void Track(string eventName, string field, object value = null, EventLevel level = EventLevel.Level_0)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] Track eventName:{eventName}  field:{field} value:{value} level:{level}");
            return;
#endif
            //最终改成统一接口，多项上报
            Track(eventName, new KVItems()
            {
                { field, value ?? 1 }
            }, level);
        }

        #endregion

        #region 特殊事件上报

        /// <summary>
        /// 特殊事件上报<para/>
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="items">事件属性[key-value]形式</param>
        /// <param name="level">事件等级不传取默认等级</param>
        public void TrackSpecial(SpecialEventName eventName, KVItems items = null,
            EventLevel level = EventLevel.Level_0)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] TrackSpecial eventName:{eventName.ToString()}  items:{items?.ToXJson()} level:{level}");
            return;
#endif
            _sdk?.TrackSpecial(eventName, items, level);
        }

        #endregion


        #region 分享(逐渐统一普通分享)

        public void Share(Action success, Action<int, string> fail)
        {
            Share(null, objects => { success?.Invoke(); }, fail, null);
        }

        public void Share(Dictionary<string, object> shareParams, Action<Dictionary<string, object>> success = null,
            Action<int, string> fail = null, Action cancel = null)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] Share 模拟触发成功");
            success?.Invoke(null);
            return;
#endif
            _sdk?.Share(shareParams, success, fail, cancel);
        }

        /// <summary>
        /// 被动触发分享监听，比如用户点击右上角的系统分享按钮时回调
        /// </summary>
        /// <param name="callback">处理被动分享时的分享选项并且返回分享参数回调</param>
        /// <param name="success">分享成功回调</param>
        /// <param name="fail">分享失败回调</param>
        /// <param name="cancel">分享取消回调</param>
        public void OnShareMessage(Func<Dictionary<string, object>, Dictionary<string, object>> callback, Action<Dictionary<string, object>> success = null,
            Action<int, string> fail = null, Action cancel = null)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] OnShareMessage 模拟触发成功");
            return;
#endif
            _sdk?.OnShareMessage(callback, success, fail, cancel);
        }
        
        #endregion

        #region 分享（微信小游戏）

        /// <summary>
        /// 分享<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="imageUrl">图片url</param>
        /// <param name="success">成功回调</param>
        public void ShareApp(string title, string imageUrl, Action success)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] ShareApp 模拟触发{_sdkPreference.WXShareResult} 参数：WXShareResult（XGameUnityTool/Channel Config->Test params）可修改");
            if (_sdkPreference.WXShareResult)
            {
                success?.Invoke();
            }

            return;
#endif
            ShareApp(title, imageUrl, null, success);
        }

        /// <summary>
        /// 分享<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="imageUrl">图片url</param>
        /// <param name="imageUrlId">图片urlId</param>
        public void ShareApp(string title, string imageUrl, string imageUrlId, Action result)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] ShareApp 模拟触发{_sdkPreference.WXShareResult} 参数：WXShareResult（XGameUnityTool/Channel Config->Test params）可修改");
            if (_sdkPreference.WXShareResult)
            {
                result?.Invoke();
            }

            return;
#endif
            _sdk?.ShareApp(title, imageUrl, imageUrlId, result);
        }

        /// <summary>
        /// 设置微信分享样式<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="imageUrl">图片url</param>
        /// <param name="imageUrlId">图片urlId</param>
        public void SetShareStyle(string title, string imageUrl, string imageUrlId)
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] SetShareStyle");
            return;
#endif
            _sdk?.SetShareStyle(title, imageUrl, imageUrlId);
        }


        /// <summary>
        /// 分享(带query参数)<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC),微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="imageUrl">图片url</param>
        /// <param name="imageUrlId">图片urlId(可传null)</param>
        /// <param name="query">携带参数,必须是 key1=val1&key2=val2 的格式</param>
        /// <param name="success">成功回调</param>
        public void ShareAppWithQuery(string title, string imageUrl, string imageUrlId, string query, Action success)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] ShareAppWithQuery 模拟触发{_sdkPreference.WXShareResult} 参数：WXShareResult（XGameUnityTool/Channel Config->Test params）可修改");
            if (_sdkPreference.WXShareResult)
            {
                success?.Invoke();
            }

            return;
#endif
            _sdk?.ShareAppWithQuery(title, imageUrl, imageUrlId, query, success);
        }


        /// <summary>
        /// 分享并发布分享任务<para/>
        /// 相关API:<para/>
        /// <see cref="GetShareTaskDetail"/>(从服务器获取分享任务详情)<para/>
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="imageUrl">图片url</param>
        /// <param name="imageUrlId">图片urlId(可传null)</param>
        /// <param name="taskId">任务ID，注意做好ID生成机制，推荐使用（用户ID+任务标识）的格式，用户ID可以通过GetSDKUserID获取，例如：user1001_Task1</param>
        /// <param name="ext">补充的自定义参数(可传null)</param>
        public void ShareAppWithTask(string title, string imageUrl, string imageUrlId, string taskId, string ext,
            Action success)
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] ShareAppWithTask 模拟触发{_sdkPreference.WXShareResult} 参数：WXShareResult（XGameUnityTool/Channel Config->Test params）可修改");
            if (_sdkPreference.WXShareResult)
            {
                success?.Invoke();
            }

            return;
#endif
            _sdk?.ShareAppWithTask(title, imageUrl, imageUrlId, taskId, ext, success);
        }

        #endregion

        #region 游戏圈（微信小游戏）

        /// <summary>
        /// 创建微信游戏圈按钮<para/>
        /// imageUrl:图片地址(默认为：images/game_club_btn.png，也可以用互联网图片代替)<para/>
        /// left：左上角横坐标（像素）<para/>
        /// top:左上角纵坐标（像素）<para/>
        /// width:宽度（像素）<para/>
        /// height：高度（像素）<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),微信(ASC)</font><para/>
        /// </summary>
        public IWXGameClubBtn CreateWXGameClubButton(int left, int top, int width, int height,
            string imageUrl = "images/game_club_btn.png")
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] CreateWXGameClubButton 模拟返回 new SimpleWXGameClubBtn");
            return new SimpleWXGameClubBtn();
#endif
            return _sdk?.CreateWXGameClubButton(left, top, width, height, imageUrl) ?? new SimpleWXGameClubBtn();
        }

        /// <summary>
        /// 获取微信游戏圈数据 (通用接口)<para/>
        /// 详细传参以微信文档为准：<see cref="https://developers.weixin.qq.com/minigame/dev/api/open-api/game-club/wx.getGameClubData.html#GameClubDataByType-%E7%9A%84%E7%BB%93%E6%9E%84"/><para/>
        /// 常用的数据获取API:<para/>
        /// 加入该游戏圈时间:<see cref="GetWxGameClubDataJoinTime"/><para/>
        /// 当天(自然日)点赞贴子数:<see cref="GetWxGameClubDataPostLikeNumber"/><para/>
        /// 当天(自然日)发表贴子数:<see cref="GetWxGameClubDataPostPublishNumber"/><para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="dataTypeList">感兴趣的数据参数</param>
        /// <param name="success">成功回调</param>
        /// <param name="fail">失败回调</param>
        public void GetWxGameClubData(List<(long type, string subkey)> dataTypeList,
            Action<GameClubDataByType[]> success,
            Action<string> fail)
        {
#if UNITY_EDITOR
            //返回测试数据
            Log($"[本地模拟] GetWxGameClubData 调用成功！返回测试数据");
            WaitTo(_sdkPreference.GetWxGameClubDataResultDelay, () =>
            {
                if (_sdkPreference.GetWxGameClubDataResult)
                {
                    success?.Invoke(_sdkPreference.GetWxGameClubDataReturnData.ToArray());
                }
                else
                {
                    fail?.Invoke("本地模拟触发失败 GetWxGameClubDataResult==false");
                }
            });
            return;
#endif
            if (_sdk != null)
            {
                _sdk.GetWxGameClubData(dataTypeList, success, fail);
                return;
            }

            fail?.Invoke("no support");
        }

        /// <summary>
        /// 获取微信游戏圈（加入该游戏圈时间,type=1）<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="success">成功回调，数据：秒级Unix时间戳</param>
        /// <param name="fail">失败或异常</param>
        public void GetWxGameClubDataJoinTime(Action<long> success, Action<string> fail)
        {
            GetWxGameClubData(new List<(long type, string subkey)>()
            {
                (1, "")
            }, (res) =>
            {
                if (res == null)
                {
                    fail?.Invoke("返回数据为null");
                    return;
                }

                var match = res.FirstOrDefault(e => e.dataType == 1);
                if (match == null)
                {
                    fail?.Invoke("找不到匹配数据");
                    return;
                }

                success?.Invoke(match.value);
            }, (err) => { fail?.Invoke(err); });
        }

        /// <summary>
        /// 获取微信游戏圈 当天(自然日)点赞贴子数,type=4）<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="success">成功回调，数据：当天(自然日)点赞贴子数</param>
        /// <param name="fail">失败或异常</param>
        public void GetWxGameClubDataPostLikeNumber(Action<long> success, Action<string> fail)
        {
            GetWxGameClubData(new List<(long type, string subkey)>()
            {
                (4, "")
            }, (res) =>
            {
                if (res == null)
                {
                    fail?.Invoke("返回数据为null");
                    return;
                }

                var match = res.FirstOrDefault(e => e.dataType == 4);
                if (match == null)
                {
                    fail?.Invoke("找不到匹配数据");
                    return;
                }

                success?.Invoke(match.value);
            }, (err) => { fail?.Invoke(err); });
        }


        /// <summary>
        /// 获取微信游戏圈 当天(自然日)发表贴子数,type=6）<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XSDK)</font><para/>
        /// </summary>
        /// <param name="success">成功回调，数据：当天(自然日)发表贴子数</param>
        /// <param name="fail">失败或异常</param>
        public void GetWxGameClubDataPostPublishNumber(Action<long> success, Action<string> fail)
        {
            GetWxGameClubData(new List<(long type, string subkey)>()
            {
                (6, "")
            }, (res) =>
            {
                if (res == null)
                {
                    fail?.Invoke("返回数据为null");
                    return;
                }

                var match = res.FirstOrDefault(e => e.dataType == 6);
                if (match == null)
                {
                    fail?.Invoke("找不到匹配数据");
                    return;
                }

                success?.Invoke(match.value);
            }, (err) => { fail?.Invoke(err); });
        }

        #endregion

        #region 分享任务相关（微信小游戏）

        public virtual void UploadShareTask(string taskId, string fromUid, string ext, Action success,
            Action<string> fail)
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] UploadShareTask 返回Test params(XGameUnityTool/Channel Config/Test params) taskId:{taskId},fromUid:{fromUid},ext:{ext ?? string.Empty}");
            WaitTo(_sdkPreference.UploadShareTaskDelayReturn, () =>
            {
                if (_sdkPreference.UploadShareTaskResult)
                {
                    success?.Invoke();
                }
                else
                {
                    fail?.Invoke("本地模拟触发失败 UploadShareTaskResult==false");
                }
            });
            return;
#endif
            if (_sdk != null)
            {
                _sdk.UploadShareTask(taskId, fromUid, ext, success, fail);
                return;
            }

            fail?.Invoke("not support");
        }

        /// <summary>
        /// 获取分享任务详情
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="success">响应成功回调（返回数据ShareTaskDetailInfo[]）</param>
        /// <param name="fail">响应失败/异常</param>
        public virtual void GetShareTaskDetail(string taskId, Action<ShareTaskDetailInfo[]> success,
            Action<string> fail)
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] GetShareTaskDetail 返回Test params(XGameUnityTool/Channel Config/Test params) taskId:{taskId}");
            WaitTo(_sdkPreference.GetShareTaskDetailDelayReturn, () =>
            {
                if (_sdkPreference.GetShareTaskDetailResult)
                {
                    success?.Invoke(_sdkPreference.GetShareTaskDetailReturnData.ToArray());
                }
                else
                {
                    fail?.Invoke("本地模拟触发失败 GetShareTaskDetailResult==false");
                }
            });
            return;
#endif
            if (_sdk != null)
            {
                _sdk.GetShareTaskDetail(taskId, success, fail);
                return;
            }

            fail?.Invoke("not support");
        }

        #endregion

        #region 请求云控参数

        /// <summary>
        /// 请求云控参数<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// <font color="#4dd276">IOS_XGUG</font><para/>
        /// </summary>
        /// <param name="complete">成功回调</param>
        /// <param name="timeOut">自定义超时</param>
        public void RequestRemoteConfig(Action<string> complete, float timeOut = 8)
        {
#if UNITY_EDITOR
            if (_sdkPreference.RemoteConfig == null)
            {
                Log(
                    $"[SDK调用成功] RequestRemoteConfig 模拟返回(RemoteConfig) (TextAsset=null) （XGameUnityTool/Channel Config->Test params）可修改");
            }
            else
            {
                Log(
                    $"[SDK调用成功] RequestRemoteConfig 模拟返回(RemoteConfig) {_sdkPreference.RemoteConfig} （XGameUnityTool/Channel Config->Test params）可修改");
            }

            if (_sdkPreference.RemoteConfig == null)
            {
                complete?.Invoke(string.Empty);
            }
            else
            {
                complete?.Invoke(_sdkPreference.RemoteConfig.text);
            }

            return;
#endif
            if (_sdk != null)
            {
                var handler = new ActionHandler<string>(complete);
                _sdk.RequestRemoteConfig((ret) => handler.Invoke(ret));
                //自定义超时返回
                WaitTo(timeOut, () => { handler.Invoke(string.Empty); });
                return;
            }

            complete?.Invoke(string.Empty);
        }

        /// <summary>
        /// 请求云控参数（多回调场景）：原生可能多次上报（如多次 OnRemoteConfigResult），每次结果都通过 <paramref name="complete"/> 回调。<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XMY)</font><para/>
        /// 其它渠道：未重写时由 `BaseSdk` 通过 <see cref="RequestRemoteConfig"/> 转调（与已实现云控拉取的子类自动对齐）。<para/>
        /// 不进行超时兜底；未挂载 SDK 时仍为立刻空字符串兜底。<para/>
        /// </summary>
        /// <param name="complete">收到云控 JSON 时回调；未挂载 SDK 时对空字符串回调</param>
        public void RequestRemoteConfigMulti(Action<string> complete)
        {
            if (complete == null)
            {
                return;
            }

#if UNITY_EDITOR
            string payload;
            if (_sdkPreference.RemoteConfig == null)
            {
                Log(
                    $"[SDK调用成功] RequestRemoteConfigMulti 模拟返回(RemoteConfig=null) （XGameUnityTool/Channel Config->Test params）可修改");
                payload = string.Empty;
            }
            else
            {
                Log(
                    $"[SDK调用成功] RequestRemoteConfigMulti 模拟返回(RemoteConfig) {_sdkPreference.RemoteConfig} （XGameUnityTool/Channel Config->Test params）可修改");
                payload = _sdkPreference.RemoteConfig.text;
            }

            complete.Invoke(payload);

            return;
#endif
            if (_sdk != null)
            {
                _sdk.RequestRemoteConfigMulti(complete);
                return;
            }

            complete.Invoke(string.Empty);
        }

        #endregion

//         #region bgm音频播放
//
//         /// <summary>
//         /// 播放小游戏bgm<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">oppo小游戏(XGP)</font><para/>
//         /// </summary>
//         [Obsolete("已过时,请改用 XGameH5Audio.Instance.PlayBgm")]
//         public void PlayMiniGameBgm(string url)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] PlayMiniGameBgm url：{url}");
//             return;
// #endif
//             _sdk?.PlayMiniGameBgm(url);
//         }
//
//
//         /// <summary>
//         /// 停止小游戏bgm<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">oppo小游戏(XGP)</font><para/>
//         /// </summary>
//         [Obsolete("已过时,请改用 XGameH5Audio.Instance.StopBgm代替")]
//         public void StopMiniGameBgm()
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] StopMiniGameBgm");
//             return;
// #endif
//             _sdk?.StopMiniGameBgm();
//         }
//
//         #endregion

//         #region 友盟打点
//
//         /// <summary>
//         /// 友盟打点 简单上报<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// </summary>
//         /// <param name="eventId"></param>
//         public void UmaTrackEvent(string eventId)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] UmaTrackEvent eventId:{eventId}");
//             return;
// #endif
//             _sdk?.UmaTrackEvent(eventId);
//         }
//
//         /// <summary>
//         /// 友盟打点 带属性上报<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// </summary>
//         /// <param name="eventId"></param>
//         /// <param name="keyValues"></param>
//         public void UmaTrackEvent(string eventId, params object[] keyValues)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] UmaTrackEvent eventId:{eventId} keyValues:{keyValues.ToXJson()}");
//             return;
// #endif
//             _sdk?.UmaTrackEvent(eventId, keyValues);
//         }
//
//         #endregion

        #region 协程

        /// <summary>
        /// 延迟执行
        /// </summary>
        public void WaitTo(float time, Action callback)
        {
            StartCoroutine(IeWaitTo(time, callback));
        }

        /// <summary>
        /// 延迟触发
        /// </summary>
        private IEnumerator IeWaitTo(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        #endregion

        #region Http POST,GET

        /// <summary>
        /// http post
        /// </summary>
        public void HttpPost(string url, string data, Action<HttpPostSuccessResult> success, Action<string> fail,
            Dictionary<string, string> header = null, int timeOut = 60)
        {
            if (IsDevelopment)
            {
                //默认走unity post
                XHttpClient.Post(url, data, success, fail, header, timeOut);
                return;
            }

            if (_sdk != null)
            {
                _sdk.HttpPost(url, data, success, fail, header, timeOut);
                return;
            }

            XHttpClient.Post(url, data, success, fail, header, timeOut);
        }


        /// <summary>
        /// http get
        /// </summary>
        public void HttpGet(string url, Action<HttpGetSuccessResult> success, Action<string> fail,
            Dictionary<string, string> header = null, int timeOut = 60)
        {
            if (IsDevelopment)
            {
                XHttpClient.Get(url, success, fail, header, timeOut);
                return;
            }

            if (_sdk != null)
            {
                _sdk.HttpGet(url, success, fail, header, timeOut);
                return;
            }

            XHttpClient.Get(url, success, fail, header, timeOut);
        }

        #endregion

        // #region 自定义云控
        //
        // /// <summary>
        // /// 加载cdn资源<para/>
        // /// </summary>
        // [Obsolete("已弃用，请使用HttpGet接口代替")]
        // public void LoadCdnConfig(string url, Action<Dictionary<string, object>> complete, int timeOut = -1)
        // {
        //     StartCoroutine(IeLoadCdnConfig(url, complete, timeOut));
        // }
        //
        // IEnumerator IeLoadCdnConfig(string url, Action<Dictionary<string, object>> complete, int timeOut = -1)
        // {
        //     var finalUrl = GetUrl(url);
        //     //云控结果
        //     var result = new Dictionary<string, object>();
        //     var uri = new Uri(finalUrl);
        //     using (var request = UnityWebRequest.Get(uri))
        //     {
        //         //超时时间
        //         if (timeOut > 0)
        //         {
        //             request.timeout = timeOut;
        //         }
        //
        //         yield return request.SendWebRequest();
        //         if (request.isHttpError || request.isNetworkError)
        //         {
        //             //云控获取失败 
        //             Log("load config error");
        //         }
        //         else
        //         {
        //             //云控获取成功
        //             var txt = request.downloadHandler.text;
        //             result = XJson.FromJson<Dictionary<string, object>>(txt);
        //             Log("load config done");
        //         }
        //
        //         complete?.Invoke(result);
        //     }
        // }
        //
        // private string GetUrl(string url)
        // {
        //     return $"{url}?={TimeStampNow()}";
        // }
        //
        // /// <summary>
        // /// 当前时间戳<para/>
        // /// </summary>
        // private static long TimeStampNow()
        // {
        //     return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        // }
        //
        // #endregion

        #region Toast

        /// <summary>
        /// Toast提示<para/>
        /// <font color="#4dd276">字符数量尽量简短避免某些渠道显示不全,最好不要超过14个字符</font><para/>
        /// </summary>
        public int ShowToast(string text, bool blocksRaycasts = false, float duration = 1.5f)
        {
#if UNITY_EDITOR
            return ToastGUI.Instance.ShowToast(text, duration);
#endif
            if (_sdk != null)
            {
                //使用新的toast代替
                return _sdk.ShowToast(text, blocksRaycasts, duration);
            }

            return ToastGUI.Instance.ShowToast(text, duration);
        }

        // public XGameSdkToast GetToast()
        // {
        //     return CustomToast.GetToast();
        // }

        #endregion

        #region 云存档

        //尝试启动本地云存档服务器
        private void InitializeLocalHostCloudArchiveServer()
        {
#if UNITY_EDITOR
            //编辑器模式下
            _localHostCloudArchiveServer = LocalHostCloudArchiveServer.Instance;
            _localHostCloudArchiveServer.StartServer(_sdkPreference.LocalHostCloudArchiveServerSaveTime);
            return;
#endif
            //使用本地云存档服务器模拟
            if (UseLocalHostCloudArchiveServer())
            {
                Log($"[XGameSdk] 渠道：{Channel} 不支持云存档功能改用本地云存档服务器");
                //创建本地云存档服务器
                _localHostCloudArchiveServer = LocalHostCloudArchiveServer.Instance;
                _localHostCloudArchiveServer.StartServer(30);
            }
        }

        //云存档是否开启
        private bool UseLocalHostCloudArchiveServer()
        {
#if UNITY_EDITOR
            return true;
#endif
            if (_SdkBindings.TryGetValue(Channel, out var match))
            {
                //如果支持云存档，返回false
                return !match.IsSupportCloudArchive;
            }

            return true;
        }


        /// <summary>
        /// 云存档初始化<para/>
        /// <font color="#4dd276">触发成功回调后再使用云存档功能</font><para/>
        /// </summary>
        public void CloudArchive_Initialize(Action success, Action<string> fail)
        {
            if (UseLocalHostCloudArchiveServer())
            {
                success?.Invoke();
            }
            else
            {
                //云存档初始化
                _sdk?.CloudArchive_Initialize(success, fail);
            }
        }


        /// <summary>
        ///  获取云存档keys,返回对应的key和版本号<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),oppo小游戏(XGP),vivo小游戏(XGP),抖音Android(XGP),抖音IOS(XGP),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        public void CloudArchive_GetKeys(Action<CloudArchiveGetKeysResult> success, Action<string> fail)
        {
            if (UseLocalHostCloudArchiveServer())
            {
                //使用本地进行模拟
                _localHostCloudArchiveServer.GetKeys(success, fail);
            }
            else
            {
                _sdk?.CloudArchive_GetKeys(success, fail);
            }
        }

        /// <summary>
        /// 获取云存档数据<para/>
        /// version为0时表示云存档中不存在该条目<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),oppo小游戏(XGP),vivo小游戏(XGP),抖音Android(XGP),抖音IOS(XGP),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        public void CloudArchive_GetData(string key, Action<CloudArchiveGetDateResult> success, Action<string> fail)
        {
            if (UseLocalHostCloudArchiveServer())
            {
                //使用本地进行模拟
                _localHostCloudArchiveServer.GetData(key, success, fail);
            }
            else
            {
                _sdk?.CloudArchive_GetData(key, success, fail);
            }
        }

        /// <summary>
        /// 设置数据，version需要大于0，content不可超64kb,太长建议拆分key的数量<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),oppo小游戏(XGP),vivo小游戏(XGP),抖音Android(XGP),抖音IOS(XGP),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        public void CloudArchive_SetData(string key, long version, string content)
        {
            if (UseLocalHostCloudArchiveServer())
            {
                _localHostCloudArchiveServer.SetData(key, version, content);
            }
            else
            {
                _sdk?.CloudArchive_SetData(key, version, content);
            }
        }

        /// <summary>
        /// 主动上传同步存档，存档内部有自动上传机制，建议关键节点才调用一次<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">微信(XGP),oppo小游戏(XGP),vivo小游戏(XGP),抖音Android(XGP),抖音IOS(XGP),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        /// </summary>
        public void CloudArchive_UploadSync()
        {
            if (UseLocalHostCloudArchiveServer())
            {
                _localHostCloudArchiveServer.UploadSync();
            }
            else
            {
                _sdk?.CloudArchive_UploadSync();
            }
        }

        #endregion

        #region 游戏存档

        /// <summary>
        /// 获取游戏存档信息（支持渠道：抖音，快手web）
        /// </summary>
        /// <param name="success">成功回调 (archive, hasArchive)</param>
        /// <param name="fail">失败回调</param>
        public void GetGameArchive(Action<string, bool> success, Action<string> fail)
        {
#if UNITY_EDITOR
            Log("[XGameSdk] GetGameArchive 编辑器模式，模拟成功回调返回空存档");
            success?.Invoke("", false);
            return;
#endif
            _sdk?.GetGameArchive(success, fail);
        }

        /// <summary>
        /// 同步游戏存档信息（支持渠道：抖音，快手web）
        /// </summary>
        /// <param name="archive">存档数据</param>
        /// <param name="success">成功回调</param>
        /// <param name="fail">失败回调</param>
        public void SyncGameArchive(string archive, Action success, Action<string> fail)
        {
#if UNITY_EDITOR
            Log("[XGameSdk] SyncGameArchive 编辑器模式，模拟成功回调");
            success?.Invoke();
            return;
#endif
            _sdk?.SyncGameArchive(archive, success, fail);
        }

        #endregion

        #region 是否为海外App

        public bool IsSeaApp()
        {
            if (_sdk != null)
            {
                return _sdk.IsSeaApp();
            }

            return CommonTool.IsSeaApp();
        }

        #endregion

        #region NTP

        /// <summary>
        /// 获取互联网时间戳,单位：秒<para/>
        /// </summary>
        public void GetNTP(Action<long> success, Action<string> fail)
        {
            if (_sdk != null)
            {
                _sdk.GetNTP(success, fail);
                return;
            }

            HttpApi.GetNTP(success, fail);
        }

        #endregion

        #region 请求Ip地址

        /// <summary>
        /// 获取IP地址<para/>
        /// 国内渠道可获得真实ip，海外渠道返回127.0.0.1<para/>
        /// </summary>
        public void GetIp(Action<string> success, Action<string> fail)
        {
            if (_sdk != null)
            {
                _sdk.GetIp(success, fail);
                return;
            }

            HttpApi.GetIp(ip =>
            {
                if (string.IsNullOrEmpty(ip))
                {
                    success?.Invoke("127.0.0.1");
                }
                else
                {
                    success?.Invoke(ip);
                }
            }, fail);
        }

        #endregion

//         #region 请求区域位置
//
//         /// <summary>
//         /// 请求区域信息<para/>
//         /// 国内返回正常信息，海外返回默认值<para/>
//         /// </summary>
//         public void GetArea(Action<AreaData> success, Action<string> fail)
//         {
// #if UNITY_EDITOR
//             var useTest = _sdkPreference.UseTestArea;
//             if (useTest)
//             {
//                 if (_sdkPreference.GetAreaReturn)
//                 {
//                     success?.Invoke(_sdkPreference.AreaData);
//                 }
//                 else
//                 {
//                     fail?.Invoke("测试数据返回失败（可从sdk测试数据中修改）");
//                 }
//
//                 return;
//             }
// #endif
//             if (_sdk != null)
//             {
//                 _sdk.GetArea(success, fail);
//                 return;
//             }
//
//             //如果本地
//             if (IsSeaApp())
//             {
//                 //返回默认值
//                 var data = new AreaData()
//                 {
//                     country = "海外",
//                     short_name = "",
//                     province = "",
//                     city = "",
//                     area = "",
//                     isp = "",
//                     ip = "127.0.0.1",
//                     code = 200,
//                     desc = "success"
//                 };
//                 //海外渠道
//                 success?.Invoke(data);
//             }
//             else
//             {
//                 GetIp((ip) =>
//                 {
//                     //从ip获取信息
//                     HttpGet($"https://ip.useragentinfo.com/json?ip={ip.ToString()}", (res) =>
//                     {
//                         var data = XJson.FromJson<AreaData>(res.Data);
//                         success?.Invoke(data);
//                     }, (error) => { fail?.Invoke(error); });
//                 }, (error) => { fail?.Invoke(error); });
//             }
//         }
//
//         #endregion

        #region 更多精彩

        /// <summary>
        /// 是否有更多精彩,部分渠道有效<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Mar</font><para/>
        /// </summary>
        public bool HasMoreGame()
        {
#if UNITY_EDITOR
            Log(
                $"[SDK调用成功] HasMoreGame 模拟返回（HasMoreGame）={_sdkPreference.HasMoreGame} （XGameUnityTool/Channel Config->Test params）可修改");
            return _sdkPreference.HasMoreGame;
#endif
            return _sdk?.HasMoreGame() ?? false;
        }

        /// <summary>
        /// 显示更多精彩,部分渠道有效<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Mar</font><para/>
        /// </summary>
        public void OpenMoreGame()
        {
#if UNITY_EDITOR
            Log($"[SDK调用成功] OpenMoreGame ");
            return;
#endif
            _sdk?.OpenMoreGame();
        }

        #endregion

//         #region 小游戏互推
//
//         /// <summary>
//         /// 小游戏互推,请求互推内容<para/>
//         /// success：成功回调，可以缓存MPushReqItemsData.result.contents，后续进行触发点击广告<para/>
//         /// fail：失败回调，返回错误信息<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// </summary>
//         public void MPush_ReqItems(string planId, int count, Action<MPushReqItemsData> success, Action<string> fail)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] MPush_ReqItems 模拟数据 从（XGameUnityTool/Channel Config->Test params）设置");
//             if (_sdkPreference.MPushReqItemFlag)
//             {
//                 success?.Invoke(new MPushReqItemsData() { contents = _sdkPreference.MPushContents.ToArray() });
//             }
//             else
//             {
//                 fail?.Invoke("MPush_ReqItems 请求失败，请从XGameUnityTool-Channel Config-Test params中开启");
//             }
//
//             return;
// #endif
//             _sdk?.MPush_ReqItems(planId, count, success, fail);
//         }
//
//         /// <summary>
//         /// 小游戏互推-点击跳转<para/>
//         /// content：互推内容<para/>
//         /// succces：跳转成功回调<para/>
//         /// fail:失败回调,返回失败信息<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">微信(XGP)</font><para/>
//         /// </summary>
//         public void MPush_ClickItem(MPushContent content, Action success, Action<string> fail)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] MPush_ClickItem 模拟触发 从（XGameUnityTool/Channel Config->Test params）设置");
//             if (_sdkPreference.MPushClickItemFlag)
//             {
//                 success?.Invoke();
//             }
//             else
//             {
//                 Debug.Log("MPush_ClickItem 失败回调，可从XGameUnityTool-Channel Config-Test params中进行设置");
//                 fail?.Invoke("MPush_ClickItem 失败回调，可从XGameUnityTool-Channel Config-Test params中进行设置");
//             }
//
//             return;
// #endif
//             _sdk?.MPush_ClickItem(content, success, fail);
//         }
//
//         #endregion

        // #region XGP API
        //
        // /// <summary>
        // /// 请求XGP服务器，user 模块下的 api，具体api路由和传输的数据格式问工具组同学<para/>
        // /// <font color="#4dd276">支持渠道:</font><para/>
        // /// <font color="#4dd276">抖音Android(XGP),抖音IOS(XGP),抖音Android(XSDK),抖音IOS(XSDK)</font><para/>
        // /// </summary>
        // /// <param name="route">路由</param>
        // /// <param name="body">请求数据</param>
        // /// <param name="response">响应成功，返回的结果</param>
        // /// <param name="fail">网络失败</param>
        // public void XGPApi_User(string route, object body, Action<XGPApiResponse> response,
        //     Action<string> fail)
        // {
        //     if (_sdk != null)
        //     {
        //         _sdk.XGPApi_User(route, body, response, fail);
        //         return;
        //     }
        //
        //     if (IsDevelopment)
        //     {
        //         XGPApiDevelopment.Instance.XGPApi_User(route, body, response, fail);
        //     }
        // }
        //
        // #endregion

        #region 特殊游戏开关（MAR）

        /// <summary>
        /// 获取特殊游戏开关，Mar渠道<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Mar</font><para/>
        /// </summary>
        public bool GetSpecialGameSwitch()
        {
#if UNITY_EDITOR
            return _sdkPreference.GetSpecialGameSwitchResult;
#endif
            return _sdk?.GetSpecialGameSwitch() ?? false;
        }

        #endregion

        #region 侧边栏进入

        /// <summary>
        /// 是否从侧边栏进入游戏,登录成功后才可获的<para/>
        /// 当为true时可以激活侧边栏奖励<para/>
        /// </summary>
        public bool IsEnterFromSidebar()
        {
#if UNITY_EDITOR
            switch (AppConfig.CHANNEL)
            {
                case AppChannel.Bilibili_XSDK:
                case AppChannel.Kuaishou_XSDK:
                case AppChannel.Kuaishou_XSDK_Android:
                case AppChannel.Douyin_XSDK_Android:
                case AppChannel.Douyin_XSDK_IOS:
                    if (!_sdkPreference.IsEnterFromSidebar)
                    {
                        Debug.Log(
                            "<color=#0f8cdc>[本地模拟] 侧边栏进入标记已关闭 IsEnterFromSidebar == false ，可以从'Channel Config/Test params' 中开启</color>");
                    }

                    return _sdkPreference.IsEnterFromSidebar;
            }

            return false;
#endif
            return _sdk?.IsEnterFromSidebar() ?? false;
        }


        /// <summary>
        /// 是否支持侧边栏功能,请登录成功再获取<para/>
        /// 当为true时可以显示侧边栏相关UI<para/>
        /// </summary>
        public bool IsSupportSidebar()
        {
#if UNITY_EDITOR
            switch (AppConfig.CHANNEL)
            {
                case AppChannel.Bilibili_XSDK:
                case AppChannel.Kuaishou_XSDK:
                case AppChannel.Kuaishou_XSDK_Android:
                case AppChannel.Douyin_XSDK_Android:
                case AppChannel.Douyin_XSDK_IOS:
                    if (!_sdkPreference.IsSupportSidebar)
                    {
                        Debug.Log(
                            "<color=#0f8cdc>[本地模拟] 当前侧边栏功能已关闭 IsSupportSidebar == false ，可以从'Channel Config/Test params' 中开启</color>");
                    }

                    return _sdkPreference.IsSupportSidebar;
            }

            return false;
#endif
            return _sdk?.IsSupportSidebar() ?? false;
        }


        /// <summary>
        /// 帮玩家跳转到侧边栏<para/>
        /// </summary>
        /// <param name="success">成功跳转</param>
        /// <param name="fail">跳转失败，可以弹出提示玩家改为手动操作</param>
        public void NavigateToSidebar(Action success, Action<string> fail)
        {
#if UNITY_EDITOR
            Debug.Log(
                "<color=#0f8cdc>[本地模拟] 触发帮玩家跳转到侧边栏 NavigateToSidebar</color>");
            if (_sdkPreference.NavigateToSidebarResult)
            {
                success?.Invoke();
            }
            else
            {
                Debug.Log(
                    "<color=#0f8cdc>[本地模拟] NavigateToSidebarResult == false 可以从'Channel Config/Test params' 中开启</color>");
                fail?.Invoke("[本地模拟] NavigateToSidebarResult == false");
            }

            return;
#endif
            _sdk?.NavigateToSidebar(success, fail);
        }

        #endregion

//         #region 抖音-视频排行榜
//
//         /// <summary>
//         /// 是否可分享视频到排行榜<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">抖音Android(XGP)</font><para/>
//         /// </summary>
//         public bool CanShareVideoToRank()
//         {
// #if UNITY_EDITOR
//             switch (AppConfig.CHANNEL)
//             {
//                 case AppChannel.Douyin_XSDK_Android:
//                 case AppChannel.Douyin_XSDK_IOS:
//                     Log($"[SDK调用成功] CanShareVideoToRank 返回测试数据：{_sdkPreference.CanShareVideoToRank}");
//                     return _sdkPreference.CanShareVideoToRank;
//                 default:
//                     return false;
//             }
// #endif
//             return _sdk?.CanShareVideoToRank() ?? false;
//         }
//
//         /// <summary>
//         /// 分享视频到排行榜<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">抖音Android(XGP)</font><para/>
//         /// </summary>
//         /// <param name="title">标题</param>
//         /// <param name="desc">描述</param>
//         /// <param name="tag">视频标签，用于筛选视频</param>
//         /// <param name="success">分享成功时触发</param>
//         /// <param name="fail">分享失败时触发</param>
//         /// <param name="cancel">取消时触发</param>
//         public void ShareVideoToRank(string title, string desc, string tag, string[] topics,
//             Action<ShareAppSuccessResult> success,
//             Action<string> fail, Action cancel)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] ShareVideoToRank title:{title} desc:{desc} tag:{tag}");
//             if (_sdkPreference.ShareVideoToRankResult)
//             {
//                 var path = $"Assets/XGameUnityTool/Editor/LocalTest/模拟数据-分享视频到排行榜成功.json";
//                 Debug.Log($"使用测试数据模拟，可自行修改：{path}");
//                 WaitTo(0.3f, () =>
//                 {
//                     var res = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
//                     var data = XJson.FromJson<Dictionary<string, object>>(res);
//                     success?.Invoke(new ShareAppSuccessResult(data));
//                 });
//                 return;
//             }
//             else
//             {
//                 fail?.Invoke($"模拟返回失败");
//             }
//
//             return;
// #endif
//             _sdk?.ShareVideoToRank(title, desc, tag, topics, success, fail, cancel);
//         }

//         /// <summary>
//         /// 请求视频点赞排行榜<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">抖音Android(XGP)</font><para/>
//         /// </summary>
//         /// <param name="tag">视频标签，仅支持全英文和数字文</param>
//         /// <param name="success">成功时触发，返回数据</param>
//         /// <param name="fail">失败时触发，返回错误信息</param>
//         /// <param name="numOfTop">拉去的排行榜条目，最多100</param>
//         /// <param name="rankType">榜单类型，支持周榜和月榜</param>
//         /// <param name="showToast">错误时是否提示toast,默认开启</param>
//         public void RequestVideoLikeRank(string tag, Action<VideoRankRsp> success, Action<string> fail,
//             int numOfTop = 100,
//             RankType rankType = RankType.Month, bool showToast = true)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] RequestVideoLikeRank tag：{tag} numOfTop：{numOfTop}");
//             if (_sdkPreference.RequestVideoRankResult)
//             {
//                 var path = $"Assets/XGameUnityTool/Editor/LocalTest/模拟数据-请求视频排行榜成功.json";
//                 Debug.Log($"使用测试数据模拟，可自行修改：{path}");
//                 WaitTo(0.3f, () =>
//                 {
//                     var res = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
//                     success?.Invoke(XJson.FromJson<VideoRankRsp>(res));
//                 });
//                 return;
//             }
//             else
//             {
//                 fail?.Invoke($"模拟返回失败");
//             }
//
//             return;
// #endif
//             _sdk?.RequestVideoLikeRank(tag, success, fail, numOfTop, rankType, showToast);
//         }
//
//         /// <summary>
//         /// 请求视频榜单（按发布时间排的）<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">抖音Android(XGP)</font><para/>
//         /// </summary>
//         /// <param name="tag">视频标签，仅支持全英文和数字文</param>
//         /// <param name="success">成功时触发，返回数据</param>
//         /// <param name="fail">失败时触发，返回错误信息</param>
//         /// <param name="numOfTop">拉去的排行榜条目，最多100</param>
//         /// <param name="showToast">错误时是否提示toast,默认开启</param>
//         public void RequestVideoTimeRank(string tag, Action<VideoRankRsp> success, Action<string> fail,
//             int numOfTop = 100, bool showToast = true)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] RequestVideoTimeRank tag：{tag} numOfTop：{numOfTop}");
//             if (_sdkPreference.RequestVideoRankResult)
//             {
//                 var path = $"Assets/XGameUnityTool/Editor/LocalTest/模拟数据-请求视频排行榜成功.json";
//                 Debug.Log($"使用测试数据模拟，可自行修改：{path}");
//                 WaitTo(0.3f, () =>
//                 {
//                     var res = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
//                     success?.Invoke(XJson.FromJson<VideoRankRsp>(res));
//                 });
//                 return;
//             }
//             else
//             {
//                 fail?.Invoke($"模拟返回失败");
//             }
//
//             return;
// #endif
//             _sdk?.RequestVideoTimeRank(tag, success, fail, numOfTop, showToast);
//         }
//
//         /// <summary>
//         /// 跳转到视频播放地址<para/>
//         /// <font color="#4dd276">支持渠道:</font><para/>
//         /// <font color="#4dd276">抖音Android(XGP)</font><para/>
//         /// </summary>
//         /// <param name="videoId">视频id</param>
//         /// <param name="success">成功回调</param>
//         /// <param name="fail">失败回调</param>
//         /// <param name="complete">完成回调（成功/失败后都会返回）</param>
//         public void NavigateToVideoView(string videoId, Action success, Action fail, Action complete)
//         {
// #if UNITY_EDITOR
//             Log($"[SDK调用成功] NavigateToVideoView {videoId}");
//             var path = $"Assets/XGameUnityTool/Editor/LocalTest/模拟数据-排行榜视频对应的URL.json";
//             Debug.Log($"使用测试数据模拟，可自行修改：{path}");
//             WaitTo(0.3f, () =>
//             {
//                 var res = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
//                 var data = XJson.FromJson<Dictionary<string, String>>(res);
//                 if (data.TryGetValue(videoId, out var match))
//                 {
//                     Debug.Log($"模拟播放视频，跳转到：{match}");
//                     Application.OpenURL(match);
//                     success?.Invoke();
//                 }
//                 else
//                 {
//                     Debug.Log($"跳转失败，模拟数据不存在{videoId}");
//                     fail.Invoke();
//                 }
//             });
//             return;
// #endif
//             _sdk?.NavigateToVideoView(videoId, success, fail, complete);
//         }
//
//         #endregion

        #region 跳转到订阅页

        /// <summary>
        /// 跳转到订阅页<para/>
        /// <font color="#4dd276">支持渠道:</font><para/>
        /// <font color="#4dd276">Google(XA),Google(XMY)</font><para/>
        /// </summary>
        public void SkipSubscribePage()
        {
            Log("[SDK调用成功] SkipSubscribePage");
#if UNITY_EDITOR
            Debug.Log("SkipSubscribePage调用成功，实际跳转请发布后进行测试");
            return;
#endif
            _sdk?.SkipSubscribePage();
        }

        #endregion

        #region 获取android assets 目录文件

        /// <summary>
        /// 获取android app 'assets/aa/Android'目录下的bundle文件<para/>
        /// 可用于检查Google国家分包情况资源<para/>
        /// </summary>
        public string[] GetAAAndroidBundleFiles()
        {
            if (_sdk != null)
            {
                return _sdk.GetAAAndroidBundleFiles();
            }

            var bundles = new HashSet<string>();
            var files = GetAndroidAssetsFiles("aa/Android");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (fileName.EndsWith(".bundle"))
                {
                    bundles.Add(fileName);
                }
            }

            return bundles.ToArray();
        }

        /// <summary>
        /// 获取android app assets 指定目录,从assets/开始<para/>
        /// 目标路径：assets/path/to/folder 填 'path/to/folder'<para/>
        /// 目标路径：assets/aa/bb 填 'aa/bb' <para/>
        /// </summary>
        public string[] GetAndroidAssetsFiles(string dir)
        {
            if (_sdk != null)
            {
                return _sdk.GetAndroidAssetsFiles(dir);
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaObject Context = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                    .GetStatic<AndroidJavaObject>("currentActivity")
                    .Call<AndroidJavaObject>("getApplicationContext");
                AndroidJavaObject assetManager = Context.Call<AndroidJavaObject>("getAssets");
                string[] fileNames = assetManager.Call<string[]>("list", dir);
                return fileNames;
            }

            return new string[0];
        }

        #endregion

        #region 隐私政策

        /// <summary>
        /// 主动弹出隐私协议
        /// </summary>
        public void ShowPrivacy()
        {
#if UNITY_EDITOR
            Log("[本地模拟] ShowPrivacy 调用成功！");
            return;
#endif
            _sdk?.ShowPrivacy();
        }

        /// <summary>
        /// 是否为欧盟地区<para/>
        /// 欧盟地区游戏需要有主动展示隐私协议的入口<para/>
        /// </summary>
        [Obsolete("逐步废弃，请改用IsSupportPrivacyBtn")]
        public bool IsEURegion()
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] IsEURegion 调用成功！ 返回Test params （XGameUnityTool/Channel Config）IsEURegion=={_sdkPreference.IsEURegion}");
            return _sdkPreference.IsEURegion;
#endif
            return _sdk?.IsEURegion() ?? false;
        }


        /// <summary>
        /// 是否支持隐私协议按钮<para/>
        /// 部分国家和地区需要有主动展示隐私协议的入口，此接口用于判断<para/>
        /// 当为true时，游戏内需要有弹出隐私协议的按钮，推荐放在设置页面中<para/>
        /// 点击按钮时调用：ShowPrivacy 打开会弹出隐私协议
        /// </summary>
        public bool IsSupportPrivacyBtn()
        {
#if UNITY_EDITOR
            var flag = _sdkPreference.IsSupportPrivacyBtn;
            Log(
                $"[本地模拟] IsSupportPrivacyBtn 返回Test params （XGameUnityTool/Channel Config）IsSupportPrivacyBtn == {flag} ");
            return flag;
#endif
            return _sdk?.IsSupportPrivacyBtn() ?? false;
        }

        #endregion

        #region 微信小游戏排行榜

        /// <summary>
        /// 根据服务端参数获取排行榜数据
        /// </summary>
        /// <param name="success">成功回调函数，在获取排行榜数据成功时调用，参数为排行榜数据</param>
        /// <param name="fail">失败回调函数，在获取排行榜数据失败时调用，参数为失败信息</param>
        public void GetRankData(RankData rankData, Action<RankResponseInfo[]> success, Action<string> fail)
        {
#if UNITY_EDITOR
            // 在 Unity 编辑器环境下，返回测试数据
            Log("[本地模拟] GetRankData 调用成功！返回测试数据");
            WaitTo(_sdkPreference.GetRankDataResultDelay, () =>
            {
                if (_sdkPreference.GetRankDataResult)
                {
                    // 成功时调用成功回调，并传递服务端返回的数据
                    success?.Invoke(ConvertToRankData(_sdkPreference.GetRankDataReturnData));
                }
                else
                {
                    fail?.Invoke("本地模拟触发失败 GetRankDataResult==false");
                }
            });
            return;
#endif

            // 实际调用获取排行榜数据的方法
            _sdk?.GetRankData(rankData, success, fail);
        }

        // 辅助方法：将 RankingInfo[] 转换为 RankData[]
        private RankResponseInfo[] ConvertToRankData(RankResponseInfo[] responseInfos)
        {
            List<RankResponseInfo> rankDataList = new List<RankResponseInfo>();
            foreach (var info in responseInfos)
            {
                // 创建 RankData 对象，并使用服务器返回的数据来初始化它
                RankResponseInfo rankData = new RankResponseInfo()
                {
                    rankId = info.rankId,
                    score = info.score,
                    extra = info.extra,
                    userId = info.userId
                };
                rankDataList.Add(rankData);
            }

            return rankDataList.ToArray();
        }


        /// <summary>
        /// 上传用户排行数据
        /// </summary>
        /// <param name="upScore">要上传的数据</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="fail">失败回调函数</param>
        public void UploadUserRankData(UpScore upScore, Action success, Action<string> fail)
        {
#if UNITY_EDITOR
            // 在 Unity 编辑器环境下，返回测试数据
            Log($"[本地模拟] UploadUserRankData 调用成功！返回测试数据:");
            WaitTo(_sdkPreference.GetUpLoadRankDataResultDelay, () =>
            {
                if (_sdkPreference.GetUpLoadRankDataResult)
                {
                    success?.Invoke();
                }
                else
                {
                    fail?.Invoke("本地模拟触发失败 GetUpLoadRankDataResult==false");
                }
            });
            return;
#endif

            // 实际调用上传用户排行数据的方法
            _sdk?.UploadUserRankData(upScore, success, fail);
        }

        #endregion

        #region Google-打开设置

        /// <summary>
        /// 打开手机wifi设置
        /// </summary>
        public void OpenWifiSettings()
        {
#if UNITY_EDITOR
            // 在 Unity 编辑器环境下，返回测试数据
            Log($"[本地模拟] OpenWifiSettings 调用成功！返回测试数据:");
            return;
#endif
            _sdk.OpenWifiSettings();
        }

        public void OpenPhoneSettings()
        {
#if UNITY_EDITOR
            // 在 Unity 编辑器环境下，返回测试数据
            Log($"[本地模拟] OpenPhoneSettings 调用成功！返回测试数据:");
            return;
#endif
            _sdk.OpenPhoneSettings();
        }

        #endregion

        #region 获取游戏的Json

        public void GetGameJson(string matchVersion, Action<string> onDataReceived)
        {
#if UNITY_EDITOR
            // 在 Unity 编辑器环境下，返回测试数据
            var GameJson = _sdkPreference.GameJson;
            onDataReceived?.Invoke(GameJson);
            Log($"[本地模拟] GetGameJson 调用成功！返回测试数据:" + GameJson);
            return;
#endif
            _sdk.GetGameJson(matchVersion, onDataReceived);
        }

        #endregion

        #region 获取登录成功后的功能白名单参数

        public void GetFeatureWhitelist(Action<string[]> success, Action fail)
        {
#if UNITY_EDITOR
            Log("[本地模拟] GetFeatureWhitelist 调用成功！返回空字符串数组");
            success?.Invoke(Array.Empty<string>());
            return;
#endif
            _sdk.GetFeatureWhitelist(success, fail);
        }

        #endregion

        #region 抖音排行榜

        public void SetImRankData(ImRankData imRankData, Action<bool, string> action)
        {
#if UNITY_EDITOR
            Log("[本地模拟] SetImRankData 调用成功！");
            action?.Invoke(true, "SetImRankData 调用成功");
            return;
#endif
            _sdk.SetImRankData(imRankData, action);
        }

        public void ShowImRankList(ImRankListInfo imRankListInfo, Action<bool, string> action)
        {
#if UNITY_EDITOR
            Log("[本地模拟] ShowImRankList 调用成功！");
            action?.Invoke(true, "ShowImRankList 调用成功");
            return;
#endif
            _sdk.ShowImRankList(imRankListInfo, action);
        }

        #endregion

        #region 资源分包pad

        public void RequestAssetDelivery(string packName, Action<string /*code*/, string /*data*/> action)
        {
#if UNITY_EDITOR
            Log("[本地模拟] RequestAssetDelivery 调用成功！");
            action?.Invoke("", "");
            return;
#endif
            _sdk.RequestAssetDelivery(packName, action);
        }

        public string GetAssetPath(string packName)
        {
#if UNITY_EDITOR
            Log("[本地模拟] GetAssetPath 调用成功！packName=" + packName);
            return "";
#endif
            return _sdk.GetAssetPath(packName);
        }

        public void RemovePack(string packName)
        {
#if UNITY_EDITOR
            Log("[本地模拟] RemovePack 调用成功！packName=" + packName);
            return;
#endif
            _sdk.RemovePack(packName);
        }

        public void CancelPack(string packName)
        {
#if UNITY_EDITOR
            Log("[本地模拟] CancelPack 调用成功！packName=" + packName);
            return;
#endif
            _sdk.CancelPack(packName);
        }

        #endregion

        #region 设备信息

        /// <summary>
        /// 获取设备信息。适用于XMY,LogSDK渠道
        /// </summary>
        /// <returns></returns>
        public DeviceInfo GetDeviceInfo()
        {
#if UNITY_EDITOR
            Log($"[本地模拟] GetDeviceInfo 调用成功！可以从'Channel Config/Test params' 中的“设备信息”设置模拟数据");
            DeviceInfo info;
            if (_sdkPreference.deviceInfo != null)
            {
                info = _sdkPreference.deviceInfo;
            }
            else
            {
                info = new DeviceInfo();
                info.gaid = SystemInfo.deviceUniqueIdentifier;
                info.GAID = info.gaid;
            }

            return info;
#endif
            if (_sdk != null)
            {
                return _sdk?.GetDeviceInfo();
            }

            return new DeviceInfo()
            {
                GAID = SystemInfo.deviceUniqueIdentifier,
                gaid = SystemInfo.deviceUniqueIdentifier,
            };
        }

        #endregion

        #region 快手小游戏-获取补贴信息

        public void GetSubsidyInfo(Action<SubsidyInfoResult> success, Action<string> fail)
        {
#if UNITY_EDITOR
            Log("[本地模拟] GetSubsidyInfo 调用成功！");
            var res = new SubsidyInfoResult();
            success?.Invoke(res);
            return;
#endif
            _sdk.GetSubsidyInfo(success, fail);
        }

        #endregion

        #region 获取安全区域位置大小

        public Rect GetSafeArea()
        {
#if UNITY_EDITOR
            var rect = Screen.safeArea;
            Log("[本地模拟] GetSafeArea 调用成功！" + rect);
            return rect;
#endif
            return _sdk.GetSafeArea();
        }

        #endregion

        #region 激励插屏

        public void ShowRewardInters(string sceneName, Action success, Action fail = null)
        {
#if UNITY_EDITOR
            Log("[本地模拟] ShowRewardInters 调用成功！");
            success?.Invoke();
            return;
#endif
            _sdk.ShowRewardInters(sceneName, success, fail);
        }

        public bool GetRewardIntersFlag()
        {
            return HasRewardInters();
        }

        public bool HasRewardInters()
        {
#if UNITY_EDITOR
            Log("[本地模拟] HasRewardInters 返回true ");
            return true;
#endif
            return _sdk.HasRewardInters();
        }

        #endregion

        #region 延时执行

        public void StartDelayedCallback(float delay, Action callback)
        {
            if (callback == null)
            {
                Log("Callback is null. No action will be executed.");
                return;
            }

            StartCoroutine(DelayedExecutionCoroutine(delay, callback));
        }

        // 协程方法：用于延时执行回调
        private IEnumerator DelayedExecutionCoroutine(float delay, Action callback)
        {
            // 等待指定的时间
            yield return new WaitForSeconds(delay);

            // 执行回调
            callback();
        }

        #endregion


        #region Google play Game服务

        /// <summary>
        /// Google play Game登录
        /// </summary>
        public void LoginGooglePlayGame(Action<LoginGooglePlayGameResult> callback)
        {
#if UNITY_EDITOR
            Log("[本地模拟] StartSnapshot 调用成功！");
            var a = new LoginGooglePlayGameResult();
            a.code = GooglePlayGameCode.LoginPass;
            a.playerId = "123123";
            a.playerName = "test";
            callback?.Invoke(a);
            return;
#endif

            _sdk.LoginGooglePlayGame(callback);
        }
        
        /// <summary>
        /// 获取Google play Game 登录状态，不会触发登录界面，用于获取如果自动登录成功时的用户信息
        /// </summary>
        public void IsAuthenticatedGooglePlayGame(Action<LoginGooglePlayGameResult> callback)
        {
#if UNITY_EDITOR
            Log("[本地模拟] StartSnapshot 调用成功！");
            var a = new LoginGooglePlayGameResult();
            a.code = GooglePlayGameCode.LoginAuthPass;
            a.playerId = "123123";
            a.playerName = "test";
            callback?.Invoke(a);
            return;
#endif

            _sdk.IsAuthenticatedGooglePlayGame(callback);
        }

        /// <summary>
        /// 加载快照存档数据
        /// </summary>
        public void LoadSnapshot(Action<LoadSnapshotResult> callback)
        {
#if UNITY_EDITOR
            Log("[本地模拟] LoadSnapshot 调用成功！");
            var s = new LoadSnapshotResult();
            s.code = GooglePlayGameCode.LoginNot;
            s.failMsg = "login no pass";
            callback?.Invoke(s);
            return;
#endif
            _sdk.LoadSnapshot(callback);
        }

        /// <summary>
        /// 保存快照存档数据
        /// </summary>
        public virtual void SaveSnapshot(string data, Action<SaveSnapshotResult> callback)
        {
#if UNITY_EDITOR
            Log("[本地模拟] SaveSnapshot 调用成功！");
            var s = new SaveSnapshotResult();
            s.code = GooglePlayGameCode.LoginNot;
            s.failMsg = "login no pass";
            callback?.Invoke(s);
            return;
#endif
            _sdk.SaveSnapshot(data, callback);
        }

        /// <summary>
        /// 提交指定排行榜的成绩
        /// </summary>
        public void SubmitScore(string leaderboardId, long score)
        {
#if UNITY_EDITOR
            Log($"[本地模拟] SubmitScore 调用成功！leaderboardId={leaderboardId}, score={score}");
            return;
#endif
            _sdk.SubmitScore(leaderboardId, score);
        }

        /// <summary>
        /// 展示排行榜，空字符串展示总排行榜
        /// </summary>
        public void ShowLeaderboard(string leaderboardId = "")
        {
#if UNITY_EDITOR
            Log($"[本地模拟] ShowLeaderboard 调用成功！leaderboardId={leaderboardId}");
            return;
#endif
            _sdk.ShowLeaderboard(leaderboardId);
        }

        /// <summary>
        /// 加载当前玩家排行榜成绩信息
        /// <param name="leaderboardId">排行榜ID</param>
        /// <param name="span">检索数据的时间范围</param>
        /// <param name="leaderboardCollection">检索分数的排行榜集合</param>
        /// </summary>
        public void LoadCurrentPlayerLeaderboardScore(string leaderboardId, LeaderboardTimeSpan span,
            LeaderboardCollection leaderboardCollection, Action<CurrentPlayerLeaderboardScoreLoadResult> callback)
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] LoadCurrentPlayerLeaderboardScore 调用成功！leaderboardId={leaderboardId} span={span} leaderboardCollection={leaderboardCollection}");
            return;
#endif
            _sdk.LoadCurrentPlayerLeaderboardScore(leaderboardId, span, leaderboardCollection, callback);
        }

        /// <summary>
        /// 加载顶部玩家排行榜成绩信息
        /// <param name="leaderboardId">排行榜ID</param>
        /// <param name="span">检索数据的时间范围</param>
        /// <param name="leaderboardCollection">检索分数的排行榜集合</param>
        /// <param name="maxResults">最大玩家成绩数量，范围[1,25]</param>
        /// <param name="forceReload">是否强制重新加载，推荐添加一个刷新按钮由用户点击刷新数据，充分利用缓存。</param>
        /// </summary>
        public void LoadTopScores(string leaderboardId, LeaderboardTimeSpan span,
            LeaderboardCollection leaderboardCollection, int maxResults, bool forceReload,
            Action<TopScoresLoadResult> callback)
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] LoadTopScores 调用成功！leaderboardId={leaderboardId} span={span} leaderboardCollection={leaderboardCollection} maxResults={maxResults} forceReload={forceReload}");
            return;
#endif
            _sdk.LoadTopScores(leaderboardId, span, leaderboardCollection, maxResults, forceReload, callback);
        }


        /// <summary>
        /// 展示成就界面
        /// </summary>
        public void ShowAchievements()
        {
#if UNITY_EDITOR
            Log($"[本地模拟] ShowAchievements 调用成功！");
            return;
#endif
            _sdk.ShowAchievements();
        }

        /// <summary>
        /// 解锁成就
        /// </summary>
        public void UnlockAchievement(string achievementId)
        {
#if UNITY_EDITOR
            Log($"[本地模拟] UnlockAchievement 调用成功！achievementId={achievementId}");
            return;
#endif
            _sdk.UnlockAchievement(achievementId);
        }

        /// <summary>
        /// 提交本次成就进度。达到谷歌后台设置的目标解锁成就
        /// <param name="achievementId">成就ID</param>
        /// <param name="incrementValue">进度值，跟上次提交的值累加计算</param>
        /// </summary>
        public void IncrementAchievement(string achievementId, int incrementValue)
        {
#if UNITY_EDITOR
            Log($"[本地模拟] IncrementAchievement 调用成功！achievementId={achievementId}, incrementValue={incrementValue}");
            return;
#endif
            _sdk.IncrementAchievement(achievementId, incrementValue);
        }

        #endregion

        #region XMY的消息推送

        /// <summary>
        /// 是否需要请求消息推送权限
        /// </summary>
        public bool IsRequirePushPermission()
        {
#if UNITY_EDITOR
            Log("[本地模拟] IsRequirePushPermission 调用成功！");
            return true;
#endif
            return _sdk.IsRequirePushPermission();
        }


        /// <summary>
        /// 开启消息推送权限
        /// </summary>
        public void OpenPush()
        {
#if UNITY_EDITOR
            Log("[本地模拟] OpenPush 调用成功！");
            return;
#endif
            _sdk.OpenPush();
        }

        /// <summary>
        /// 获取推送的激励信息
        /// </summary>
        /// <param name="key">自定义的奖励key，不传默认 REWARD_PUSH_DATA</param>
        /// <returns>推送后台填写的值, 可能为empty</returns>
        public string GetPushRewardMessage(string key = "REWARD_PUSH_DATA")
        {
#if UNITY_EDITOR
            Log("[本地模拟] GetPushRewardMessage 调用成功！");
            return "";
#endif
            return _sdk.GetPushRewardMessage(key);
        }

        #endregion

        #region XMY的互推

        /// <summary>
        /// 获取互推信息列表
        /// </summary>
        /// <param name="type">互推类型，比如：icon，inters_portrait，inters_landscape，banner</param>
        /// <param name="count">最大数量</param>
        /// <returns>互推信息数组</returns>
        public CrossInfo[] GetCrossList(string type, int count)
        {
#if UNITY_EDITOR
            var res = Array.Empty<CrossInfo>();
            var arr = _sdkPreference.CrossInfos;
            if (null != arr)
            {
                var infos = arr.Where(info => info.type == type);
                if (infos.Count() > count)
                {
                    res = infos.Take(count).ToArray();
                }
            }

            Log($"[本地模拟] GetCrossList 调用成功！type={type}, count={count}, CrossInfos={res.ToXJson()}");
            return res;
#endif
            return _sdk.GetCrossList(type, count);
        }

        /// <summary>
        /// 互推事件上报(比如：展示，点击)
        /// </summary>
        /// <param name="action">互推事件类型</param>
        /// <param name="crossInfo">互推信息</param>
        /// <param name="sceneId">上报的场景</param>
        public void CrossAction(CrossActionType action, CrossInfo crossInfo, string sceneId)
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] CrossAction action={action.ToString()}, crossInfo={crossInfo?.ToXJson()}, sceneId={sceneId}, 调用成功！");
            return;
#endif
            _sdk.CrossAction(action, crossInfo, sceneId);
        }

        /// <summary>
        /// 获取互推的激励信息
        /// </summary>
        /// <param name="key">自定义的奖励key，不传默认 REWARD_CROSS_DATA</param>
        /// <returns>我们互推后台填的值, 可能为empty</returns>
        public string GetCrossRewardMessage(string key = "REWARD_CROSS_DATA")
        {
#if UNITY_EDITOR
            Log("[本地模拟] GetCrossRewardMessage 调用成功！");
            return "";
#endif
            return _sdk.GetCrossRewardMessage(key);
        }

        #endregion

        #region XMY应用更新

        /// <summary>
        /// 判断Google商店是否有新版本
        /// </summary>
        public bool IsAvailableNewVersion()
        {
#if UNITY_EDITOR
            Log("[本地模拟] IsAvailableNewVersion 调用成功！");
            return true;
#endif
            return _sdk.IsAvailableNewVersion();
        }

        /// <summary>
        /// 如果有新版本的情况，立即更新
        /// </summary>
        public void StartUpdateNewVersion()
        {
#if UNITY_EDITOR
            Log("[本地模拟] StartUpdateNewVersion 调用成功！");
            return;
#endif
            _sdk.StartUpdateNewVersion();
        }

        #endregion


        #region 请求订阅信息通知

        /// <summary>
        /// 请求订阅信息通知
        /// </summary>
        /// <param name="tmplIds">模板ID，一次最多五个。B站固定模板ID：NEW_VERSION : 小游戏版本更新</param>
        /// <param name="success">成功回调，返回Dictionary参数，key: 模板ID : value: 值包括'accept'(用户同意订阅)、'reject'(用户拒绝订阅)、'ban'(已被后台封禁)</param>
        /// <param name="fail">失败回调，返回错误码和错误信息</param>
        public void RequestSubscribeMessage(string[] tmplIds, Action<Dictionary<string, string>> success,
            Action<int, string> fail)
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] RequestSubscribeMessage 调用成功！tmplIds={tmplIds}，可以从（XGameUnityTool/Channel Config->Test params）IsRequestSubscribeMessageSuc 和 RequestSubscribeMessageSuccessParams set Test params");

            if (_sdkPreference.IsRequestSubscribeMessageSuc)
            {
                var dic = new Dictionary<string, string>();
                if (null != _sdkPreference.RequestSubscribeMessageSuccessParams)
                {
                    foreach (var keyValuePair in _sdkPreference.RequestSubscribeMessageSuccessParams)
                    {
                        dic.Add(keyValuePair.Key, keyValuePair.Value.ToString());
                    }
                }

                if (null != tmplIds)
                {
                    foreach (var tmplId in tmplIds)
                    {
                        if (!dic.ContainsKey(tmplId))
                        {
                            dic.Add(tmplId, "accept");
                        }
                    }
                }


                success?.Invoke(dic);
            }
            else
            {
                fail?.Invoke(444, "失败");
            }


            return;
#endif
            _sdk.RequestSubscribeMessage(tmplIds, success, fail);
        }

        #endregion


        #region 抖音的推荐流直出游戏

        /// <summary>
        /// 抖音推荐流直出游戏功能，
        /// 需要初始化成功后调用，
        /// 获取启动参数来判断加载显示对应游戏界面
        /// </summary>
        /// <returns></returns>
        public FeedDirectPlayInfo GetLaunchInfoForFeedDirectPlay()
        {
#if UNITY_EDITOR
            Log($"[本地模拟] GetLaunchInfoForFeedDirectPlay");
            var info = new FeedDirectPlayInfo();
            info.gameScene = FeedDirectPlayInfo.GameScene.OfflineEarnings;
            return info;
#endif
            return _sdk.GetLaunchInfoForFeedDirectPlay();
        }

        /// <summary>
        /// 抖音推荐流直出游戏功能，需要初始化成功后调用，向平台侧传达游戏场景加载完成、达到可交互状态的时机，同时也便于开发者进行场景加载耗时统计和分析
        /// </summary>
        /// <param name="costTimeMs">加载完成耗时，单位毫秒</param>
        public void ReportGameReadyForFeedDirectPlay(long costTimeMs)
        {
#if UNITY_EDITOR
            Log($"[本地模拟] GameReadyReportForFeedDirectPlay costTimeMs={costTimeMs}");
            return;
#endif
            _sdk.ReportGameReadyForFeedDirectPlay(costTimeMs);
        }

        /// <summary>
        /// 抖音推荐流直出游戏功能，需要游戏登录成功后调用，查询该功能用户是否订阅
        /// </summary>
        /// <param name="scene">（非全场景下必传），订阅的场景</param>
        /// <param name="isAllScene">是否为全场景订阅</param>
        /// <param name="success">是否已经订阅回调</param>
        /// <param name="failed">查询失败回调</param>
        public void CheckSubscribeStatusForFeedDirectPlay(FeedDirectPlayInfo.GameScene scene, bool isAllScene,
            Action<bool> success, Action<int, string> failed)
        {
#if UNITY_EDITOR
            Log($"[本地模拟] CheckSubscribeStatusForFeedDirectPlay scene={scene} isAllScene={isAllScene}");
            success?.Invoke(false);
            return;
#endif
            _sdk.CheckSubscribeStatusForFeedDirectPlay(scene, isAllScene, success, failed);
        }

        /// <summary>
        /// 抖音推荐流直出游戏功能，需要游戏登录成功后调用，向用户请求订阅该功能
        /// </summary>
        /// <param name="scene">（非全场景下必传），订阅的场景</param>
        /// <param name="contentIds">（非全场景下必传），在抖音后台申请开通直玩能力后可获取</param>
        /// <param name="isAllScene">是否为全场景订阅</param>
        /// <param name="success">是否订阅成功回调</param>
        /// <param name="failed">订阅失败回调</param>
        public void RequestSubscribeForFeedDirectPlay(FeedDirectPlayInfo.GameScene scene, List<string> contentIds,
            bool isAllScene, Action<bool> success, Action<int, string> failed)
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] RequestSubscribeForFeedDirectPlay scene={scene} contentIds={contentIds?.ToXJson()} isAllScene={isAllScene}");
            success?.Invoke(true);
            return;
#endif
            _sdk.RequestSubscribeForFeedDirectPlay(scene, contentIds, isAllScene, success, failed);
        }

        /// <summary>
        /// 抖音推荐流直出游戏功能，需要游戏登录成功后调用，存储游戏是否直玩就绪状态
        /// </summary>
        /// <param name="scene">订阅的场景 ID</param>
        /// <param name="contentId">自定义文案的 contentID，contentID 在后台申请开通直玩能力后可获取</param>
        /// <param name="status">满足运算公式后，对应直玩场景是否就绪。0：未就绪， 1：就绪</param>
        /// <param name="rightValue">运算公式的右值</param>
        /// <param name="operatorType">运算符，比如 =,=>,!=,>,《=，》</param>
        /// <param name="leftValue">运算公式的左值，当前只支持"timeStampMs"，即毫秒级时间戳</param>
        /// <param name="extra">可选，自定义补充字段</param>
        /// <param name="success">成功回调</param>
        /// <param name="failed">失败回调</param>
        public virtual void StoreFeedDataForFeedDirectPlay(FeedDirectPlayInfo.GameScene scene, string contentId,
            int status, string rightValue, string operatorType = ">=", string leftValue = "timeStampMs",
            string extra = null, Action success = null, Action<int, string> failed = null)
        {
#if UNITY_EDITOR
            Log(
                $"[本地模拟] StoreFeedDataForFeedDirectPlay scene={scene}, contentId={contentId}, status={status}, leftValue={leftValue}, operatorType={operatorType}, rightValue={rightValue}, extra={extra}");
            success?.Invoke();
            return;
#endif
            _sdk.StoreFeedDataForFeedDirectPlay(scene, contentId, status, rightValue, operatorType, leftValue, extra,
                success, failed);
        }

        #endregion


        #region 监听切入切出

        /// <summary>
        /// 设置切入切出监听.
        /// 适用渠道：XMY
        /// </summary>
        /// <param name="callback">监听回调，回调参数是扩展参数，无用时不用管</param>
        public void SetInOutCallback(Action<string> callback)
        {
#if UNITY_EDITOR
            Log($"[本地模拟] 调用SetInOutCallback 成功");
            return;
#endif
            _sdk.SetInOutCallback(callback);
        }

        #endregion
        
        
        #region 获取小游戏启动参数
        /// <summary>
        /// 获取小游戏启动参数，需要在初始化回调成功后调用，支持渠道：抖音
        /// </summary>
        /// <returns>字典对象，具体的key-value参考小游戏文档</returns>
        public Dictionary<string, object> GetLaunchOptionsSync()
        {
#if UNITY_EDITOR
            Log($"[本地模拟] 调用 GetLaunchOptionsSync 成功");
            return null;
#endif
            return _sdk.GetLaunchOptionsSync();
        }
        
        #endregion


        #region 小游戏前后台回调监听
        
        /// <summary>
        /// 小游戏进入前台监听，支持渠道：抖音
        /// </summary>
        /// <param name="callback">回调，回调参数具体的key-value参考小游戏文档</param>
        public void OnMiniGameShow(Action<Dictionary<string, object>> callback)
        {
#if UNITY_EDITOR
            Log($"[本地模拟] 调用 OnMiniGameShow 成功");
            return;
#endif
            _sdk.OnMiniGameShow(callback);
        }
        
        /// <summary>
        /// 小游戏进入后台监听，支持渠道：抖音
        /// </summary>
        /// <param name="callback">回调</param>
        public void OnMiniGameHide(Action<Dictionary<string, object>> callback)
        {
#if UNITY_EDITOR
            Log($"[本地模拟] 调用 OnMiniGameHide 成功");
            return;
#endif
            _sdk.OnMiniGameHide(callback);

        }

        #endregion
        
        
    }
}