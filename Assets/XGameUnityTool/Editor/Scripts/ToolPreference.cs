using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using XGame.BuildApp;

namespace XGame
{
    /// <summary>
    /// 部署配置
    /// </summary>
    public class ToolPreference : SerializedScriptableObject
    {
        private static GUIStyle _boldLabel = null;

        public static GUIStyle BoldLabel
        {
            get
            {
                if (_boldLabel == null)
                {
                    var style = GUI.skin.label;
                    style.fontStyle = FontStyle.Bold;
                    _boldLabel = style;
                }

                return _boldLabel;
            }
        }

        //检查更新
        [LabelText("Auto check for updates")] [PropertyOrder(-1)] [LabelWidth(140)] [HorizontalGroup("th1", 180)]
        public bool CheckUpdate = true;

        [LabelText("Important Notice to Me")] [HorizontalGroup("th1")] [PropertyOrder(-1)] [LabelWidth(140)]
        public bool MajorNotice = true;

        
        #region XMYGoogle配置

        // [NonSerialized] private string _xmyGoogleGradlePath = null;
        
        // private const string XMY_GRADLE_PATH = ".xgameunitytool/xmy_gradle_path.txt";

        // public string XMYGoogleGradlePath
        // {
        //     get
        //     {
        //         if (_xmyGoogleGradlePath == null)
        //         {
        //             var path = XMY_GRADLE_PATH;
        //             if (!File.Exists(path))
        //             {
        //                 var dir = Path.GetDirectoryName(path);
        //                 if (!Directory.Exists(dir))
        //                 {
        //                     Directory.CreateDirectory(dir);
        //                 }
        //
        //                 File.WriteAllText(path, "");
        //             }
        //
        //             _xmyGoogleGradlePath = File.ReadAllText(path);
        //         }
        //
        //         return _xmyGoogleGradlePath;
        //     }
        // }

        [FoldoutGroup("Google（XMY，LogSDK）配置")]
        [Sirenix.OdinInspector.FilePath(AbsolutePath = true)]
        [LabelText("google-services.json文件路径(?)")]
        [Tooltip("可选，向运营要取该文件，需要跟游戏一一对应。设置该文件时会和母包aar生成一个*_allPackage.zip，方便直接给运营使用这个zip来打包。如果使用了国家分包和pad分包，也会包含在这个zip里。")]
        [LabelWidth(200)]
        public string XMYGoogleServicesJsonPath;
        
        [FoldoutGroup("Google（XMY，LogSDK）配置")]
        [LabelText("gradle依赖库(?)")]
        [Tooltip("可选，添加依赖库到最终母包(*_allPackage.zip)的sdk_pack_config.json文件里。填写依赖格式例如：androidx.appcompat:appcompat:1.6.1")]
        [LabelWidth(200)]
        public List<string> XMYGradleDependencyLibrary;
        
        [FoldoutGroup("Google（XMY，LogSDK）配置")]
        [LabelText("资源包(?)")]
        [Tooltip("可选，添加资源包（*.aar,*.jar）到最终母包(*_allPackage.zip)的game/libs目录里")]
        [LabelWidth(200)]
        [Sirenix.OdinInspector.FilePath(AbsolutePath = true)]
        public List<string> XMYResourceFilePaths;
        
        [FoldoutGroup("Google（XMY，LogSDK）配置")]
        [LabelText("资源目录(?)")]
        [Tooltip("可选，添加res或者assets资源目录到最终母包(*_allPackage.zip)的game目录里")]
        [LabelWidth(200)]
        [Sirenix.OdinInspector.FolderPath(AbsolutePath = true)]
        public List<string> XMYResourceFolderPaths;
        
        [FoldoutGroup("Google（XMY，LogSDK）配置")] 
        [LabelText("使用自定义unity-classes.jar(?)")]
        [LabelWidth(200)]
        [Tooltip("一般情况不用管。只有因为使用pad分包功能导致游戏启动闪退才能使用！")]
        public bool OverrideXMYUnityClassJar = false;

        [FoldoutGroup("Google（XMY，LogSDK）配置")]
        [Sirenix.OdinInspector.FilePath(AbsolutePath = true)]
        [ShowIf("OverrideXMYUnityClassJar")]
        [LabelText("unity-classes.jar路径")]
        public string XMYUnityClassPath;

        
        // private void BrowseXMYGoogleGradlePath()
        // {
        //     var path = ToolPreferenceInspector.BrowseGradle();
        //     if (!string.IsNullOrWhiteSpace(path))
        //     {
        //         _xmyGoogleGradlePath = path;
        //         XGameEditorUtil.CreateFileAndWriteText(XMY_GRADLE_PATH, path);
        //         EditorUtility.SetDirty(this);
        //         AssetDatabase.SaveAssets();
        //     }
        // }

