using System;
using UnityEditor;
using UnityEngine;

namespace XGame.BuildApp
{
    public class XGameBuildAppUtility : ScriptableObject
    {

        public static void udpateGeVersion(AppChannel channel)
        {
                        //更新ToolPreference
            switch (channel)
            {
                case AppChannel.Douyin_XSDK_Android:
                case AppChannel.Douyin_XSDK_IOS:
                    if (ToolPreference.Global.OpenByteDanceGe && ToolPreference.Global.ByteDanceGeAutoVersionCode)
                    {
                        //开启了自动版本号
                        ToolPreference.Global.ByteDanceGeVersionCode += 1;
                        EditorUtility.SetDirty(ToolPreference.Global);
                        AssetDatabase.SaveAssets();
                        XGameEditorUtil.Log($"[抖音]引力引擎 自动APP版本号-->{ToolPreference.Global.ByteDanceGeVersionCode}",
                            XGameEditorUtil.LogColor.Success);
                    }

                    break;
                case AppChannel.Kuaishou_XSDK:
                case AppChannel.Kuaishou_XSDK_Android:
                    if (ToolPreference.Global.OpenKuaishouGe && ToolPreference.Global.KuaishouGeAutoVersionCode)
                    {
                        //开启了自动版本号
                        ToolPreference.Global.KuaishouGeVersionCode += 1;
                        EditorUtility.SetDirty(ToolPreference.Global);
                        AssetDatabase.SaveAssets();
                        XGameEditorUtil.Log($"[快手]引力引擎 自动APP版本号-->{ToolPreference.Global.KuaishouGeVersionCode}",
                            XGameEditorUtil.LogColor.Success);
                    }
                    break;
                case AppChannel.Huawei_XSDK:
                    if (ToolPreference.Global.OpenGeHuaweiKuaiGame && ToolPreference.Global.GeAutoVersionCodeHuaweiKuaiGame)
                    {
                        //开启了自动版本号
                        ToolPreference.Global.GeVersionCodeHuaweiKuaiGame += 1;
                        EditorUtility.SetDirty(ToolPreference.Global);
                        AssetDatabase.SaveAssets();
                        XGameEditorUtil.Log($"[华为快游戏]引力引擎 自动APP版本号-->{ToolPreference.Global.GeVersionCodeHuaweiKuaiGame}",
                            XGameEditorUtil.LogColor.Success);
                    }
                    break;
                case AppChannel.Bilibili_XSDK:
                    if (ToolPreference.Global.OpenBilibiliGe && ToolPreference.Global.BilibiliGeAutoVersionFlag)
                    {
                        //开启了自动版本号
                        ToolPreference.Global.BilibiliGeVersion += 1;
                        EditorUtility.SetDirty(ToolPreference.Global);
                        AssetDatabase.SaveAssets();
                        XGameEditorUtil.Log($"[B站小游戏]引力引擎 自动APP版本号-->{ToolPreference.Global.BilibiliGeVersion}",
                            XGameEditorUtil.LogColor.Success);
                    }
                    break;
            }
        }


