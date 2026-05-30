using System.IO;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 发布ios
    /// </summary>
    public class PublishIOS
    {
        private static string DateTimeNumberString => XGameEditorUtil.DateTimeToPublishSuffixString();

        /// <summary>
        /// 发布XCODEG工程
        /// </summary>
        public static void PublishXCodeProject(XGameIOSAppSetting setting, string name, string publishName)
        {
            var path =
                $"XGameOutPut/{name}_{DateTimeNumberString}";
            if (!string.IsNullOrWhiteSpace(publishName))
            {
                path = $"XGameOutPut/{publishName}_{DateTimeNumberString}";
            }

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.iOS, BuildOptions.None);

            //打开文件
            if (Directory.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
                Debug.Log($"发布完成 {path}");
                var result = new PublishResult();
                result.AppSetting = setting;
                result.XCodeProjectPath = path;
                setting?.InvokePublishComplete(result);
            }
        }
    }
}