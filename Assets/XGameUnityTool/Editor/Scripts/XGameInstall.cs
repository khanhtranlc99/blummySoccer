using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using XGame.BuildApp;

namespace XGame
{
    [InitializeOnLoad]
    public class XGameInstallOnLoad
    {
        /// <summary>
        /// 支付回调类模板路径
        /// </summary>
        private const string PAY_RESULT_CLASS_TEMPLATE =
            "Assets/XGameUnityTool/Res/Template/XGameSdkPayResult.template";

        /// <summary>
        /// 支付回调类文件路径
        /// </summary>
        private const string PAY_RESULT_CLASS_FILE_PATH = "Assets/XGameUnityTool_Gen/XGameSdkPayResult.cs";


        /// <summary>
        /// 兑换回调类模板路径
        /// </summary>
        private const string GIFT_RESULT_CLASS_TEMPLATE =
            "Assets/XGameUnityTool/Res/Template/XGameSdkGiftResult.template";

        /// <summary>
        /// 兑换回调类文件路径
        /// </summary>
        private const string GIFT_RESULT_CLASS_FILE_PATH = "Assets/XGameUnityTool_Gen/XGameSdkGiftResult.cs";

        //忽略package
        private const string GITIGNORE_FILE_PATH = "Assets/XGameUnityTool/Res/UnityPackage/.gitignore";


        static XGameInstallOnLoad()
        {
            CreateTemplateCSharpFile(PAY_RESULT_CLASS_TEMPLATE, PAY_RESULT_CLASS_FILE_PATH);
            CreateTemplateCSharpFile(GIFT_RESULT_CLASS_TEMPLATE, GIFT_RESULT_CLASS_FILE_PATH);
            CreateGitIgnoreFile();
            CheckNewtonsoftJson();
        }

        private static void CreateTemplateCSharpFile(string template, string filePath)
        {
            if (File.Exists(template))
            {
                if (!File.Exists(filePath))
                {
                    Debug.Log($"生成回调类:{filePath}");
                    //生成支付回调模板类
                    var code = File.ReadAllText(template);
                    FileInfo fileInfo = new FileInfo(filePath);
                    //创建文件夹
                    XGameEditorUtil.CheckCreateFolder(fileInfo.Directory.FullName);
                    File.WriteAllText(filePath, code);
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                Debug.LogError($"找不到模板：{template}");
            }
        }


        private static Type GetType(string typeName)
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

        private static bool HasDefine(string define)
        {
            var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            var strs = defines.Split(';');
            return strs.Contains(define);
        }

        //添加定义
        private static void AddDefine(string define, BuildTargetGroup buildTarget)
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);
            var elements = defines.Split(';').ToList();
            bool isOwn = false;
            foreach (var element in elements)
            {
                if (element == define)
                {
                    isOwn = true;
                    break;
                }
            }

            if (!isOwn)
            {
                elements.Add(define);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (!string.IsNullOrWhiteSpace(element))
                    {
                        sb.Append(element);
                        if (i != element.Length - 1)
                        {
                            sb.Append(";");
                        }
                    }
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, sb.ToString());
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        //移除定义
        private static void RemoveDefine(string define, BuildTargetGroup buildTarget)
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);
            var elements = defines.Split(';').ToList();
            bool needMove = false;

            for (int i = elements.Count - 1; i >= 0; i--)
            {
                var element = elements[i];
                if (element == define)
                {
                    elements.RemoveAt(i);
                    needMove = true;
                }
            }

            if (needMove)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (!string.IsNullOrWhiteSpace(element))
                    {
                        sb.Append(element);
                        if (i != element.Length - 1)
                        {
                            sb.Append(";");
                        }
                    }
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, sb.ToString());
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        //创建.gitignore文件，防止unitypackage被忽略上传
        private static void CreateGitIgnoreFile()
        {
            if (!File.Exists(GITIGNORE_FILE_PATH))
            {
                File.WriteAllText(GITIGNORE_FILE_PATH, "!*.unitypackage");
            }
        }

        private static void CheckNewtonsoftJson()
        {
            if (!XGameEditorUtil.HasNewtonsoftJson())
            {
                var xgame_tool_check_newtonsoft_install_flag = "xgame_tool_check_newtonsoft_install_flag";
                if (!EditorPrefs.HasKey(xgame_tool_check_newtonsoft_install_flag))
                {
                    XGameEditorUtil.ShowMessageBox("Can't find Newtonsoft.Json, install it?", XGameEditorUtil.InstallNewtonsoftJson);
                    EditorPrefs.SetInt(xgame_tool_check_newtonsoft_install_flag, 1);
                }
                else
                {
                    Debug.LogError("Newtonsoft.Json cannot be found, please install it from the XGameUnityTool menu");
                }
            }
            else
            {
                XGameEditorUtil.TryOverrideXJsonCSharp();
            }
        }
    }

    public class XGameInstall
    {
        private const string KEY_XGAME_UNITY_TOOL_INSTALL_CHECK_CLOSE = "KEY_XGAME_UNITY_TOOL_INSTALL_CHECK_CLOSE";
        public const string DECOMPRESS_PATH = "./XGameUnityToolPackages";


        public static bool IsCloseCheck
        {
            get => EditorPrefs.GetBool(KEY_XGAME_UNITY_TOOL_INSTALL_CHECK_CLOSE, false);
            set => EditorPrefs.SetBool(KEY_XGAME_UNITY_TOOL_INSTALL_CHECK_CLOSE, value);
        }


        public static bool IsInstalled()
        {
            return true;
        }


        public static void Install()
        {
        }

        public static void UnInstall()
        {
        }
    }
}