        public static void UpdateAppConfig()
        {
            var douyinXSDKChannelId = ToolPreference.Global.DouyinXSDKChannel;
            var byteDanceGeIsOn = ToolPreference.Global.OpenByteDanceGe;
            var byteDanceGeAccessToken = ToolPreference.Global.ByteDanceGeAccessToken;
            var byteDanceGeVersion = ToolPreference.Global.ByteDanceGeVersionCode;
            
            var kuaishouXSDKChannelId = ToolPreference.Global.KuaishouXSDKChannel;
            var kuaishouAppId = ToolPreference.Global.KuaishouAppId;
            var kuaishouGeIsOn = ToolPreference.Global.OpenKuaishouGe;
            var kuaishouGeAccessToken = ToolPreference.Global.KuaishouGeAccessToken;
            var kuaishouGeVersion = ToolPreference.Global.KuaishouGeVersionCode;
            
            var oppoXSDKChannelId = ToolPreference.Global.OppoXSDKSdkChannelId;
            
            var vivoXSDKChannelId = ToolPreference.Global.VivoXSDKSdkChannelId;
            
            var wxXSDKChannelId = ToolPreference.Global.WeChatXSDKSdkChannelId;
            
            var huaweiXSDKChannelId = ToolPreference.Global.HuaweiXSDKChannel;
            var huaweiXSDKVersionName = ToolPreference.Global.HuaweiXSDKVersionName;
            var huaweiAppId = ToolPreference.Global.HuaweiAppId;
            var geIsOnhHuaweiKuaiGame = ToolPreference.Global.OpenGeHuaweiKuaiGame;
            var geAccessTokenHuaweiKuaiGame = ToolPreference.Global.GeAccessTokenHuaweiKuaiGame;
            var geVersionCodeHuaweiKuaiGame = ToolPreference.Global.GeVersionCodeHuaweiKuaiGame;
            
            
            AppConfigAsset.Instance.DOUYIN_XSDK_CHANNEL_ID = douyinXSDKChannelId;
            
            AppConfigAsset.Instance.OPPO_XSDK_CHANNEL_ID = oppoXSDKChannelId;
            
            AppConfigAsset.Instance.VIVO_XSDK_CHANNEL_ID = vivoXSDKChannelId;
            
            AppConfigAsset.Instance.WX_XSDK_CHANNEL_ID = wxXSDKChannelId;
            
            AppConfigAsset.Instance.KUAISHOU_XSDK_CHANNEL_ID = kuaishouXSDKChannelId;
            AppConfigAsset.Instance.KUAISHOU_APP_ID = kuaishouAppId;
            AppConfigAsset.Instance.KUAISHOU_GE_IS_ON = kuaishouGeIsOn;
            AppConfigAsset.Instance.KUAISHOU_GE_ACCESS_TOKEN = kuaishouGeAccessToken;
            AppConfigAsset.Instance.KUAISHOU_GE_VERSION = kuaishouGeVersion;
            
            AppConfigAsset.Instance.BYTE_DANCE_GE_IS_ON = byteDanceGeIsOn;
            AppConfigAsset.Instance.BYTE_DANCE_GE_ACCESS_TOKEN = byteDanceGeAccessToken;
            AppConfigAsset.Instance.BYTE_DANCE_GE_VERSION = byteDanceGeVersion;
            
            AppConfigAsset.Instance.HUAWEI_XSDK_CHANNEL_ID = huaweiXSDKChannelId;
            AppConfigAsset.Instance.HUAWEI_XSDK_VERSION_NAME = huaweiXSDKVersionName;
            AppConfigAsset.Instance.HUAWEI_APP_ID = huaweiAppId;
            AppConfigAsset.Instance.HUAWEI_KUAIGAME_GE_ACCESS_TOKEN = geAccessTokenHuaweiKuaiGame;
            AppConfigAsset.Instance.HUAWEI_KUAIGAME_GE_IS_ON = geIsOnhHuaweiKuaiGame;
            AppConfigAsset.Instance.HUAWEI_KUAIGAME_GE_VERSION = geVersionCodeHuaweiKuaiGame;
            
            AppConfigAsset.Instance.BILIBILI_XSDK_CHANNEL_ID = ToolPreference.Global.BilibiliXSDKChannel;
            AppConfigAsset.Instance.BILIBILI_GE_IS_ON = ToolPreference.Global.OpenBilibiliGe;
            AppConfigAsset.Instance.BILIBILI_GE_ACCESS_TOKEN = ToolPreference.Global.BilibiliGeAccessToken;
            AppConfigAsset.Instance.BILIBILI_GE_VERSION = ToolPreference.Global.BilibiliGeVersion;
            
            AppConfigAsset.Instance.WX_GE_IS_ON = ToolPreference.Global.OpenWxGe;
            AppConfigAsset.Instance.WX_GE_ACCESS_TOKEN = ToolPreference.Global.WxGeAccessToken;
            AppConfigAsset.Instance.WX_GE_VERSION = ToolPreference.Global.WxGeVersion;
            
            EditorUtility.SetDirty(AppConfigAsset.Instance);
            AssetDatabase.SaveAssets();
            
            Debug.Log("修改AppConfigAsset成功！");
            
        }
        
