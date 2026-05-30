using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;


namespace XGame
{
    //安装完成回调
    public delegate void InstallPluginZipCompleteDelegate(string pluginPath);


    public class XGameEditorUtil
    {
        public enum LogColor
        {
            Normal,
            Warn,
            Success,
            Error,
        }

        public static string GradlePath()
        {
            var gradlePath = "";
#if UNITY_ANDROID
            //gradlePath = UnityEditor.Android.AndroidExternalToolsSettings.gradlePath;
            var type = GetType("UnityEditor.Android.AndroidExternalToolsSettings");
            if (null != type)
            {
                var info = type.GetProperty("gradlePath", BindingFlags.Public | BindingFlags.Static);
                if (null != info)
                {
                    gradlePath = info.GetValue(null) as string;
                }
            }
#endif
            return gradlePath;
        }

        public static string GradlewBatPath()
        {
            var gradlePath = GradlePath();
            // Debug.Log("GradlePath="+ gradlePath);
            if (string.IsNullOrEmpty(gradlePath)) return "";
            if (!Directory.Exists(gradlePath)) return "";
            var dir = Path.GetDirectoryName(gradlePath);
            if (!Directory.Exists(dir)) return "";
            var path = Path.Combine(dir, "VisualStudioGradleTemplates", "gradlew.bat");
            if (!File.Exists(path)) return "";
            Debug.Log("GradlewBatPath=" + path);
            return path;
        }

        public static string GradleWrapperJarPath()
        {
            var gradlePath = GradlePath();
            // Debug.Log("GradlePath="+ gradlePath);
            if (string.IsNullOrEmpty(gradlePath)) return "";
            if (!Directory.Exists(gradlePath)) return "";
            var dir = Path.GetDirectoryName(gradlePath);
            if (!Directory.Exists(dir)) return "";
            var path = Path.Combine(dir, "VisualStudioGradleTemplates", "gradle-wrapper.jar");
            if (!File.Exists(path)) return "";
            Debug.Log("GradleWrapperJarPath=" + path);
            return path;
        }


        /// <summary>
        /// 将Newtonsoft.Json的JArray和JObject对象转Dictionary，List的json字符串转对象方法
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static Dictionary<string, object> DeserializeJsonToDictionary(string json)
        {
            var dict = XJson.FromJson<Dictionary<string, object>>(json);
            var jArray = GetType("Newtonsoft.Json.Linq.JArray");
            var jObject = GetType("Newtonsoft.Json.Linq.JObject");
            // Debug.Log("jArray="+jArray);
            // Debug.Log("jObject="+jObject);
            return (Dictionary<string, object>)ConvertJTokens(dict, jObject, jArray);
        }

        public static List<object> DeserializeJsonToList(string json)
        {
            var dict = XJson.FromJson<List<object>>(json);
            var jArray = GetType("Newtonsoft.Json.Linq.JArray");
            var jObject = GetType("Newtonsoft.Json.Linq.JObject");
            // Debug.Log("jArray="+jArray);
            // Debug.Log("jObject="+jObject);
            return (List<object>)ConvertJTokens(dict, jObject, jArray);
        }

        private static object ConvertJTokens(object obj, Type jObject, Type jArray)
        {
            if (obj is IDictionary)
            {
                var dict = (Dictionary<string, object>)obj;
                foreach (var key in dict.Keys.ToList())
                {
                    if (dict[key].GetType() == jObject)
                    {
                        dynamic jObj = dict[key];
                        dict[key] = ConvertJTokens(jObj.ToObject<Dictionary<string, object>>(), jObject, jArray);
                    }
                    else if (dict[key].GetType() == jArray)
                    {
                        dynamic jObj = dict[key];
                        dict[key] = ConvertJTokens(jObj.ToObject<List<object>>(), jObject, jArray);
                    }
                }
            }
            else if (obj is IList)
            {
                var list = (List<object>)obj;
                for (var i = 0; i < list.Count; i++)
                {
                    var a = list[i];
                    if (a.GetType() == jObject)
                    {
                        dynamic jObj = a;
                        list[i] = ConvertJTokens(jObj.ToObject<Dictionary<string, object>>(), jObject, jArray);
                    }
                    else if (a.GetType() == jArray)
                    {
                        dynamic jObj = a;
                        list[i] = ConvertJTokens(jObj.ToObject<List<object>>(), jObject, jArray);
                    }
                }
            }


            return obj;
        }


        /// <summary>
        /// 获取代码文件路径
        /// </summary>
        public static string GetScriptPath<T>() where T : ScriptableObject

        {
#if UNITY_EDITOR
            var instance = ScriptableObject.CreateInstance<T>();
            var script = MonoScript.FromScriptableObject(instance);
            Object.DestroyImmediate(instance);
            return AssetDatabase.GetAssetPath(script);
#endif
            return string.Empty;
        }

        /// <summary>
        /// 获取代码所在文件夹路径
        /// </summary>
        public static string GetScriptFolderPath<T>() where T : ScriptableObject
        {
            var filePath = GetScriptPath<T>();
            var info = new FileInfo(filePath);
            var folderPath = info.Directory.FullName;
            //转成unity路径
            return FullPathToAssetDatabasePath(folderPath);
        }


        /// <summary>
        /// 全路径转AssetDatabase路径
        /// </summary>
        public static string FullPathToAssetDatabasePath(string path)
        {
            path = path.Replace("\\", "/");
            path = path.Replace(Application.dataPath, "Assets");
            return path;
        }


