using System.IO;
using UnityEditor;
using UnityEngine;


namespace XGame
{
    public class PublishWebgl
    {
        private static string DateTimeNumberString => XGameEditorUtil.DateTimeToPublishSuffixString();

        /// <summary>
        /// 发布web工程
        /// </summary>
        public static void PublishWebProject(XGameIOSAppSetting setting, string name, string publishName)
        {
            var path =
                $"XGameOutPut/{name}_{DateTimeNumberString}";
            if (!string.IsNullOrWhiteSpace(publishName))
            {
                path = $"XGameOutPut/{publishName}_{DateTimeNumberString}";
            }

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.WebGL, BuildOptions.None);

            //打开文件
            if (Directory.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
                Debug.Log($"发布完成 {path}");
                var result = new PublishResult();
                result.AppSetting = setting;
                result.webProjectPath = path;
                setting?.InvokePublishComplete(result);
            }
        }
    }
}