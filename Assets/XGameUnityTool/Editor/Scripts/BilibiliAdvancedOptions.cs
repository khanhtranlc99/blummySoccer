using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace XGame
{
        /// <summary>
    /// B站小游戏高级设置
    /// </summary>
    [Serializable]
    public class BilibiliAdvancedOptions
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
            [Space(5)] [Header("预下载选项")] [LabelText("文件列表(;间隔，模糊匹配)")]
            public string preloadFiles = "";


            private static IEnumerable AssetLoadTypeOptions = new ValueDropdownList<int>()
            {
                { "CDN", 0 },
                { "小游戏包内", 1 },
            };
        }



        [Serializable]
        public class CompileOptions
        {
            /// <summary>
            /// Development Build
            /// </summary>
            [LabelText("Development Build")] [LabelWidth(150)] [HorizontalGroup("hco1", 150)] [ToggleLeft]
            public bool DevelopBuild = false;

            /// <summary>
            /// Autoconnect Profiler
            /// </summary>
            [LabelText("Autoconnect Profiler")] [LabelWidth(150)] [HorizontalGroup("hco1", 150)] [ToggleLeft]
            public bool AutoProfile = false;

            /// <summary>
            /// Scripts Only Build
            /// </summary>
            [LabelText("Scripts Only Build")] [LabelWidth(150)] [HorizontalGroup("hco1", 150)] [ToggleLeft]
            public bool ScriptOnly = false;

            /// <summary>
            /// Profiling Funcs
            /// </summary>
            [HorizontalGroup("hco2", 150)] [LabelText("Profiling Funcs")] [LabelWidth(150)] [ToggleLeft]
            public bool profilingFuncs = false;

            /// <summary>
            /// ProfilingMemory
            /// </summary>
            [HorizontalGroup("hco2", 150)] [LabelWidth(150)] [LabelText("Profiling Memory")] [ToggleLeft]
            public bool ProfilingMemory = false;

            /// <summary>
            /// WebGL2.0
            /// </summary>
            // [HorizontalGroup("hco2", 150)] [LabelWidth(150)] [LabelText("WebGL2.0(beta)")] [ToggleLeft]
            // public bool Webgl2 = false;

            /// <summary>
            /// DeleteStreamingAssets
            /// </summary>
            [HorizontalGroup("hco3", 150)] [LabelWidth(150)] [LabelText("ClearStreamingAssets")] [ToggleLeft]
            public bool DeleteStreamingAssets = true;


            /// <summary>
            /// CleanBuild
            /// </summary>
            [HorizontalGroup("hco3", 150)] [LabelWidth(150)] [LabelText("CleanWebGLBuild")] [ToggleLeft]
            public bool CleanBuild = true;
        }

        // [LabelText("启动Loader设置")]
        [Header("启动Loader设置")] [HideLabel] public ProjectConf _ProjectConf = new ProjectConf();
        
        //[LabelText("调试编译选项")]
        [Header("调试编译选项")] [HideLabel] public CompileOptions _CompileOptions = new CompileOptions();
    }
    
}