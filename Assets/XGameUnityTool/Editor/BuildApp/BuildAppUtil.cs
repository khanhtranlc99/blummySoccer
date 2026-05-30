using System;
using System.IO;
using UnityEditor;


namespace XGame.BuildApp
{
    /// <summary>
    /// BuildApp模块封装类
    /// </summary>
    public class BuildAppUtil
    {
        /// <summary>
        ///  切换平台
        /// </summary>
        public static void SwitchPlatformTo(BuildTarget buildTarget, Action complete)
        {
            if (EditorUserBuildSettings.activeBuildTarget == buildTarget)
            {
                complete?.Invoke();
            }
            else
            {
                ShowMessageBox($"Switch platform to {buildTarget}?",
                    () =>
                    {
                        switch (buildTarget)
                        {
                            case BuildTarget.Android:
                            {
                                bool isDone =
                                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android,
                                        buildTarget);
                                if (isDone)
                                {
                                    complete?.Invoke();
                                }
                            }
                                break;
                            case BuildTarget.iOS:
                            {
                                bool isDone =
                                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS,
                                        buildTarget);
                                if (isDone)
                                {
                                    complete?.Invoke();
                                }
                            }
                                break;
                            case BuildTarget.WebGL:
                            {
                                bool isDone =
                                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL,
                                        buildTarget);
                                if (isDone)
                                {
                                    complete?.Invoke();
                                }
                            }
                                break;
#if UNITY_OPENHARMONY
                            case BuildTarget.OpenHarmony:
                            {
                                bool isDone =
                                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.OpenHarmony,
                                        buildTarget);
                                if (isDone)
                                {
                                    complete?.Invoke();
                                }
                            }
                                break;
#endif
                            default:
                                throw new Exception($"Unhandled build target: {buildTarget}");
                        }
                    });
            }
        }


        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static void DeleteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                FileUtil.DeleteFileOrDirectory(folderPath);
                AssetDatabase.Refresh();
            }
        }


        /// <summary>
        /// 复制并更新文件夹
        /// </summary>
        public static void CopyToAndReplaceFolder(string src, string dest)
        {
            if (Directory.Exists(src))
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }

                FileUtil.ReplaceDirectory(src, dest);
                AssetDatabase.Refresh();
            }
        }


        /// <summary>
        /// 显示提示框
        /// </summary>
        public static void ShowMessageBox(string msg, Action success = null, Action fail = null)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("Confirm", msg, "Yes"))
                success?.Invoke();
            else
                fail?.Invoke();
#endif
        }

        /// <summary>
        /// 显示进度条
        /// </summary>
        public static void ShowProgressBar(string title, string info, float progress)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayProgressBar(title, info, progress);
#endif
        }

        /// <summary>
        /// 清理进度条
        /// </summary>
        public static void ClearProgressBar()
        {
#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
        }


        // /// <summary>
        // /// 尝试备份MiniGameConfig.asset
        // /// </summary>
        // public static void TryBackUpMiniGameConfigAsset()
        // {
        //     //备份wx
        //     var pathWXConfig = Paths.WX_MINI_GAME_CONFIG_ASSET;
        //     if (File.Exists(pathWXConfig))
        //     {
        //         EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<Object>(pathWXConfig));
        //         AssetDatabase.SaveAssets();
        //         //存在，进行缓存
        //         FileInfo cachePath = new FileInfo(Paths.CACHE_WX_MINI_GAME_CONFIG_ASSET);
        //         XGameEditorUtil.CheckCreateFolder(cachePath.Directory.FullName);
        //         //备份
        //         File.Copy(pathWXConfig, cachePath.FullName, true);
        //         // Debug.Log("尝试备份MiniGameConfig.asset");
        //     }
        //
        //     //备份vivo小游戏配置
        //     var pathVivoConfig = Paths.VIVO_MINI_GAME_CONFIG_ASSET;
        //     if (File.Exists(pathVivoConfig))
        //     {
        //         EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<Object>(pathVivoConfig));
        //         AssetDatabase.SaveAssets();
        //         //存在，进行缓存
        //         FileInfo cachePath = new FileInfo(Paths.CACHE_VIVO_MINI_GAME_CONFIG_ASSET);
        //         XGameEditorUtil.CheckCreateFolder(cachePath.Directory.FullName);
        //         //备份
        //         File.Copy(pathVivoConfig, cachePath.FullName, true);
        //     }
        //
        //
        // }


        // /// <summary>
        // /// 尝试加载缓存的MiniGameConfig.asset
        // /// </summary>
        // public static void TryLoadMiniGameConfigAsset()
        // {
        //     if (EditorApplication.isCompiling)
        //     {
        //         return;
        //     }
        //
        //     if (File.Exists(Paths.CACHE_WX_MINI_GAME_CONFIG_ASSET) && File.Exists(Paths.WX_MINI_GAME_CONFIG_ASSET))
        //     {
        //         File.Copy(Paths.CACHE_WX_MINI_GAME_CONFIG_ASSET, Paths.WX_MINI_GAME_CONFIG_ASSET, true);
        //         File.Delete(Paths.CACHE_WX_MINI_GAME_CONFIG_ASSET);
        //     }
        //
        //     if (File.Exists(Paths.CACHE_VIVO_MINI_GAME_CONFIG_ASSET) && File.Exists(Paths.VIVO_MINI_GAME_CONFIG_ASSET))
        //     {
        //         File.Copy(Paths.CACHE_VIVO_MINI_GAME_CONFIG_ASSET, Paths.VIVO_MINI_GAME_CONFIG_ASSET, true);
        //         File.Delete(Paths.CACHE_VIVO_MINI_GAME_CONFIG_ASSET);
        //     }
        //
        // }
    }
}