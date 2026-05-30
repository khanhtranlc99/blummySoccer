using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using XGame.BuildApp;


namespace XGame
{
    [CreateAssetMenu(menuName = "XGame Build/XGame WebGL App Setting")]
    public class XGameWebglAppSetting : WebglAppSetting, IXGameAppSetting
    {
        public enum WebGlTemplate
        {
            Default,
            Minimal,
        }

        [FoldoutGroup("Publish Setting")] public string PublishName;

        //参数修改
        // [FoldoutGroup("自定义")] public AppChannel Channel;
        [FoldoutGroup("Publish Setting")]
        public GraphicsDeviceType[] GraphicsAPIs = new[] { GraphicsDeviceType.OpenGLES2 };

        [FoldoutGroup("Publish Setting")] public bool OverrideWebglTemplate = false;
        [FoldoutGroup("Publish Setting")] public WebGlTemplate WebglTemplate = WebGlTemplate.Minimal;
        [FoldoutGroup("Publish Setting")] public bool OverrideWebglCompressionFormat = false;
        [FoldoutGroup("Publish Setting")] public WebGLCompressionFormat CompressionFormat = WebGLCompressionFormat.Gzip;

        [FoldoutGroup("Publish Setting")]
        public WebGLExceptionSupport ExceptionSupport = WebGLExceptionSupport.FullWithoutStacktrace;

        [FoldoutGroup("Publish Setting")] public bool DebugSymbols = false;

#if UNITY_2021_3_OR_NEWER
        [FoldoutGroup("Publish Setting")] [LabelText("Texture Compression")]
        public WebGLTextureSubtarget TextureSubtarget = WebGLTextureSubtarget.ASTC;
#endif


        [FoldoutGroup("AA Setting", expanded: true)]
        [LabelText("Enable Addressable Settings")]
        [ShowIf("HasAddressable")]
        [LabelWidth(180)]
        public bool EnableAddressableSetting = false;


        [FoldoutGroup("AA Setting")] [ShowIf("ShowAddressableUseProfile")] [ValueDropdown("ProfileSettingsOption")]
        public string AddressableUseProfile;


        [FoldoutGroup("AA Setting")]
        [ShowIf("ShowAddressableUseProfile")]
        [SerializeField]
        [ListDrawerSettings(DefaultExpandedState = true)]
        [LabelText("Addressable parameter modification")]
        //aa构建版本
        public List<AddressableVariableModify> AAVariables = new List<AddressableVariableModify>();


        public AppChannel GetChannel => Channel;

        public bool GetEnableAddressableSetting => EnableAddressableSetting;


        public string GetAddressableUseProfile => AddressableUseProfile;


        public List<AddressableVariableModify> GetAAVariablesModify => AAVariables;


        public override void BuildAA()
        {
            BuildAddressableAssetTool.BuildExisting(GetAddressableUseProfile, setting: this);
        }

        private string[] ProfileSettingsOption
        {
            get
            {
                if (AddressableReflection.HasModule())
                {
                    return AddressableReflection.Settings?.profileSettings?.GetAllProfileNames().ToArray();
                }

                return new string[0];
            }
        }

        protected override void OnAfterSwitchPlateComplete()
        {
            base.OnAfterSwitchPlateComplete();

            if (GraphicsAPIs.Length > 0)
            {
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
                PlayerSettings.SetGraphicsAPIs(BuildTarget.WebGL, GraphicsAPIs);
            }
            else
            {
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, true);
            }

#if UNITY_2021_3_OR_NEWER
            EditorUserBuildSettings.webGLBuildSubtarget = TextureSubtarget;
#endif
            if (OverrideWebglCompressionFormat)
            {
                PlayerSettings.WebGL.compressionFormat = CompressionFormat;
            }

            if (OverrideWebglTemplate)
            {
                switch (WebglTemplate)
                {
                    case WebGlTemplate.Default:
                        PlayerSettings.WebGL.template = "APPLICATION:Default";
                        break;
                    case WebGlTemplate.Minimal:
                        PlayerSettings.WebGL.template = "APPLICATION:Minimal";
                        break;
                }
            }

            ApplyTool.ApplyChannelSDK(Channel);

            PlayerSettings.WebGL.exceptionSupport = ExceptionSupport;
            PlayerSettings.WebGL.debugSymbols = DebugSymbols;
            //生成config
            XGameBuildAppUtility.udpateGeVersion(GetChannel);
            XGameBuildAppUtility.CreateAppConfig(GetChannel, VersionString, BuildType);
            //修改addressable
            XGameBuildAppUtility.ModifyAddressableConfig(this);
            AssetDatabase.Refresh();