        /// <summary>
        /// 创建一个或获取一个asset
        /// </summary>
        private static T CreateOrGetScriptableObject<T>() where T : ScriptableObject
        {
            var folder = GetScriptFolderPath<T>();
            var assets = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { folder });
            //asset保存路径
            var preferencePath = $"{folder}/{typeof(T).Name}.asset";
            if (assets == null || assets.Length <= 0)
            {
                //不存在资源
                var instance = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(instance, preferencePath);
                AssetDatabase.Refresh();
                return instance;
            }
            else
            {
                var instance =
                    AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[0]));
                return instance;
            }
        }


        /// <summary>
        /// 加载或者创建实例
        /// </summary>
        public static ScriptableObject LoadOrCreate(string path, Type type)
        {
            var target = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            if (target.GetType() == type)
            {
                return target;
            }

            var instance = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.Refresh();
            return instance;
        }

        /// <summary>
        /// 加载或者创建实例
        /// </summary>
        public static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var target = AssetDatabase.LoadAssetAtPath<T>(path);

            if (target != null && target.GetType() == typeof(T))
            {
                return target;
            }

            FileInfo info = new FileInfo(path);
            CheckCreateFolder(info.Directory.FullName);
            var instance = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.Refresh();
            return instance;
        }


        public static void CheckCreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 递归删除空目录
        /// </summary>
        public static void DeleteEmptyDirectories(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(path);

            // 先处理子目录
            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                DeleteEmptyDirectories(subDir.FullName);
            }

            // 判断当前目录是否为空（无文件、无子目录）
            bool isEmpty = !dirInfo.EnumerateFileSystemInfos().Any();

            if (isEmpty)
            {
                try
                {
                    dirInfo.Delete();
                    Debug.Log($"已删除空目录: {path}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"无法删除目录 {path}: {e.Message}");
                }
            }
        }


        /// <summary>
        /// 创建配置
        /// </summary>
        public static T CreateScriptableObject<T>(string path) where T : ScriptableObject
        {
            var result = ScriptableObject.CreateInstance<T>();
            FileInfo info = new FileInfo(path);
            CheckCreateFolder(info.Directory.FullName);
            AssetDatabase.CreateAsset(result, path);
            AssetDatabase.Refresh();
            return result;
        }


        /// <summary>
        /// 显示提示框
        /// </summary>
        public static void ShowMessageBox(string msg, Action success = null, Action fail = null)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("Prompt", msg, "OK"))
                success?.Invoke();
            else
                fail?.Invoke();
#endif
        }

        /// <summary>
        /// 导入 .unitypackage；若抛异常则提示并打开包所在目录，便于手动导入。
        /// </summary>
        public static void SafeImportUnityPackage(string packagePath, bool interactive)
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += () =>
            {
                try
                {
                    AssetDatabase.ImportPackage(packagePath, interactive);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    var fullPath = UnityPackagePathToFullPath(packagePath);
                    EditorUtility.DisplayDialog(
                        "Import failed",
                        $"Unity package import failed. Double-click the package in File Explorer to import manually:\n{fullPath}\n\n{e.Message}",
                        "OK");
                    try
                    {
                        if (!string.IsNullOrEmpty(fullPath) && File.Exists(fullPath))
                            EditorUtility.RevealInFinder(fullPath);
                        else
                        {
                            var dir = Path.GetDirectoryName(fullPath);
                            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                                EditorUtility.RevealInFinder(dir);
                        }
                    }
                    catch (Exception revealEx)
                    {
                        Debug.LogWarning(revealEx);
                    }
                }
            };

