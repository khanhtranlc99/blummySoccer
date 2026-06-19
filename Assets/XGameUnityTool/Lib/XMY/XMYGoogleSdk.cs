using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// xmy sdk<para/>
    /// <font color="5bdb5b">更新工具包后可能遇到接口不一致导致的编译报错,在'发布'窗口中应用后可处理</font><para/>
    /// <font color="5bdb5b">如果无法打开'发布'窗口执行应用,尝试手动删除'Assets/XGameUnityTool/Lib'文件夹，编译通过后重试'</font><para/>
    /// </summary>
    [Preserve]
    public class XMYGoogleSdk : BaseSdk
    {
        private const string NAME_ACTION = "action"; //行为
        private const string NAME_TIME_STAMP_MS = "time_stamp_ms"; //时间戳毫秒
        private const string NAME_ENGINE_TIME = "engine_time_since_start"; //引擎时间，从启动开始

        public AndroidUnityPlayerInstance UnityPlayerJava; //Android UnityPlayer java实例
        private XMYGoogleCallback _callback; //回调

        private bool _isCallGoogleLogin;
        private Action<string, string> _loginGoogleSuccess; //登录成功回调
        private Action _loginGoogleFail; //登录失败回调

        private bool _isCallLogin;
        private Action _loginSuccess; //登录成功回调
        private Action _loginFail; //登录失败回调

        private Action<bool> _onVideoDelegate; //视频广告回调
        private Action<bool> _onRewardIntersDelegate; //激励插屏广告回调
        private XMYAdBridge _xmyAdBridge = null; //广告模块
        private Action<string> _remoteConfigResult; //云控配置回调
        private Action<string> _remoteConfigMultiResult; //云控多次 OnRemoteResult：仅保留最后一次注册的回调
        private RequestProductInfoResult _requestProductInfoResult; //请求商品信息回调
        private RequestSubscribeStatesResult _requestSubscribeStatesResult; //订阅信息回调
        private IADResultHandlerInvoker _bannerInvoker;
        private IADResultHandlerInvoker _intersInvoker;
        private IADResultHandlerInvoker _videoInvoker;
        private IADResultHandlerInvoker _bigNativeInvoker;

        private bool _videoRewardFag; //视频广告奖励发放标记
        private bool _intersRewardFag; //激励插屏广告奖励发放标记

        // 插页广告展示回调委托
        private Action _intersShowDelegate;

        //用户ID
        private string _userId;
        private string _userPicUrl;

        //云存档加载器
        // private XMYCloudArchiveLoader _archiveLoader;
        // private XMYCloudArchiveManager _cloudArchiveManager;


        //广告实例
        private XMYAdBridge XMYAdBridge
        {
            get
            {
                if (_xmyAdBridge == null)
                {
                    _xmyAdBridge = new XMYAdBridge();
                }

                return _xmyAdBridge;
            }
        }

        //缓存商品信息
        private ProductInfo[] _cacheProducts;

        //缓存的订阅信息
        private SubscribeState[] _cacheSubscribeState;

        //插屏关闭回调
        private Action _intersCloseDelegate;

        private Action _nativeCloseDelegate;


        private Action<string, string> _requestAssetDeliveryResult;

        protected override void OnCreate()
        {
            Debug.Log("[XMYGoogleSdk] OnCreate");
            //构建实例
            UnityPlayerJava = new AndroidUnityPlayerInstance();
            //创建回调实例
            Log("create (unsdk_callback)");
            var clone = new GameObject("(unsdk_callback)");
            _callback = clone.AddComponent<XMYGoogleCallback>();
            GameObject.DontDestroyOnLoad(clone);
            //初始化回调
            _callback.Initialize(OnLoginResult, OnPayResult, OnAdResult, OnSnapshotResult,
                OnRemoteConfigResult,
                OnProductInfoResult, OnReqSubStateResult, OnFetchPadResult, OnInOutResult);
            _bannerInvoker = XGameADListener.BannerHandler as ADResultHandler;
            _intersInvoker = XGameADListener.InterHandler as ADResultHandler;
            _videoInvoker = XGameADListener.VideoHandler as ADResultHandler;
            _bigNativeInvoker = XGameADListener.BigNativeHandler as ADResultHandler;
            // _archiveLoader = XMYCloudArchiveLoader.CreateInstance(LoadCloudArchive, SaveCloudArchive);
            // _cloudArchiveManager = XMYCloudArchiveManager.CreateInstance(LoadCloudArchive, SaveCloudArchive);
            Debug.Log("[XMYGoogleSdk] OnCreate Done!");

            //构建SDK实例
            // TrackXGameSdkEvent("xgame_sdk_on_create_done");
        }

        #region 初始化sdk

        public override void InitSdk(Action success, Action fail)
        {
            // TrackXGameSdkEvent("xgame_sdk_init_sdk");
            Log($"InitSdk");
            //无实际逻辑，直接成功
            success?.Invoke();
        }

        #endregion

        #region 登录

        public override void LoginGoogle(Action<string, string> success, Action fail)
        {
            // TrackXGameSdkEvent("xgame_sdk_login");
            _isCallGoogleLogin = true;
            _loginGoogleSuccess = success;
            _loginGoogleFail = fail;
            Log("LoginGoogle...");
            UnityPlayerJava.Call("login");
        }

        public override void Login(Action success, Action fail)
        {
            // TrackXGameSdkEvent("xgame_sdk_login");
            _isCallLogin = true;
            _loginSuccess = success;
            _loginFail = fail;
            Log("Login...");
            UnityPlayerJava.Call("login");
        }

        private void OnLoginResult(XMYLoginResult result)
        {
            // TrackXGameSdkEvent("xgame_sdk_on_login_result");
            Log($"OnLoginResult...{result}");
            if (result == null)
            {
                // TrackXGameSdkEvent("xgame_sdk_on_login_error");
                //登录失败
                if (_isCallGoogleLogin)
                {
                    _isCallGoogleLogin = false;
                    Log($"google login fail");
                    _loginGoogleFail?.Invoke();
                }

                if (_isCallLogin)
                {
                    _isCallLogin = false;
                    Log($"login fail");
                    _loginFail?.Invoke();
                }
            }
            else
            {
                // TrackXGameSdkEvent("xgame_sdk_on_login_success");

                if (_isCallGoogleLogin)
                {
                    _isCallGoogleLogin = false;
                    if (!string.IsNullOrEmpty(result.uid) && result.uid != "test" && result.uid != "null")
                    {
                        _userId = result.uid;
                        _userPicUrl = result.userPic;
                        Log($"google login success uid={result.uid}, name={result.name}");
                        _loginGoogleSuccess?.Invoke(result.uid, result.name ?? result.uid);
                    }
                    else
                    {
                        Log($"google login fail1");
                        _loginGoogleFail?.Invoke();
                    }
                }

                if (_isCallLogin)
                {
                    _isCallLogin = false;
                    Log($"login success");
                    _loginSuccess?.Invoke();
                }

                // TrackXGameSdkEvent("xgame_sdk_on_login_done");
            }
        }

        #endregion

        #region 获取用户ID,用户头像

        public override string GetSDKUserID()
        {
            Log($"GetSDKUserID={_userId}");
            return _userId;
        }

        public override string GetSdkUID()
        {
            Log($"GetSdkUID={_userId}");
            return _userId;
        }

        public override string GetSDKUserPicUrl()
        {
            Log($"GetSDKUserPicUrl={_userPicUrl}");
            return _userPicUrl;
        }

        #endregion


        #region Banner

        public override void ShowBanner(BannerType bannerType)
        {
            Log($"ShowBanner...{bannerType}");
            XMYAdBridge.Call("showBannerAd");
        }

        public override void HideBanner()
        {
            Log($"HideBanner");
            XMYAdBridge.Call("hideBannerAd");
        }

        #endregion

        #region 插页广告

        public override bool HasInters()
        {
            var flag = XMYAdBridge.CallReturn<bool>("isIntersFlag");
            Log($"HasInters flag:{flag}");
            return flag;
        }

        public override bool CanShowInters()
        {
            var flag = XMYAdBridge.CallReturn<bool>("isCanShow");
            Log($"CanShowInters flag:{flag}");
            return flag;
        }

        public override void ShowInters(string sceneName = "unknown", Action onClose = null)
        {
            // 设置插屏广告展示回调
            _intersShowDelegate = null;
            _intersCloseDelegate = onClose;
            //绑定插页关闭回调
            Log($"ShowInters：{sceneName}");
            XMYAdBridge.Call("showInterstitialAd", sceneName);
        }

        public override void ShowInterstitial(string sceneName, Action onShow, Action onClose = null)
        {
            Log($"ShowInterstitial");

            // 设置插屏广告展示回调
            _intersShowDelegate = onShow;
            // 设置插屏广告关闭回调
            _intersCloseDelegate = onClose;

            // 调用广告桥接器展示插屏广告
            XMYAdBridge.Call("showInterstitialAd", sceneName);

            // 这里不直接返回插屏广告是否可展示的信息，而是由回调函数通知调用者插屏广告是否成功展示
        }

        #endregion

        #region 视频广告

        public override bool HasVideo()
        {
            var flag = XMYAdBridge.CallReturn<bool>("isRewardFlag");
            Log($"HasVideo flag:{flag}");
            return flag;
        }

        public override void ShowVideo(string sceneName, Action success, Action fail = null)
        {
            //重新绑定视频回调
            _onVideoDelegate = (ret) =>
            {
                if (ret)
                {
                    success?.Invoke();
                }
                else
                {
                    fail?.Invoke();
                }
            };
            Log($"ShowVideo sceneName：{sceneName}");
            XMYAdBridge.Call("showRewardAd", sceneName);
        }

        #endregion

        #region 模板广告

        //模板广告加载标记
        public override bool GetTemplateAdFlag()
        {
            Log($"GetTemplateAdFlag...");
            var flag = XMYAdBridge.CallReturn<bool>("isTemplateFlag");
            Log($"GetTemplateAdFlag：{flag}");
            return flag;
        }

        public override void ShowTemplateAd(string scene)
        {
            Log($"ShowTemplateAd：{scene}");
            XMYAdBridge.Call("showTemplateAd", scene);
        }

        public override void HideTemplateAd()
        {
            Log($"HideTemplateAd");
            XMYAdBridge.Call("hideTemplateAd");
        }

        #endregion

        #region 原生大图

        public override bool HasBigNative()
        {
            var flag = XMYAdBridge.CallReturn<bool>("isNativeFlag");
            Log($"HasBigNative flag:{flag}");
            return flag;
        }

        public override void ShowBigNative(string scene)
        {
            Log($"ShowBigNative scene:{scene}");
            XMYAdBridge.Call("showNativeAd", scene);
        }

        public override void HideBigNative()
        {
            Log($"HideBigNative");
            XMYAdBridge.Call("hideNativeAd");
        }

        #region 原生广告

        public override bool HasNative()
        {
            var flag = XMYAdBridge.CallReturn<bool>("isNativeFlag");
            Log($"HasBigNative flag:{flag}");
            return flag;
        }

        public override void ShowNativeAd(string scene)
        {
            Log($"ShowNative scene:{scene}");
            XMYAdBridge.Call("showNativeAd", scene);
        }

        public override void ShowNativeAd(string scene, Action onClose)
        {
            Log($"ShowNative scene:{scene}, onClose={onClose}");
            _nativeCloseDelegate = onClose;
            XMYAdBridge.Call("showNativeAd", scene);
        }

        public override void HideNative()
        {
            Log($"HideNative");
            XMYAdBridge.Call("hideNativeAd");
        }

        #endregion

        #endregion

        #region 贴片广告

        public override bool GetPatchAdFlag()
        {
            Log($"GetPatchAdFlag...");
            var flag = XMYAdBridge.CallReturn<bool>("getPatchFlag");
            Log($"GetPatchAdFlag flag:{flag}");
            return flag;
        }

        public override void ShowPatchAd(string scene, PatchAdType type, float xNormalize, float yNormalize,
            float widthNormalize,
            float heightNormalize)
        {
            var x = Mathf.Clamp01(xNormalize) * 100;
            var y = Mathf.Clamp01(1 - (yNormalize + heightNormalize)) * 100;
            Log($"ShowPatchAd 游戏端左上角位置：({xNormalize},{yNormalize}) --> SDK端左下角位置({x}%,{y}%)");
            var param = new
            {
                sceneId = scene, type = (int)type, width = widthNormalize * 100, height = heightNormalize * 100,
                posx = x, posy = y
            };
            var json = param.ToXJson();
            Log($"ShowPatchAd {json} ");
            XMYAdBridge.Call("showPatchAd", json);
        }

        public override void HidePatchAd()
        {
            Log($"HidePatchAd");
            XMYAdBridge.Call("hidePatchAd");
        }

        #endregion

        #region 激励插屏

        public override void ShowRewardInters(string sceneName, Action success, Action fail = null)
        {
            //重新绑定视频回调
            _onRewardIntersDelegate = (ret) =>
            {
                if (ret)
                {
                    success?.Invoke();
                }
                else
                {
                    fail?.Invoke();
                }
            };
            Log($"ShowRewardInters sceneName：{sceneName}");
            XMYAdBridge.Call("showRewardInterstitialAd", sceneName);
        }

        public override bool HasRewardInters()
        {
            var flag = XMYAdBridge.CallReturn<bool>("isRewardIntersFlag");
            Log($"HasRewardInters flag:{flag}");
            return flag;
        }

        #endregion


        #region 广告回调

        //广告回调
        private void OnAdResult(XMYAdResult result)
        {
            Log($"OnAdResult：adtype:{result.adType},eventType:{result.eventType},scene:{result.adId}");
            //触发激励视频奖励
            if (result.adType == XMYAdResult.AD_TYPE_VIDEO && result.eventType == XMYAdResult.AD_EVENT_REWARDED)
            {
                _videoRewardFag = true;
                _onVideoDelegate?.Invoke(true);

                _intersRewardFag = true;
                _onRewardIntersDelegate?.Invoke(true);
                _onRewardIntersDelegate = null;
            }

            IADResultHandlerInvoker invoker = null;
            //触发其它事件
            switch (result.adType)
            {
                case XMYAdResult.AD_TYPE_BANNER:
                    invoker = _bannerInvoker;
                    break;
                case XMYAdResult.AD_TYPE_INTERS:
                    invoker = _intersInvoker;
                    break;
                case XMYAdResult.AD_TYPE_VIDEO:
                    invoker = _videoInvoker;
                    break;
                case XMYAdResult.AD_TYPE_NATIVE:
                    invoker = _bigNativeInvoker;
                    break;
            }

            switch (result.eventType)
            {
                case XMYAdResult.AD_EVENT_SHOWED:
                {
                    //视频广告,重置标记
                    if (result.adType == XMYAdResult.AD_TYPE_VIDEO)
                    {
                        _videoRewardFag = false;
                    }

                    // 触发插页广告展示回调
                    if (result.adType == XMYAdResult.AD_TYPE_INTERS)
                    {
                        var temp = _intersShowDelegate;
                        _intersShowDelegate = null;
                        temp?.Invoke();
                    }

                    invoker?.InvokeOnShowSuccess();
                }
                    break;
                case XMYAdResult.AD_EVENT_CLOSED:
                {
                    if (invoker == _videoInvoker)
                    {
                        //视频广告关闭
                        if (!_videoRewardFag)
                        {
                            _onVideoDelegate?.Invoke(false);
                        }

                        if (!_intersRewardFag)
                        {
                            _onRewardIntersDelegate?.Invoke(false);
                            _onRewardIntersDelegate = null;
                        }
                    }

                    if (invoker == _intersInvoker)
                    {
                        //插页关闭回调
                        _intersCloseDelegate?.Invoke();
                        XGameSdk.Log($"插页关闭回调{_intersCloseDelegate == null}");
                    }

                    if (invoker == _bigNativeInvoker)
                    {
                        //原生关闭回调
                        _nativeCloseDelegate?.Invoke();
                        XGameSdk.Log($"原生关闭回调{_nativeCloseDelegate == null}");
                    }

                    invoker?.InvokeOnAdClosed();
                }
                    break;
                case XMYAdResult.AD_EVENT_CLICK:
                {
                    if (invoker == _intersInvoker || invoker == _videoInvoker)
                    {
                        invoker?.InvokeOnAdClicked();
                    }
                }
                    break;
            }
        }

        #endregion

        #region 支付

        public override void Pay(int price, string productId, string productName, string productDesc)
        {
            Pay(price, productId, productName, productDesc, null);
        }

        public override void Pay(int price, string productId, string productName, string productDesc, string offerToken)
        {
            Log(
                $"Pay price:{price}, productId:{productId}, productName:{productName}, productDesc:{productDesc}, offerToken:{offerToken}");
            var map = new Dictionary<string, object>();
            map.Add("productId", productId);
            if (!string.IsNullOrEmpty(productName))
            {
                map.Add("productName", productName);
            }

            if (!string.IsNullOrEmpty(productDesc))
            {
                map.Add("productDesc", productDesc);
            }

            map.Add("price", price);
            if (!string.IsNullOrEmpty(offerToken))
            {
                map.Add("offerToken", offerToken);
            }

            UnityPlayerJava.Call("pay", map.ToXJson());
        }

        //支付回调
        private void OnPayResult(XMYPayResult result)
        {
            Log($"OnPayResult {result}");
            if (result == null)
            {
                // ShowToast($"pay fail！");
                //支付失败
                Log($"OnPayResult fail");
                return;
            }

            Log(
                $"支付结果={result.state}，商品ID={result.productId}，订单ID={result.cpOrderId}，token={result.token}，失败码={result.code}，失败信息={result.msg}");
            //触发支付成功回调
            SdkListener.InvokeOnPayResult(result.state, result.productId, result.cpOrderId, result.token,
                result.code ?? "",
                result.msg ?? "");

            Log("发货完成。。");
        }

        #endregion

        #region 请求商品信息

        //请求商品列表
        public override void RequestProductInfo(RequestProductInfoResult infoResult)
        {
            Log("RequestProduct");
            _requestProductInfoResult = infoResult;
            if (_cacheProducts != null)
            {
                Log($"RequestProduct use cache:{_cacheProducts}");
                _requestProductInfoResult?.Invoke(true, _cacheProducts);
                return;
            }

            UnityPlayerJava.Call("reqProductInfo");
        }

        //商品信息回调
        private void OnProductInfoResult(bool success)
        {
            if (success)
            {
                _cacheProducts = GetProductInfoArray();
            }

            var info = _cacheProducts ?? new ProductInfo[0];
            _requestProductInfoResult?.Invoke(success, info);
        }

        private ProductInfo[] GetProductInfoArray()
        {
            Log("GetProductInfoArray");
            var json = UnityPlayerJava.CallReturn<string>("getProductInfo");
            Log($"GetProductInfoArray:{json}");
            try
            {
                //解析成商品数据
                return XJson.FromJson<ProductInfo[]>(json);
            }
            catch (Exception e)
            {
                Log($"解析商品信息异常:{e}");
                Log($"解析商品信息失败:{json},返回空清单");
                return new ProductInfo[0];
            }
        }

        #endregion

        #region 请求订阅信息

        //请求订阅信息
        public override void RequestSubscribeStates(RequestSubscribeStatesResult complete, bool reqNew)
        {
            Log($"RequestSubscribeStates");
            _requestSubscribeStatesResult = complete;
            if (_cacheSubscribeState != null && !reqNew)
            {
                //使用缓存
                _requestSubscribeStatesResult?.Invoke(true, _cacheSubscribeState);
                return;
            }

            UnityPlayerJava.Call("reqSubStatus");
        }

        //当收到订阅回调时
        private void OnReqSubStateResult(bool flag)
        {
            Log($"OnReqSubStateResult {flag}");
            if (flag)
            {
                _cacheSubscribeState = GetSubscribeStates() ?? new SubscribeState[0];
            }

            var states = _cacheSubscribeState ?? new SubscribeState[0];
            _requestSubscribeStatesResult?.Invoke(flag, states);
        }


        //获取订阅信息
        private SubscribeState[] GetSubscribeStates()
        {
            Log("GetSubscribeStates");
            var json = UnityPlayerJava.CallReturn<string>("getAllSubInfo");
            try
            {
                var subscribeState = XJson.FromJson<Dictionary<string, SubscribeState>>(json);
                var arr = subscribeState.Values.ToArray();
                //安卓传过来的是秒，这里转毫秒
                foreach (var subState in arr)
                {
                    subState.expireTime *= 1000L;
                }

                return arr;
            }
            catch (Exception e)
            {
                Log($"解析订阅信息失败: {json}");
                return null;
            }
        }

        #endregion

        #region 调试日志

        //输出日志
        public static void Log(string content)
        {
            XGameSdk.Log($"[XMYGoogleSdk] {content}");
        }

        #endregion

        #region 评价

        public override void OpenReview()
        {
            Log($"OpenReview");
            OpenStore("");
        }

        //原生评价页
        public override void OpenNativeReview()
        {
            Log($"OpenNativeReview");
            UnityPlayerJava.Call("showReviewAlert");
        }

        #endregion

        #region 跳转到google商店页

        public override void OpenStore(string pkm)
        {
            Log($"OpenStore,pkm:{pkm}");
            UnityPlayerJava.Call("startAppStore", pkm);
        }

        #endregion

        #region 事件上报

        //多项上报
        public override void Track(string eventName, KVItems items, EventLevel level)
        {
            Log($"Track，eventName:{eventName},items:{items.ToXJson()},level:{level}");
            var dic = new Dictionary<string, object>();
            foreach (var kv in items)
            {
                var key = kv.Key;
                var v = kv.Value;
                if (v == null)
                {
                    dic.Add(key.ToString(), 1);
                    continue;
                }

                // key.ToString()
                if (v is float || v is double || v is long || v is int || v is ushort || v is uint)
                {
                    dic.Add(key.ToString(), v);
                }
                else
                {
                    dic.Add(key.ToString(), v.ToString());
                }
            }

            var data = new Dictionary<string, object>();
            data.Add("eventName", eventName);
            data.Add("data", dic);
            data.Add("level", (int)level);
            //匿名对象在高裁剪等级时，属性都会被裁剪掉，所以改用Dictionary
            // var msg = (new { eventName, data = dic, level = (int)level }).ToXJson();
            var msg = data.ToXJson();
            UnityPlayerJava.Call("track", msg);
        }

        #endregion

        #region 上报用户属性

        public override void TrackUserProperty(KVItems items)
        {
            Log($"TrackUserProperty，items:{items.ToXJson()}");
            var dic = new Dictionary<string, object>();
            foreach (var kv in items)
            {
                var key = kv.Key;
                var v = kv.Value;
                if (v == null)
                {
                    dic.Add(key.ToString(), 1);
                    continue;
                }

                // key.ToString()
                if (v is float || v is double || v is long || v is int || v is ushort || v is uint)
                {
                    dic.Add(key.ToString(), v);
                }
                else
                {
                    dic.Add(key.ToString(), v.ToString());
                }
            }

            UnityPlayerJava.Call("trackUserSet", dic.ToXJson());
        }

        #endregion

        #region 获取网络类型

        public override NetworkType GetNetworkType()
        {
            Log($"GetNetworkType");
            //设置回调方法
            var value = UnityPlayerJava.CallReturn<string>("getNetworkType").ToUpper();
            switch (value)
            {
                case "WIFI":
                    return NetworkType.WIFI;
                case "5G":
                    return NetworkType._5G;
                case "4G":
                    return NetworkType._4G;
                case "3G":
                    return NetworkType._3G;
                case "2G":
                    return NetworkType._2G;
                case "NO":
                    return NetworkType.NO;
                case "UNKNOWN":
                    return NetworkType.UnKnown;
            }

            return NetworkType.UnKnown;
        }

        #endregion

        #region 请求云控参数

        public override void RequestRemoteConfig(Action<string> callback)
        {
            _remoteConfigResult = callback;
            Log($"RequestRemoteConfig");
            UnityPlayerJava.Call("reqRemoteConfig");
        }

        /// <inheritdoc />
        public override void RequestRemoteConfigMulti(Action<string> callback)
        {
            if (callback == null)
            {
                return;
            }

            _remoteConfigMultiResult = callback;
            Log($"RequestRemoteConfigMulti");
            UnityPlayerJava.Call("reqRemoteConfig");
        }

        private void OnRemoteConfigResult(string config)
        {
            Log($"OnRemoteConfigResult:{config}");
            _remoteConfigResult?.Invoke(config);
            _remoteConfigMultiResult?.Invoke(config);
        }

        #endregion

        #region 跳转到商店订阅页

        public override void SkipSubscribePage()
        {
            Log("SkipSubscribePage");
            UnityPlayerJava.Call("skipSub");
        }

        #endregion

        #region 打开设置

        public override void OpenWifiSettings()
        {
            XGameSdk.Log("openWifiSettings is true");
            UnityPlayerJava.Call("openWifiSettings");
        }

        public override void OpenPhoneSettings()
        {
            XGameSdk.Log("openPhoneSettings is true");
            UnityPlayerJava.Call("openPhoneSettings");
        }

        #endregion

        #region Google play Game服务

        private void OnSnapshotResult(XMYSnapshotResult result)
        {
            Log($"OnSnapshotResult type:{result.type} ,code:{result.code} ,data:{result.data}");

            if (XMYSnapshotResult.TYPE_AUTH == result.type)
            {
                try
                {
                    var code = (GooglePlayGameCode)int.Parse(result.code);
                    var playerId = "";
                    var playerName = "";
                    var playerIcon = "";
                    var failMsg = "";
                    if (code == GooglePlayGameCode.LoginAuthPass)
                    {
                        var player = XJson.FromJson<Dictionary<string, object>>(result.data);
                        if (player.TryGetValue("player_id", out var mPlayerId))
                        {
                            playerId = mPlayerId.ToString();
                        }

                        if (player.TryGetValue("player_name", out var mPlayerName))
                        {
                            playerName = mPlayerName.ToString();
                        }

                        if (player.TryGetValue("player_icon", out var mPlayerIcon))
                        {
                            playerIcon = mPlayerIcon.ToString();
                        }
                    }
                    else
                    {
                        failMsg = result.data;
                    }

                    var s = new LoginGooglePlayGameResult();
                    s.code = code;
                    s.playerId = playerId;
                    s.playerName = playerName;
                    s.failMsg = failMsg;
                    s.playerIcon = playerIcon;

                    _authLoginGooglePlayGameCallback?.Invoke(s);
                }
                catch (Exception e)
                {
                    Log($"OnSnapshotResult auth login Exception: " + e);
                    var s = new LoginGooglePlayGameResult();
                    s.code = GooglePlayGameCode.LoginAuthFailed;
                    s.playerId = "";
                    s.playerName = "";
                    s.playerIcon = "";
                    s.failMsg = e.Message;
                    _authLoginGooglePlayGameCallback?.Invoke(s);
                }
            }
            else if (XMYSnapshotResult.TYPE_LOGIN == result.type)
            {
                try
                {
                    var code = (GooglePlayGameCode)int.Parse(result.code);
                    var playerId = "";
                    var playerName = "";
                    var playerIcon = "";
                    var failMsg = "";
                    if (code == GooglePlayGameCode.LoginPass)
                    {
                        var player = XJson.FromJson<Dictionary<string, object>>(result.data);
                        if (player.TryGetValue("player_id", out var mPlayerId))
                        {
                            playerId = mPlayerId.ToString();
                        }

                        if (player.TryGetValue("player_name", out var mPlayerName))
                        {
                            playerName = mPlayerName.ToString();
                        }

                        if (player.TryGetValue("player_icon", out var mPlayerIcon))
                        {
                            playerIcon = mPlayerIcon.ToString();
                        }
                    }
                    else
                    {
                        failMsg = result.data;
                    }

                    var s = new LoginGooglePlayGameResult();
                    s.code = code;
                    s.playerId = playerId;
                    s.playerName = playerName;
                    s.failMsg = failMsg;
                    s.playerIcon = playerIcon;

                    _loginGooglePlayGameCallback?.Invoke(s);
                }
                catch (Exception e)
                {
                    Log($"OnSnapshotResult login Exception: " + e);
                    var s = new LoginGooglePlayGameResult();
                    s.code = GooglePlayGameCode.LoginNot;
                    s.playerId = "";
                    s.playerName = "";
                    s.playerIcon = "";
                    s.failMsg = e.Message;
                    _loginGooglePlayGameCallback?.Invoke(s);
                }
            }
            else if (XMYSnapshotResult.TYPE_LOAD == result.type)
            {
                try
                {
                    var code = (GooglePlayGameCode)int.Parse(result.code);
                    var data = "";
                    var failMsg = "";
                    if (code == GooglePlayGameCode.LoadCompleted)
                    {
                        data = result.data;
                    }
                    else
                    {
                        failMsg = result.data;
                    }

                    var s = new LoadSnapshotResult();
                    s.code = code;
                    s.data = data;
                    s.failMsg = failMsg;

                    _loadSnapshotCallback?.Invoke(s);
                }
                catch (Exception e)
                {
                    Log($"OnSnapshotResult load Exception: " + e);
                    var s = new LoadSnapshotResult();
                    s.code = GooglePlayGameCode.LoginNot;
                    s.data = "";
                    s.failMsg = e.Message;
                    _loadSnapshotCallback?.Invoke(s);
                }
            }
            else if (XMYSnapshotResult.TYPE_SAVE == result.type)
            {
                try
                {
                    var code = (GooglePlayGameCode)int.Parse(result.code);
                    var failMsg = "";
                    if (code != GooglePlayGameCode.SaveCompleted)
                    {
                        failMsg = result.data;
                    }

                    var s = new SaveSnapshotResult();
                    s.code = code;
                    s.failMsg = failMsg;

                    _saveSnapshotCallback?.Invoke(s);
                }
                catch (Exception e)
                {
                    Log($"OnSnapshotResult save Exception: " + e);
                    var s = new SaveSnapshotResult();
                    s.code = GooglePlayGameCode.LoginNot;
                    s.failMsg = e.Message;
                    _saveSnapshotCallback?.Invoke(s);
                }
            }
            else if (XMYSnapshotResult.TYPE_SCORE == result.type)
            {
                try
                {
                    var code = (GooglePlayGameCode)int.Parse(result.code);
                    var s = XJson.FromJson<CurrentPlayerLeaderboardScoreLoadResult>(result.data);
                    s.code = code;
                    _currentPlayerLeaderboardScoreLoadCallback?.Invoke(s);
                }
                catch (Exception e)
                {
                    Log($"OnSnapshotResult score Exception: " + e);
                    var s = new CurrentPlayerLeaderboardScoreLoadResult();
                    s.code = GooglePlayGameCode.CurrentPlayerLeaderboardScoreLoadFailed;
                    s.failMsg = e.Message;
                    _currentPlayerLeaderboardScoreLoadCallback?.Invoke(s);
                }
            }
            else if (XMYSnapshotResult.TYPE_TOP_SCORES == result.type)
            {
                try
                {
                    var code = (GooglePlayGameCode)int.Parse(result.code);
                    var s = XJson.FromJson<TopScoresLoadResult>(result.data);
                    s.code = code;
                    _topScoresLoadCallback?.Invoke(s);
                }
                catch (Exception e)
                {
                    Log($"OnSnapshotResult topScores Exception: " + e);
                    var s = new TopScoresLoadResult();
                    s.code = GooglePlayGameCode.TopScoresLoadFailed;
                    s.failMsg = e.Message;
                    _topScoresLoadCallback?.Invoke(s);
                }
            }
            else
            {
                Log($"OnSnapshotResult 未知type");
            }
        }

        private Action<LoginGooglePlayGameResult> _loginGooglePlayGameCallback;
        private Action<LoginGooglePlayGameResult> _authLoginGooglePlayGameCallback;
        private Action<LoadSnapshotResult> _loadSnapshotCallback;
        private Action<SaveSnapshotResult> _saveSnapshotCallback;
        private Action<CurrentPlayerLeaderboardScoreLoadResult> _currentPlayerLeaderboardScoreLoadCallback;
        private Action<TopScoresLoadResult> _topScoresLoadCallback;

        public override void IsAuthenticatedGooglePlayGame(Action<LoginGooglePlayGameResult> callback)
        {
            Log("IsAuthenticatedGooglePlayGame");
            _authLoginGooglePlayGameCallback = callback;
            UnityPlayerJava.Call("isAuthenticatedGooglePlayGame");
        }

        public override void LoginGooglePlayGame(Action<LoginGooglePlayGameResult> callback)
        {
            Log($"LoginGooglePlayGame");
            _loginGooglePlayGameCallback = callback;
            UnityPlayerJava.Call("loginGooglePlayGame");
        }

        public override void LoadSnapshot(Action<LoadSnapshotResult> callback)
        {
            Log($"LoadSnapshot");
            _loadSnapshotCallback = callback;
            UnityPlayerJava.Call("loadSnapshot");
        }

        public override void SaveSnapshot(string data, Action<SaveSnapshotResult> callback)
        {
            Log($"SaveSnapshot");
            _saveSnapshotCallback = callback;
            UnityPlayerJava.Call("saveSnapshot", data);
        }

        public override void SubmitScore(string leaderboardId, long score)
        {
            Log($"SubmitScore leaderboardId={leaderboardId} ,score={score}");
            UnityPlayerJava.Call("submitScore", leaderboardId, score.ToString());
        }

        public override void ShowLeaderboard(string leaderboardId)
        {
            Log($"ShowLeaderboard leaderboardId={leaderboardId}");
            UnityPlayerJava.Call("showLeaderboard", leaderboardId);
        }

        public override void LoadCurrentPlayerLeaderboardScore(string leaderboardId, LeaderboardTimeSpan span,
            LeaderboardCollection leaderboardCollection, Action<CurrentPlayerLeaderboardScoreLoadResult> callback)
        {
            Log(
                $"loadCurrentPlayerLeaderboardScore leaderboardId={leaderboardId} span={span} leaderboardCollection={leaderboardCollection}");
            _currentPlayerLeaderboardScoreLoadCallback = callback;
            UnityPlayerJava.Call("loadCurrentPlayerLeaderboardScore", leaderboardId, (int)span,
                (int)leaderboardCollection);
        }

        public override void LoadTopScores(string leaderboardId, LeaderboardTimeSpan span,
            LeaderboardCollection leaderboardCollection, int maxResults, bool forceReload,
            Action<TopScoresLoadResult> callback)
        {
            Log(
                $"LoadTopScores leaderboardId={leaderboardId} span={span} leaderboardCollection={leaderboardCollection} maxResults={maxResults} forceReload={forceReload}");
            _topScoresLoadCallback = callback;
            UnityPlayerJava.Call("loadTopScores", leaderboardId, (int)span, (int)leaderboardCollection, maxResults,
                forceReload);
        }

        public override void ShowAchievements()
        {
            Log($"ShowAchievements");
            UnityPlayerJava.Call("showAchievements");
        }

        public override void UnlockAchievement(string achievementId)
        {
            Log($"UnlockAchievement achievementId={achievementId}");
            UnityPlayerJava.Call("unlockAchievement", achievementId);
        }

        public override void IncrementAchievement(string achievementId, int incrementValue)
        {
            Log($"IncrementAchievement achievementId={achievementId} incrementValue={incrementValue}");
            UnityPlayerJava.Call("incrementAchievement", achievementId, incrementValue);
        }

        #endregion


        #region 消息推送

        public override bool IsRequirePushPermission()
        {
            try
            {
                var b = UnityPlayerJava.CallReturn<bool>("isPushPermission");
                Log($"IsRequirePushPermission {b}");
                return b;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public override void OpenPush()
        {
            try
            {
                Log($"OpenPush");
                UnityPlayerJava.Call("openPush");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override string GetPushRewardMessage(string key = "REWARD_PUSH_DATA")
        {
            try
            {
                Log($"GetPushRewardMessage {key}");
                return UnityPlayerJava.CallReturn<string, string>("getRewardMessage", key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }

        #endregion


        #region 互推

        public override CrossInfo[] GetCrossList(string type, int count)
        {
            try
            {
                var s = UnityPlayerJava.CallReturn<string, string, int>("getCrossList", type, count);
                Log($"GetCrossList type={type}, count={count}, list={s}");

                if (string.IsNullOrEmpty(s))
                {
                    return Array.Empty<CrossInfo>();
                }

                var dic = XJson.FromJson<List<Dictionary<string, object>>>(s);
                var list = new List<CrossInfo>();

                foreach (var o in dic)
                {
                    var c = new CrossInfo();
                    c.type = o["game_type"] as string ?? "";
                    c.name = o["game_name"] as string ?? "";
                    c.package = o["game_package"] as string ?? "";
                    c.pic = o["game_pic"] as string ?? "";
                    c.desc = o["game_text"] as string ?? "";
                    c.isInstall = (o["game_install"] as int? ?? 0) == 1;
                    c.weight = o["game_weight"] as int? ?? 0;

                    list.Add(c);
                }

                Log($"返回的crossList为 {list.ToXJson()}");
                return list.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Array.Empty<CrossInfo>();
            }
        }

        public override void CrossAction(CrossActionType action, CrossInfo crossInfo, string sceneId)
        {
            try
            {
                Log($"CrossAction action={action.ToString()}, crossInfo={crossInfo.ToXJson()}, sceneId={sceneId}");

                if (string.IsNullOrEmpty(crossInfo.package))
                {
                    Log("crossInfo.package is empty");
                    return;
                }

                if (string.IsNullOrEmpty(crossInfo.type))
                {
                    Log("crossInfo.type is empty");
                    return;
                }

                switch (action)
                {
                    case CrossActionType.Show:
                    {
                        UnityPlayerJava.Call("crossAction", "show", crossInfo.package, crossInfo.type, sceneId);
                    }
                        break;
                    case CrossActionType.Click:
                    {
                        UnityPlayerJava.Call("crossAction", "click", crossInfo.package, crossInfo.type, sceneId);
                    }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override string GetCrossRewardMessage(string key = "REWARD_CROSS_DATA")
        {
            try
            {
                Log($"GetCrossRewardMessage {key}");
                return UnityPlayerJava.CallReturn<string, string>("getRewardMessage", key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }

        #endregion

        #region 应用更新

        public override bool IsAvailableNewVersion()
        {
            try
            {
                var s = UnityPlayerJava.CallReturn<string>("getUpdateInfo");
                Log($"IsAvailableNewVersion {s}");
                return !string.IsNullOrEmpty(s);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public override void StartUpdateNewVersion()
        {
            try
            {
                Log($"StartUpdateNewVersion..");
                UnityPlayerJava.Call("startUpdate");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region 隐私协议

        //展示隐私协议
        public override void ShowPrivacy()
        {
            Log($"ShowPrivacy");
            UnityPlayerJava.Call("showPrivacyOptions");
        }

        //是否为欧盟地区
        public override bool IsEURegion()
        {
            Log($"GetEURegionFlag...");
            var flag = UnityPlayerJava.CallReturn<bool>("isEURegion");
            Log($"GetEURegionFlag flag:{flag}");
            return flag;
        }

        //是否支持隐私协议按钮
        public override bool IsSupportPrivacyBtn()
        {
            return IsEURegion();
        }

        #endregion

        #region 设备信息

        public override DeviceInfo GetDeviceInfo()
        {
            Log($"GetDeviceInfo...");

            try
            {
                var infoString = UnityPlayerJava.CallReturn<string>("getDeviceInfo");
                Log($"{infoString}");
                var deviceInfo = XJson.FromJson<DeviceInfo>(infoString);
                deviceInfo.GAID = deviceInfo.gaid;
                return deviceInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        #endregion

        //上报sdk事件
        // private void TrackXGameSdkEvent(string eventName)
        // {
        //     Track(eventName, new KVItems()
        //     {
        //         { NAME_ENGINE_TIME, Time.realtimeSinceStartup },
        //         { NAME_TIME_STAMP_MS, GetTimeStampNow() },
        //     }, EventLevel.Level_10);
        // }

        //获取当前时间戳
        // private double GetTimeStampNow()
        // {
        //     return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        // }

        #region 手机震动

        public override void PhoneVibrate(string type)
        {
            Log($"PhoneVibrate is type:" + type);
            UnityPlayerJava.Call("phoneVibrate", type);
        }

        #endregion

        #region 资源分包pad

        private void OnFetchPadResult(string code, string data)
        {
            Log($"OnFetchPadResult code={code}, data={data}");
            if (code == "4")
            {
                Log($"[PAD][data]{data}");
                var packName = data.Replace("{", "").Replace("}", "").Replace(" ", "").Split(',')
                    .First(e => e.StartsWith("packName="))
                    .Replace("packName=", "");
                Log($"[PAD][packName]{packName}");
                //复制文件到指定路径
                var path = GetAssetPath(packName);
                var toPath = PlayAssetDeliveryPaths.RuntimePathRoot;
                CopyFolderIfNoExistsOrMD5NoMatch(path, toPath);
            }

            _requestAssetDeliveryResult?.Invoke(code ?? "", data ?? "");
        }

        private static string ChangePackName(string packName)
        {
            var name = "";
            if (string.IsNullOrEmpty(packName)) return name;
            const string start = "pad_";
            name = packName.Replace(" ", "").ToLower();
            while (name.StartsWith(start))
            {
                name = name.Remove(0, start.Length);
            }

            name = start + name;

            return name;
        }

        public override void RequestAssetDelivery(string packName, Action<string /*code*/, string /*data*/> action)
        {
            Log($"RequestAssetDelivery packName={packName}");
            var name = ChangePackName(packName);
            _requestAssetDeliveryResult = action;
            UnityPlayerJava.Call("requestAssetDelivery", name);
        }

        public override string GetAssetPath(string packName)
        {
            Log($"GetAssetPath packName={packName}");
            var name = ChangePackName(packName);
            return UnityPlayerJava.CallReturn<string, string>("getAssetPath", name) ?? "";
        }

        public override void RemovePack(string packName)
        {
            Log($"RemovePack packName={packName}");
            var name = ChangePackName(packName);
            UnityPlayerJava.Call("removePack", name);
        }

        public override void CancelPack(string packName)
        {
            Log($"CancelPack packName={packName}");
            var name = ChangePackName(packName);
            UnityPlayerJava.Call("cancelPack", name);
        }

        private void CopyFolderIfNoExistsOrMD5NoMatch(string from, string to)
        {
            Log($"[PAD][CopyFolderIfNoExistsOrMD5NoMatch]{from} {to}");
            if (!Directory.Exists(from))
            {
                return;
            }

            var prefix = $"{from.Replace("\\", "/")}/assets";
            var dirInfo = new DirectoryInfo(from);
            var files = dirInfo.GetFiles("*", SearchOption.AllDirectories);
            foreach (var element in files)
            {
                //复制到的目录
                var copyTo = element.FullName.Replace("\\", "/").Replace(prefix, to);
                if (PadFileIsMatch(element.FullName, copyTo))
                {
                    Log($"[PAD][文件一致，跳过复制]from：{element.FullName} to:{copyTo}");
                    continue;
                }

                var dir = Path.GetDirectoryName(copyTo);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                Log($"[PAD][复制文件]从 {element.FullName} 到 {copyTo}");
                File.Copy(element.FullName, copyTo, true);
            }
        }

        private static bool PadFileIsMatch(string filePath1, string filePath2)
        {
            var file2Exists = File.Exists(filePath2);
            if (!file2Exists)
            {
                return false;
            }

            using (var md5 = MD5.Create())
            {
                byte[] hash1 = null;
                byte[] hash2 = null;
                using (var stream1 = File.OpenRead(filePath1))
                {
                    hash1 = md5.ComputeHash(stream1);
                }

                using (var stream2 = File.OpenRead(filePath2))
                {
                    hash2 = md5.ComputeHash(stream2);
                }

                return hash1.SequenceEqual(hash2);
            }
        }

        #endregion


        #region 监听切入切出

        private void OnInOutResult(string ext)
        {
            _inOutCallback?.Invoke(ext);
        }

        private Action<string> _inOutCallback;

        public override void SetInOutCallback(Action<string> callback)
        {
            Log($"SetInOutCallback...");
            _inOutCallback = callback;
        }

        #endregion
    }
}