            Debug.Log("Switch completed.");
        }

        protected override void OnPublish()
        {
            base.OnPublish();
            switch (Channel)
            {
                case AppChannel.WeChat_XSDK:
                    BuildWXMiniGameProject();
                    break;
                case AppChannel.Douyin_XSDK_IOS:
                    BuildDouyinGame();
                    break;
                case AppChannel.Oppo_XSDK:
                    BuildOppoMiniGameProject();
                    break;
                case AppChannel.Vivo_XSDK:
                    BuildVivoMiniGameProject();
                    break;
                case AppChannel.Huawei_XSDK:
                    BuildHuaweiQuickGame();
                    break;
                case AppChannel.Bilibili_XSDK:
                    BuildBilibiliMiniGame();
                    break;
                case AppChannel.Kuaishou_XSDK:
                    BuildKuaishouGame();
                    break;
            }
        }

        private void BuildDouyinGame()
        {
            var type = XGameEditorUtil.GetType("XGame.PublishDouyinXSDK");
            if (type != null)
            {
                dynamic publish = Activator.CreateInstance(type);
                publish.Publish(this);
            }
        }


        private void BuildKuaishouGame()
        {
            var type = XGameEditorUtil.GetType("XGame.PublishKuaishouGame");
            if (type != null)
            {
                dynamic publish = Activator.CreateInstance(type);
                publish.Publish(this);
            }
        }

        private void BuildBilibiliMiniGame()
        {
            var type = XGameEditorUtil.GetType("XGame.PublishBilibiliMiniGame");
            if (type != null)
            {
                dynamic publish = Activator.CreateInstance(type);
                publish.Publish(this);
            }
        }

        private void BuildHuaweiQuickGame()
        {
            if (AppConfig.HUAWEI_XSDK_CHANNEL_ID <= 0)
            {
                XGameEditorUtil.ShowMessageBox(
                    "Huawei Quick Game XSDK Channel ID is invalid. Open settings?",
                    XGameMenuWindow.Open);
                return;
            }

            if (string.IsNullOrEmpty(AppConfig.HUAWEI_APP_ID))
            {
                XGameEditorUtil.ShowMessageBox(
                    "Huawei Quick Game app id is empty. Open settings?",
                    XGameMenuWindow.Open);
                return;
            }

            if (AppConfig.HUAWEI_KUAIGAME_GE_IS_ON && string.IsNullOrEmpty(AppConfig.HUAWEI_KUAIGAME_GE_ACCESS_TOKEN))
            {
                XGameEditorUtil.ShowMessageBox(
                    "Huawei Quick Game: Gravity Engine analytics is on but accessToken is empty. Open settings?",
                    XGameMenuWindow.Open);
                return;
            }

            var msg = "See the Quick Game export docs: export a WebGL project, import it in the Quick Game developer tool, then fill in the parameters to convert to a Quick Game project.\n" +
                      "Documentation: https://gitee.com/petal-gaming-services/UnityToQuickGame/blob/main/doc/Unity-WebGL%E5%BF%AB%E6%B8%B8%E6%88%8F%E9%80%82%E9%85%8D%E6%96%B9%E6%A1%88.md\n" +
                      "Quick Game developer tool: https://developer.huawei.com/consumer/cn/doc/quickApp-Guides/quickgame-tool-download-0000001166035569\n" +
                      "Copy URLs from the log if needed.";
            Debug.Log(msg);
            XGameEditorUtil.ShowMessageBox(msg);
        }


        //构建微信小游戏啊工程
        private void BuildWXMiniGameProject()
        {
            //使用反射进行发布
            var type = XGameEditorUtil.GetType("XGame.PublishWxMiniGame");
            if (type != null)
            {
                dynamic publish = Activator.CreateInstance(type);
                publish.Publish(this);
            }
        }

        //构建oppo小游戏工程
        private void BuildOppoMiniGameProject()
        {
            //使用反射进行发布
            var type = XGameEditorUtil.GetType("XGame.PublishOppoMiniGame");
            if (type != null)
            {
                dynamic publish = Activator.CreateInstance(type);
                publish.Publish(this);
            }
        }


        //构建vivo小游戏工程
        private void BuildVivoMiniGameProject()
        {
            //使用反射进行发布
            var type = XGameEditorUtil.GetType("XGame.PublishVivoMini");
            if (type != null)
            {
                dynamic publish = Activator.CreateInstance(type);
                publish.Publish(this);
            }
        }


        private bool HasAddressable => AddressableReflection.HasModule();

        private bool ShowAddressableUseProfile => HasAddressable && EnableAddressableSetting;
    }
}