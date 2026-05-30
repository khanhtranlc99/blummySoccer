using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// using XGameHttpModel;

namespace XGame
{
    /// <summary>
    /// sdk 总接口 
    /// </summary>
    [Preserve]
    public abstract class BaseSdk
    {
        protected XHttpClient _httpClient;

        protected XHttpClient XHttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = XHttpClient.CreateInstance();
                    GameObject.DontDestroyOnLoad(_httpClient.gameObject);
                }

                return _httpClient;
            }
        }

        protected BaskSdkListener SdkListener { get; set; }

        #region 构建

        public void OnCreate(BaskSdkListener sdkListener)
        {
            SdkListener = sdkListener;
            OnCreate();
        }

        protected abstract void OnCreate();

        #endregion

        #region 初始化SDK

        public abstract void InitSdk(Action success, Action fail = null);

        #endregion

        #region 登录

        public virtual void LoginGoogle(Action<string, string> success = null, Action fail = null)
        {
            XGameSdk.Log($"[BaseSdk] LoginGoogle 空实现！");
        }

        public virtual void Login(Action success = null, Action fail = null)
        {
            XGameSdk.Log("[BaseSdk] Login 空实现！");
        }

        #endregion

        #region 激励视频

        public virtual void ShowVideo(string sceneName, Action success, Action fail = null)
        {
            XGameSdk.Log($"[BaseSdk] ShowVideo 空实现！sceneName：{sceneName} 触发失败回调");
            fail?.Invoke();
        }

        public virtual bool HasVideo()
        {
            XGameSdk.Log($"[BaseSdk] HasVideo 默认返回:false");
            return false;
        }

        public virtual bool IsClickTriggerVideo()
        {
            XGameSdk.Log($"[BaseSdk] IsClickTriggerVideo 默认返回:false");
            return false;
        }

        #endregion

        #region 插页广告

        public virtual bool HasInters()
        {
            XGameSdk.Log($"[BaseSdk] HasInters 默认返回:false");
            return false;
        }

        public virtual bool CanShowInters()
        {
            XGameSdk.Log($"[BaseSdk] CanShowInters 默认返回:true");
            return true;
        }

        public virtual void ShowInters(string scene = "unknown", Action onClose = null)
        {
            XGameSdk.Log($"[BaseSdk] ShowInters  空实现！ sceneName：{scene}");
            XGameSdk.Log($"[BaseSdk] ShowInters  触发onclose");
            onClose?.Invoke();
        }


        public virtual void ShowInterstitial(string sceneName, Action onShow, Action onClose = null)
        {
            XGameSdk.Log($"[BaseSdk] ShowInterstitial 空实现！sceneName：{sceneName}");
            XGameSdk.Log("[BaseSdk] ShowInterstitial  onShow");
            XGameSdk.Log("[BaseSdk] ShowInterstitial  onClose");
            onShow?.Invoke();
            onClose?.Invoke();
        }

        #endregion

        #region Banner

        public virtual void ShowBanner(BannerType bannerType)
        {
            XGameSdk.Log($"[BaseSdk] ShowBanner  空实现！ bannerType：{bannerType}");
        }

        public virtual void HideBanner()
        {
            XGameSdk.Log($"[BaseSdk] HideBanner  空实现！");
        }

        #endregion

        #region 原生大图

        public virtual bool HasBigNative()
        {
            XGameSdk.Log($"[BaseSdk] HasBigNative  默认返回:false");
            return false;
        }

        public virtual void HideBigNative()
        {
            XGameSdk.Log($"[BaseSdk] HideBigNative  空实现！");
        }

        public virtual void ShowBigNative(string scene)
        {
            XGameSdk.Log($"[BaseSdk] ShowBigNative  空实现！scene:{scene} ");
        }

        #region 原生广告-Google

        public virtual bool HasNative()
        {
            XGameSdk.Log($"[BaseSdk] HasNative  默认返回:false");
            return false;
        }

        public virtual void ShowNativeAd(string scene)
        {
            XGameSdk.Log($"[BaseSdk] showNative 空实现！ scene：{scene}");
        }

        public virtual void ShowNativeAd(string scene, Action onClose)
        {
            XGameSdk.Log($"[BaseSdk] showNative 空实现！ scene：{scene} onClose={onClose}");
            onClose?.Invoke();
        }

        public virtual void HideNative()
        {
            XGameSdk.Log($"[BaseSdk] HideNative 空实现！");
        }

        #endregion

        #endregion

        #region MAR-悬浮ICON

        public virtual void ShowFloatIconAd(float posX, float posY)
        {
            XGameSdk.Log($"[BaseSdk] ShowFloatIconAd  空实现！posX:{posX} posY:{posY}");
        }

        public virtual void HideFloatIcon()
        {
            XGameSdk.Log($"[BaseSdk] HideFloatIcon  空实现！");
        }

        #endregion

        // #region 微信-自定义广告-version1
        //
        // public virtual void ShowCustomAd(int type, float x, float y)
        // {
        //     XGameSdk.Log(
        //         $"[BaseSdk] ShowCustomAd  空实现！ type:{type} x:{x} y:{y}");
        // }
        //
        // public virtual void HideCustomAd(int type)
        // {
        //     XGameSdk.Log(
        //         $"[BaseSdk] HideCustomAd  空实现！ type:{type} ");
        // }
        //
        // #endregion
        //
        // #region 微信-自定义广告-version2
        //
        // public virtual void ShowCustomAdPos(string sceneId, float pivotX, float pivotY, float x, float y)
        // {
        //     XGameSdk.Log(
        //         $"[BaseSdk] ShowCustomAdPos  空实现！ sceneId:{sceneId} pivot：{pivotX} pivotY:{pivotY} x:{x} y:{y}");
        // }
        //
        // public virtual void HideCustomAdPos(string sceneId)
        // {
        //     XGameSdk.Log($"[BaseSdk] HideCustomAdPos  空实现！ sceneId:{sceneId}");
        // }
        //
        // #endregion

        #region 模板广告

        public virtual bool GetTemplateAdFlag()
        {
            XGameSdk.Log($"[BaseSdk] GetTemplateAdFlag  不支持！返回false");
            return false;
        }

        public virtual void ShowTemplateAd(string scene)
        {
            XGameSdk.Log($"[BaseSdk] ShowTemplateAd  空实现！ scene:{scene}");
        }

        public virtual void HideTemplateAd()
        {
            XGameSdk.Log($"[BaseSdk] HideTemplateAd  空实现！");
        }

        #endregion

        #region 单格子广告

        public virtual bool GetNativeIconFlag()
        {
            XGameSdk.Log($"[BaseSdk] GetNativeIconFlag 默认返回false");
            return false;
        }

        public virtual void ShowNativeIcon(string scene, float spx, float spy)
        {
            XGameSdk.Log($"[BaseSdk] ShowNativeIcon 空实现！scene:{scene} spx:{spx}  spy:{spy}");
        }

        public virtual void HideNativeIcon()
        {
            XGameSdk.Log($"[BaseSdk] HideNativeIcon 空实现！");
        }

        #endregion

        #region 单格子广告2

        public virtual bool GetNativeIcon2Flag()
        {
            XGameSdk.Log($"[BaseSdk] GetNativeIcon2Flag 默认返回false");
            return false;
        }

        public virtual void ShowNativeIcon2(string scene, float spx, float spy)
        {
            XGameSdk.Log($"[BaseSdk] ShowNativeIcon2 空实现！scene:{scene} spx:{spx}  spy:{spy}");
        }

        public virtual void HideNativeIcon2()
        {
            XGameSdk.Log($"[BaseSdk] HideNativeIcon2 空实现！");
        }

        #endregion

        #region 多格广告

        public virtual bool GetBlockFlag()
        {
            XGameSdk.Log($"[BaseSdk] GetBlockFlag 默认返回:false");
            return false;
        }

        public virtual void ShowBlock(string scene)
        {
            XGameSdk.Log($"[BaseSdk] ShowBlock 空实现！ scene：{scene}");
        }

        public virtual void HideBlock()
        {
            XGameSdk.Log($"[BaseSdk] HideBlock 空实现！");
        }

        #endregion

        // #region 互推盒子banner
        //
        // public virtual bool GetNavigateBoxBannerFlag()
        // {
        //     XGameSdk.Log($"[BaseSdk] GetNavigateBoxBannerFlag 默认返回:false");
        //     return false;
        // }
        //
        // public virtual void ShowNavigateBoxBanner(string scene)
        // {
        //     XGameSdk.Log($"[BaseSdk] ShowNavigateBoxBanner 空实现！ scene:{scene}");
        // }
        //
        // public virtual void HideNavigateBoxBanner()
        // {
        //     XGameSdk.Log($"[BaseSdk] HideNavigateBoxBanner 空实现！");
        // }
        //
        // #endregion

        #region 贴片广告

        public virtual bool GetPatchAdFlag()
        {
            XGameSdk.Log($"[BaseSdk] GetNavigateBoxBannerFlag 默认返回:false");
            return false;
        }

        public virtual void ShowPatchAd(string scene, PatchAdType type, float xNormalize, float yNormalize,
            float widthNormalize,
            float heightNormalize)
        {
            XGameSdk.Log(
                $"[BaseSdk] ShowPatchAd 空实现！ scene：{scene} type：{type} xNormalize：{xNormalize} yNormalize：{yNormalize} widthNormalize：{widthNormalize} heightNormalize：{heightNormalize}");
        }


        public virtual void HidePatchAd()
        {
            XGameSdk.Log(
                $"[BaseSdk] HidePatchAd 空实现！");
        }

        #endregion

        // #region 动态开启广告状态
        //
        // public virtual void SetADLoadingState(bool isOn)
        // {
        //     XGameSdk.Log($"[BaseSdk] SetADLoadingState 不支持！ isOn:{isOn}");
        // }
        //
        // #endregion

        #region 退出挽留

        //支持退出挽留
        public virtual bool IsSupportExitRetention()
        {
            XGameSdk.Log($"[BaseSdk] IsSupportExitRetention 不支持！ 返回false");
            return false;
        }

        //弹出退出挽留
        public virtual void PopUpExitRetention()
        {
            XGameSdk.Log($"[BaseSdk] PopUpExitRetention 不支持，空实现！");
        }

        #endregion

        #region 退出

        public virtual bool HasExit()
        {
            XGameSdk.Log($"[BaseSdk] HasExit 默认返回：true");
            return true;
        }

        public virtual void Exit()
        {
            XGameSdk.Log($"[BaseSdk] Exit 默认调用:Application.Quit()");
            Application.Quit();
        }

        #endregion

        #region 渠道细分

        public virtual ChannelName GetChannelName()
        {
            XGameSdk.Log($"[BaseSdk] GetChannelName 未实现！  默认返回:Unknown");
            return ChannelName.Unknown;
        }

        #endregion

        #region 游戏忠告

        public virtual bool HasGameCounsel()
        {
            XGameSdk.Log($"[BaseSdk] HasGameCounsel  默认返回 false");
            return false;
        }

        #endregion

        #region 评价

        public virtual void OpenReview()
        {
            XGameSdk.Log($"[BaseSdk] OpenReview 不支持！空实现");
        }

        //原生评价页
        public virtual void OpenNativeReview()
        {
            XGameSdk.Log($"[BaseSdk] OpenNativeReview 不支持！空实现");
        }

        #endregion

        #region Google-跳转到应用商店

        public virtual void OpenStore(string pkm)
        {
            XGameSdk.Log($"[BaseSdk] OpenStore 不支持！空实现");
        }

        #endregion

        #region 获取国家网络代号

        public virtual string GetNetWorkCountryIso()
        {
            XGameSdk.Log($"[BaseSdk] GetNetWorkCountryIso 不支持！返回:unknown");
            return "unknown";
        }

        #endregion

        #region 获取设备类型

        public virtual DeviceType GetDeviceType()
        {
            var result = DeviceType.UnKnown;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    result = DeviceType.Android;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    result = DeviceType.Ios;
                    break;
            }

            XGameSdk.Log($"[BaseSdk] GetDeviceType result：{result}");
            return result;
        }

        #endregion

        #region 获取网络类型

        public virtual NetworkType GetNetworkType()
        {
            var result = NetworkType.UnKnown;
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return NetworkType.NO;
            }

            XGameSdk.Log($"[BaseSdk] GetNetworkType result：{result}");
            return result;
        }

        #endregion

        #region 获取用户ID, 用户头像

        public virtual string GetSDKUserID()
        {
            var result = SystemInfo.deviceUniqueIdentifier;
            XGameSdk.Log($"[BaseSdk] GetSDKUserID={result}");
            return result;
        }

        public virtual string GetSdkUID()
        {
            var result = SystemInfo.deviceUniqueIdentifier;
            XGameSdk.Log($"[BaseSdk] GetSdkUID={result}");
            return result;
        }

        public virtual string GetSDKUserPicUrl()
        {
            XGameSdk.Log($"[BaseSdk] GetSDKUserPicUrl=");
            return "";
        }

        #endregion

        #region 获取设备唯一码

        public virtual string GetDeviceUniqueID()
        {
            var result = SystemInfo.deviceUniqueIdentifier;
            XGameSdk.Log($"[BaseSdk] GetDeviceUniqueID={result}");
            return result;
        }

        #endregion

        #region 获取屏幕大小

        public virtual Vector2 GetMiniGameScreenSize()
        {
            var result = new Vector2(Screen.width, Screen.height);
            XGameSdk.Log($"[BaseSdk] GetMiniGameScreenSize={result}");
            return result;
        }

        #endregion

        #region 抖音-录屏分享

        public virtual bool CanRecord()
        {
            XGameSdk.Log($"[BaseSdk] CanRecord 不支持！返回false");
            return false;
        }

        public virtual bool StartRecord()
        {
            XGameSdk.Log($"[BaseSdk] StartRecord 不支持！返回false");
            return false;
        }

        public virtual bool StopRecord()
        {
            XGameSdk.Log($"[BaseSdk] StopRecord 不支持！返回false");
            return false;
        }

        public virtual float GetRecordDuration()
        {
            XGameSdk.Log($"[BaseSdk] GetRecordDuration 不支持！返回0");
            return 0;
        }

        public virtual bool CanShareRecord()
        {
            XGameSdk.Log($"[BaseSdk] CanShareRecord 不支持！返回false");
            return false;
        }

        public virtual void ShareRecord(Action success, Action<string> fail, Action cancel, string title,
            params string[] topics)
        {
            XGameSdk.Log($"[BaseSdk] ShareRecord 不支持！触发失败回调，title：{title}  topics：{topics.ToXJson()}");
            fail?.Invoke("no support");
        }

        public virtual void ClearRecord()
        {
            XGameSdk.Log($"[BaseSdk] ClearRecord 不支持！模拟空实现");
        }

        #endregion

        #region 抖音-IM客服页

        public virtual void OpenCustomerServiceConversation(Action success = null, Action fail = null)
        {
            XGameSdk.Log($"[BaseSdk] OpenCustomerServiceConversation 不支持！触发失败回调");
            fail?.Invoke();
        }

        public virtual bool HasCustomerServiceConversation()
        {
            XGameSdk.Log($"[BaseSdk] HasCustomerServiceConversation 不支持！返回false");
            return false;
        }

        #endregion

        #region 抖音-客服页

        public virtual void OpenCustomerService(Action success = null, Action fail = null)
        {
            XGameSdk.Log($"[BaseSdk] OpenCustomerService 不支持！触发失败回调");
            fail?.Invoke();
        }

        public virtual bool HasCustomerService()
        {
            XGameSdk.Log($"[BaseSdk] HasCustomerService 不支持！返回false");
            return false;
        }

        #endregion

        #region 快捷方式

        public virtual bool IsSupportShortcut()
        {
            XGameSdk.Log($"[BaseSdk] IsSupportShortcut 不支持！返回false");
            return false;
        }

        public virtual void CreateShortcut(Action success, Action fail = null)
        {
            XGameSdk.Log(
                $"[BaseSdk] CreateShortcut 不支持！ 触发失败回调");
            fail?.Invoke();
        }

        #endregion

        #region 常用

        public virtual bool IsSupportCommonUse()
        {
            XGameSdk.Log($"[BaseSdk] IsSupportCommonUse 不支持！返回false");
            return false;
        }

        public virtual void AddCommonUse(Action success, Action fail = null)
        {
            XGameSdk.Log(
                $"[BaseSdk] AddCommonUse 不支持！ 触发失败回调");
            fail?.Invoke();
        }

        #endregion

        #region 内购

        public virtual void Pay(int price, string productId, string productName, string productDesc)
        {
            XGameSdk.Log(
                $"[BaseSdk] Pay 未实现！ 模拟购买失败 productId：{productId} productName：{productName} productDesc：{productDesc}");
        }
        
        public virtual void Pay(int price, string productId, string productName, string productDesc, string offerToken)
        {
            XGameSdk.Log(
                $"[BaseSdk] Pay 未实现！ 模拟购买失败 productId：{productId} productName：{productName} productDesc：{productDesc} offerToken：{offerToken}");
            //TODO//模拟购买失败
        }

        /// <summary>
        /// 恢复购买过的非消耗类型和自动订阅类型的订阅情况
        /// </summary>
        public virtual void RestoreCompletedPayInfo(Action success = null, Action<string> fail = null)
        {
            XGameSdk.Log("[BaseSdk] RestoreCompletedTransactions 不支持! 默认触发失败回调");
            fail?.Invoke("no support in  current app channel");
        }

        #endregion

        #region Google-请求商品清单

        public virtual void RequestProductInfo(RequestProductInfoResult complete)
        {
            XGameSdk.Log($"[BaseSdk] RequestProductInfo 不支持！触发失败回调");
            complete?.Invoke(false, new ProductInfo[0]);
        }

        #endregion

        #region Google-请求订阅内容状态

        public virtual void RequestSubscribeStates(RequestSubscribeStatesResult complete, bool requestNew = true)
        {
            XGameSdk.Log($"[BaseSdk] RequestSubscribeStates 不支持！触发失败回调 requestNew：{requestNew}");
            complete?.Invoke(false, new SubscribeState[0]);
        }

        #endregion

        #region 兑换码

        public virtual void Gift(string giftCode)
        {
            XGameSdk.Log($"[BaseSdk] Gift 不支持！ giftCode:{giftCode}");
        }

        public virtual bool CanGift()
        {
            XGameSdk.Log($"[BaseSdk] CanGift 不支持！ 返回false");
            return false;
        }

        #endregion

        #region 事件上报-ver1

        public virtual void DotEvent(string eventName, string field, string value = "",
            EventLevel level = EventLevel.Level_0)
        {
            XGameSdk.Log(
                $"[BaseSdk] DotEvent  未实现！  eventName：{eventName}  field:{field} value:{value} level:{level}");
        }

        public virtual void DotEvent(string eventName, string field, int value, EventLevel level = EventLevel.Level_0)
        {
            XGameSdk.Log(
                $"[BaseSdk] DotEvent  未实现！  eventName：{eventName}  field:{field} value:{value} level:{level}");
        }

        public virtual void DotEvent(string eventName, string field, float value, EventLevel level = EventLevel.Level_0)
        {
            XGameSdk.Log(
                $"[BaseSdk] DotEvent  未实现！  eventName：{eventName}  field:{field} value:{value} level:{level}");
        }

        public virtual void DoEventMultipleWithLevel(string eventName, EventLevel level, params object[] keyValues)
        {
            XGameSdk.Log(
                $"[BaseSdk] DoEventMultipleWithLevel  未实现！  eventName：{eventName}  level:{level} keyValues:{keyValues?.ToXJson()}");
        }

        public virtual void DoEventMultiple(string eventName, params object[] keyValues)
        {
            XGameSdk.Log(
                $"[BaseSdk] DoEventMultiple  未实现！  eventName：{eventName} keyValues:{keyValues?.ToXJson()}");
        }

        public virtual void DoEventMultiple(string eventName, KVItems items, EventLevel level = EventLevel.Level_0)
        {
            XGameSdk.Log(
                $"[BaseSdk] DoEventMultiple  未实现！  eventName：{eventName} items:{items?.ToXJson()} level:{level}");
        }

        #endregion

        #region 用户属性上报-ver1

        public virtual void SetUserProperty(params object[] data)
        {
            XGameSdk.Log(
                $"[BaseSdk] SetUserProperty  未实现！  data：{data?.ToXJson()} ");
        }

        public virtual void SetUserProperty(KVItems items)
        {
            XGameSdk.Log(
                $"[BaseSdk] SetUserProperty  未实现！  items：{items?.ToXJson()} ");
        }

        #endregion

        #region 用户属性上报-ver2

        public virtual void TrackUserProperty(KVItems items)
        {
            XGameSdk.Log(
                $"[BaseSdk] TrackUserProperty  未实现！  items：{items?.ToXJson()}");
        }

        #endregion

        #region 事件上报-ver2

        public virtual void Track(string eventName, KVItems items, EventLevel level = EventLevel.Level_0)
        {
            XGameSdk.Log(
                $"[BaseSdk] Track  未实现！ eventName：{eventName} items：{items?.ToXJson()} level：{level}");
        }

        #endregion

        #region 特殊事件上报 （比如：腾讯广告归因，分享，	游戏可玩（比如进入游戏大厅），用户点击看激励视频广告按钮，完成新手教程事件）

        public virtual void TrackSpecial(SpecialEventName eventName, KVItems items = null,
            EventLevel level = EventLevel.Level_0)
        {
            XGameSdk.Log(
                $"[BaseSdk] TrackSpecial  未实现！ eventName：{eventName.ToString()} items：{items?.ToXJson()} level：{level}");
        }

        #endregion


        #region 分享(逐渐统一普通分享)

        public virtual void Share(Dictionary<string, object> shareParams, Action<Dictionary<string, object>> success = null, Action<int, string> fail = null, Action cancel = null)
        {
            XGameSdk.Log($"[BaseSdk] Share  不支持！ shareParams：{shareParams?.ToXJson()}");
            fail?.Invoke(-1, "不支持分享");
        }

        public virtual void OnShareMessage(Func<Dictionary<string, object>, Dictionary<string, object>> callback, Action<Dictionary<string, object>> success = null, Action<int, string> fail = null, Action cancel = null)
        {
            XGameSdk.Log($"[BaseSdk] OnShareMessage 不支持！");
        }
        
        #endregion

        #region 微信-分享

        public virtual void ShareApp(string title, string imageUrl, string imageUrlId, Action result)
        {
            XGameSdk.Log($"[BaseSdk] ShareApp  不支持！ title：{title} imageUrl：{imageUrl} imageUrlId:{imageUrlId}");
        }

        public virtual void SetShareStyle(string title, string imageUrl, string imageUrlId)
        {
            XGameSdk.Log($"[BaseSdk] SetShareStyle  不支持！ title：{title} imageUrl：{imageUrl} imageUrlId:{imageUrlId}");
        }

        //带参数的分享
        public virtual void ShareAppWithQuery(string title, string imageUrl, string imageUrlId, string query,
            Action success)
        {
            XGameSdk.Log(
                $"[BaseSdk] ShareAppWithQuery  不支持！ title：{title ?? string.Empty} imageUrl：{imageUrl ?? string.Empty} imageUrlId:{imageUrlId ?? string.Empty} query:{query ?? string.Empty}");
        }

        /// <summary>
        /// 带任务的分享
        /// </summary>
        public virtual void ShareAppWithTask(string title, string imageUrl, string imageUrlId, string taskId,
            string ext,
            Action success)
        {
            XGameSdk.Log($"[BaseSdk] ShareAppWithTask  不支持！");
        }

        #endregion

        #region 微信-游戏圈

        public virtual IWXGameClubBtn CreateWXGameClubButton(int left, int top, int width, int height,
            string imageUrl = "images/game_club_btn.png")
        {
            XGameSdk.Log(
                $"[BaseSdk] CreateWXGameClubButton  不支持！返回模拟按钮 left：{left} top：{top} width:{width} height：{height} imageUrl：{imageUrl}");
            var btn = new SimpleWXGameClubBtn();
            return btn;
        }


        /// <summary>
        /// 获取微信游戏圈数据
        /// </summary>
        public virtual void GetWxGameClubData(List<(long type, string subkey)> dataTypeList,
            Action<GameClubDataByType[]> success,
            Action<string> fail)
        {
            XGameSdk.Log($"[BaseSdk] GetWxGameClubData  不支持！默认触发失败回调");
            fail?.Invoke("no support");
        }

        #endregion

        #region 微信-分享任务

        /// <summary>
        /// 上传分享任务
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="fromUid">分享者ID</param>
        /// <param name="ext">拓展参数</param>
        public virtual void UploadShareTask(string taskId, string fromUid, string ext, Action success,
            Action<string> fail)
        {
            XGameSdk.Log(
                $"[BaseSdk] UploadShareTask  不支持！默认触发失败回调 taskId:{taskId} fromUid:{fromUid},ext:{ext ?? string.Empty} ");
            fail?.Invoke("not support");
        }


        /// <summary>
        /// 获取分享
        /// </summary>
        public virtual void GetShareTaskDetail(string taskId, Action<ShareTaskDetailInfo[]> success,
            Action<string> fail)
        {
            XGameSdk.Log($"[BaseSdk] GetShareTaskDetail,taskId:{taskId}  不支持！ 默认触发失败回调");
            fail?.Invoke("not support");
        }

        #endregion

        #region 微信小游戏排行榜

        //获取排行榜数据
        //获取排行榜数据
        public virtual void GetRankData(RankData rankData, Action<RankResponseInfo[]> success, Action<string> fail)
        {
            // 在基类中，这个方法暂时不支持，直接调用失败回调并传递失败信息
            XGameSdk.Log("[BaseSdk] GetRankData 不支持！ 默认触发失败回调");
            fail?.Invoke("no support");
        }

        //上传用户排行榜数据
        public virtual void UploadUserRankData(UpScore upScore, Action success, Action<string> fail)
        {
            XGameSdk.Log($"[BaseSdk] UploadUserRankData, 不支持！ 默认触发失败回调");
            fail?.Invoke("no support");
        }

        #endregion

        #region 云控参数

        public virtual void RequestRemoteConfig(Action<string> complete)
        {
            XGameSdk.Log($"[BaseSdk] RequestRemoteConfig  不支持！ 返回空");
            complete?.Invoke("");
        }

        /// <summary>
        /// 请求云控参数；若原生会多次触发结果回调，应将每次结果都转给<strong>最近一次</strong>通过本方法注册的 callback（后注册覆盖前者）。<para/>
        /// 默认实现转调 <see cref="RequestRemoteConfig"/>：仅重写云控拉取逻辑时子类也会自动兼容本方法（运行时多态到子类的 <see cref="RequestRemoteConfig"/>）。
        /// </summary>
        public virtual void RequestRemoteConfigMulti(Action<string> complete)
        {
            RequestRemoteConfig(complete);
        }

        #endregion

        // #region H5-播放小游戏音效
        //
        // public virtual void PlayMiniGameBgm(string url)
        // {
        //     XGameSdk.Log($"[BaseSdk] PlayMiniGameBgm  不支持！ url：{url}");
        // }
        //
        // public virtual void StopMiniGameBgm()
        // {
        //     XGameSdk.Log($"[BaseSdk] StopMiniGameBgm  不支持！");
        // }
        //
        // #endregion

        // #region 微信-友盟上报
        //
        // public virtual void UmaTrackEvent(string eventId)
        // {
        //     XGameSdk.Log($"[BaseSdk] UmaTrackEvent 未实现！ eventId:{eventId}");
        // }
        //
        // public virtual void UmaTrackEvent(string eventId, params object[] keyValues)
        // {
        //     XGameSdk.Log($"[BaseSdk] UmaTrackEvent 未实现！ eventId:{eventId} keyValues:{keyValues.ToXJson()}  ");
        // }
        //
        // #endregion

        #region HTTP请求

        public virtual void HttpPost(string url, string data, Action<HttpPostSuccessResult> success,
            Action<string> fail,
            Dictionary<string, string> header = null, int timeOut = 60)
        {
            XGameSdk.Log($"[BaseSdk] HttpPost url:{url} data:{data}  header:{header}");
            //默认走unity post
            XHttpClient.Post(url, data, success, fail, header, timeOut);
        }

        public virtual void HttpGet(string url, Action<HttpGetSuccessResult> success, Action<string> fail,
            Dictionary<string, string> header = null, int timeOut = 60)
        {
            XGameSdk.Log($"[BaseSdk] HttpGet url:{url} header:{header}");
            XHttpClient.Get(url, success, fail, header, timeOut);
        }

        #endregion

        #region Toast

        public virtual int ShowToast(string text, bool blocksRaycasts = false, float duration = 1.5f)
        {
            XGameSdk.Log($"[BaseSdk] ShowToast text:{text} duration:{duration}");
            return ToastGUI.Instance.ShowToast(text, duration);
        }

        #endregion

        #region 云存档

        //云存档初始化
        public virtual void CloudArchive_Initialize(Action success, Action<string> fail)
        {
            XGameSdk.Log(
                $"[BaseSdk] CloudArchive_Initialize  默认初始化成功！");
            success?.Invoke();
        }

        public virtual void CloudArchive_GetKeys(Action<CloudArchiveGetKeysResult> success, Action<string> fail)
        {
            XGameSdk.Log(
                $"[BaseSdk] CloudArchive_GetKeys  不支持！触发失败回调");
            fail?.Invoke("no support");
        }

        public virtual void CloudArchive_GetData(string key, Action<CloudArchiveGetDateResult> success,
            Action<string> fail)
        {
            XGameSdk.Log(
                $"[BaseSdk] CloudArchive_GetData 不支持！ key:{key} 触发失败回调");
            fail?.Invoke("no support");
        }

        public virtual void CloudArchive_SetData(string key, long version, string content)
        {
            XGameSdk.Log(
                $"[BaseSdk] CloudArchive_SetData 不支持！ key:{key} version:{version} content:{content}");
        }

        public virtual void CloudArchive_UploadSync()
        {
            XGameSdk.Log($"[BaseSdk] CloudArchive_UploadSync 不支持！");
        }

        #endregion

        #region 游戏存档

        public virtual void GetGameArchive(Action<string, bool> success, Action<string> fail)
        {
            XGameSdk.Log($"[BaseSdk] GetGameArchive 不支持！触发失败回调");
            fail?.Invoke("not support");
        }

        public virtual void SyncGameArchive(string archive, Action success, Action<string> fail)
        {
            XGameSdk.Log($"[BaseSdk] SyncGameArchive 不支持！触发失败回调");
            fail?.Invoke("not support");
        }

        #endregion

        #region 是否为海外渠道

        public virtual bool IsSeaApp()
        {
            var flag = CommonTool.IsSeaApp();
            XGameSdk.Log($"[BaseSdk] IsSeaApp flag = {flag}");
            return flag;
        }

        #endregion

        #region 网络对时

        public virtual void GetNTP(Action<long> success, Action<string> fail)
        {
            XGameSdk.Log("[BaseSdk] GetNTP");
            HttpApi.GetNTP(success, fail);
        }

        #endregion

        #region IP地址

        public virtual void GetIp(Action<string> success, Action<string> fail)
        {
            XGameSdk.Log("[BaseSdk] GetIp");
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

        // #region 地区,定位
        //
        // public virtual void GetArea(Action<AreaData> success, Action<string> fail)
        // {
        //     XGameSdk.Log("[BaseSdk] GetArea");
        //     //如果本地
        //     if (IsSeaApp())
        //     {
        //         //返回默认值
        //         var data = new AreaData()
        //         {
        //             country = "海外",
        //             short_name = "",
        //             province = "",
        //             city = "",
        //             area = "",
        //             isp = "",
        //             ip = "127.0.0.1",
        //             code = 200,
        //             desc = "success"
        //         };
        //         //海外渠道
        //         success?.Invoke(data);
        //     }
        //     else
        //     {
        //         GetIp((ip) =>
        //         {
        //             //从ip获取信息
        //             HttpGet($"https://ip.useragentinfo.com/json?ip={ip.ToString()}", (res) =>
        //             {
        //                 var data = XJson.FromJson<AreaData>(res.Data);
        //                 success?.Invoke(data);
        //             }, (error) => { fail?.Invoke(error); });
        //         }, (error) => { fail?.Invoke(error); });
        //     }
        // }
        //
        // #endregion

        #region 国内硬核-更多精彩

        public virtual bool HasMoreGame()
        {
            XGameSdk.Log("[BaseSdk] HasMoreGame 不支持，返回false");
            return false;
        }

        public virtual void OpenMoreGame()
        {
            XGameSdk.Log("[BaseSdk] OpenMoreGame 默认不实现");
        }

        #endregion

        // #region 小游戏互推
        //
        // public virtual void MPush_ReqItems(string planId, int count, Action<MPushReqItemsData> success,
        //     Action<string> fail)
        // {
        //     XGameSdk.Log("[BaseSdk] MPush_ReqItems 不支持,触发失败回调");
        //     fail.Invoke("no support");
        // }
        //
        // public virtual void MPush_ClickItem(MPushContent content, Action success, Action<string> fail)
        // {
        //     XGameSdk.Log("[BaseSdk] MPush_ClickItem 不支持,触发失败回调");
        //     fail.Invoke("no support");
        // }
        //
        // #endregion
        //
        // #region XGP-API
        //
        // public virtual void XGPApi_User(string route, object body, Action<XGPApiResponse> response,
        //     Action<string> fail)
        // {
        //     XGameSdk.Log("[BaseSdk] XGPApi_User 不支持,触发失败回调");
        //     fail.Invoke("no support");
        // }
        //
        // #endregion

        #region Mar-特殊游戏开关

        public virtual bool GetSpecialGameSwitch()
        {
            XGameSdk.Log("[BaseSdk] GetSpecialGameSwitch 不支持,返回false");
            return false;
        }

        #endregion

        #region 侧边栏

        public virtual bool IsEnterFromSidebar()
        {
            XGameSdk.Log("[BaseSdk] IsEnterFromSidebar 不支持,返回false");
            return false;
        }

        public virtual bool IsSupportSidebar()
        {
            XGameSdk.Log("[BaseSdk] IsSupportSidebar 不支持,返回false");
            return false;
        }

        public virtual void NavigateToSidebar(Action success, Action<string> fail)
        {
            XGameSdk.Log("[BaseSdk] NavigateToSidebar 不支持，触发失败回调");
            fail?.Invoke("not support");
        }

        #endregion

        // #region 抖音-视频排行榜
        //
        // public virtual bool CanShareVideoToRank()
        // {
        //     XGameSdk.Log("[BaseSdk] CanShareVideoToRank return false");
        //     return false;
        // }
        //
        // public virtual void ShareVideoToRank(string title, string desc, string tag, string[] topics,
        //     Action<ShareAppSuccessResult> success,
        //     Action<string> fail, Action cancel)
        // {
        //     XGameSdk.Log("[BaseSdk] ShareVideoToRank 不支持，返回失败");
        //     fail?.Invoke("not support");
        // }
        //
        // public virtual void RequestVideoLikeRank(string tag, Action<VideoRankRsp> success, Action<string> fail,
        //     int numOfTop = 100,
        //     RankType rankType = RankType.Month, bool showToast = true)
        // {
        //     XGameSdk.Log("[BaseSdk] RequestVideoLikeRank 不支持，返回失败");
        //     fail?.Invoke("not support");
        // }
        //
        //
        // public virtual void RequestVideoTimeRank(string tag, Action<VideoRankRsp> success, Action<string> fail,
        //     int numOfTop = 100, bool showToast = true)
        // {
        //     XGameSdk.Log("[BaseSdk] RequestVideoTimeRank 不支持，返回失败");
        //     fail?.Invoke("not support");
        // }
        //
        // public virtual void NavigateToVideoView(string videoId, Action success, Action fail, Action complete)
        // {
        //     XGameSdk.Log("[BaseSdk] NavigateToVideoView 不支持，返回失败");
        //     fail?.Invoke();
        //     complete?.Invoke();
        // }
        //
        // #endregion

        #region Google-跳转订阅页

        public virtual void SkipSubscribePage()
        {
            XGameSdk.Log("[BaseSdk] SkipSubscribePage 空实现");
        }

        #endregion

        #region 获取android assets 目录文件

        public virtual string[] GetAAAndroidBundleFiles()
        {
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

        public virtual string[] GetAndroidAssetsFiles(string dir)
        {
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

        #region Google-隐私协议

        public virtual void ShowPrivacy()
        {
            XGameSdk.Log("[BaseSdk] ShowPrivacy 空实现!");
        }

        public virtual bool IsEURegion()
        {
            XGameSdk.Log("[BaseSdk] IsEURegion 不支持! 默认返回false");
            return false;
        }

        #endregion

        #region 获取游戏Json

        public virtual void GetGameJson(string matchVersion, Action<string> onDataReceived)
        {
            XGameSdk.Log("[BaseSdk] GetGameJson 空实现!");
        }

        #endregion

        #region 隐私协议入口按钮

        /// <summary>
        /// 是否支持隐私协议按钮
        /// </summary>
        public virtual bool IsSupportPrivacyBtn()
        {
            XGameSdk.Log("[BaseSdk] IsSupportPrivacyBtn 不支持! 默认返回false");
            return false;
        }

        #endregion

        #region 打开设置

        /// <summary>
        /// 打开wifi设置
        /// </summary>
        public virtual void OpenWifiSettings()
        {
            XGameSdk.Log("[BaseSdk] OpenWifiSettings 空实现!");
        }

        /// <summary>
        /// 打开手机设置
        /// </summary>
        public virtual void OpenPhoneSettings()
        {
            XGameSdk.Log("[BaseSdk] OpenPhoneSettings 空实现!");
        }

        #endregion

        #region 手机震动

        public virtual void PhoneVibrate(string type)
        {
            XGameSdk.Log("[BaseSdk] Vibrate 空实现!");
        }

        #endregion

        #region 获取登录成功后的功能白名单参数

        public virtual void GetFeatureWhitelist(Action<string[]> success, Action fail)
        {
            XGameSdk.Log("[BaseSdk] GetFeatureWhitelist 空实现!");
            fail?.Invoke();
        }

        #endregion

        #region 抖音排行榜

        public virtual void SetImRankData(ImRankData imRankData, Action<bool, string> action)
        {
            XGameSdk.Log("[BaseSdk] SetImRankData 空实现!");
            action?.Invoke(false, "SetImRankData 空实现!");
        }

        public virtual void ShowImRankList(ImRankListInfo imRankListInfo, Action<bool, string> action)
        {
            XGameSdk.Log("[BaseSdk] ShowImRankList 空实现!");
            action?.Invoke(false, "ShowImRankList 空实现!");
        }

        #endregion

        #region 资源分包pad

        public virtual void RequestAssetDelivery(string packName, Action<string, string> action)
        {
            XGameSdk.Log("[BaseSdk] RequestAssetDelivery 空实现!");
            action?.Invoke("", "");
        }

        public virtual string GetAssetPath(string packName)
        {
            XGameSdk.Log("[BaseSdk] GetAssetPath 空实现!");
            return "";
        }

        public virtual void RemovePack(string packName)
        {
            XGameSdk.Log("[BaseSdk] RemovePack 空实现!");
        }

        public virtual void CancelPack(string packName)
        {
            XGameSdk.Log("[BaseSdk] CancelPack 空实现!");
        }

        #endregion

        #region 获取设备信息

        //获取设备信息
        public virtual DeviceInfo GetDeviceInfo()
        {
            XGameSdk.Log(
                $"[BaseSdk] GetDeviceInfo GAID默认取SystemInfo.deviceUniqueIdentifier:{SystemInfo.deviceUniqueIdentifier}");
            return new DeviceInfo()
            {
                GAID = SystemInfo.deviceUniqueIdentifier,
                gaid = SystemInfo.deviceUniqueIdentifier
            };
        }

        #endregion

        #region 快手小游戏-获取补贴信息

        public virtual void GetSubsidyInfo(Action<SubsidyInfoResult> success, Action<string> fail)
        {
            XGameSdk.Log("[BaseSdk] GetSubsidyInfo 空实现!");
            fail?.Invoke("not support");
        }

        #endregion

        #region 获取安全区域位置大小

        public virtual Rect GetSafeArea()
        {
            var rect = Screen.safeArea;
            XGameSdk.Log($"[BaseSdk] GetSafeArea={rect}");
            return rect;
        }

        #endregion

        #region 激励插屏

        public virtual void ShowRewardInters(string sceneName, Action success, Action fail = null)
        {
            XGameSdk.Log($"[BaseSdk] ShowRewardInters 空实现！sceneName：{sceneName} 触发失败回调");
            fail?.Invoke();
        }

        public virtual bool HasRewardInters()
        {
            XGameSdk.Log($"[BaseSdk] HasRewardInters 默认返回:false");
            return false;
        }

        #endregion

        #region Google play Game服务

        public virtual void LoginGooglePlayGame(Action<LoginGooglePlayGameResult> callback)
        {
            XGameSdk.Log($"[BaseSdk] LoginGooglePlayGame 空实现 触发失败回调");
            var s = new LoginGooglePlayGameResult();
            s.code = GooglePlayGameCode.LoginNot;
            s.failMsg = "login no pass";
            callback?.Invoke(s);
        }
        
        
        public virtual void IsAuthenticatedGooglePlayGame(Action<LoginGooglePlayGameResult> callback)
        {
            XGameSdk.Log($"[BaseSdk] IsAuthenticatedGooglePlayGame 空实现 触发失败回调");
            var s = new LoginGooglePlayGameResult();
            s.code = GooglePlayGameCode.LoginAuthFailed;
            s.failMsg = "no login";
            callback?.Invoke(s);
        }
        
        
        

        public virtual void LoadSnapshot(Action<LoadSnapshotResult> callback)
        {
            XGameSdk.Log($"[BaseSdk] LoadSnapshot 空实现 触发失败回调");
            var s = new LoadSnapshotResult();
            s.code = GooglePlayGameCode.LoginNot;
            s.failMsg = "login no pass";
            callback?.Invoke(s);
        }

        public virtual void SaveSnapshot(string data, Action<SaveSnapshotResult> callback)
        {
            XGameSdk.Log($"[BaseSdk] SaveSnapshot 空实现 触发失败回调");
            var s = new SaveSnapshotResult();
            s.code = GooglePlayGameCode.LoginNot;
            s.failMsg = "login no pass";
            callback?.Invoke(s);
        }

        public virtual void SubmitScore(string leaderboardId, long score)
        {
            XGameSdk.Log($"[BaseSdk] SubmitScore 空实现");
        }

        public virtual void ShowLeaderboard(string leaderboardId)
        {
            XGameSdk.Log($"[BaseSdk] ShowLeaderboard 空实现");
        }

        public virtual void LoadCurrentPlayerLeaderboardScore(string leaderboardId, LeaderboardTimeSpan span,
            LeaderboardCollection leaderboardCollection, Action<CurrentPlayerLeaderboardScoreLoadResult> callback)
        {
            XGameSdk.Log($"[BaseSdk] LoadCurrentPlayerLeaderboardScore 空实现");
        }

        public virtual void LoadTopScores(string leaderboardId, LeaderboardTimeSpan span,
            LeaderboardCollection leaderboardCollection, int maxResults, bool forceReload,
            Action<TopScoresLoadResult> callback)
        {
            XGameSdk.Log($"[BaseSdk] LoadTopScores 空实现");
        }

        public virtual void ShowAchievements()
        {
            XGameSdk.Log($"[BaseSdk] ShowAchievements 空实现");
        }

        public virtual void UnlockAchievement(string achievementId)
        {
            XGameSdk.Log($"[BaseSdk] UnlockAchievement 空实现");
        }

        public virtual void IncrementAchievement(string achievementId, int incrementValue)
        {
            XGameSdk.Log($"[BaseSdk] IncrementAchievement 空实现");
        }

        #endregion

        #region XMY的消息推送

        public virtual bool IsRequirePushPermission()
        {
            XGameSdk.Log($"[BaseSdk] IsRequirePushPermission 空实现");
            return false;
        }

        public virtual void OpenPush()
        {
            XGameSdk.Log($"[BaseSdk] OpenPush 空实现");
        }

        public virtual string GetPushRewardMessage(string key = "REWARD_PUSH_DATA")
        {
            XGameSdk.Log($"[BaseSdk] GetPushRewardMessage 空实现");
            return "";
        }

        #endregion

        #region XMY的互推

        public virtual CrossInfo[] GetCrossList(string type, int count)
        {
            XGameSdk.Log($"[BaseSdk] GetCrossList 空实现");
            return Array.Empty<CrossInfo>();
        }

        public virtual void CrossAction(CrossActionType action, CrossInfo crossInfo, string sceneId)
        {
            XGameSdk.Log($"[BaseSdk] CrossAction 空实现");
        }

        public virtual string GetCrossRewardMessage(string key = "REWARD_CROSS_DATA")
        {
            XGameSdk.Log($"[BaseSdk] GetCrossRewardMessage 空实现");
            return "";
        }

        #endregion

        #region XMY应用更新

        public virtual bool IsAvailableNewVersion()
        {
            XGameSdk.Log($"[BaseSdk] IsAvailableNewVersion 空实现");
            return false;
        }

        public virtual void StartUpdateNewVersion()
        {
            XGameSdk.Log($"[BaseSdk] StartUpdateNewVersion 空实现");
        }

        #endregion


        #region 请求订阅信息通知

        public virtual void RequestSubscribeMessage(string[] tmplIds, Action<Dictionary<string, string>> success,
            Action<int, string> fail)
        {
            XGameSdk.Log($"[BaseSdk] RequestSubscribeMessage 空实现 tmplIds={tmplIds}");
        }

        #endregion


        #region 抖音的推荐流直出游戏

        public virtual FeedDirectPlayInfo GetLaunchInfoForFeedDirectPlay()
        {
            XGameSdk.Log($"[BaseSdk] GetLaunchInfoForFeedDirectPlay 空实现");
            return null;
        }

        public virtual void ReportGameReadyForFeedDirectPlay(long costTimeMs)
        {
            XGameSdk.Log($"[BaseSdk] ReportGameReadyForFeedDirectPlay 空实现");
        }

        public virtual void CheckSubscribeStatusForFeedDirectPlay(FeedDirectPlayInfo.GameScene scene, bool isAllScene,
            Action<bool> success, Action<int, string> failed)
        {
            XGameSdk.Log($"[BaseSdk] CheckSubscribeStatusForFeedDirectPlay 空实现");
        }

        public virtual void RequestSubscribeForFeedDirectPlay(FeedDirectPlayInfo.GameScene scene,
            List<string> contentIds, bool isAllScene, Action<bool> success, Action<int, string> failed)
        {
            XGameSdk.Log($"[BaseSdk] RequestSubscribeForFeedDirectPlay 空实现");
        }

        public virtual void StoreFeedDataForFeedDirectPlay(FeedDirectPlayInfo.GameScene scene, string contentId,
            int status, string rightValue, string operatorType = ">=", string leftValue = "timeStampMs",
            string extra = null, Action success = null, Action<int, string> failed = null)
        {
            XGameSdk.Log($"[BaseSdk] StoreFeedDataForFeedDirectPlay 空实现");
        }

        #endregion


        #region Google 监听切入切出

        public virtual void SetInOutCallback(Action<string> callback)
        {
            XGameSdk.Log($"[BaseSdk] SetInOutCallback 空实现");
        }

        #endregion

        #region 获取启动参数，支持渠道：抖音
        
        public virtual Dictionary<string, object> GetLaunchOptionsSync()
        {
            XGameSdk.Log($"[BaseSdk] GetLaunchOptionsSync 空实现");
            return null;
        }
        
        #endregion


        #region 小游戏前后台回调监听，支持渠道：抖音

        public virtual void OnMiniGameShow(Action<Dictionary<string, object>> callback)
        {
            XGameSdk.Log($"[BaseSdk] OnMiniGameShow 空实现");
        }
        
        public virtual void OnMiniGameHide(Action<Dictionary<string, object>> callback)
        {
            XGameSdk.Log($"[BaseSdk] OnMiniGameHide 空实现");
        }

        #endregion
        
        
    }
}