        //生成AppConfig
        public static void CreateAppConfig(AppChannel channel,
            string versionString, BuildType buildType)
        {
            var dateTime = DateTime.Now;
            var dateTimeString =
                $"{dateTime.Year.ToString().Remove(0, 2)}{dateTime.Month.ToString().PadLeft(2, '0')}{dateTime.Day.ToString().PadLeft(2, '0')}";
            //修改设置
            AppConfigAsset.Instance.CHANNEL = channel;
            
            AppConfigAsset.Instance.VERSION = versionString;
            AppConfigAsset.Instance.BUILD_TYPE = buildType;
            AppConfigAsset.Instance.BUILD_DATE_TIME = dateTimeString;
            
            UpdateAppConfig();
            
        }

        public static void ModifyGlobalMacros(AppChannel channel)
        {
            XGameEditorUtil.ClearScriptingDefine(new[] { "GRAVITY_WECHAT_GAME_MODE", "GRAVITY_KUAISHOU_GAME_MODE", "GRAVITY_BYTEDANCE_GAME_MODE","GRAVITY_BILIBILI_GAME_MODE","GRAVITY_KUAISHOU_WEBGL_GAME_MODE","GRAVITY_BYTEDANCE_TT_GAME_MODE","GRAVITY_HUAWEI_GAME_MODE" });
            switch (channel)
            {
                case AppChannel.WeChat_XSDK:
                    XGameEditorUtil.AddScriptingDefine(new[] { "GRAVITY_WECHAT_GAME_MODE" });
                    break;
                case AppChannel.Douyin_XSDK_Android:
                case AppChannel.Douyin_XSDK_IOS:
                    XGameEditorUtil.AddScriptingDefine(new[] { "GRAVITY_BYTEDANCE_TT_GAME_MODE" });
                    break;
                case AppChannel.Kuaishou_XSDK:
                    XGameEditorUtil.AddScriptingDefine(new[] { "GRAVITY_KUAISHOU_WEBGL_GAME_MODE" });
                    break;
                case AppChannel.Kuaishou_XSDK_Android:
                    XGameEditorUtil.AddScriptingDefine(new[] { "GRAVITY_KUAISHOU_GAME_MODE" });
                    break;
                case AppChannel.Bilibili_XSDK:
                    XGameEditorUtil.AddScriptingDefine(new[] { "GRAVITY_BILIBILI_GAME_MODE" });
                    break;
                case AppChannel.Huawei_XSDK:
                    XGameEditorUtil.AddScriptingDefine(new[] { "GRAVITY_HUAWEI_GAME_MODE" });
                    break;
                
            }
        }


        //修改addressable配置
        public static void ModifyAddressableConfig(IXGameAppSetting appSetting)
        {
            if (!appSetting.GetEnableAddressableSetting)
            {
                return;
            }

            if (!AddressableReflection.HasDefaultSettings())
            {
                return;
            }

            var settings = AddressableReflection.Settings;
            //激活profile
            settings.activeProfileId =
                settings.profileSettings.GetProfileId(appSetting.GetAddressableUseProfile);
            //调整参数
            if (appSetting.GetAAVariablesModify != null)
            {
                foreach (var modify in appSetting.GetAAVariablesModify)
                {
                    BuildAddressableAssetTool.SetAddressableValueByName(appSetting.GetAddressableUseProfile,
                        modify.VariableName, modify.Value);
                }
            }
        }
    }
}