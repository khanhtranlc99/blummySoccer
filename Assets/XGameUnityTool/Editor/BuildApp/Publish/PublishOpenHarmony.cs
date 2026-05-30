using System.IO;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    public class PublishOpenHarmony
    {
        private static string DateTimeNumberString => XGameEditorUtil.DateTimeToPublishSuffixString();

        //切换到hap模式
        public static void SwitchToHap()
        {
#if UNITY_OPENHARMONY
            EditorUserBuildSettings.exportAsOpenHarmonyProject = false;
#endif
        }


        public static void SwitchToProject()
        {
#if UNITY_OPENHARMONY
            EditorUserBuildSettings.exportAsOpenHarmonyProject = true;
            EditorUserBuildSettings.exportProjectType = ExportProjectType.UseAsALibrary;
#endif
        }

        public static void PublishHap(XGameOpenHarmonyAppSetting setting, string name, string publishName,
            BuildOptions buildOptions = BuildOptions.None)
        {
#if UNITY_OPENHARMONY
            SwitchToHap();
            var path =
                $"XGameOutPut/{name}_{DateTimeNumberString}.hap";
            if (!string.IsNullOrWhiteSpace(publishName))
            {
                path = $"XGameOutPut/{publishName}_{DateTimeNumberString}.hap";
            }

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.OpenHarmony,
                buildOptions);
            //打开文件
            if (File.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
                Debug.Log($"发布完成 {path}");
                //触发完成回调！
                var result = new PublishResult();
                result.AppSetting = setting;
                result.HapPath = path;
                setting?.InvokePublishComplete(result);
            }
#endif
        }

        //发布默认工程包
        public static void PublishProject(XGameOpenHarmonyAppSetting setting, string name, string publishName)
        {
#if UNITY_OPENHARMONY
            SwitchToProject();
            var path =
                $"XGameOutPut/OpenHarmony/{name}_{DateTimeNumberString}";

            if (!string.IsNullOrWhiteSpace(publishName))
            {
                path = $"XGameOutPut/OpenHarmony/{publishName}_{DateTimeNumberString}";
            }

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.OpenHarmony,
                BuildOptions.None);
            //打开文件
            if (!Directory.Exists(path)) return;
            EditorUtility.RevealInFinder(path);
            Debug.Log($"发布完成 {path}");
            //触发完成回调！
            var result = new PublishResult();
            result.AppSetting = setting;
            result.OpenHarmonyProjectPath = path;
            setting.InvokePublishComplete(result);
            
#endif
        }
    }
}