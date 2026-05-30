using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;


namespace XGame
{
    /// <summary>
    /// 微信小游戏高级设置
    /// </summary>
    [Serializable]
    public class WXAdvancedOptions
    {
        [Serializable]
        public class ProjectConf
        {
            /// <summary>
            /// 加载阶段视频url
            /// </summary>
            [LabelText("加载阶段视频url")] public string VideoUrl;

            /// <summary>
            ///  首包资源加载方式
            /// </summary>
            [ValueDropdown("AssetLoadTypeOptions")] [LabelText("首包资源加载方式")]
            public int assetLoadType = 0;

            [LabelText("压缩首包资源(?)")]
            [Tooltip("将首包资源Brotli压缩, 降低资源大小. 注意: 首次启动耗时可能会增加200ms, 仅推荐使用小游戏分包加载时节省包体大小使用")]
            public bool compressDataPackage;
            
            /// <summary>
            /// 不自动缓存文件类型(;分隔)
            /// </summary>
            [LabelText("不自动缓存文件类型(;分隔)")] public string bundleExcludeExtensions = "json;";

            /// <summary>
            /// bundle的hash长度
            /// </summary>
            [LabelText("Bundle名中的Hash长度")] public int bundleHashLength = 32;
            
            /// <summary>
            /// 预下载列表
            /// </summary>
            [LabelText("预下载文件列表(;间隔，模糊匹配)")]
            public string preloadFiles = "";


            private static IEnumerable AssetLoadTypeOptions = new ValueDropdownList<int>()
            {
                { "CDN", 0 },
                { "小游戏包内", 1 },
            };
        }

        [Serializable]
        public class SDKOptions
        {
            
            [LabelText("使用好友关系链")] 
            // [HorizontalGroup("hsko1", 150)] 
            // [ToggleLeft]
            public bool UseFriendRelation;
            
            [LabelText("使用社交组件")]
            public bool UseMiniGameChat;
            
            [LabelText("预加载微信字体(?")]
            [Tooltip("在game.js执行开始时预载微信系统字体，运行期间可使用WX.GetWXFont获取微信字体")]
            public bool PreloadWXFont;
            
            [LabelText("禁用多点触控")]
            public bool disableMultiTouch;
        }

        [Serializable]
        public class CompileOptions
        {
            [LabelText("Development Build")]
            public bool DevelopBuild;
            
            [LabelText("Auto connect Profiler")] 
            public bool AutoProfile;
            
            [LabelText("Scripts Only Build")] 
            public bool ScriptOnly;

            [LabelText("Il2Cpp Optimize Size(?)")]
            [Tooltip("对应于Il2CppCodeGeneration选项，勾选时使用OptimizeSize(默认推荐)，生成代码小15%左右，取消勾选则使用OptimizeSpeed。游戏中大量泛型集合的高频访问建议OptimizeSpeed，在使用HybridCLR等第三方组件时只能用OptimizeSpeed。(Dotnet Runtime模式下该选项无效)")]
            public bool Il2CppOptimizeSize = true;
            
            [LabelText("Profiling Funcs")]
            public bool profilingFuncs = true;
            
            [LabelText("Profiling Memory")]
            public bool ProfilingMemory;
            
            [LabelText("WebGL2.0")]
            public bool Webgl2;
            
            [LabelText("iOSPerformancePlus(?)")]
            [Tooltip("是否使用iOS高性能+渲染方案，有助于提升渲染兼容性、降低WebContent进程内存")]
            public bool enableIOSPerformancePlus;
            
            [LabelText("Clear Streaming Assets")]
            public bool DeleteStreamingAssets = true;
            
            [LabelText("Clean WebGL Build")]
            public bool CleanBuild;


            [LabelText("首包资源优化(?)")]
            [Tooltip("导出时自动清理UnityEditor默认打包但游戏项目从未使用的资源，瘦身首包资源体积。（团结引擎已无需开启该能力）")]
            [HorizontalGroup("hsko1")] 
            // [ShowIf("GetEngineVersion")]
            [LabelWidth(180)]
            public bool fbslim;


            // private bool GetEngineVersion()
            // {
            //     var t = Type.GetType("WeChatWASM.UnityUtil,wx-editor");
            //     if (t != null)
            //     {
            //         return t.GetMethod("GetEngineVersion")?.Invoke(null,null)?.ToXInt() == 0;
            //     }
            //     return true;
            // }
            
            [Button("首包资源优化设置")]
            // [ShowIf("GetEngineVersion")]
            [HorizontalGroup("hsko1")] 
            private void OpenWXFbSettingWindow()
            {
                var t = Type.GetType("WeChatWASM.WXFbSettingWindow,wx-editor");
                if (t == null) return;
                var fbWin = EditorWindow.GetWindow(t, false, "首包资源优化配置面板", true);
                fbWin.minSize = new Vector2(680, 350);
                fbWin.Show();

            }

            
            [LabelText("自适应屏幕尺寸(?)")]
            [Tooltip("移动端旋转屏幕和PC端拉伸窗口时，自动调整画布尺寸")]
            public bool autoAdaptScreen;       
            
            [LabelText("显示优化建议弹窗")]
            public bool showMonitorSuggestModal = true;
            
            [LabelText("显示性能面板")]
            public bool enableProfileStats;
            
            
            [LabelText("显示渲染日志(dev only)")]
            public bool enableRenderAnalysis;
            
            [LabelText("brotli多线程压缩(?)")]
            [Tooltip("开启多线程压缩可以提高出包速度，但会降低压缩率。如若不使用wasm代码分包请勿用多线程出包上线")]
            public bool brotliMT = true;



        }

        // [LabelText("启动Loader设置")]
        [FoldoutGroup("启动Loading配置")] [HideLabel] public ProjectConf _ProjectConf = new ProjectConf();

        //[LabelText("SDK 功能选项")]
        [FoldoutGroup("SDK功能选项")] [HideLabel] public SDKOptions _SDKOptions = new SDKOptions();

        //[LabelText("调试编译选项")]
        [FoldoutGroup("调试编译选项")] [HideLabel] public CompileOptions _CompileOptions = new CompileOptions();
    }
}