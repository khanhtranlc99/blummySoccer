using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 导出资源包
    /// </summary>
    public class ExportPackageTool
    {
        //备份路径
        private const string BACK_UP_PATH = "Assets/XGameUnityTool_BackUp";

        /// <summary>
        /// 导出XGameUnityTool
        /// </summary>
        [MenuItem("XGameUnityTool/Developer/Export/xgame-unity-tool-downloader.unitypackage")]
        private static void ExportXGameUnityToolDownLoader()
        {
            var exportPath =
                EditorUtility.SaveFilePanel("Export as: ", Application.dataPath, "xgame-unity-tool-downloader", "unitypackage");
            if (string.IsNullOrWhiteSpace(exportPath))
            {
                return;
            }

            //导出文件
            var folders = new[]
            {
                @"Assets/XGameUnityTool/Editor/ToolDownLoaderOnLoaded.cs",
                @"Assets/XGameUnityTool/Editor/ToolDownLoaderWindow.cs",
            };

            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder) && !File.Exists(folder))
                {
                    throw new Exception($"Export failed, folder not found: {folder}");
                }
            }

            AssetDatabase.ExportPackage(folders, exportPath, ExportPackageOptions.Recurse);
            AssetDatabase.Refresh();
            //定位到文件
            EditorUtility.RevealInFinder(exportPath);
            XGameEditorUtil.ShowMessageBox("Export succeeded.");
        }


        /// <summary>
        /// 导出XGameUnityTool
        /// </summary>
        [MenuItem("XGameUnityTool/Developer/Export/XGameUnityTool.unitypackage")]
        private static void ExportXGameUnityTool()
        {
            var exportPath =
                EditorUtility.SaveFilePanel("Export as:", Application.dataPath, "XGameUnityTool", "unitypackage");
            if (string.IsNullOrWhiteSpace(exportPath))
            {
                return;
            }

            //导出文件
            var folders = new[]
            {
                @"Assets/Plugins/Sirenix",
                @"Assets/XGameUnityTool/Editor/BuildApp",
                @"Assets/XGameUnityTool/Editor/CodeTemplate",
                @"Assets/XGameUnityTool/Editor/Scripts",
                @"Assets/XGameUnityTool/Editor/ToolBox",
                @"Assets/XGameUnityTool/Editor/Bat",
                @"Assets/XGameUnityTool/Editor/ZipTool",
                @"Assets/XGameUnityTool/Editor/LocalTest",
                @"Assets/XGameUnityTool/Res",
                @"Assets/XGameUnityTool/Runtime",
                @"Assets/XGameUnityTool/CHANGELOG.md",
                @"Assets/XGameUnityTool/package.json",
                @"Assets/XGameUnityTool/Editor/Texture",
            };

            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder) && !File.Exists(folder))
                {
                    throw new Exception($"Export failed, folder not found: {folder}");
                }
            }

            AssetDatabase.ExportPackage(folders, exportPath, ExportPackageOptions.Recurse);
            AssetDatabase.Refresh();
            //定位到文件
            EditorUtility.RevealInFinder(exportPath);
            XGameEditorUtil.ShowMessageBox("Export succeeded.");
        }

        /// <summary>
        /// 导出Mar资源包
        /// </summary>
        [MenuItem("XGameUnityTool/Developer/Export/OpenHarmony_Light.unitypackage")]
        private static void ExportOpenHarmonyLight()
        {
            var exportPath = Paths.PACKAGE_OPEN_HARMONY_LIGHT_SDK;
            var manifest = new List<string>()
            {
                @"Assets/Plugins/OpenHarmony",
                @"Assets/XGameUnityTool/Lib/OpenHarmonyLight",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }
        
        /// <summary>
        /// 导出Mar资源包
        /// </summary>
        [MenuItem("XGameUnityTool/Developer/Export/Mar.unitypackage")]
        private static void ExportMar()
        {
            var exportPath = Paths.PACKAGE_MAR_SDK;
            var manifest = new List<string>()
            {
                @"Assets/Plugins/Android",
                @"Assets/XGameUnityTool/Lib/Mar",
                @"Assets/Plugins/LitJson",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }
        /// <summary>
        /// 导出Light资源包
        /// </summary>
        [MenuItem("XGameUnityTool/Developer/Export/Light.unitypackage")]
        private static void ExportLight()
        {
            var exportPath = Paths.PACKAGE_LIGHT_SDK;
            var manifest = new List<string>()
            {
                @"Assets/Plugins/Android",
                @"Assets/XGameUnityTool/Lib/Light",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }

 


        /// <summary>
        /// 导出构建配置
        /// </summary>
        [MenuItem("XGameUnityTool/Developer/Export/常用打包设置.unitypackage")]
        private static void ExportBuildAppAssets()
        {
            var folder = @"Assets/XGameUnityTool/Editor/打包设置";
            var exportPath = @"Assets/XGameUnityTool/Res/UnityPackage/常用打包设置.unitypackage";
            if (!Directory.Exists(folder))
            {
                throw new Exception($"Export failed, folder not found: {folder}");
            }

            if (File.Exists(exportPath))
            {
                XGameEditorUtil.CheckCreateFolder(BACK_UP_PATH);
                //备份旧package
                string backUpPath =
                    $"{BACK_UP_PATH}/常用打包设置_{XGameEditorUtil.DateTimeToPublishSuffixString()}.unitypackage";
                FileUtil.ReplaceFile(exportPath, backUpPath);
            }

            AssetDatabase.ExportPackage(folder, exportPath, ExportPackageOptions.Recurse);
            AssetDatabase.Refresh();
            XGameEditorUtil.PingObject(exportPath);
            XGameEditorUtil.ShowMessageBox("Export succeeded.");
        }

        
        [MenuItem("XGameUnityTool/Developer/Export/XMY.unitypackage")]
        private static void ExportXMYSDK()
        {
            var exportPath = Paths.PACKAGE_XMY_SDK;
            var manifest = new List<string>()
            {
                @"Assets/Plugins/Android",
                @"Assets/XGameUnityTool/Lib/XMY"
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }
        
        
        [MenuItem("XGameUnityTool/Developer/Export/Google_Log_SDK.unitypackage")]
        private static void ExportGoogleLogSDK()
        {
            var exportPath = Paths.PACKAGE_GOOGLE_LOG_SDK;
            var manifest = new List<string>()
            {
                @"Assets/Plugins/Android",
                @"Assets/XGameUnityTool/Lib/GoogleLogSDK"
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }


        /// <summary>
        /// 导出WX_XSDK资源包
        /// </summary>
        [MenuItem("XGameUnityTool/Developer/Export/WX_XSDK.unitypackage")]
        private static void ExportWX_XSDK()
        {
            var exportPath = Paths.PACKAGE_WX_XSDK_SDK;
            var manifest = new List<string>()
            {
                @"Assets/XGameUnityTool/Lib/WX_XSDK",
                // @"Assets/WX-WASM-SDK-V2",
                // @"Assets/WebGLTemplates",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }


        /// <summary>
        /// 导出VIVO_XSDK资源包
        /// </summary>
        [MenuItem("XGameUnityTool/Developer/Export/VIVO_XSDK.unitypackage")]
        private static void ExportVivoXSDK()
        {
            var exportPath = Paths.PACKAGE_VIVO_XSDK_SDK;
            var manifest = new List<string>()
            {
                @"Assets/XGameUnityTool/Lib/VIVO_XSDK",
                @"Assets/VIVO-GAME-SDK",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }


        [MenuItem("XGameUnityTool/Developer/Export/OPPO_XSDK.unitypackage")]
        private static void ExportOPPOXSDK()
        {
            var exportPath = Paths.PACKAGE_OPPO_XSDK_SDK;
            var manifest = new List<string>()
            {
                @"Assets/XGameUnityTool/Lib/OPPO_XSDK",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }


        [MenuItem("XGameUnityTool/Developer/Export/Douyin_XSDK.unitypackage")]
        private static void ExportDouyinXSDK()
        {
            var exportPath = Paths.PACKAGE_DOUYIN_XSDK_SDK;
            var manifest = new List<string>()
            {
                // @"Assets/Plugins/Android",
                @"Assets/Plugins/ByteGame/com.bytedance.starksdk",
                @"Assets/Plugins/ByteGame/com.bytedance.bgdt",
                // @"Assets/Plugins/ByteGame/com.bytedance.starksdk.unitytools",
                @"Assets/XGameUnityTool/Lib/DOUYIN_XSDK",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }

        [MenuItem("XGameUnityTool/Developer/Export/KUAISHOU_XSDK.unitypackage")]
        private static void ExportKuaishouXSDK()
        {
            var exportPath = Paths.PACKAGE_KUAISHOU_XSDK_SDK;
            var manifest = new List<string>()
            {
                @"Assets/XGameUnityTool/Lib/KUAISHOU_XSDK",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }
        
        [MenuItem("XGameUnityTool/Developer/Export/KUAISHOU_WEB_XSDK.unitypackage")]
        private static void ExportKuaishouWebXSDK()
        {
            var exportPath = Paths.PACKAGE_KUAISHOU_WEB_XSDK_SDK;
            var manifest = new List<string>()
            {
                @"Assets/KS-WASM-SDK",
                @"Assets/WebGLTemplates",
                @"Assets/XGameUnityTool/Lib/KUAISHOU_WEB_XSDK",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }
        
        [MenuItem("XGameUnityTool/Developer/Export/HUAWEI_XSDK.unitypackage")]
        private static void ExportHuaweiXSDK()
        {
            var exportPath = Paths.PACKAGE_HUAWEI_XSDK_SDK;
            var manifest = new List<string>()
            {
                @"Assets/XGameUnityTool/Lib/HUAWEI_XSDK",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }
        
        [MenuItem("XGameUnityTool/Developer/Export/BILIBILI_XSDK.unitypackage")]
        private static void ExportBilibiliXSDK()
        {
            var exportPath = Paths.PACKAGE_BILIBILI_XSDK_SDK;
            var manifest = new List<string>()
            {
                @"Assets/BiliGame-SDK",
                @"Assets/WebGLTemplates",
                @"Assets/XGameUnityTool/Lib/BILIBILI_XSDK",
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }

        [MenuItem("XGameUnityTool/Developer/Export/IOS_XGUG.unitypackage")]
        private static void ExportIOSXGUGSDK()
        {
            var exportPath = Paths.PACKAGE_IOS_XGUG_SDK;
            var manifest = new List<string>()
            {
                @"Assets/XGameUnityTool/Lib/IOS_XGUG"
            };
            if (!MakeSureExport(manifest))
            {
                return;
            }

            if (!CheckMissFiles(manifest))
            {
                return;
            }

            ExportUnityPackage(manifest, exportPath, out var detail);
        }


        // private static string GetGravityEngineSDKAAR()
        // {
        //     var androidFolder = "Assets/Plugins/Android";
        //     if (!Directory.Exists(androidFolder))
        //     {
        //         throw new Exception($"缺少：{androidFolder},无法找到引力引擎SDK AAR");
        //     }
        //
        //     var dir = new DirectoryInfo(androidFolder);
        //     var files = dir.GetFiles("*.aar");
        //     var aarPath = string.Empty;
        //     foreach (var file in files)
        //     {
        //         if (file.Name.StartsWith("GravityEngineSDK"))
        //         {
        //             aarPath = $"{androidFolder}/{file.Name}";
        //         }
        //     }
        //
        //     if (string.IsNullOrEmpty(aarPath))
        //     {
        //         throw new Exception("找不到GravityEngineSDK aar包");
        //     }
        //
        //     return aarPath;
        // }


        /*询问是否导出*/
        public static bool MakeSureExport(List<string> manifest)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Export the following assets?");
            foreach (var element in manifest)
            {
                sb.AppendLine(element);
            }

            return EditorUtility.DisplayDialog("Confirm", sb.ToString(), "Yes", "No");
        }


        /*检查是否缺少文件*/
        public static bool CheckMissFiles(List<string> manifest, bool logError = true)
        {
            var miss = new List<string>();
            var result = true;
            foreach (var element in manifest)
            {
                if (!IsFileOrDirectoryExist(element))
                {
                    miss.Add(element);
                    result = false;
                }
            }

            if (logError)
            {
                foreach (var element in miss)
                {
                    Debug.LogError($"缺少文件：{element}");
                }
            }

            return result;
        }

        /*检查文件或文件夹是否存在*/
        public static bool IsFileOrDirectoryExist(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }

            if (Directory.Exists(path))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 导出UnityPackage
        /// </summary>
        /// <param name="source">资源文件或文件夹路径</param>
        /// <param name="savePath">保存的unitypackage路径</param>
        /// <param name="manifest">导出的详细资源清单</param>
        public static void ExportUnityPackage(List<string> source, string savePath, out HashSet<string> manifest)
        {
            manifest = new HashSet<string>();
            //遍历获取所有的文件和文件夹
            foreach (var path in source)
            {
                manifest.Add(ToUnityPath(path));
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    foreach (var e in files)
                    {
                        if (e.EndsWith(".meta")) continue;
                        // Debug.Log("Export file="+e);
                        manifest.Add(ToUnityPath(e));
                    }
                }
            }


            var backup =
                $"{BACK_UP_PATH}/{Path.GetFileNameWithoutExtension(savePath)}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(savePath)}";
            var manifestPath = $"{savePath}.manifest";
            var manifest_backup = $"{backup}.manifest";
            var dir = Path.GetDirectoryName(backup);
            XGameEditorUtil.CheckCreateFolder(dir);
            //备份unitypackage
            if (File.Exists(savePath))
            {
                Debug.Log($"备份：{savePath}-->{backup}");
                File.Copy(savePath, backup, true);
            }

            //备份清单
            if (File.Exists(manifestPath))
            {
                Debug.Log($"备份：{manifestPath}-->{manifest_backup}");
                File.Copy(manifestPath, manifest_backup, true);
            }

            AssetDatabase.SaveAssets();
            dir = Path.GetDirectoryName(savePath);
            XGameEditorUtil.CheckCreateFolder(dir);
            AssetDatabase.SaveAssets();
            Debug.Log($"导出:{savePath}");
            AssetDatabase.ExportPackage(source.ToArray(), savePath, ExportPackageOptions.Recurse);
            //导出清单
            Debug.Log($"导出清单:{manifestPath}");
            var sb = new StringBuilder();
            foreach (var element in manifest)
            {
                sb.AppendLine(element);
            }

            File.WriteAllText(manifestPath, sb.ToString());
            AssetDatabase.Refresh();
            if (File.Exists(savePath))
            {
                Debug.Log("导出成功");
                EditorUtility.RevealInFinder(savePath);
            }
        }

        public static string ToUnityPath(string path)
        {
            return XGameEditorUtil.FullPathToAssetDatabasePath(path);
        }
    }
}