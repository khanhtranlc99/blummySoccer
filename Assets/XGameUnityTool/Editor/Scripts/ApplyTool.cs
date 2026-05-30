using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class ApplyTool
    {
        //全路径
        private static HashSet<string> MaybeFolder = new HashSet<string>()
        {
            @"Assets/Plugins/OpenHarmony",
            @"Assets/Plugins/LitJson",
            @"Assets/Plugins/Android",
            @"Assets/XGameUnityTool/Lib",
            @"Assets/Plugins/ByteGame",
            @"Assets/WX-WASM-SDK",
            @"Assets/WX-WASM-SDK-V2",
            @"Assets/BiliGame-SDK",
            @"Assets/WebGLTemplates",
            @"Assets/VIVO-GAME-SDK",
            @"Assets/Plugins/kwaiGame",
            @"Assets/Plugins/kwaiGameAndroid",
            @"Assets/KS-WASM-SDK",
        };

        private static string WxPkgId = "https://gitee.com/wechat-minigame/minigame-tuanjie-transform-sdk.git";
        // private static string CurrentChannelKey = "XGAME_APPLY_TOOL_KEY_CURRENT_CHANNEL";
        // private static string LastChannelKey = "XGAME_APPLY_TOOL_KEY_LAST_CHANNEL";
        //
        // private static AppChannel CurrentChannel
        // {
        //     get => (AppChannel)EditorPrefs.GetInt(CurrentChannelKey, 0);
        //     set => EditorPrefs.SetInt(CurrentChannelKey, (int)value);
        // }
        //
        // private static AppChannel LastChannel
        // {
        //     get => (AppChannel)EditorPrefs.GetInt(LastChannelKey, 0);
        //     set => EditorPrefs.SetInt(LastChannelKey, (int)value);
        // }
        
        public static void ApplyChannelSDK(AppChannel channel)
        {
            if (channel != AppChannel.WeChat_XSDK)
            {
                XGameEditorUtil.RemovePkg(WxPkgId);
            }
            
            switch (channel)
            {
                case AppChannel.XMYGoogle:
                    ApplyXMY();
                    break;
                case AppChannel.Google_Log_SDK:
                    ApplyLogSDK();
                    break;
                case AppChannel.Mar:
                    ApplyMar();
                    break;
                case AppChannel.WeChat_XSDK:
                    ApplyWeChat_XSDK();
                    break;
                case AppChannel.Vivo_XSDK:
                    ApplyVivo_XSDK();
                    break;
                case AppChannel.Oppo_XSDK:
                    ApplyOppoXSDK();
                    break;
                case AppChannel.Douyin_XSDK_Android:
                case AppChannel.Douyin_XSDK_IOS:
                    ApplyDouyinXSDK();
                    break;
                case AppChannel.IOS_XGUG_China:
                case AppChannel.IOS_XGUG_Sea:
                    ApplyIOS_XGUG_SDK();
                    break;
                case AppChannel.Android_Light:
                    ApplyLight();
                    break;
                case AppChannel.Kuaishou_XSDK_Android:
                    ApplyKuaishouXSDK();
                    break;
                case AppChannel.Huawei_XSDK:
                    ApplyHuaweiXSDK();
                    break;
                case AppChannel.Bilibili_XSDK:
                    ApplyBilibiliXSDK();
                    break;
                case AppChannel.Kuaishou_XSDK:
                    ApplyKuaishouWebXSDK();
                    break;
                case AppChannel.OpenHarmony_Light:
                    ApplyOpenHarmonyLightSDK();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, "应用渠道失败，找不到对应渠道SDK");
            }
            
        }

        public static void ApplyOpenHarmonyLightSDK()
        {
            
            Debug.Log($"导入... {Paths.PACKAGE_OPEN_HARMONY_LIGHT_SDK}");
            ImportUnityPackage(Paths.PACKAGE_OPEN_HARMONY_LIGHT_SDK);
            var manifest_path = $"{Paths.PACKAGE_OPEN_HARMONY_LIGHT_SDK}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }
        
        
        // public static void ApplyXa()
        // {
        //     Debug.Log($"导入... {Paths.PACKAGE_XA_SDK}");
        //     ImportUnityPackage(Paths.PACKAGE_XA_SDK);
        //     var manifest_path = $"{Paths.PACKAGE_XA_SDK}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }


        // public static void ApplyWx()
        // {
        //     Debug.Log($"导入... {Paths.PACKAGE_WX_SDK}");
        //     ImportUnityPackage(Paths.PACKAGE_WX_SDK);
        //     var manifest_path = $"{Paths.PACKAGE_WX_SDK}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }


        public static void ApplyMar()
        {
            Debug.Log($"导入... {Paths.PACKAGE_MAR_SDK}");
            ImportUnityPackage(Paths.PACKAGE_MAR_SDK);
            var manifest_path = $"{Paths.PACKAGE_MAR_SDK}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }

        public static void ApplyLight()
        {
            Debug.Log($"导入... {Paths.PACKAGE_LIGHT_SDK}");
            ImportUnityPackage(Paths.PACKAGE_LIGHT_SDK);
            var manifest_path = $"{Paths.PACKAGE_LIGHT_SDK}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }


        // public static void ApplyBytedance()
        // {
        //     Debug.Log($"导入... {Paths.PACKAGE_BYTE_DANCE_SDK}");
        //     ImportUnityPackage(Paths.PACKAGE_BYTE_DANCE_SDK);
        //     var manifest_path = $"{Paths.PACKAGE_BYTE_DANCE_SDK}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }

        // public static void ApplyOppo()
        // {
        //     Debug.Log($"导入... {Paths.PACKAGE_OPPO_MINI_SDK}");
        //     ImportUnityPackage(Paths.PACKAGE_OPPO_MINI_SDK);
        //     var manifest_path = $"{Paths.PACKAGE_OPPO_MINI_SDK}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }

        // public static void ApplyVivo()
        // {
        //     Debug.Log($"导入... {Paths.PACKAGE_VIVO_MINI_SDK}");
        //     ImportUnityPackage(Paths.PACKAGE_VIVO_MINI_SDK);
        //     var manifest_path = $"{Paths.PACKAGE_VIVO_MINI_SDK}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }


        // public static void ApplyTest()
        // {
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, default);
        //     }
        //
        //     // ImportUnityPackage(Paths.PACKAGE_LIT_JSON);
        //     AssetDatabase.Refresh();
        // }


        // public static void ApplyIOS()
        // {
        //     Debug.Log($"导入... {Paths.PACKAGE_IOS_SDK}");
        //     ImportUnityPackage(Paths.PACKAGE_IOS_SDK);
        //     var manifest_path = $"{Paths.PACKAGE_IOS_SDK}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }

        public static void ApplyXMY()
        {
            Debug.Log($"导入... {Paths.PACKAGE_XMY_SDK}");
            ImportUnityPackage(Paths.PACKAGE_XMY_SDK);
            var manifest_path = $"{Paths.PACKAGE_XMY_SDK}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }

        public static void ApplyLogSDK()
        {
            Debug.Log($"导入... {Paths.PACKAGE_GOOGLE_LOG_SDK}");
            ImportUnityPackage(Paths.PACKAGE_GOOGLE_LOG_SDK);
            var manifest_path = $"{Paths.PACKAGE_GOOGLE_LOG_SDK}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }
        
        // public static void ApplyWxASC()
        // {
        //     Debug.Log($"导入... {Paths.PACKAGE_WX_ASC_SDK}");
        //     ImportUnityPackage(Paths.PACKAGE_WX_ASC_SDK);
        //     var manifest_path = $"{Paths.PACKAGE_WX_ASC_SDK}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }

        // public static void ApplyVivoASC()
        // {
        //     var pack = Paths.PACKAGE_VIVO_ASC_SDK;
        //     Debug.Log($"导入... {pack}");
        //     ImportUnityPackage(pack);
        //     var manifest_path = $"{pack}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }


        // public static void ApplyOppoASC()
        // {
        //     var pack = Paths.PACKAGE_OPPO_ASC_SDK;
        //     Debug.Log($"导入... {pack}");
        //     ImportUnityPackage(pack);
        //     var manifest_path = $"{pack}.manifest";
        //     Debug.Log($"读取资源清单...{manifest_path}");
        //     var manifest = ReadManifest(manifest_path);
        //     Debug.Log($"清理多余文件...");
        //     foreach (var folder in MaybeFolder)
        //     {
        //         ClearFilesInDirectory(folder, manifest);
        //     }
        //
        //     AssetDatabase.Refresh();
        // }

        public static void ApplyWeChat_XSDK()
        {
            var b = XGameEditorUtil.AddPkg(WxPkgId);
            if (!b)
            {
                throw new Exception("Git installation of WeChat SDK failed!");
            }
            Debug.Log($"导入... {Paths.PACKAGE_WX_XSDK_SDK}");
            ImportUnityPackage(Paths.PACKAGE_WX_XSDK_SDK);
            var manifest_path = $"{Paths.PACKAGE_WX_XSDK_SDK}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }


        //导入vivo_xsdk
        public static void ApplyVivo_XSDK()
        {
            
            var package = Paths.PACKAGE_VIVO_XSDK_SDK;
            Debug.Log($"导入... {package}");
            ImportUnityPackage(package);
            var manifest_path = $"{package}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
            
        }
        
        
        //导入oppo xsdk
        public static void ApplyOppoXSDK()
        {
            
            var pack = Paths.PACKAGE_OPPO_XSDK_SDK;
            Debug.Log($"导入... {pack}");
            ImportUnityPackage(pack);
            var manifest_path = $"{pack}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }


        //导入Douyin_Xsdk
        public static void ApplyDouyinXSDK()
        {
            var package = Paths.PACKAGE_DOUYIN_XSDK_SDK;
            Debug.Log($"导入... {package}");
            ImportUnityPackage(package);
            var manifest_path = $"{package}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }

        //导入KuaishouXsdk
        public static void ApplyKuaishouXSDK()
        {
            var package = Paths.PACKAGE_KUAISHOU_XSDK_SDK;
            Debug.Log($"导入... {package}");
            ImportUnityPackage(package);
            var manifest_path = $"{package}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }
        
        public static void ApplyKuaishouWebXSDK()
        {
            var package = Paths.PACKAGE_KUAISHOU_WEB_XSDK_SDK;
            Debug.Log($"导入... {package}");
            ImportUnityPackage(package);
            var manifest_path = $"{package}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }

        //导入HuaweiXsdk
        public static void ApplyHuaweiXSDK()
        {
            var package = Paths.PACKAGE_HUAWEI_XSDK_SDK;
            Debug.Log($"导入... {package}");
            ImportUnityPackage(package);
            var manifest_path = $"{package}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }

        //导入bilibiliXsdk
        public static void ApplyBilibiliXSDK()
        {
            
            var package = Paths.PACKAGE_BILIBILI_XSDK_SDK;
            Debug.Log($"导入... {package}");
            ImportUnityPackage(package);
            var manifest_path = $"{package}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }
        
        //导入IOS_XGUG_SDK
        public static void ApplyIOS_XGUG_SDK()
        {
            var package = Paths.PACKAGE_IOS_XGUG_SDK;
            Debug.Log($"导入... {package}");
            ImportUnityPackage(package);
            var manifest_path = $"{package}.manifest";
            Debug.Log($"读取资源清单...{manifest_path}");
            var manifest = ReadManifest(manifest_path);
            Debug.Log($"清理多余文件...");
            foreach (var folder in MaybeFolder)
            {
                ClearFilesInDirectory(folder, manifest);
            }

            AssetDatabase.Refresh();
        }


        /*清理文件夹,保留文件*/
        public static void ClearFilesInDirectory(string folder, HashSet<string> keep)
        {
            if (keep == null)
            {
                keep = new HashSet<string>();
            }

            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
                foreach (var element in files)
                {
                    var path = XGameEditorUtil.FullPathToAssetDatabasePath(element);
                    if (keep.Contains(path))
                    {
                        continue;
                    }

                    File.Delete(path);
                }

                files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
                if (files.Length <= 0 || OnlyMetaFiles(files))
                {
                    var folderMeta = Path.ChangeExtension(folder, ".meta");
                    if (File.Exists(folderMeta))
                    {
                        File.Delete(folderMeta);
                    }
                    Directory.Delete(folder, true);
                    // Debug.Log($"删除空文件夹：{folder}");
                    return;
                }
            }
        }


        /// <summary>
        /// 是否只有.meta
        /// </summary>
        public static bool OnlyMetaFiles(string[] infos)
        {
            foreach (var info in infos)
            {
                if (!info.EndsWith(".meta"))
                {
                    return false;
                }
            }

            return true;
        }

        /*读取清单*/
        public static HashSet<string> ReadManifest(string path)
        {
            if (!File.Exists(path))
            {
                return new HashSet<string>();
            }

            var contents = File.ReadAllLines(path);
            var result = new HashSet<string>();
            foreach (var e in contents)
            {
                if (!string.IsNullOrEmpty(e))
                {
                    result.Add(e);
                }
            }

            return result;
        }


        /*导入unity package*/
        public static void ImportUnityPackage(string package)
        {
            var manifestPath = $"{package}.manifest";
            var manifest = ReadManifest(manifestPath);
            Debug.Log("manifest count="+manifest.Count);
            // var isReImport = true;
            // if (manifest.Count > 0)
            // {
            //     foreach (var path in manifest)
            //     {
            //         if (path.EndsWith(".meta"))
            //         {
            //             continue;
            //         }
            //         if (File.Exists(path) || Directory.Exists(path))
            //         {
            //             continue;
            //         }
            //         Debug.Log(path+" 找不到");
            //         isReImport = true;
            //         break;
            //     }
            // }

            // if (isReImport)
            // {
                // Debug.Log("应用时需要重新删除后导入");
                //还是得删除后再导入，耗时差不多避免重复导入时unity奇怪异常
                foreach (var folder in MaybeFolder)
                {
                    ClearFilesInDirectory(folder, null);
                }
                Debug.Log("完成删除后重新导入渠道文件");

            // }
            // else
            // {
                // Debug.Log("应用时不需要重新删除导入");
            // }
            AssetDatabase.Refresh();
            XGameEditorUtil.SafeImportUnityPackage(package, false);
        }
    }
}