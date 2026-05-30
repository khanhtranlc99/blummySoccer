using System.IO;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 清理一些旧版本残留文件，当工具更新时
    /// </summary>
    [InitializeOnLoad]
    public class ClearHistoryFilesWhenUpdateTool
    {
        private const string CHECKER_VERSION = "20240229a";

        private static string[] _removeManifest = new[]
        {
            "Assets/XGameUnityTool/Runtime/Sdk/IOS/IOSSdkCallBack.cs",
            "Assets/XGameUnityTool/Runtime/Sdk/IOS/IOSSdkInstance.cs",
            "Assets/XGameUnityTool/Runtime/IOS/UnityToIOS.h",
            "Assets/XGameUnityTool/Runtime/IOS/UnityToIOS.m",
            "Assets/XGameUnityTool/Runtime/Plugins/IOS/UnityToIOS.h",
            "Assets/XGameUnityTool/Runtime/Plugins/IOS/UnityToIOS.m",
        };

        static ClearHistoryFilesWhenUpdateTool()
        {
            if (IsClearDone())
            {
                return;
            }

            Debug.Log("清理历史版本残留文件...");
            foreach (var file in _removeManifest)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }

            WriteMark();
        }

        private static string GetMarkPath() => $"Assets/XGameUnityTool_Gen/Editor/checker_mark_{CHECKER_VERSION}.txt";


        private static bool IsClearDone() => File.Exists(GetMarkPath());

        private static void WriteMark()
        {
            var path = GetMarkPath();
            //清理历史残留文件
            var dir = Path.GetDirectoryName(path);
            XGameEditorUtil.CheckCreateFolder(dir);
            File.WriteAllText(path, "");
            AssetDatabase.Refresh();
        }
    }
}