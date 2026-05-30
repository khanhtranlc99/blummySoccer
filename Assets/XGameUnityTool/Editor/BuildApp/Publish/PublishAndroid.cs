
using System.IO;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace XGame
{
    /// <summary>
    /// 发布安卓
    /// </summary>
    public class PublishAndroid
    {
        private static string DateTimeNumberString => XGameEditorUtil.DateTimeToPublishSuffixString();

        //切换到apk模式
        public static void SwitchToApk()
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
#if UNITY_2019_1_OR_NEWER
            EditorUserBuildSettings.buildAppBundle = false;
#endif
        }


        public static void SwitchToProject()
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
#if UNITY_2019_1_OR_NEWER
            EditorUserBuildSettings.buildAppBundle = false;
#endif
        }


        public static void SwitchToAAB()
        {
#if UNITY_2019_1_OR_NEWER
            EditorUserBuildSettings.buildAppBundle = true;
#endif
        }

        #region 默认发布
        
        //发布默认APK
        public static void PublishApk(XGameAndroidAppSetting setting, string name, string publishName, BuildOptions buildOptions = BuildOptions.None)
        {
            SwitchToApk();
            var path =
                $"XGameOutPut/{name}_{DateTimeNumberString}.apk";
            if (!string.IsNullOrWhiteSpace(publishName))
            {
                path = $"XGameOutPut/{publishName}_{DateTimeNumberString}.apk";
            }

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.Android,
                buildOptions);
            //打开文件
            if (File.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
                Debug.Log($"发布完成 {path}");
                //触发完成回调！
                var result = new PublishResult();
                result.AppSetting = setting;
                result.ApkPath = path;
                setting?.InvokePublishComplete(result);
            }
        }

        //发布默认工程包
        public static void PublishProject(XGameAndroidAppSetting setting, string name, string publishName)
        {
            SwitchToProject();
            var path =
                $"XGameOutPut/{name}_{DateTimeNumberString}";

            if (!string.IsNullOrWhiteSpace(publishName))
            {
                path = $"XGameOutPut/{publishName}_{DateTimeNumberString}";
            }

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.Android,
                BuildOptions.None);
            //打开文件
            if (Directory.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
                Debug.Log($"发布完成 {path}");
                //触发完成回调！
                var result = new PublishResult();
                result.AppSetting = setting;
                result.AndroidProjectPath = path;
                setting?.InvokePublishComplete(result);
            }
        }

        //发布默认aab
        public static void PublishAAB(XGameAndroidAppSetting setting, string name, string publishName)
        {
            SwitchToAAB();
            var path =
                $"XGameOutPut/{name}_{DateTimeNumberString}.aab";
            if (!string.IsNullOrWhiteSpace(publishName))
            {
                path = $"XGameOutPut/{publishName}_{DateTimeNumberString}.aab";
            }

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.Android,
                BuildOptions.None);
            //打开文件
            if (File.Exists(path))
            {
                EditorUtility.RevealInFinder(path);
                Debug.Log($"发布完成 {path}");
                //触发完成回调！
                var result = new PublishResult();
                result.AppSetting = setting;
                result.AABPath = path;
                setting?.InvokePublishComplete(result);
            }
        }

        #endregion
        
    }
}