        [FoldoutGroup("Google（XMY，LogSDK）配置"), PropertyOrder(-1)]
        [OnInspectorGUI]
        private void GoogleXMYGUITips()
        {
            var temColor = GUI.color;
            GUI.color = new Color(0.11f, 0.84f, 0.42f, 1f);
            GUILayout.Label("1.发布到欧盟地区,游戏内必须有主动弹出隐私协议的入口\n   相关API：XGameSdk.IsSupportPrivacyBtn, XGameSdk.ShowPrivacy");

            var last = GUILayoutUtility.GetLastRect();
            var btnDetail = last;
            btnDetail.width = 60;
            btnDetail.height = 24;
            btnDetail.x = last.xMax - btnDetail.width;
            btnDetail.x -= 4;
            if (GUI.Button(btnDetail, "接入指南"))
            {
                Application.OpenURL(
                    "https://qu2tef36bb.feishu.cn/docx/Vasjd7bhOoNqMHxcAUCcMVrGnNg#WX6WdLrQ5oi5fbx5A3hctpkUnLb");
            }

            GUILayout.Label("2.如果想生成一个最终母包(*_allPackage.zip)，方便运营直接使用。\n请首先设置google-services.json文件路径，该文件需要跟游戏一一对应，请向运营要取。");

            
            GUI.color = temColor;
            
            
            // GUI.color = EditorGUIUtility.isProSkin ? new Color(1f, 0.72f, 0f, 1f) : new Color(0.6f, 0.43f, 0f, 1f);
            // GUILayout.Label(
                // "1.Google有两套SDK，分别为[XA] [XMY],具体用哪套问运营同学,此为XMY配置栏\n2.发布窗口中找不到对应打包配置，再次导入常用打包配置即可（注意备份，以免覆盖后信息丢失）\n3.XMY打包配置:  (Google_XMY_SDK)  (Google_XMY_SDK_APK包)");
            // GUI.color = temColor;
            //
            // GUILayout.BeginHorizontal();
            // GUILayout.Label("Gradle路径:", GUILayout.Height(32), GUILayout.Width(80));
            // var lastPath = XMYGoogleGradlePath;
            // lastPath = GUILayout.TextArea(lastPath, GUILayout.Height(32));
            // if (lastPath != XMYGoogleGradlePath)
            // {
            //     EditorUtility.SetDirty(this);
            // }
            //
            // if (GUILayout.Button("浏览", GUILayout.Height(32), GUILayout.Width(60)))
            // {
            //     BrowseXMYGoogleGradlePath();
            // }
            //
            // GUILayout.EndHorizontal();
            // if (!XMYGradlePathIsValid())
            // {
            //     SirenixEditorGUI.ErrorMessageBox("gradle 路径无效 找不到bin/gradle.bat");
            // }
        }
        
        #endregion

        #region 抖音小游戏配置
        
        [FoldoutGroup("抖音小游戏配置")] [Header("XSDK SDK 专用参数")] [LabelText("XSDK Channel ID（问运营同学要）")] [LabelWidth(200)]
        public int DouyinXSDKChannel = -1;
        
