#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace XGame
{
    /// <summary>
    /// sdk 参数
    /// </summary>
    public class SdkPreference : SerializedScriptableObject
    {
        
        public enum RequestSubscribeMessageSuccessState
        {
            accept,
            reject,
            ban
        }
        [Header("请求订阅信息通知")]
        [Tooltip("请求订阅信息是否成功")] public bool IsRequestSubscribeMessageSuc = true;
        [Tooltip("请求订阅信息成功回调数据")] public Dictionary<string, RequestSubscribeMessageSuccessState> RequestSubscribeMessageSuccessParams = new Dictionary<string, RequestSubscribeMessageSuccessState>();

        
        [Header("广告")] [Tooltip("激励视频加载成功标记")]
        //视频flag
        public bool GetVideoFlag = true;

        [Tooltip("大图广告加载成功标记")]
        //原生大图Flag
        public bool GetBigNativeFlag = true;

        //原生广告
        [Tooltip("原生广告加载成功标记")] public bool GetNativeFlag = true;

        [Tooltip("插页广告加载成功标记")]
        //插页广告Flag
        public bool GetIntersFlag = true;

        [Tooltip("插页广告是否可展示（编辑器模拟）")] public bool CanShowInters = true;

        [Tooltip("视频回调结果")]
        //视频回调
        public bool VideoWatchSuccess = true;

        [Tooltip("模板广告加载成功标记")] public bool GetTemplateAdFlag = true;

        [Tooltip("贴片广告加载成功标记")] public bool GetPatchAdFlag = true;

        [Tooltip("单格子广告加载成功标记")] public bool GetNativeIconFlag = true;
        [Tooltip("多格子广告加载成功标记")] public bool GetBlockFlag = true;
        [Tooltip("互推盒子Banner广告加载成功标记")] public bool GetNavigateBoxBannerFlag = true;

        [Header("内购")] [Tooltip("返回支付成功/失败")] public bool PaySuccess = true;
        [Tooltip("开启支付延时返回")] public bool PayDelayReturn = false;

        [Tooltip("支付延迟返回时间")] [ShowIf("PayDelayReturn")]
        public int PayDelayReturnTime = 3;

        [Header("请求商品信息")] [Tooltip("请求商品信息(返回成功/失败)")]
        public bool RequestProductInfoSuccess = true;

        [Tooltip("请求商品信息(延迟返回时间)")] public float RequestProductInfoReturnDelay = 1f;

        [Tooltip("type:1是消耗类型，2是订阅类型")]
        [LabelText("商品信息(?)")] public ProductInfo[] ProductInfos = new[]
        {
            new ProductInfo()
            {
                id = "google_test_product_1",
                name = "google测试商品1",
                desc = "google测试商品1...描述",
                price = "$1",
                priceCurrencyCode = "USD",
                type = 1,
            }
        };

        [Header("设备信息")] public DeviceInfo deviceInfo;
        

        [Header("XMY互推")]
        [Tooltip("互推信息")] public CrossInfo[] CrossInfos = new[]
        {
            new CrossInfo()
            {
                type = "icon",
                name = "Mini Relaxing draw line",
                desc = "Mini Relaxing draw line描述",
                package = "com.xgame.DrawLineChallenge",
                weight = 100,
                isInstall = false,
                pic = "https://play-lh.googleusercontent.com/wk0_W01lol72GCiMsgUzmSJRQVMIX9hY-F4MlqtggS-_cWH7l0WIG9TAI6OtT0RZzcU=w240-h480-rw"
            }
        };
        
        [Header("订阅")] [Tooltip("请求订阅情况返回结果（成功/失败）")]
        public bool RequestSubscribeStatesSuccess = true;

        [Tooltip("请求订阅情况返回（延迟触发时）")] public float RequestSubscribeDelayReturn = 1f; //订阅返回延迟

        [Tooltip("订阅信息")]
        // [VerticalGroup("Google内购测试参数/订阅信息")]
        public SubscribeState[] SubscribeStates = new[]
        {
            new SubscribeState()
            {
                expireVaild = true,
                productId = "testSubscribe",
                expireTime = 2539998634000,
            }
        };

        [Header("恢复购买机制(IOS需要)")] [Tooltip("恢复购买回调结果：成功/失败")]
        public bool RestoreCompletePayInfoCallResult = true;

        [Tooltip("恢复购买回调延迟触发时间")] public float RestoreCompletePayInfoDelayTime = 1f;

        [Tooltip("恢复购买返回的商品清单")] public List<string> RestoreCompletePayInfoDataResult = new List<string>()
        {
            "模拟商品1(请改为正式商品ID)",
            "模拟商品2(请改为正式商品ID)"
        };


        [Header("隐私协议按钮相关（包括欧盟地区）")] [Tooltip("是否为欧盟地区")]
        public bool IsEURegion = true;

        [Tooltip("是否支持隐私协议按钮")] public bool IsSupportPrivacyBtn = true;


        [Header("兑换码")] [Tooltip("是否可以兑换礼物")] public bool CanGift = true;

        [Tooltip("兑换成功")] public bool GiftSuccess = true;

        [Tooltip("礼物道具数量")] public int GiftProductCount = 1;

        [Tooltip("礼物道具名")] public string GiftProductName = "test_gift_product_name";


        [Header("云存档")] [Tooltip("本地云存档服务器自动同步时间(单位：秒)")] [VerticalGroup("云存档")] [LabelText("同步时间间隔(单位：秒)")]
        public int LocalHostCloudArchiveServerSaveTime = 30;


        [Button("预览本地云存档数据", ButtonSizes.Large)]
        [VerticalGroup("云存档")]
        [HorizontalGroup("云存档/云存档H")]
        [GUIColor(0.36f, 0.71f, 1f)]
        private void PingCloudArchiveServer()
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                _ = LocalHostCloudArchive.Global;
                var pingTarget = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(LocalHostCloudArchive.JsonPath);
                if (pingTarget != null)
                {
                    EditorGUIUtility.PingObject(pingTarget);
                    Selection.activeObject = pingTarget;
                }
#endif
            }
        }

        [Button("清空本地云存档数据", ButtonSizes.Large)]
        [VerticalGroup("云存档")]
        [HorizontalGroup("云存档/云存档H")]
        [GUIColor(0.37f, 0.88f, 0.39f)]
        private void ClearCloudArchiveServer()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && !EditorApplication.isCompiling &&
                !EditorApplication.isPaused)
            {
                ShowMessage("Clear local cloud save data? This cannot be undone.", () =>
                {
                    LocalHostCloudArchive.ClearAllEditorStorage();
                    Debug.Log("Cleared.");
                });
            }
            else
            {
                ShowMessage("Cannot clear while the game is running.", null);
            }