#endif
        }

        static string UnityPackagePathToFullPath(string packagePath)
        {
            if (string.IsNullOrEmpty(packagePath))
                return packagePath;
            if (Path.IsPathRooted(packagePath))
                return Path.GetFullPath(packagePath);
            if (packagePath.StartsWith("Assets", StringComparison.OrdinalIgnoreCase))
            {
                var rel = packagePath.Substring("Assets".Length).TrimStart('/', '\\');
                return Path.GetFullPath(Path.Combine(Application.dataPath, rel));
            }

            return Path.GetFullPath(packagePath);
        }


        /// <summary>
        /// 时间转导出后缀
        /// </summary>
        /// <returns></returns>
        public static string DateTimeToPublishSuffixString()
        {
            return
                $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second}";
        }


        /// <summary>
        /// ping 对象
        /// </summary>
        public static void PingObject(string path)
        {
#if UNITY_EDITOR
            var target = AssetDatabase.LoadAssetAtPath<Object>(path);
            PingObject(target);
#endif
        }

        /// <summary>
        /// ping 对象
        /// </summary>
        public static void PingObject(Object target)
        {
#if UNITY_EDITOR
            if (target != null) EditorGUIUtility.PingObject(target);

#endif
        }


        /// <summary>
        /// 反射获取类型
        /// </summary>
        public static Type GetType(string typeName)
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

        //获取用户文件夹
        public static string GetWindowUserFolder()
        {
            var persistentDataPath = Application.persistentDataPath;
            var info = new DirectoryInfo(persistentDataPath);
            var fullPath = info.FullName;
            var match = @"AppData\LocalLow";
            if (fullPath.Contains(match))
            {
                var index = fullPath.IndexOf(match);
                var path = fullPath.Substring(0, index - 1);

                return path.Replace("\\", "/");
            }

            //返回根目录
            return info.Root.FullName.Replace("\\", "");
        }

        //获取根目录
        public static string GetProjectRootDisk()
        {
            var info = new DirectoryInfo(Application.dataPath);
            return info.Root.FullName;
        }

        // #region 安装open ssl
        //
        // public static bool IsInstallOpenssl()
        // {
        //     var installPath = JsonPluginConfig.Default.open_ssl_install_path;
        //     if (!string.IsNullOrEmpty(installPath))
        //     {
        //         var path = $"{installPath}/bin/openssl.exe";
        //         if (File.Exists(path))
        //         {
        //             return true;
        //         }
        //     }
        //
        //     return false;
        // }
        //
        // public static void InstallOpenssl()
        // {
        //     var installPath =
        //         EditorUtility.OpenFolderPanel("请选择安装目录，注意：不要安装在Assets下！", GetProjectRootDisk(), "XGamePlugin");
        //
        //     if (string.IsNullOrEmpty(installPath))
        //     {
        //         Debug.Log("无效路径");
        //         return;
        //     }
        //
        //     //文件名
        //     var folderName = new DirectoryInfo(Paths.OPON_SSL_TOOL_PATH).Name;
        //     folderName = folderName.Replace(".zip", "");
        //     var finalFolderPath = $"{installPath}/{folderName}";
        //     if (Directory.Exists(finalFolderPath))
        //     {
        //         throw new Exception($"存在相同路径，{finalFolderPath},请删除后再安装！");
        //     }
        //
        //     ZipTool.UnZip(Paths.OPON_SSL_TOOL_PATH, installPath, () =>
        //     {
        //         EditorUtility.ClearProgressBar();
        //         EditorUtility.RevealInFinder(finalFolderPath);
        //         //写入配置
        //         JsonPluginConfig.Default.open_ssl_install_path = finalFolderPath;
        //         JsonPluginConfig.Default.WriteAndSave();
        //     }, (progress) =>
        //     {
        //         EditorUtility.DisplayProgressBar($"安装openssl...",
        //             $"请稍等...{(int)(progress * 100)}%",
        //             progress);
        //     });
        //     // //解压文件到目录
        //     // using (ZipFile zip = ZipFile.Read(Paths.OPON_SSL_TOOL_PATH))
        //     // {
        //     //     var totalFiles = zip.Count;
        //     //     var filesExtracted = 0;
        //     //     zip.ExtractProgress += (a, e) =>
        //     //     {
        //     //         if (e.EventType != ZipProgressEventType.Extracting_BeforeExtractEntry)
        //     //             return;
        //     //         filesExtracted++;
        //     //         var progress = (float)filesExtracted / totalFiles;
        //     //         EditorUtility.DisplayProgressBar($"安装openssl...",
        //     //             $"请稍等...{(int)(progress * 100)}%",
        //     //             progress);
        //     //         if (filesExtracted >= totalFiles)
        //     //         {
        //     //             EditorUtility.ClearProgressBar();
        //     //             EditorUtility.RevealInFinder(finalFolderPath);
        //     //             //写入配置
        //     //             JsonPluginConfig.Default.open_ssl_install_path = finalFolderPath;
        //     //             JsonPluginConfig.Default.WriteAndSave();
        //     //         }
        //     //     };
        //     //     zip.ExtractAll(installPath);
        //     // }
        // }
        //
        // #endregion

        #region 安装nodejs v15

        //是否安装nodejs 15
        public static bool IsInstallNodeJs15()
        {
            var installPath = JsonPluginConfig.Default.node_js_v15;
            if (!string.IsNullOrEmpty(installPath))
            {
                var path = $"{installPath}/node.exe";
                if (File.Exists(path))
                {
                    return true;
                }
            }

            return false;
        }

        //安装node js 15,微信和B站需要这个
        public static void InstallNodeJs15()
        {
            var installPath =
                EditorUtility.OpenFolderPanel("Choose install folder. Do not install under Assets.",
                    GetProjectRootDisk(), "XGamePlugin");

            if (string.IsNullOrEmpty(installPath))
            {
                Debug.Log("无效路径");
                return;
            }

            //文件名
            var folderName = new DirectoryInfo(Paths.NODE_JS_15_ZIP_PATH).Name;
            folderName = folderName.Replace(".zip", "");
            var finalFolderPath = $"{installPath}/{folderName}";
            if (Directory.Exists(finalFolderPath))
            {
                throw new Exception($"存在相同路径，{finalFolderPath},请删除后再安装！");
            }

            ZipTool.UnZip(Paths.NODE_JS_15_ZIP_PATH, installPath, () =>
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.RevealInFinder(finalFolderPath);
                //写入配置
                JsonPluginConfig.Default.node_js_v15 = finalFolderPath;
                JsonPluginConfig.Default.WriteAndSave();
            }, (progress) =>
            {
                EditorUtility.DisplayProgressBar("Installing Node.js v15...",
                    $"Please wait... {(int)(progress * 100)}%",
                    progress);
            });
        }

        #endregion

        // #region 安装nodejs v10.13.0
        //
        // //是否安装nodejs v10.13.0
        // public static bool IsInstallNodeJs_v10_13_0()
        // {
        //     var installPath = JsonPluginConfig.Default.node_js_v10_13_0;
        //     if (!string.IsNullOrEmpty(installPath))
        //     {
        //         var path = $"{installPath}/node.exe";
        //         if (File.Exists(path))
        //         {
        //             return true;
        //         }
        //     }
        //
        //     return false;
        // }

        //安装node js v10.13.0
        // public static void InstallNodeJs_v10_13_0()
        // {
        //     InstallPluginZip(Paths.NODE_JS_10_13_0_ZIP, "安装nodejs v10.13.0", (plugin) =>
        //     {
        //         //安装完毕，写入配置
        //         JsonPluginConfig.Default.node_js_v10_13_0 = plugin;
        //         JsonPluginConfig.Default.WriteAndSave();
        //         try
        //         {
        //             EditorUtility.RevealInFinder($"{plugin}/node.exe");
        //             Debug.Log("<color=#2bdc70>安装nodejs v10.13.0 成功!</color>");
        //         }
        //         catch (Exception e)
        //         {
        //             Debug.LogError("安装失败");
        //             JsonPluginConfig.Default.node_js_v10_13_0 = "";
        //             JsonPluginConfig.Default.WriteAndSave();
        //         }
        //     });
        // }

        // #endregion

        // #region 安装QuickGameToolkitUnity
        //
        // public static bool IsIntallQuickGameToolkitUnity()
        // {
        //     //获取安装目录
        //     var installPath = JsonPluginConfig.Default.oppo_quick_game_tool_install_path;
        //
        //     if (!string.IsNullOrEmpty(installPath))
        //     {
        //         //检查工具是否有效
        //         if (Directory.Exists(installPath))
        //         {
        //             var file1 = $"{installPath}/lib/bin/quickgame";
        //             var file2 = $"{installPath}/lib/bin/quickgame.cmd";
        //             var file3 = $"{installPath}/lib/bin/index.js";
        //             var packageJson = $"{installPath}/package.json";
        //             var node_modules = $"{installPath}/node_modules";
        //
        //             if (File.Exists(file1) && File.Exists(file2) && File.Exists(file3) && File.Exists(packageJson) &&
        //                 Directory.Exists(node_modules))
        //             {
        //                 return true;
        //             }
        //         }
        //     }
        //
        //     return false;
        // }

        //安装quickgame-toolkit-unity
        // public static void InstallQuickGameToolkitUnity()
        // {
        //     InstallPluginZip(Paths.OPPO_QUICK_GAME_TOOL_ZIP, "安装quickgame-toolkit-unity...", (path) =>
        //     {
        //         //安装完毕，写入配置
        //         JsonPluginConfig.Default.oppo_quick_game_tool_install_path = path;
        //         JsonPluginConfig.Default.WriteAndSave();
        //         try
        //         {
        //             EditorUtility.RevealInFinder($"{path}/node_modules");
        //             Debug.Log("<color=#2bdc70>安装quickgame-toolkit-unity 成功!</color>");
        //         }
        //         catch (Exception e)
        //         {
        //             Debug.LogError("安装失败");
        //             JsonPluginConfig.Default.oppo_quick_game_tool_install_path = "";
        //             JsonPluginConfig.Default.WriteAndSave();
        //         }
        //     });
        // }

        // #endregion

        //安装插件
        public static void InstallPluginZip(string zip, string tips, InstallPluginZipCompleteDelegate complete = null)
        {
            var installPath =
                EditorUtility.OpenFolderPanel("Choose install folder. Do not install under Assets.",
                    GetProjectRootDisk(), "XGamePlugin");
            if (string.IsNullOrEmpty(installPath))
            {
                Debug.Log("无效路径");
                return;
            }

            if (Path.GetFullPath(installPath).StartsWith(Path.GetFullPath(Application.dataPath)))
            {
                Debug.Log("无效路径,不可安装在Asset目录下");
                return;
            }

            //文件名
            var folderName = Path.GetFileNameWithoutExtension(zip);
            var pluginFolder = $"{installPath}/{folderName}";
            if (Directory.Exists(pluginFolder))
            {
                throw new Exception($"存在相同路径，{pluginFolder},请删除后再安装！");
            }

            //解压到指定目录
            RunUnZip(zip, installPath, tips, () => { complete?.Invoke(pluginFolder); });
        }

        //解压
        public static void RunUnZip(string from, string to, string tips, Action complete)
        {
            var nextProcess = 0f;
            ZipTool.UnZip(from, to, () =>
            {
                EditorUtility.ClearProgressBar();
                complete?.Invoke();
            }, (progress) =>
            {
                if (progress > nextProcess)
                {
                    EditorUtility.DisplayProgressBar($"{tips}",
                        $"Please wait... {(int)(progress * 100)}%",
                        progress);
                    nextProcess += 0.01f;
                }
            });
            // //解压文件到目录
            // using (ZipFile zip = ZipFile.Read(Path.GetFullPath(from)))
            // {
            //     var totalFiles = zip.Count;
            //     var filesExtracted = 0;
            //     zip.ExtractProgress += (a, e) =>
            //     {
            //         if (e.EventType != ZipProgressEventType.Extracting_BeforeExtractEntry)
            //             return;
            //         filesExtracted++;
            //         var progress = (float)filesExtracted / totalFiles;
            //         EditorUtility.DisplayProgressBar($"{tips}",
            //             $"请稍等...{(int)(progress * 100)}%",
            //             progress);
            //         if (filesExtracted >= totalFiles)
            //         {
            //             EditorUtility.ClearProgressBar();
            //             complete?.Invoke();
            //         }
            //     };
            //     zip.ExtractAll(to, ExtractExistingFileAction.OverwriteSilently);
            // }
        }

        public static void ImportBuildSetting()
        {
            SafeImportUnityPackage(@"Assets\XGameUnityTool\Res\UnityPackage\常用打包设置.unitypackage", true);
        }

        //是否有中文字符
        public static bool HasChineseChar(string content)
        {
            Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
            Match m = RegCHZN.Match(content);
            return m.Success;
        }

        //获取匹配项
        public static int IndexOfMatchLine(string[] lines, params string[] check)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                bool isMatch = true;
                var element = lines[i];
                foreach (var s in check)
                {
                    if (!element.Contains(s))
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    return i;
                }
            }

            return -1;
        }

        //安装vivo 小游戏脚手架2019
        // public static void InstallVivoCliSrcU2019()
        // {
        //     var installPath =
        //         EditorUtility.OpenFolderPanel("请选择安装目录，注意：不要安装在Assets下！", GetProjectRootDisk(), "XGamePlugin");
        //
        //     if (string.IsNullOrEmpty(installPath))
        //     {
        //         Debug.Log("无效路径");
        //         return;
        //     }
        //
        //     //文件名
        //     var folderName = new DirectoryInfo(Paths.VIVO_CLI_SRC_2019_ZIP).Name;
        //     folderName = folderName.Replace(".zip", "");
        //     var finalFolderPath = $"{installPath}/{folderName}";
        //     if (Directory.Exists(finalFolderPath))
        //     {
        //         throw new Exception($"存在相同路径，{finalFolderPath},请删除后再安装！");
        //     }
        //
        //     ZipTool.UnZip(Paths.VIVO_CLI_SRC_2019_ZIP, installPath, () =>
        //     {
        //         EditorUtility.ClearProgressBar();
        //         EditorUtility.RevealInFinder(finalFolderPath);
        //         //写入配置
        //         JsonPluginConfig.Default.vivo_cli_src_unity_2019 = finalFolderPath;
        //         JsonPluginConfig.Default.WriteAndSave();
        //     }, (progress) =>
        //     {
        //         EditorUtility.DisplayProgressBar($"安装vivo cli src u2019...",
        //             $"请稍等...{(int)(progress * 100)}%",
        //             progress);
        //     });
        //     //解压文件到目录
        //     // using (ZipFile zip = ZipFile.Read(Paths.VIVO_CLI_SRC_2019_ZIP))
        //     // {
        //     //     var totalFiles = zip.Count;
        //     //     var filesExtracted = 0;
        //     //     zip.ExtractProgress += (a, e) =>
        //     //     {
        //     //         if (e.EventType != ZipProgressEventType.Extracting_BeforeExtractEntry)
        //     //             return;
        //     //         filesExtracted++;
        //     //         var progress = (float)filesExtracted / totalFiles;
        //     //         EditorUtility.DisplayProgressBar($"安装vivo cli src u2019...",
        //     //             $"请稍等...{(int)(progress * 100)}%",
        //     //             progress);
        //     //         if (filesExtracted >= totalFiles)
        //     //         {
        //     //             EditorUtility.ClearProgressBar();
        //     //             EditorUtility.RevealInFinder(finalFolderPath);
        //     //             //写入配置
        //     //             JsonPluginConfig.Default.vivo_cli_src_unity_2019 = finalFolderPath;
        //     //             JsonPluginConfig.Default.WriteAndSave();
        //     //         }
        //     //     };
        //     //     zip.ExtractAll(installPath);
        //     // }
        // }

        //安装vivo 小游戏脚手架2020
        // public static void InstallVivoCliSrcU2020()
        // {
        //     var installPath =
        //         EditorUtility.OpenFolderPanel("请选择安装目录，注意：不要安装在Assets下！", GetProjectRootDisk(), "XGamePlugin");
        //
        //     if (string.IsNullOrEmpty(installPath))
        //     {
        //         Debug.Log("无效路径");
        //         return;
        //     }
        //
        //     //文件名
        //     var folderName = new DirectoryInfo(Paths.VIVO_CLI_SRC_2020_ZIP).Name;
        //     folderName = folderName.Replace(".zip", "");
        //     var finalFolderPath = $"{installPath}/{folderName}";
        //     if (Directory.Exists(finalFolderPath))
        //     {
        //         throw new Exception($"存在相同路径，{finalFolderPath},请删除后再安装！");
        //     }
        //
        //     ZipTool.UnZip(Paths.VIVO_CLI_SRC_2020_ZIP, installPath, () =>
        //     {
        //         EditorUtility.ClearProgressBar();
        //         EditorUtility.RevealInFinder(finalFolderPath);
        //         //写入配置
        //         JsonPluginConfig.Default.vivo_cli_src_unity_2020 = finalFolderPath;
        //         JsonPluginConfig.Default.WriteAndSave();
        //     }, (progress) =>
        //     {
        //         EditorUtility.DisplayProgressBar($"安装vivo cli src u2020...",
        //             $"请稍等...{(int)(progress * 100)}%",
        //             progress);
        //     });
        //     //解压文件到目录
        //     // using (ZipFile zip = ZipFile.Read(Paths.VIVO_CLI_SRC_2020_ZIP))
        //     // {
        //     //     var totalFiles = zip.Count;
        //     //     var filesExtracted = 0;
        //     //     zip.ExtractProgress += (a, e) =>
        //     //     {
        //     //         if (e.EventType != ZipProgressEventType.Extracting_BeforeExtractEntry)
        //     //             return;
        //     //         filesExtracted++;
        //     //         var progress = (float)filesExtracted / totalFiles;
        //     //         EditorUtility.DisplayProgressBar($"安装vivo cli src u2020...",
        //     //             $"请稍等...{(int)(progress * 100)}%",
        //     //             progress);
        //     //         if (filesExtracted >= totalFiles)
        //     //         {
        //     //             EditorUtility.ClearProgressBar();
        //     //             EditorUtility.RevealInFinder(finalFolderPath);
        //     //             //写入配置
        //     //             JsonPluginConfig.Default.vivo_cli_src_unity_2020 = finalFolderPath;
        //     //             JsonPluginConfig.Default.WriteAndSave();
        //     //         }
        //     //     };
        //     //     zip.ExtractAll(installPath);
        //     // }
        // }

        //执行bat
        public static void RunBat(string batFile)
        {
            if (!File.Exists(batFile))
            {
                Debug.LogError("bat文件不存在：" + batFile);
            }
            else
            {
                var workingDir = Path.GetDirectoryName(batFile);
                var fileName = Path.GetFileName(batFile);
                Process proc = null;
                try
                {
                    proc = new Process();
                    proc.StartInfo.WorkingDirectory = workingDir;
                    proc.StartInfo.FileName = fileName;
                    // proc.StartInfo.RedirectStandardOutput = true;
                    // proc.StartInfo.UseShellExecute = false;
                    //proc.StartInfo.Arguments = args;
                    //proc.StartInfo.CreateNoWindow = true;
                    //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//disable dos window
                    proc.Start();
                    proc.WaitForExit();
                    proc.Close();
                    // var output = proc.StandardOutput.ReadToEnd();
                    // Debug.Log(output);
                }
                catch (Exception ex)
                {
                    Debug.LogFormat("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
                }
            }
        }

        //获取md5码
        public static string GetMD5WithString(String input)
        {
            MD5 md5Hash = MD5.Create();
            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder str = new StringBuilder();
            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                str.Append(data[i].ToString("x2")); //加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
            }

            // 返回十六进制字符串  
            return str.ToString();
        }

        [Serializable]
        public class JsonPackageInfo
        {
            public long versionCode;
            public string version;
        }

        [Serializable]
        public class JsonUpdateVersion
        {
            public string version;
            public long versionCode;
            public string url;
        }

        //获取工具版本号
        public static long GetToolVersionCode()
        {
            if (File.Exists(Paths.PACKAGE_JSON))
            {
                var info = XJson.FromJson<JsonPackageInfo>(File.ReadAllText(Paths.PACKAGE_JSON));
                return info.versionCode;
            }

            return 0;
        }

        //获取工具版本号
        public static string GetToolVersion()
        {
            if (File.Exists(Paths.PACKAGE_JSON))
            {
                var info = XJson.FromJson<JsonPackageInfo>(File.ReadAllText(Paths.PACKAGE_JSON));
                return info.version;
            }

            return "unknown version";
        }


        //检查更新
        public static async void CheckUpdate()
        {
            Debug.Log("检查更新...请稍等");
            HttpGet($"{Paths.REMOTE_XGAME_UNTIY_TOOL_URL}/lastest.json", (res) =>
            {
                try
                {
                    var updateVersion = XJson.FromJson<JsonUpdateVersion>(res);
                    //与当前版本比对
                    if (updateVersion.versionCode > GetToolVersionCode())
                    {
                        //打开更新窗口
                        UpdateToolWindow.Open(updateVersion.url, updateVersion.version);
                    }
                    else
                    {
                        ShowMessageBox("It's already the latest version");
                    }
                }
                catch (Exception e)
                {
                    ShowMessageBox($"Check failed, unable to parse. {res}");
                }
            }, (error) => { ShowMessageBox($"Inspection failed.{error}"); }, 5);
        }

        //自动检查更新
        public static void AutoCheckUpdate()
        {
            if (!ToolPreference.Global.CheckUpdate)
            {
                return;
            }

            HttpGet($"{Paths.REMOTE_XGAME_UNTIY_TOOL_URL}/lastest.json", (res) =>
            {
                try
                {
                    var updateVersion = XJson.FromJson<JsonUpdateVersion>(res);
                    //与当前版本比对
                    if (updateVersion.versionCode > GetToolVersionCode())
                    {
                        if (EditorUtility.DisplayDialog("New version available",
                                $"A new version is available: {updateVersion.version}. Update now?",
                                "Yes", "Ignore"))
                        {
                            CheckUpdate();
                        }
                    }
                }
                catch
                {
                }
            }, null, 5);
        }


        //自动检查重大修复
        public static void AutoCheckMajorFixed(Action success, Action fail)
        {
            if (!ToolPreference.Global.MajorNotice)
            {
                fail?.Invoke();
                return;
            }

            HttpGet($"{Paths.REMOTE_XGAME_UNTIY_TOOL_URL}/major_fixed.json", (res) =>
            {
                try
                {
                    var data = XJsonObject.FromJson(res);
                    var versionCode = data["versionCode"].ToXLong();
                    var content = data["content"].ToString();
                    //与当前版本比对
                    if (versionCode > GetToolVersionCode())
                    {
                        ShowMessageBox($"A major fix is available! Check for updates?\n\n{content}", CheckUpdate);
                        success?.Invoke();
                    }
                    else
                    {
                        fail?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    fail?.Invoke();
                }
            }, (error) => { fail?.Invoke(); }, 5);
        }

        public static async void HttpGet(string url, Action<string> success, Action<string> error = null,
            int timeOut = 60)
        {
            HttpWebRequest req =
                (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.Timeout = timeOut * 1000;
            req.ContentType = "application/json";
            try
            {
                var resp = await req.GetResponseAsync();
                var response = (HttpWebResponse)resp;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        var json = reader.ReadToEnd();
                        success?.Invoke(json);
                    }
                }
                else
                {
                    error?.Invoke(response.StatusCode.ToString());
                }
            }
            catch (Exception e)
            {
                error?.Invoke(e.Message);
            }
        }


        public static bool IsDevelop()
        {
            return File.Exists("Assets/XGameUnityTool/develop.txt");
        }

        public static bool HasNewtonsoftJson()
        {
            return GetType("Newtonsoft.Json.JsonConvert") != null;
        }

        [MenuItem("XGameUnityTool/Install Newtonsoft.Json")]
        public static void InstallNewtonsoftJson()
        {
//             var isUnity2018 = false;
// #if UNITY_2018
// 	isUnity2018 = true;
// #endif
//             if (isUnity2018)
//             {
//                 var manifest_json = $"{Path.GetDirectoryName(Application.dataPath)}/Packages/manifest.json";
//                 if (!File.Exists(manifest_json))
//                 {
//                     Log($"安装Newtonsoft.Json 失败！error：找不到{manifest_json}", LogColor.Error);
//                 }
//                 else
//                 {
//                     var path = "\"com.unity.nuget.newtonsoft-json\": \"3.0.2\"";
//                     ShowMessageBox($"请手动编辑Packages/manifest.json,添加一条记录：{path}，复制记录&跳转目录？", () =>
//                     {
//                         EditorGUIUtility.systemCopyBuffer = path;
//                         Debug.Log($"复制：{path},成功！");
//                         EditorUtility.RevealInFinder(manifest_json);
//                     });
//                 }
//
//                 return;
//             }

            // string url = "https://gitee.com/zhangweijian0712/typhoon.jsonmodule.git#latest";
            string url = "com.unity.nuget.newtonsoft-json";
            Log("install Newtonsoft.Json...", LogColor.Normal);
            AddRequest request = Client.Add(url);
            while (!request.IsCompleted)
            {
                // Wait for the request to complete.
            }

            if (request.Status == StatusCode.Success)
            {
                Log("Newtonsoft.Json installation successful!", LogColor.Success);
            }
            else if (request.Status >= StatusCode.Failure)
            {
                Log($"Installation of Newtonsoft.Json failed! error：{request.Error.message}", LogColor.Error);
            }
        }

        public static List<PackageInfo> QueryInstalledPackages()
        {
            // Debug.Log("QueryInstalledPackages...");

            var listRequest = Client.List(); // 列出已为项目安装的包

            while (!listRequest.IsCompleted)
            {
                // Wait for the request to complete.
            }

            switch (listRequest.Status)
            {
                case StatusCode.Success:
                {
                    var infos = new List<PackageInfo>();

                    foreach (var package in listRequest.Result)
                    {
                        // Debug.Log("Package name: " + package.name);
                        infos.Add(package);
                    }

                    return infos;
                }
                case StatusCode.Failure:
                    Debug.LogError(listRequest.Error.message);
                    break;
            }

            return null;
        }

        public static bool RemovePkg(string pkgId, bool openLog = false)
        {
            if (openLog)
            {
                Debug.Log("RemovePkg: " + pkgId);
            }

            var isInstalledInstantGame = false;
            PackageInfo info = null;
            var infos = QueryInstalledPackages();
            if (null == infos)
            {
                if (openLog)
                {
                    Debug.LogError($"{pkgId} 移除失败，因为获取已安装包失败！");
                }

                return false;
            }

            foreach (var package in infos)
            {
                // Debug.Log($"Query Installed Package Name={package.name}, packageId={package.packageId}, version={package.version}");
                if (!package.packageId.Contains(pkgId)) continue;
                info = package;
                isInstalledInstantGame = true;
                break;
            }


            if (isInstalledInstantGame)
            {
                var addRequest = Client.Remove(info.name);
                while (!addRequest.IsCompleted)
                {
                    // Wait for the request to complete.
                }

                if (addRequest.Status == StatusCode.Success)
                {
                    if (openLog)
                    {
                        Debug.Log($"移除 {pkgId} 成功！");
                    }

                    return true;
                }
                else if (addRequest.Status == StatusCode.Failure)
                {
                    if (openLog)
                    {
                        Debug.LogError($"移除 {pkgId} 失败！error：{addRequest.Error.message}");
                    }

                    return false;
                }
            }
            else
            {
                if (openLog)
                {
                    Debug.Log($"{pkgId} 没找到");
                }

                return true;
            }

            return false;
        }


        public static bool AddPkg(string pkgId)
        {
            Debug.Log("AddPkg...");

            var isInstalledInstantGame = false;

            var infos = QueryInstalledPackages();
            if (null == infos)
            {
                Debug.LogError($"{pkgId} 安装失败，因为获取已安装包失败！");
                return false;
            }

            foreach (var package in infos)
            {
                // Debug.Log($"Query Installed Package Name={package.name}, packageId={package.packageId}, version={package.version}");
                if (!package.packageId.Contains(pkgId)) continue;
                isInstalledInstantGame = true;
                break;
            }


            if (!isInstalledInstantGame)
            {
                var addRequest = Client.Add(pkgId);
                while (!addRequest.IsCompleted)
                {
                    // Wait for the request to complete.
                }

                switch (addRequest.Status)
                {
                    case StatusCode.Success:
                        Debug.Log($"安装 {pkgId} 成功！");
                        return true;
                    case StatusCode.Failure:
                        Debug.LogError($"安装 {pkgId} 失败！error：{addRequest.Error.message}");
                        return false;
                }
            }
            else
            {
                Debug.Log($"{pkgId} 已经安装了");
                return true;
            }

            return false;
        }


        // [MenuItem("XGameUnityTool/安装InstantGame")]
        public static void InstallInstantGame()
        {
            CheckInstantGameInstall();
        }

        public static void CheckInstantGameInstall(Action<bool> completion = null)
        {
            // if (!Application.unityVersion.StartsWith("2019.4.29f1c1"))
            // {
            //     Debug.LogError("unity版本不对，请使用指定版本，下载地址为：https://unity.cn/instantgame");
            //     EditorUtility.DisplayDialog("unity版本不对，请使用指定版本", "下载地址为：https://unity.cn/instantgame",
            //         "ok");
            //     return;
            // }

            Debug.Log("InstallInstantGame...");

            var packageName = "com.unity.instantgame"; // 安装的包名

            var isInstalledInstantGame = false;

            var infos = QueryInstalledPackages();
            if (null == infos)
            {
                Debug.LogError("安装 InstantGame 失败，因为获取已安装包失败！");
                completion?.Invoke(false);
                return;
            }

            foreach (var package in infos)
            {
                // Debug.Log("Query Installed Package Name: " + package.name);
                if (package.name == packageName)
                {
                    isInstalledInstantGame = true;
                    break;
                }
            }


            if (!isInstalledInstantGame)
            {
                AddRequest addRequest = Client.Add(packageName);
                while (!addRequest.IsCompleted)
                {
                    // Wait for the request to complete.
                }

                if (addRequest.Status == StatusCode.Success)
                {
                    Debug.Log("安装 InstantGame 成功！");
                    completion?.Invoke(true);
                }
                else if (addRequest.Status >= StatusCode.Failure)
                {
                    Debug.LogError($"安装 InstantGame 失败！error：{addRequest.Error.message}");
                    completion?.Invoke(false);
                }
            }
            else
            {
                Debug.Log(" InstantGame已经安装了");
                completion?.Invoke(true);
            }
        }


        public static void TryOverrideXJsonCSharp()
        {
            var path = $"Assets/XGameUnityTool/Runtime/XJson.cs";
            if (!File.Exists(path))
            {
                throw new Exception($"找不到文件：{path}");
            }

            var code = File.ReadAllText(path);


            if (HasNewtonsoftJson())
            {
                string pattern = @"^#define\s+NEWTONSOFT$";
                bool isMatch = Regex.IsMatch(code, pattern);
                if (!isMatch)
                {
                    //不匹配，更新cs
                }
            }
        }

        /// <summary>
        ///  删除文件夹,适配长路径
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFolder(string path)
        {
            try
            {
                Alphaleonis.Win32.Filesystem.Directory.Delete(path, true);
            }
            catch (Exception e)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true); // 递归删除
                }
            }
        }


        /// <summary>
        ///  删除文件,适配长路径
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(string path)
        {
            try
            {
                Alphaleonis.Win32.Filesystem.File.Delete(path, true);
            }
            catch (Exception e)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        public static void CopyFolder(string from, string to, bool isSkipMetaFile = false)
        {
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
            }

            var info = new DirectoryInfo(from);
            var files = info.GetFiles("*", SearchOption.TopDirectoryOnly);
            var dir = info.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (var element in files)
            {
                if (isSkipMetaFile)
                {
                    if (element.FullName.EndsWith(".meta"))
                    {
                        continue;
                    }
                }

                var copyTo = $"{to}/{element.Name}";
                File.Copy(element.FullName, copyTo, true);
            }

            foreach (var element in dir)
            {
                var copyDir = $"{to}/{element.Name}";
                CopyFolder(element.FullName, copyDir, isSkipMetaFile);
            }
        }

        // 创建并写入文本
        public static void CreateFileAndWriteText(string path, string content)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                AssetDatabase.Refresh();
            }

            File.WriteAllText(path, content);
        }

        /// <summary>
        /// 复制文件夹，带进度
        /// </summary>
        public static async Task CopyFolderWithProcess(string from, string to, Action<float> onProcess, Action complete,
            int stepUnit = 200)
        {
            var dir = new HashSet<string>();
            var files = new Dictionary<string, string>();
            GetCopyFolderManifest(from, to, dir, files);
            var total = dir.Count + files.Count;
            var current = 0;
            var step = 0;
            if (total <= 0)
            {
                complete?.Invoke();
                return;
            }

            foreach (var element in dir)
            {
                current += 1;
                step += 1;
                if (!Directory.Exists(element))
                {
                    Directory.CreateDirectory(element);
                }

                if (step > stepUnit)
                {
                    step = 0;
                    onProcess?.Invoke((float)current / total);
                    await Task.Yield();
                }
            }

            foreach (var element in files)
            {
                current += 1;
                step += 1;
                var copyFrom = element.Key;
                var copyTo = element.Value;
                File.Copy(copyFrom, copyTo, true);
                if (step > stepUnit)
                {
                    step = 0;
                    onProcess?.Invoke((float)current / total);
                    await Task.Yield();
                }
            }

            complete?.Invoke();
        }


        private static void GetCopyFolderManifest(string from, string to, HashSet<string> dirData,
            Dictionary<string, string> filesData)
        {
            dirData.Add(to);
            var info = new DirectoryInfo(from);
            var files = info.GetFiles("*", SearchOption.TopDirectoryOnly);
            var dir = info.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (var element in files)
            {
                var copyTo = $"{to}/{element.Name}";
                filesData[element.FullName] = copyTo;
            }

            foreach (var element in dir)
            {
                var copyDir = $"{to}/{element.Name}";
                GetCopyFolderManifest(element.FullName, copyDir, dirData, filesData);
            }
        }

        /// <summary>
        /// 加载编辑器图片
        /// </summary>
        public static Texture LoadEditorTexture(string textureName, ref Texture texture)
        {
            if (texture == null)
            {
                texture = AssetDatabase.LoadAssetAtPath<Texture>(
                    $"Assets/XGameUnityTool/Editor/Texture/{textureName}.png");
            }

            return texture;
        }

        // [MenuItem("测试/测试-1")]
        private static void Test()
        {
            // var style = EditorStyles.toolbarSearchField;
            // var style2 = new GUIStyle(style);
            // Debug.Log(style2.ToXJson());
            //
            // var skin = GUI.skin;
            // Debug.Log(DateTime.Now.ToString("yyyyMMddHHmmss"));
            return;

            //XJson.FromJson<>()
            var version = new JsonUpdateVersion()
            {
                url = "aaaa",
                version = "1.0.1",
                versionCode = 100001,
            };

            Debug.Log(XJson.ToJson(version));
            var json = @"{""url"":""11123333444""}";
            Debug.Log(XJson.ToJson(XJson.FromJson<JsonUpdateVersion>(json)));


            // var type = GetType("Newtonsoft.Json.JsonConvert");
            // MethodInfo _methodToJson = type.GetMethod("SerializeObject", new[] { typeof(object) });
            // MethodInfo _methodFromJson = type.GetMethod("DeserializeObject", new[] { typeof(string), typeof(Type) });
            // Debug.Log($"{_methodFromJson} {_methodToJson}");
        }


        #region 宏定义引入

        public static void AddScriptingDefine(params string[] DEFINES)
        {
            var actTarget = EditorUserBuildSettings.activeBuildTarget;
            var currentTarget = BuildPipeline.GetBuildTargetGroup(actTarget);
            Debug.Log("AddScriptingDefine currentTarget=" + currentTarget);
            if (currentTarget == BuildTargetGroup.Unknown)
            {
                return;
            }

            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            var defines = definesString.Split(';');

            bool changed = false;

            foreach (var define in DEFINES)
            {
                if (defines.Contains(define) == false)
                {
                    if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
                    {
                        definesString += ";";
                    }

                    definesString += define;
                    changed = true;
                }
            }

            if (changed)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
                foreach (var element in DEFINES)
                {
                    Debug.Log($"<color=#51d364>添加宏定义：{element}</color>");
                }
            }
        }

        public static void ClearScriptingDefine(params string[] DEFINES)
        {
            var actTarget = EditorUserBuildSettings.activeBuildTarget;
            var currentTarget = BuildPipeline.GetBuildTargetGroup(actTarget);
            Debug.Log("ClearScriptingDefine currentTarget=" + currentTarget);
            if (currentTarget == BuildTargetGroup.Unknown)
            {
                return;
            }

            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            var defines = definesString.Split(';');
            var current = defines.Where(e => !string.IsNullOrEmpty(e));
            var changed = current.Any(DEFINES.Contains);

            if (changed)
            {
                var removeDefines = new List<string>();
                definesString = string.Join(";", current.Where(e =>
                {
                    var s = !DEFINES.Contains(e);
                    if (!s)
                    {
                        removeDefines.Add(e);
                    }

                    return s;
                }));

                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);

                foreach (var removeDefine in removeDefines)
                {
                    Debug.Log($"<color=#51d364>移除宏定义：{removeDefine}</color>");
                }
            }
        }

        #endregion

        #region 日志输出

        public static void Log(string content, LogColor color)
        {
            switch (color)
            {
                case LogColor.Normal:
                    Debug.Log($"<color=#2fc1df>{content}</color>");
                    break;
                case LogColor.Warn:
                    Debug.Log($"<color=#fbc21c>{content}</color>");
                    break;
                case LogColor.Success:
                    Debug.Log($"<color=#51d25f>{content}</color>");
                    break;
                case LogColor.Error:
                    Debug.Log($"<color=#e74032>{content}</color>");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        #endregion


        public static string ToJsonIgnoreNull(object obj)
        {
            if (null == obj)
            {
                return null;
            }

            //反射获取JsonSerializerSettings
            var typeOfJsonSerializerSettings = GetType("Newtonsoft.Json.JsonSerializerSettings");
            //反射获取JsonConvert
            var typeOfJsonConvert = GetType("Newtonsoft.Json.JsonConvert");
            var typeOfNullValueHandling = GetType("Newtonsoft.Json.NullValueHandling");
            if (typeOfJsonConvert == null || typeOfJsonSerializerSettings == null ||
                typeOfNullValueHandling == null)
            {
                throw new Exception(
                    "The Newtonsoft.Json module is missing. Please install it from the XGameUnityTool menu and try again");
            }

            dynamic enumVal = Enum.Parse(typeOfNullValueHandling, "Ignore");
            // object enumValue = Convert.ChangeType(obj, typeOfNullValueHandling);
            dynamic settingInstance = Activator.CreateInstance(typeOfJsonSerializerSettings);
            settingInstance.NullValueHandling = enumVal;
            var methodOfSerializeObject = typeOfJsonConvert.GetMethod("SerializeObject",
                new[] { typeof(object), typeOfJsonSerializerSettings });
            Debug.Log(methodOfSerializeObject);
            dynamic json = methodOfSerializeObject.Invoke(null,
                new object[] { obj, settingInstance });
            return json;
        }
    }
}