        [FoldoutGroup("抖音小游戏配置")]
        [LabelText("开启引力引擎统计")]
        [Header("引力引擎统计")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public bool OpenByteDanceGe = true;

        [FoldoutGroup("抖音小游戏配置")]
        [LabelText("AccessToken（问运营同学要）")]
        [ShowIf("OpenByteDanceGe")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public string ByteDanceGeAccessToken = string.Empty;

        [FoldoutGroup("抖音小游戏配置")]
        [LabelText("自动版本号")]
        [ShowIf("OpenByteDanceGe")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public bool ByteDanceGeAutoVersionCode = true;

        [FoldoutGroup("抖音小游戏配置")]
        [LabelText("发布版本（统计时划分APP版本）")]
        [ShowIf("ShowByteDanceGeVersionCode")]
        [LabelWidth(200)]
        [PropertyOrder(200)]
        public int ByteDanceGeVersionCode = 1;

        private bool ShowByteDanceGeVersionCode => !ByteDanceGeAutoVersionCode && OpenByteDanceGe;

        [FoldoutGroup("抖音小游戏配置"), PropertyOrder(-1)]
        [LabelWidth(200)]
        [OnInspectorGUI]
        private void ByteDanceTipsGUI()
        {
            var temColor = GUI.color;
            GUI.color = new Color(0.11f, 0.84f, 0.42f, 1f);
            GUILayout.Label("①安卓推荐使用Native模式发布,限制Unity版本 2021.3.14");
            GUILayout.Label("②安卓需要接入录屏分享功能，含有内购的游戏需要接入客服功能");
            GUILayout.Label("③IOS发布走的webgl模式,推荐Unity版本：2021（支持astc压缩,坑最少）");
            GUILayout.Label("④如果需要上IOS端，webgl不支持多线程，修改代码进行适配");
            GUILayout.Label("⑤需要接入侧边栏功能");
            GUI.color = temColor;
            // GUI.color = EditorGUIUtility.isProSkin ? new Color(1f, 0.72f, 0f, 1f) : new Color(0.6f, 0.43f, 0f, 1f);
            // GUILayout.Label(
            //     "1.抖音小游戏相关SDK一共有两套，分别为[XGP],[XSDK],具体用哪套问运营同学,填写其专用参数即可\n2.发布窗口中找不到对应打包配置，再次导入常用打包配置即可（注意备份，以免覆盖后信息丢失）\n3.XGP打包配置:  (抖音_XGP_Android)  (抖音_XGP_IOS)\n4.XSDK打包配置:  (抖音_XSDK_Android)  (抖音_XSDK_IOS)");
            // GUI.color = temColor;
        }


        [FoldoutGroup("抖音小游戏配置")]
        [LabelWidth(200)]
        [PropertyOrder(200)]
        [ShowIf("OpenByteDanceGe")]
        [OnInspectorGUI]
        private void OpenByteDanceGeTipsGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorIcons.UnityInfoIcon, GUILayout.Width(30), GUILayout.Height(32));
            var color = GUI.color;
            GUI.color = Color.green;
            GUILayout.TextArea(
                "注意：字节开发者后台添加request合法域名:https://backend.gravity-engine.com和https://api.gravity-engine.com",
                BoldLabel, GUILayout.Height(32));
            GUI.color = color;
            GUILayout.EndHorizontal();
        }

        #endregion

        #region 微信小游戏配置

        [FoldoutGroup("微信小游戏配置"), PropertyOrder(-1)]
        [OnInspectorGUI]
        private void WXTipsGUI()
        {
            var temColor = GUI.color;
            GUI.color = new Color(0.11f, 0.84f, 0.42f, 1f);
            GUILayout.Label("①使用ugui时，EventSystem上添加WXTouchInputAdapter组件，否则不支持多点触控");
            // GUILayout.Label("②使用addressable作为加载方式时，远端资源需要放在导出目录：webgl/StreamingAssets下");
            // GUILayout.Label("      对应修改好addressable远端下载地址，否则不会进行缓存，造成重复下载，导致cdn浪费");
            GUILayout.Label("②webgl不支持多线程，修改代码进行适配");
            // GUI.color = EditorGUIUtility.isProSkin ? new Color(1f, 0.72f, 0f, 1f) : new Color(0.6f, 0.43f, 0f, 1f);
            // GUILayout.Label(
            //     "1.微信相关SDK一共有三套，分别为[XGP],[ASC],[XSDK],具体用哪套问运营同学,填写其专用参数即可\n2.发布窗口中找不到对应打包配置，再次导入常用打包配置即可（注意备份，以免覆盖后信息丢失）\n3.XGP打包配置:  (微信小游戏_XGP_SDK)\n4.ASC打包配置:  (微信小游戏_ASC_SDK)\n5.XSDK打包配置:  (微信小游戏_XSDK_SDK)");
            GUI.color = temColor;
        }
        
        [Header("XSDK SDK 专用参数")] [FoldoutGroup("微信小游戏配置")] [LabelText("XSDK Channel ID（问运营同学要）")] [LabelWidth(200)]
        public int WeChatXSDKSdkChannelId = -1;
        [FoldoutGroup("微信小游戏配置")] [LabelText("versionName（预上线功能）")] [LabelWidth(200)]
        public string WeChatXSDKVersionName = "";
        
        [Header("通用参数")] [FoldoutGroup("微信小游戏配置")] [LabelText("游戏appid（问运营同学要）")] [LabelWidth(200)]
        public string WeChatAppId;

        [FoldoutGroup("微信小游戏配置")] [LabelText("游戏资源CDN")] [LabelWidth(200)]
        public string WeChatCdn = "http://www.xxx.com/wxcdn/v1001";

        [FoldoutGroup("微信小游戏配置")] [LabelText("小游戏项目名")] [LabelWidth(200)]
        public string WeChatProjName = "微信小游戏";

        [FoldoutGroup("微信小游戏配置")] [LabelText("预留内存MB (建议256~1024)")] [Range(256, 1024)] [LabelWidth(200)]
        public int WeChatUnityHeap = 512;

        [FoldoutGroup("微信小游戏配置")] [LabelText("导出路径")] [FolderPath(AbsolutePath = true)] [LabelWidth(200)]
        public string WeChatExportProj = $"wxgame";


        [FoldoutGroup("微信小游戏配置")] [LabelText("游戏方向")] [LabelWidth(200)]
        public GameResolution WeChatResolution = GameResolution.Portrait;

        [FoldoutGroup("微信小游戏配置")]
        [LabelText("加载背景图(?)")]
        [Tooltip("如果不设置，就使用微信默认背景图")]
        [ValidateInput("WeChatLoadingBackgroundMatch", "只支持.png或.jpg")]
        [LabelWidth(200)]
        public Texture WeChatLoadingBackground;

        [FoldoutGroup("微信小游戏配置")] [LabelText("高级设置")] [LabelWidth(200)]
        public WXAdvancedOptions AdvancedOptions = new WXAdvancedOptions();

        [FoldoutGroup("微信小游戏配置")]
        [LabelText("分享的图片 (悬浮查看详细说明) (推荐宽高比：5：4)")]
        [Tooltip("发布后会自动拷贝到小游戏工程 share_images 目录下,调用分享API时图片参数填：share_images/xxx.jpg")]
        [LabelWidth(200)]
        public Texture2D[] WeChatShareImages;

        [FoldoutGroup("微信小游戏配置")]
        [LabelText("游戏圈按钮图标 (1:1)(png格式)(?)")]
        [LabelWidth(200)]
        [Tooltip("发布后会自动拷贝到小游戏工程 images目录下,images/xxx.png")]
        public Texture2D WeChatGameClubButtonImage;


        // [FoldoutGroup("微信小游戏配置")] [LabelText("发布后弹出理压缩提示窗口")] [LabelWidth(200)]
        // public bool AskWXCompressTexture = false;

        
        [FoldoutGroup("微信小游戏配置")] [Header("腾讯广告归因")] [LabelText("开启腾讯广告归因")] [PropertyOrder(100)] [LabelWidth(200)]
        public bool OpenTaAttribution = false;
        
        [FoldoutGroup("微信小游戏配置")] [LabelText("AppId")] [ShowIf("OpenTaAttribution")] [PropertyOrder(100)] [LabelWidth(200)]
        public string TaAttributionAppId = "";
        
        [FoldoutGroup("微信小游戏配置")] [LabelText("SecretKey")] [ShowIf("OpenTaAttribution")] [PropertyOrder(100)] [LabelWidth(200)]
        public string TaAttributionSecretKey = "";
        
        [FoldoutGroup("微信小游戏配置")] [LabelText("SourceId")] [ShowIf("OpenTaAttribution")] [PropertyOrder(100)] [LabelWidth(200)]
        public int TaAttributionSourceId = 0;
        
        // [FoldoutGroup("微信小游戏配置")] [LabelText("开启友盟统计")] [Header("友盟统计")] [PropertyOrder(100)] [LabelWidth(200)]
        // public bool OpenWXUma = false;

        // [FoldoutGroup("微信小游戏配置")]
        // [LabelText("友盟AppKey（问运营同学要）")]
        // [ShowIf("OpenWXUma")]
        // [PropertyOrder(100)]
        // [LabelWidth(200)]
        // public string UmaWXAppKey = "";
        //
        // [FoldoutGroup("微信小游戏配置")]
        // [PropertyOrder(100)]
        // [ShowIf("OpenWXUma")]
        // [OnInspectorGUI]
        // private void UmaWXTipsGUI()
        // {
        //     GUILayout.BeginHorizontal();
        //     GUILayout.Label(EditorIcons.UnityInfoIcon, GUILayout.Width(30), GUILayout.Height(32));
        //     var color = GUI.color;
        //     GUI.color = Color.green;
        //     GUILayout.TextArea(
        //         "注意：微信开发者后台添加request合法域名:https://umini.shujupie.com",
        //         BoldLabel, GUILayout.Height(32));
        //     GUI.color = color;
        //     GUILayout.EndHorizontal();
        // }

        [FoldoutGroup("微信小游戏配置")] [LabelText("开启引力引擎统计")] [Header("引力引擎统计")] [PropertyOrder(200)] [LabelWidth(200)]
        public bool OpenWxGe = false;

        [FoldoutGroup("微信小游戏配置")]
        [LabelText("AccessToken（问运营同学要）")]
        [ShowIf("OpenWxGe")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public string WxGeAccessToken = string.Empty;

        [FoldoutGroup("微信小游戏配置")]
        [LabelText("开启自动 Version")]
        [ShowIf("OpenWxGe")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public bool WxGeAutoVersionFlag = true;

        [FoldoutGroup("微信小游戏配置")]
        [LabelText("Version")]
        [ShowIf("ShowWxGeVersion")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public int WxGeVersion = 0;


        private bool ShowWxGeVersion => OpenWxGe & !WxGeAutoVersionFlag;


        [FoldoutGroup("微信小游戏配置")]
        [PropertyOrder(200)]
        [ShowIf("OpenWxGe")]
        [OnInspectorGUI]
        private void OpenWxGeTipsGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorIcons.UnityInfoIcon, GUILayout.Width(30), GUILayout.Height(32));
            var color = GUI.color;
            GUI.color = Color.green;
            GUILayout.TextArea(
                "注意：微信开发者后台添加request合法域名:https://backend.gravity-engine.com和https://api.gravity-engine.com",
                BoldLabel, GUILayout.Height(32));
            GUI.color = color;
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Oppo小游戏配置

        [FoldoutGroup("Oppo小游戏配置"), PropertyOrder(-1)]
        [OnInspectorGUI]
        private void OppoTipsGUI()
        {
            var temColor = GUI.color;
            GUI.color = new Color(0.11f, 0.84f, 0.42f, 1f);
            // GUILayout.TextArea("①限制Unity版本：2019.2.1", "label", GUILayout.Height(22));
            // GUILayout.TextArea("②oppo不支持addressables,请改用assetbundle作为加载方式,工具提供了简易的ab加载插件,", "label",
                // GUILayout.Height(22));
            // GUILayout.TextArea("      搜索assetbundle-typhoon.unitypackage导入使用", "label", GUILayout.Height(22));
            // GUILayout.TextArea("③需要有隐私协议窗口，适龄提醒，详情问运营同学", "label", GUILayout.Height(22));
            GUILayout.TextArea("webgl不支持多线程，修改代码进行适配", "label", GUILayout.Height(22));
            // GUI.color = EditorGUIUtility.isProSkin ? new Color(1f, 0.72f, 0f, 1f) : new Color(0.6f, 0.43f, 0f, 1f);
            // GUILayout.Label(
            //     "1.OPPO小游戏相关SDK一共有三套，分别为[XGP]（弃用）,[ASC]（弃用）,[XSDK],具体用哪套问运营同学,填写其专用参数即可\n2.发布窗口中找不到对应打包配置，再次导入常用打包配置即可（注意备份，以免覆盖后信息丢失）\n3.XGP打包配置:  (Oppo小游戏_XGP_SDK)（弃用）\n4.ASC打包配置:  (Oppo小游戏_ASC_SDK)（弃用）\n5.XSDK打包配置:  (Oppo小游戏_XSDK_SDK)");
            GUI.color = temColor;
        }
        
        [FoldoutGroup("Oppo小游戏配置")] [Header("XSDK SDK 专用参数")] [LabelText("XSDK Channel ID（问运营同学要）")] [LabelWidth(220)]
        public int OppoXSDKSdkChannelId = -1;
        [FoldoutGroup("Oppo小游戏配置")] [LabelText("versionName（预上线功能）")] [LabelWidth(220)]
        public string OppoXSDKVersionName = "";
        
        [FoldoutGroup("Oppo小游戏配置")] [LabelText("限制帧率(偶发动画抖动问题尝试把帧率降到50或更低)")] [LabelWidth(300)]
        public bool OppoMiniLimitFrameRate = false;

        [FoldoutGroup("Oppo小游戏配置")]
        [LabelText("目标帧率")]
        [Range(30, 60)]
        [ShowIf("OppoMiniLimitFrameRate")]
        [LabelWidth(200)]
        public int OppoMiniFrameRate = 50;


        // [FoldoutGroup("Oppo小游戏配置")]
        // [LabelText("签名：private.pem（问运营同学要）")]
        // [Sirenix.OdinInspector.FilePath]
        // [ValidateInput("$OppoPrivatePemIsMatch", "请选择private.pem文件")]
        // [LabelWidth(200)]
        // public string OppoMiniPrivatePemPath;

        // [FoldoutGroup("Oppo小游戏配置")]
        // [LabelText("签名：certificate.pem（问运营同学要）")]
        // [Sirenix.OdinInspector.FilePath]
        // [ValidateInput("$OppoCertificatePemIsMatch", "请选择certificate.pem文件")]
        // [LabelWidth(200)]
        // public string OppoMiniCertificatePemPath;

        // [FoldoutGroup("Oppo小游戏配置")]
        // [Button("创建签名", ButtonSizes.Medium)]
        // private void CreateOppoSign()
        // {
        //     CreateOpensslSign();
        // }

        #endregion

        #region Vivo小游戏

        
        [FoldoutGroup("Vivo小游戏配置")] [Header("XSDK SDK 专用参数")] [LabelText("XSDK Channel ID（问运营同学要）")] [LabelWidth(200)]
        public int VivoXSDKSdkChannelId = -1;  
        [FoldoutGroup("Vivo小游戏配置")] [LabelText("versionName（预上线功能）")] [LabelWidth(200)]
        public string VivoXSDKVersionName = "";
        
        [FoldoutGroup("Vivo小游戏配置")]
        [LabelText("游戏icon (大小<100k)")]
        [ValidateInput("$VivoMiniLogoMatch", "只支持png文件")]
        [LabelWidth(200)]
        public Texture VivoMiniLogo = null;

        [FoldoutGroup("Vivo小游戏配置")] [LabelText("限制帧率(偶发动画抖动问题尝试把帧率降到50或更低)")] [LabelWidth(300)]
        public bool VivoMiniLimitFrameRate = false;
        
        [FoldoutGroup("Vivo小游戏配置")]
        [LabelText("目标帧率")]
        [Range(30, 60)]
        [ShowIf("VivoMiniLimitFrameRate")]
        [LabelWidth(200)]
        public int VivoMiniFrameRate = 50;
        
        
        [FoldoutGroup("Vivo小游戏配置")]
        [LabelText("签名：private.pem（问运营同学要）")]
        [Sirenix.OdinInspector.FilePath(AbsolutePath = true)]
        [ValidateInput("$VivoPrivatePemIsMatch", "请选择private.pem文件")]
        [LabelWidth(200)]
        public string VivoMiniPrivatePemPath;
        
        [FoldoutGroup("Vivo小游戏配置")]
        [LabelText("签名：certificate.pem（问运营同学要）")]
        [Sirenix.OdinInspector.FilePath(AbsolutePath = true)]
        [ValidateInput("$VivoCertificatePemIsMatch", "请选择certificate.pem文件")]
        [LabelWidth(200)]
        public string VivoMiniCertificatePemPath;
        
        // [FoldoutGroup("Vivo小游戏配置")]
        // [Button("创建签名", ButtonSizes.Medium)]
        // private void CreateVivoSign()
        // {
        //     CreateOpensslSign();
        // }

        [FoldoutGroup("Vivo小游戏配置"), PropertyOrder(-1)]
        [OnInspectorGUI]
        private void VivoTipsGUI()
        {
            var temColor = GUI.color;
            GUI.color = new Color(0.11f, 0.84f, 0.42f, 1f);
            // GUILayout.Label("①推荐Unity版本：2019,2020");
            // GUILayout.Label("②需要有隐私协议窗口，适龄提醒，详情问运营同学");
            GUILayout.Label("webgl不支持多线程，修改代码进行适配");
            // GUI.color = EditorGUIUtility.isProSkin ? new Color(1f, 0.72f, 0f, 1f) : new Color(0.6f, 0.43f, 0f, 1f);
            // GUILayout.Label(
                // "1.VIVO小游戏SDK一共有三套，分别为[XGP],[ASC],[XSDK],具体用哪套问运营同学,填写其专用参数即可\n2.发布窗口中找不到对应打包配置，再次导入常用打包配置即可（注意备份，以免覆盖后信息丢失）\n3.XGP打包配置:  (Vivo小游戏_XGP_SDK)\n4.ASC打包配置:  (Vivo小游戏_ASC_SDK)\n5.XSDK打包配置:  (Vivo小游戏_XSDK_SDK)");
            GUI.color = temColor;
            var last = GUILayoutUtility.GetLastRect();
        }

        #endregion

        #region 快手小游戏
        
        [FoldoutGroup("快手小游戏配置")] [Header("XSDK SDK 专用参数")] [LabelText("XSDK Channel ID（问运营同学要）")] [LabelWidth(200)]
        public int KuaishouXSDKChannel = -1;
        
        [FoldoutGroup("快手小游戏配置")]
        [Header("快手平台参数")]
        [LabelText("快手小游戏 app ID（问运营同学要）")]
        [LabelWidth(200)]
        public string KuaishouAppId = "";
        
        [FoldoutGroup("快手小游戏配置")]
        [Header("快手小游戏Web发布配置")]
        [LabelText("导出路径")]
        [FolderPath(AbsolutePath = true)]
        [LabelWidth(200)]
        public string KuaishouExportProj = "";
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("游戏资源CDN")]
        [LabelWidth(200)]
        public string KuaishouCdn = "";
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("不自动缓存文件类型(?)")]
        [PropertyTooltip("(使用;分割)当请求url包含资源'cdn+StreamingAssets'时会自动缓存，但StreamingAssets目录下不是所有文件都需缓存，此选项配置不需要自动缓存的文件拓展名。默认值json")]
        [LabelWidth(200)]
        public string KuaishouBundleExcludeExtensions = "json;";
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("Bundle名称Hash长度(?)")]
        [PropertyTooltip("自定义Bundle文件名中hash部分长度，默认值32，用于缓存控制")]
        [LabelWidth(200)]
        public int KuaishouBundleHashLength = 32;
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("WASM代码包Brotli压缩")]
        [LabelWidth(200)]
        public bool KuaishouEnableWasmBrCompress = false;
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("并行下载代码包和资源")]
        [LabelWidth(200)]
        public bool KuaishouEnableParallel = false; 
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("data资源包Brotli压缩(?)")]
        [PropertyTooltip("将首包资源Brotli压缩, 降低资源大小. 注意: 仅推荐使用小游戏并行下载时，节省包体大小使用")]
        [LabelWidth(200)]
        public bool KuaishouEnableDataBrCompress = false;
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("预下载Bundle列表(?)")]
        [PropertyTooltip("只需配置文件名称，使用;间隔")]
        [LabelWidth(200)]
        public string KuaishouPreloadFiles = "";
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("动态资源列表接口")]
        [LabelWidth(200)]
        public string KuaishouPreloadUrl = "";
        
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("Development Build")]
        [LabelWidth(200)]
        public bool KuaishouDevelopBuild = false;
        
        
        [FoldoutGroup("快手小游戏配置")]
        [Header("引力引擎统计")]
        [LabelText("开启引力引擎统计")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public bool OpenKuaishouGe = false;
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("AccessToken（问运营同学要）")]
        [ShowIf("OpenKuaishouGe")]
        [LabelWidth(200)]
        [PropertyOrder(200)]
        public string KuaishouGeAccessToken = string.Empty;
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("自动版本号")]
        [ShowIf("OpenKuaishouGe")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public bool KuaishouGeAutoVersionCode = true;
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelText("发布版本（统计时划分APP版本）")]
        [ShowIf("ShowKuaishouGeVersionCode")]
        [LabelWidth(200)]
        [PropertyOrder(200)]
        public int KuaishouGeVersionCode = 1;
        
        private bool ShowKuaishouGeVersionCode => OpenKuaishouGe && !KuaishouGeAutoVersionCode;
        
        
        [FoldoutGroup("快手小游戏配置")]
        [LabelWidth(200)]
        [PropertyOrder(200)]
        [ShowIf("OpenKuaishouGe")]
        [OnInspectorGUI]
        private void OpenGeTipsGUIKuaishouGame()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorIcons.UnityInfoIcon, GUILayout.Width(30), GUILayout.Height(32));
            var color = GUI.color;
            GUI.color = Color.green;
            GUILayout.TextArea(
                "快手小游戏后台确认是否需要添加request合法域名:https://backend.gravity-engine.com和https://api.gravity-engine.com",
                BoldLabel, GUILayout.Height(32));
            GUI.color = color;
            GUILayout.EndHorizontal();
        }
        
        
        #endregion

        #region b站小游戏
        
        [FoldoutGroup("b站小游戏配置")] [Header("XSDK SDK 专用参数")] [LabelText("XSDK Channel ID（问运营同学要）")] [LabelWidth(200)]
        public int BilibiliXSDKChannel = -1;
        
        [FoldoutGroup("b站小游戏配置")] [LabelText("appid（问运营同学要）")] [LabelWidth(200)]
        public string BilibiliAppId;

        [FoldoutGroup("b站小游戏配置")] [LabelText("游戏资源CDN")] [LabelWidth(200)]
        public string BilibiliCdn = "";

        [FoldoutGroup("b站小游戏配置")] [LabelText("小游戏项目名")] [LabelWidth(200)]
        public string BilibiliProjName = "";

        [FoldoutGroup("b站小游戏配置")] [LabelText("预留内存MB (建议256~1024)")] [Range(256, 1024)] [LabelWidth(200)]
        public int BilibiliUnityHeap = 512;

        [FoldoutGroup("b站小游戏配置")] [LabelText("导出路径")] [FolderPath] [LabelWidth(200)]
        public string BilibiliExportProj = "";


        [FoldoutGroup("b站小游戏配置")] [LabelText("横竖屏模式")] [LabelWidth(200)]
        public GameResolution BilibiliResolution = GameResolution.Portrait;

        [FoldoutGroup("b站小游戏配置")]
        [LabelText("加载背景图")]
        [ValidateInput("BilibiliLoadingBackgroundMatch", "只支持.png或.jpg")]
        [LabelWidth(200)]
        public Texture BilibiliLoadingBackground;

        [FoldoutGroup("b站小游戏配置")] [LabelText("高级设置")] [LabelWidth(200)]
        public BilibiliAdvancedOptions BilibiliAdvancedOptions = new BilibiliAdvancedOptions();
        
        [FoldoutGroup("b站小游戏配置")] [LabelText("开启引力引擎统计")] [Header("引力引擎统计")] [PropertyOrder(200)] [LabelWidth(200)]
        public bool OpenBilibiliGe = false;

        [FoldoutGroup("b站小游戏配置")]
        [LabelText("AccessToken（问运营同学要）")]
        [ShowIf("OpenBilibiliGe")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public string BilibiliGeAccessToken = string.Empty;

        [FoldoutGroup("b站小游戏配置")]
        [LabelText("开启自动 Version")]
        [ShowIf("OpenBilibiliGe")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public bool BilibiliGeAutoVersionFlag = true;

        [FoldoutGroup("b站小游戏配置")]
        [LabelText("Version")]
        [ShowIf("ShowBilibiliGeVersion")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public int BilibiliGeVersion = 1;
        
        private bool ShowBilibiliGeVersion => OpenBilibiliGe && !BilibiliGeAutoVersionFlag;
        
        [FoldoutGroup("b站小游戏配置")]
        [PropertyOrder(200)]
        [ShowIf("OpenBilibiliGe")]
        [OnInspectorGUI]
        private void OpenBilibiliGeTipsGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorIcons.UnityInfoIcon, GUILayout.Width(30), GUILayout.Height(32));
            var color = GUI.color;
            GUI.color = Color.green;
            GUILayout.TextArea(
                "注意：b站开发者后台添加request合法域名:https://backend.gravity-engine.com和https://api.gravity-engine.com",
                BoldLabel, GUILayout.Height(32));
            GUI.color = color;
            GUILayout.EndHorizontal();
        }
        
        
        #endregion


        #region 鸿蒙

        [FoldoutGroup("鸿蒙配置"), PropertyOrder(-1)]
        [OnInspectorGUI]
        private void HarmonyLightTipsGUI()
        {
            var temColor = GUI.color;
            GUI.color = new Color(0.11f, 0.84f, 0.42f, 1f);
            GUILayout.Label("注意事项：");
            GUILayout.Label("请务必在build settings里的player settings鸿蒙平台设置好icon，游戏名，包名，版本号，版本名，签名文件，屏幕方向。");
            GUILayout.Label("这样发布生成的鸿蒙工程可以直接运行测试和构建安装包。");
            GUI.color = temColor;
        }
        
        [Serializable]
        public class HarmonyLightConfigs
        {
            // {
            //     "appId": "15",
            //     "appKey": "22ad6d6fe2e34eaea44d4b8082050256",
            //     "ltChannelID": 12213,
            //     "ltUrl": "https://api.server.xgame.xplaymobile.com",
            //     "channelParams": {
            //     },
            //     "adParams": {
            //         "HARMONY_AD_INTERS_IDS": "testb4znbuh3n2",
            //         "HARMONY_AD_VIDEO_IDS": "testx9dtjwj8hp"
            //     }
            // }
            
            [Serializable]
            [LabelText("渠道参数")]
            public class ChannelParams
            {
            }
            [Serializable]
            [LabelText("广告参数")]
            public class AdParams
            {
                [LabelText("插屏广告ID")]
                public string HARMONY_AD_INTERS_IDS = "";
                [LabelText("视频广告ID")]
                public string HARMONY_AD_VIDEO_IDS = "";
            }
            
            public string appId = "";
            public string appKey = "";
            public int ltChannelID;
            public string ltUrl = "https://api.server.xgame.xplaymobile.com";

            public ChannelParams channelParams = new ChannelParams();
            public AdParams adParams = new AdParams();

        }
        
        [FoldoutGroup("鸿蒙配置")]
        [Header("SDK参数")]
        [LabelText("Light SDK 参数(?)")]
        [Tooltip("请跟运营同学确认具体的参数")]
        public HarmonyLightConfigs harmonyLightConfig = new HarmonyLightConfigs();

        [FoldoutGroup("鸿蒙配置")]
        [Header("启动界面设置")]
        [LabelText("启动图")]
        [Sirenix.OdinInspector.FilePath(AbsolutePath = true)]
        public string harmonyLightLaunchImage;
        
        [FoldoutGroup("鸿蒙配置")]
        [LabelText("适龄图，不填默认八岁")]
        [Sirenix.OdinInspector.FilePath(AbsolutePath = true)]
        public string harmonyLightAgeAppropriateImage;
        
        [FoldoutGroup("鸿蒙配置")]
        [LabelText("游戏名称")]
        public string harmonyLightGameName;
        
        [FoldoutGroup("鸿蒙配置")]
        [LabelText("著作权人")]
        public string harmonyLightCopyrightOwner;
        
        [FoldoutGroup("鸿蒙配置")]
        [LabelText("用户协议地址")]
        public string harmonyLightUserAgreement;

        [FoldoutGroup("鸿蒙配置")]
        [LabelText("隐私协议地址")]
        public string harmonyLightPrivacyPolicy;

        [FoldoutGroup("鸿蒙配置")]
        [Header("引力引擎统计")]
        [LabelText("启用引力引擎上报")]
        public bool harmonyLightEnableGravityEngine;

        [FoldoutGroup("鸿蒙配置")]
        [LabelText("引力 Access Token")]
        [ShowIf("harmonyLightEnableGravityEngine")]
        public string gravityAccessToken = string.Empty;

        [FoldoutGroup("鸿蒙配置")]
        [LabelText("是否内购游戏(?)")]
        [Tooltip("控制游戏的启动界面隐私政策内容显示")]
        public bool harmonyLightIsPurchaseGame;
        

        [FoldoutGroup("鸿蒙配置")]
        [LabelText("批准文号")]
        [ShowIf("harmonyLightIsPurchaseGame")]
        public string harmonyLightApprovalNumber;
        
        [FoldoutGroup("鸿蒙配置")]
        [LabelText("出版服务单位")]
        [ShowIf("harmonyLightIsPurchaseGame")]
        public string harmonyLightPublishingServiceUnit;
        
        [FoldoutGroup("鸿蒙配置")]
        [LabelText("出版物号")]
        [ShowIf("harmonyLightIsPurchaseGame")]
        public string harmonyLightPublicationNumber;
        
        [FoldoutGroup("鸿蒙配置")]
        [LabelText("游戏备案识别码")]
        [ShowIf("harmonyLightIsPurchaseGame")]
        public string harmonyLightGameRegistrationIdentificationCode;
        
        [FoldoutGroup("鸿蒙配置")]
        [LabelText("备案查询网址")]
        [ShowIf("harmonyLightIsPurchaseGame")]
        public string harmonyLightRecordInquiryWebsite;
        


        
        #endregion
        
        #region 华为快游戏

        [FoldoutGroup("华为快游戏配置")] [Header("XSDK SDK 专用参数")] [LabelText("XSDK Channel ID（问运营同学要）")] [LabelWidth(200)]
        public int HuaweiXSDKChannel = -1;
        
        [FoldoutGroup("华为快游戏配置")] [LabelText("versionName（预上线功能）")] [LabelWidth(200)]
        public string HuaweiXSDKVersionName = "";
        
        [FoldoutGroup("华为快游戏配置")]
        [Header("华为平台参数")]
        [LabelText("华为快游戏 app ID（问运营同学要）")]
        [LabelWidth(200)]
        public string HuaweiAppId = "";

        [FoldoutGroup("华为快游戏配置")]
        [Header("引力引擎统计")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        [LabelText("开启引力引擎统计")]
        public bool OpenGeHuaweiKuaiGame = false;
        
        [FoldoutGroup("华为快游戏配置")]
        [LabelText("AccessToken（问运营同学要）")]
        [ShowIf("OpenGeHuaweiKuaiGame")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public string GeAccessTokenHuaweiKuaiGame = string.Empty;

        [FoldoutGroup("华为快游戏配置")]
        [LabelText("自动版本号")]
        [ShowIf("OpenGeHuaweiKuaiGame")]
        [PropertyOrder(200)]
        [LabelWidth(200)]
        public bool GeAutoVersionCodeHuaweiKuaiGame = true;
        
        
        [FoldoutGroup("华为快游戏配置")]
        [LabelText("发布版本（统计时划分APP版本）")]
        [ShowIf("ShowGeVersionCodeHuaweiKuaiGame")]
        [LabelWidth(200)]
        [PropertyOrder(200)]
        public int GeVersionCodeHuaweiKuaiGame = 1;
        
        private bool ShowGeVersionCodeHuaweiKuaiGame => !GeAutoVersionCodeHuaweiKuaiGame && OpenGeHuaweiKuaiGame;
        
        
        [FoldoutGroup("华为快游戏配置")]
        [LabelWidth(200)]
        [PropertyOrder(200)]
        [ShowIf("OpenGeHuaweiKuaiGame")]
        [OnInspectorGUI]
        private void OpenGeTipsGUIHuaweiKuaiGame()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorIcons.UnityInfoIcon, GUILayout.Width(30), GUILayout.Height(32));
            var color = GUI.color;
            GUI.color = Color.green;
            GUILayout.TextArea(
                "华为开发者后台确认是否需要添加request合法域名:https://backend.gravity-engine.com和https://api.gravity-engine.com",
                BoldLabel, GUILayout.Height(32));
            GUI.color = color;
            GUILayout.EndHorizontal();
        }
        
        
        #endregion
        
#if UNITY_EDITOR
        private void Awake()
        {
            if (WeChatGameClubButtonImage == null)
            {
                WeChatGameClubButtonImage =
                    AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/XGameUnityTool/Res/Texture/game_club_btn.png");
            }
        }
#endif

        
        //xmy gradle路径是否有效
        // public bool XMYGradlePathIsValid()
        // {
        //     return !string.IsNullOrEmpty(XMYGoogleGradleBatPath());
        // }

        // public string XMYGoogleGradleBatPath()
        // {
        //     if (!string.IsNullOrWhiteSpace(XMYGoogleGradlePath))
        //     {
        //         DirectoryInfo info = new DirectoryInfo(XMYGoogleGradlePath);
        //         var gradeBatPath = $"{info.FullName}/bin/gradle.bat";
        //         if (File.Exists(gradeBatPath))
        //         {
        //             return gradeBatPath;
        //         }
        //     }
        //
        //     return "";
        // }
        
        private static ToolPreference _global = null;

        //全局配置
        public static ToolPreference Global
        {
            get
            {
                if (_global == null)
                {
                    _global = XGameEditorUtil.LoadOrCreate<ToolPreference>(
                        "Assets/XGameUnityTool_Gen/Editor/ToolPreference.asset");
                    //补充.gitignore
                    var path = $".xgameunitytool/.gitignore";
                    if (!File.Exists(path))
                    {
                        XGameEditorUtil.CreateFileAndWriteText(path, "*");
                    }
                }

                return _global;
            }
        }

        #region 微信参数检查

        private bool WeChatLoadingBackgroundMatch
        {
            get
            {
                if (WeChatLoadingBackground != null)
                {
                    var path = AssetDatabase.GetAssetPath(WeChatLoadingBackground);
                    return path.EndsWith(".png") || path.EndsWith(".jpg");
                }

                return true;
            }
        }
        
        #endregion        
        
        #region B站参数检查

        private bool BilibiliLoadingBackgroundMatch
        {
            get
            {
                if (BilibiliLoadingBackground != null)
                {
                    var path = AssetDatabase.GetAssetPath(BilibiliLoadingBackground);
                    return path.EndsWith(".png") || path.EndsWith(".jpg");
                }

                return true;
            }
        }
        
        #endregion
        
        #region vivo参数检查
        
        private bool VivoPrivatePemIsMatch
        {
            get
            {
                if (!string.IsNullOrEmpty(VivoMiniPrivatePemPath))
                {
                    return VivoMiniPrivatePemPath.EndsWith("private.pem");
                }
        
                return true;
            }
        }
        
        private bool VivoCertificatePemIsMatch
        {
            get
            {
                if (!string.IsNullOrEmpty(VivoMiniCertificatePemPath))
                {
                    return VivoMiniCertificatePemPath.EndsWith("certificate.pem");
                }
        
                return true;
            }
        }
        
        private bool VivoMiniLogoMatch
        {
            get
            {
                if (VivoMiniLogo != null)
                {
                    return AssetDatabase.GetAssetPath(VivoMiniLogo).EndsWith(".png");
                }
        
                return true;
            }
        }

        #endregion
        
    }
}