#endif
        }


        [Header("其它")] [Tooltip("渠道名（Mar分发的渠道）")]
        public ChannelName ChannelName = ChannelName.Unknown;

        [Tooltip("是否有游戏忠告")] public bool HasGameCounsel = true;
        [Tooltip("国家代号")] public string CountryIso = "CN";
        [Tooltip("设备类型")] public DeviceType DeviceType = DeviceType.UnKnown;
        [Tooltip("SDK用户ID")] public string SDKUserID = "xgame_sdk_test_user_id";
        [Tooltip("SDK用户ID")] public string SdkUID = "xgame_sdk_test_uid";
        [Tooltip("SDK用户头像URL")] public string SDKUserPicUrl = "";
        [Tooltip("先使用Unity提供的网络判定确定是否有网络")] public bool UseUnityInternetReachability = true;
        [Tooltip("网络类型")] public NetworkType NetworkType = NetworkType.UnKnown;
        [Tooltip("模拟云控参数")] public TextAsset RemoteConfig;
        [Tooltip("是否有更多精彩")] public bool HasMoreGame = true;

        [Tooltip("获取游戏信息json")] public string GameJson = @"
    {
        ""matchVersion"": ""1.0.0.0"",
        ""gameChannelCodeNo"": ""12345"",
        ""debugFlag"": true
    }";


        [Header("区域信息")] [Tooltip("使用测试数据")] [VerticalGroup("区域信息")]
        public bool UseTestArea = false;

        [Tooltip("返回成功/失败")] [VerticalGroup("区域信息")] [ShowIf("UseTestArea")]
        public bool GetAreaReturn = true;

        // [Tooltip("区域信息")] [VerticalGroup("区域信息")] [ShowIf("UseTestArea")] [HideLabel]
        // public AreaData AreaData = new AreaData() { country = "海外", code = 200, desc = "success", ip = "127.0.0.1" };
        //
        // [Button("重置", ButtonSizes.Medium)]
        // [ShowIf("UseTestArea")]
        // [GUIColor(0.37f, 0.88f, 0.39f)]
        // [VerticalGroup("区域信息")]
        // private void ResetAreaData()
        // {
        //     AreaData = new AreaData() { country = "海外", code = 200, desc = "success", ip = "127.0.0.1" };
        // }

        [Header("Mar相关")] [Tooltip("是否有特殊游戏开关")]
        public bool GetSpecialGameSwitchResult = true;

        [Header("抖音小游戏相关")] [Tooltip("是否可录屏分享")]
        public bool CanRecord = true;

        [Tooltip("分享结果（true/false）")] public bool ShareRecordResult = true;
        [Tooltip("是否支持侧边栏功能")] public bool IsSupportSidebar = true;
        [Tooltip("是否从侧边栏进入")] public bool IsEnterFromSidebar = true;
        [Tooltip("跳转侧边栏返回")] public bool NavigateToSidebarResult = true;
        [Tooltip("是否支持快捷方式")] public bool IsSupportShortcut = true;
        [Tooltip("创建快捷方式返回结果")] public bool CreateShortcutReturnSuccess = true;
        [Tooltip("是否有客服功能")] public bool HasCustomerService = true;
        [Tooltip("开启客服页面回调结果(true/false)")] public bool OpenCustomerServiceResult = true;
        [Tooltip("可分享视频到排行榜(true/false)")] public bool CanShareVideoToRank = true;
        [Tooltip("分享视频到排行榜返回(成功/失败)")] public bool ShareVideoToRankResult = true;
        [Tooltip("请求视频排行榜返回(成功/失败)")] public bool RequestVideoRankResult = true;


        [Header("微信小游戏相关")] [Tooltip("微信分享回调返回")]
        public bool WXShareResult = true;

        [Header("分享任务（微信）")] [Tooltip("上传分享任务（触发成功/失败）")]
        public bool UploadShareTaskResult = true;

        [Tooltip("上传分享任务（延迟时间）")] public float UploadShareTaskDelayReturn = 1;

        [Tooltip("获取分享任务详情（触发成功/失败）")] public bool GetShareTaskDetailResult = true;
        [Tooltip("获取分享任务详情(延迟时间)")] public float GetShareTaskDetailDelayReturn = 1;

        [Tooltip("获取分享任务详情(模拟返回数据)")] public List<ShareTaskDetailInfo> GetShareTaskDetailReturnData =
            new List<ShareTaskDetailInfo>()
            {
                new ShareTaskDetailInfo()
                {
                    uid = "测试uid01",
                    ext = "拓展参数...",
                },
                new ShareTaskDetailInfo()
                {
                    uid = "测试uid02",
                    ext = "拓展参数...",
                },
            };

        [Header("拉取游戏圈数据（微信）")] [Tooltip("拉取微信游戏圈数据(触发成功/失败)")]
        public bool GetWxGameClubDataResult = true;

        [Tooltip("拉取微信游戏圈数据(模拟延迟返回)")] public float GetWxGameClubDataResultDelay = 1f;

        [Tooltip("拉取微信游戏圈数据(模拟返回数据)")] public List<GameClubDataByType> GetWxGameClubDataReturnData =
            new List<GameClubDataByType>()
            {
                new GameClubDataByType()
                {
                    dataType = 1,
                    value = 1711595137,
                },
                new GameClubDataByType()
                {
                    dataType = 3,
                    value = 0,
                },
                new GameClubDataByType()
                {
                    dataType = 4,
                    value = 100,
                },
                new GameClubDataByType()
                {
                    dataType = 5,
                    value = 30,
                },
                new GameClubDataByType()
                {
                    dataType = 6,
                    value = 10,
                },
                new GameClubDataByType()
                {
                    dataType = 7,
                    value = 5,
                },
                new GameClubDataByType()
                {
                    dataType = 8,
                    value = 20,
                },
                new GameClubDataByType()
                {
                    dataType = 9,
                    value = 6,
                },
                new GameClubDataByType()
                {
                    dataType = 10,
                    value = 600,
                },
            };

        [Header("微信小游戏排行榜")] [Tooltip("获取微信游戏排行榜数据(触发成功/失败)")]
        public bool GetRankDataResult = true;

        [Tooltip("获取微信游戏排行榜数据(模拟延迟返回)")] public float GetRankDataResultDelay = 1f;

        [Tooltip("获取排行榜数据(模拟返回数据)")] public RankResponseInfo[] GetRankDataReturnData = new RankResponseInfo[]
        {
            new RankResponseInfo()
            {
                rankId = "1",
                userId = "1",
                score = "1000",
                extra = "other"
            },
            new RankResponseInfo()
            {
                rankId = "2",
                userId = "2",
                score = "1001",
                extra = "other"
            },
        };

        [Tooltip("上传用户微信游戏排行榜数据(触发成功/失败)")] public bool GetUpLoadRankDataResult = true;

        [Tooltip("上传微信游戏排行榜数据(模拟延迟返回)")] public float GetUpLoadRankDataResultDelay = 1f;

        [Tooltip("上传微信用户排行榜数据(模拟返回数据)")] public UpScore[] GetUpLoadRankDataReturnData = new UpScore[]
        {
            new UpScore()
            {
                rankId = "1",
                rankCategory = "Level",
                score = 1000,
                extra = "other",
                groupId = "1"
            }
            // Add more simulated data as needed
        };

        // [Header("互推广告")] [LabelText("请求互推广告返回条目")]
        // public List<MPushContent> MPushContents = new List<MPushContent>()
        // {
        //     new MPushContent()
        //     {
        //         name = "互推广告-测试1",
        //         icon = "icon_1",
        //         content_id = 1001,
        //     },
        //     new MPushContent()
        //     {
        //         name = "互推广告-测试2",
        //         icon = "icon_2",
        //         content_id = 1002,
        //     },
        //     new MPushContent()
        //     {
        //         name = "互推广告-测试3",
        //         icon = "icon_3",
        //         content_id = 1003,
        //     }
        // };

        [LabelText("请求互推广告返回结果：true/false")] public bool MPushReqItemFlag = true;


        [LabelText("点击互推广告触发回调：true/false")] public bool MPushClickItemFlag = true;


        [VerticalGroup("XGP API")] [LabelText("XGP API 用户名")] [ShowInInspector] [Header("XGP API 相关")]
        public string XGPApi_UserName = string.Empty;

        [VerticalGroup("XGP API")]
        [Button("新账户")]
        public void XGPApiNewTestUser()
        {
            XGPApi_UserName =
                $"test_dev_{SystemInfo.deviceUniqueIdentifier}_{DateTime.UtcNow.ToString("yyyyMMddHHmmssss")}";
        }

        public string GetXGPApiUserName()
        {
            if (string.IsNullOrEmpty(XGPApi_UserName))
            {
                XGPApiNewTestUser();
            }

            return XGPApi_UserName;
        }


        /// <summary>
        /// 全局配置路径
        /// </summary>
        public const string GlobalPath = "Assets/XGameUnityTool_Gen/Editor/SdkPreference.asset";

        private static SdkPreference _global = null;

        //全局配置
        public static SdkPreference Global
        {
            get
            {
#if UNITY_EDITOR
                if (_global == null)
                {
                    var target =
                        AssetDatabase.LoadAssetAtPath<ScriptableObject>(GlobalPath) as SdkPreference;
                    if (target == null)
                    {
                        var instance = CreateInstance<SdkPreference>();
                        AssetDatabase.CreateAsset(instance, GlobalPath);
                        AssetDatabase.Refresh();
                    }

                    _global = target;
                }
#endif
                return _global;
            }
        }


        private static void ShowMessage(string content, Action success, Action fail = null)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("Confirm", content, "Yes"))
                success?.Invoke();
            else
                fail?.Invoke();
#endif
        }
    }
}
#endif