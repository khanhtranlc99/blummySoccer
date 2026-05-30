using System;
using System.IO;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 工具config json
    /// </summary>
    [Serializable]
    public class JsonPluginConfig
    {
        /// <summary>
        /// oppo 小游戏工具安装目录
        /// </summary>
        public string oppo_quick_game_tool_install_path = "";

        /// <summary>
        /// openssl 安装目录
        /// </summary>
        // public string open_ssl_install_path = "";


        /// <summary>
        /// vivo小游戏发布脚手架2019
        /// </summary>
        public string vivo_cli_src_unity_2019 = "";


        /// <summary>
        /// vivo小游戏发布脚手架2020
        /// </summary>
        public string vivo_cli_src_unity_2020 = "";


        /// <summary>
        /// node js 15位置
        /// </summary>
        public string node_js_v15 = "";

        /// <summary>
        /// node js v10.13.0
        /// </summary>
        // public string node_js_v10_13_0 = "";

        /// <summary>
        /// node js 18位置
        /// </summary>
        // public string node_js_v18 = "";


        //尝试加载配置
        private static void TryLoadConfig(out JsonPluginConfig config)
        {
            config = null;
            var path = GetConfigFilePath();
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    config = JsonUtility.FromJson<JsonPluginConfig>(json);
                }
                catch (Exception e)
                {
                }
            }
        }

        //获取安装未知
        private static string GetConfigFilePath() =>
            $"{XGameEditorUtil.GetWindowUserFolder()}/Local/XGameUnityTool/config.json";


        private static JsonPluginConfig _instance = null;


        public static JsonPluginConfig Default
        {
            get
            {
                if (_instance == null)
                {
                    TryLoadConfig(out _instance);
                    if (_instance == null)
                    {
                        _instance = new JsonPluginConfig();
                        _instance.WriteAndSave();
                    }
                }

                return _instance;
            }
        }

        //保存配置
        public void WriteAndSave()
        {
            FileInfo info = new FileInfo(GetConfigFilePath());
            XGameEditorUtil.CheckCreateFolder(info.Directory.FullName);
            File.WriteAllText(info.FullName, JsonUtility.ToJson(this));
        }
    }
}