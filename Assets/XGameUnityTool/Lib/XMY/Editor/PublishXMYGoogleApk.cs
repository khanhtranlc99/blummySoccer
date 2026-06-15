using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;
using XGame.BuildApp;


namespace XGame
{
    public class PublishXMYGoogleApk : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 0;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            Debug.Log("PublishXMYGoogleApk.OnPostGenerateGradleAndroidProject at path " + path);

            var currentChannel = EditorPreference.Global.currentChannel;
            var currentPublishMode = EditorPreference.Global.currentPublishMode;
            var isAddTestAdToXmyApk = EditorPreference.Global.isAddTestAdToXmyApk;

            Debug.Log("PublishXMYGoogleApk.OnPostGenerateGradleAndroidProject currentChannel=" + currentChannel);
            Debug.Log("PublishXMYGoogleApk.OnPostGenerateGradleAndroidProject currentPublishMode=" +
                      currentPublishMode);
            Debug.Log("PublishXMYGoogleApk.OnPostGenerateGradleAndroidProject isAddTestAdToXmyApk=" +
                      isAddTestAdToXmyApk);

            const string form = "Assets/XGameUnityTool/Lib/XMY/Editor/EasyTestAdHelper-release.aar";

            var addCode = "\n    implementation(name: 'EasyTestAdHelper-release', ext:'aar')";
            addCode += "\n    implementation 'androidx.constraintlayout:constraintlayout:2.1.4'";
            addCode += "\n    implementation 'com.google.android.gms:play-services-ads-lite:23.6.0'";
            addCode += "\n";

            if (currentChannel == AppChannel.XMYGoogle &&
                currentPublishMode == PublishMode.APK &&
                isAddTestAdToXmyApk)
            {
                if (File.Exists(form))
                {
                    var config = Path.Combine(path, "build.gradle");
                    var to = Path.Combine(path, "libs", Path.GetFileName(form));
                    if (File.Exists(config) && Directory.Exists(Path.GetDirectoryName(to)))
                    {
                        var code = File.ReadAllText(config);

                        if (!code.Contains(addCode))
                        {
                            var position = "dependencies {";
                            var index = code.IndexOf(position, StringComparison.Ordinal);

                            if (index != -1)
                            {
                                File.Copy(form, to, true);
                                code = code.Insert(
                                    index + position.Length,
                                    addCode
                                );

                                File.WriteAllText(config, code);
                            }
                        }
                    }
                }
            }
            else
            {
                var to = Path.Combine(path, "libs", Path.GetFileName(form));
                var config = Path.Combine(path, "build.gradle");

                if (File.Exists(to))
                {
                    File.Delete(to);
                }

                if (File.Exists(config))
                {
                    var code = File.ReadAllText(config);
                    if (code.Contains(addCode))
                    {
                        code = code.Replace(addCode, "");
                        File.WriteAllText(config, code);
                    }
                }
            }
        }

        public void Publish(XGameAndroidAppSetting setting)
        {
            if (setting.isOutputAar)
            {
                PreprocessBuild();
            }

            PublishAndroid.SwitchToApk();
            
            //不能重新构建AA资源
            UpdateBuildAddressablesWithPlayerBuild();
            
            var path =
                $"XGameOutPut/XMY/{setting.name}_{DateTimeNumberString}.apk";
            if (!string.IsNullOrWhiteSpace(setting.PublishName))
            {
                path = $"XGameOutPut/XMY/{setting.PublishName}_{DateTimeNumberString}.apk";
            }

            var s = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.Android,
                setting.BuildOptions);

            //打开文件
            if (File.Exists(path))
            {
                if (setting.isOutputAar)
                {
                    PostprocessBuild(setting, path);
                }

                EditorUtility.RevealInFinder(path);
                Debug.Log($"发布完成 {path}");
                //触发完成回调！
                var result = new PublishResult();
                result.AppSetting = setting;
                result.ApkPath = path;
                setting?.InvokePublishComplete(result);
            }
        }


        private void UpdateBuildAddressablesWithPlayerBuild()
        {
#if UNITY_2021_2_OR_NEWER
            //UNITY_2021_2_OR_NEWER之后版本，当player build时设置为不重新构建AA资源
            //生成aar时不能重新构建AA资源

            var settingsAsset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
            var addressableAssetSettings = "UnityEditor.AddressableAssets.Settings.AddressableAssetSettings";
            var playerBuildOption = "UnityEditor.AddressableAssets.Settings.AddressableAssetSettings+PlayerBuildOption";
            var option = XGameEditorUtil.GetType(playerBuildOption);
            if (null == option)
            {
                Debug.Log(playerBuildOption + "不存在");
                return;
            }

            var ss = XGameEditorUtil.GetType(addressableAssetSettings);
            if (null == ss)
            {
                Debug.Log(addressableAssetSettings + "不存在");
                return;
            }

            // dynamic settings = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settingsAsset) as AddressableAssetSettings;
            dynamic settings = AssetDatabase.LoadAssetAtPath(settingsAsset, ss);
            if (settings == null)
            {
                Debug.Log($"{settingsAsset} couldn't be found or isn't " +
                          $"a settings object.");
                return;
            }


            FieldInfo[] fieldInfos = option.GetFields(BindingFlags.Public | BindingFlags.Static);
            dynamic playerBuild = null;


            // 遍历所有枚举成员
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                // 将枚举成员名称转换为枚举值
                var value = Enum.Parse(option, fieldInfo.Name);

                // 输出枚举值
                // Debug.Log($"Name: {fieldInfo.Name}, Value: {value}");
                if (fieldInfo.Name == "DoNotBuildWithPlayer")
                {
                    playerBuild = value;
                }
            }

            if (null == playerBuild)
            {
                Debug.Log($"反射获取PlayerBuildOption枚举失败。");
                return;
            }


            Debug.Log("BuildAddressablesWithPlayerBuild = " + playerBuild);
            // settings.BuildAddressablesWithPlayerBuild = AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer;
            settings.BuildAddressablesWithPlayerBuild = playerBuild;

#endif
        }

        private string GetAarPath()
        {
            var dataPath = Application.dataPath;
            // 使用Path.GetDirectoryName获取Assets的上级目录（项目根目录）
            var projectRootPath = Path.GetDirectoryName(dataPath);
            // 组合出Library目录的完整路径
            var libraryPath = Path.Combine(projectRootPath, "Library");
            return Path.Combine(libraryPath, "Bee", "Android", "Prj", "IL2CPP", "Gradle", "unityLibrary", "build",
                "intermediates", "local_aar_for_lint", "release", "out.aar");
        }


        private void PreprocessBuild()
        {
            var aar = GetAarPath();
            if (!string.IsNullOrEmpty(aar) && File.Exists(aar))
            {
                Debug.Log("删除旧out.aar");
                File.Delete(aar);
                var aarZip = Path.ChangeExtension(aar, ".zip");
                if (File.Exists(aarZip))
                {
                    File.Delete(aarZip);
                }
            }
        }


        // APK 构建完成后触发
        private void PostprocessBuild(XGameAndroidAppSetting setting, string outputPath)
        {
            var aar = GetAarPath();
            if (!string.IsNullOrEmpty(aar) && File.Exists(aar))
            {
                Debug.Log("PostprocessBuild 开始导出aar");
                var aarZip = Path.ChangeExtension(aar, ".zip");
                if (File.Exists(aarZip))
                {
                    File.Delete(aarZip);
                }

                File.Copy(aar, aarZip);

                var dir = Path.GetDirectoryName(outputPath);
                // Debug.Log("outputPath GetDirectoryName="+dir);

                // var fileNameWithoutExtension =
                //     Path.GetFileNameWithoutExtension(outputPath).Replace("_APK包", "");
                var tempDir = Path.Combine(dir, "_Temp");
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                else
                {
                    XGameEditorUtil.DeleteFolder(tempDir);
                    if (!Directory.Exists(tempDir))
                    {
                        Directory.CreateDirectory(tempDir);
                    }
                }
                // Debug.Log("tempDir="+tempDir);


                var fileName = Path.GetFileName(outputPath).Replace("_APK包", "");
                fileName = Path.ChangeExtension(fileName, ".aar");
                var outputAarPath = Path.Combine(dir, fileName);

                Debug.Log($"PostprocessBuild outputAarPath={outputAarPath}");

                ZipTool.UnZip(aarZip, tempDir, () => { Debug.Log("PostprocessBuild 解压out.aar完成"); });

                Debug.Log("开始压缩生成母包aar");
                var fds = new Dictionary<string, string>();
                var files = Directory.GetFiles(tempDir, "*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (Path.GetFileName(file) == "R.jar")
                    {
                        continue;
                    }

                    // Debug.Log("aar里的文件="+file);
                    fds.Add(file, Path.GetRelativePath(tempDir, Path.GetDirectoryName(file)));
                }

                ZipTool.Zip(outputAarPath, fds);

                if (Directory.Exists(tempDir))
                {
                    // Debug.Log($"开始删除 tempDir={tempDir}");
                    XGameEditorUtil.DeleteFolder(tempDir);
                }

                AssemblyPackage(setting, outputAarPath);
            }
            else
            {
                Debug.Log($"PostprocessBuild {aar} 不存在！");
            }
        }

        private static string DateTimeNumberString => XGameEditorUtil.DateTimeToPublishSuffixString();


        private void AssemblyPackage(XGameAndroidAppSetting setting, string aarExportPath)
        {
            Debug.Log("开始 AssemblyPackage aarExportPath=" + aarExportPath);

            if (!File.Exists(aarExportPath))
            {
                Debug.Log("AssemblyPackage aarExportPath is not exists");
                return;
            }

            var aarExportDir = Path.GetDirectoryName(aarExportPath);
            var aarFileName = Path.GetFileNameWithoutExtension(aarExportPath);

            var tempDir = Path.Combine(aarExportDir, "__tempDirXMY");

            var assemblyPackageList = new Dictionary<string, string>();

            var googleServicesJson = ToolPreference.Global.XMYGoogleServicesJsonPath;
            if (!string.IsNullOrEmpty(googleServicesJson))
            {
                if (File.Exists(googleServicesJson))
                {
                    if (!Directory.Exists(tempDir))
                    {
                        Directory.CreateDirectory(tempDir);
                    }

                    var tempGoogleServices = Path.Combine(tempDir, "google-services.json");
                    File.WriteAllText(tempGoogleServices, File.ReadAllText(googleServicesJson));
                    assemblyPackageList.Add(tempGoogleServices, "game");
                }
                else
                {
                    Debug.LogError("XMYGoogleServicesJsonPath 发现无效google-services.json文件路径");
                }
            }

            var resourceFilePaths = ToolPreference.Global.XMYResourceFilePaths;
            if (null != resourceFilePaths && resourceFilePaths.Count > 0)
            {
                foreach (var filePath in resourceFilePaths)
                {
                    if (filePath.Length > 0 && File.Exists(filePath) && (Path.GetExtension(filePath) == ".jar" ||
                                                                         Path.GetExtension(filePath) == ".aar"))
                    {
                        assemblyPackageList.TryAdd(filePath, Path.Combine("game", "libs"));
                    }
                    else
                    {
                        Debug.Log($"XMYResourceFilePaths 发现无效路径：{filePath}");
                    }
                }
            }

            var resourceFolderPaths = ToolPreference.Global.XMYResourceFolderPaths;
            if (null != resourceFolderPaths && resourceFolderPaths.Count > 0)
            {
                foreach (var folderPath in resourceFolderPaths)
                {
                    if (folderPath.Length > 0 && Directory.Exists(folderPath))
                    {
                        var folderName = Path.GetFileName(folderPath);
                        if (folderName == "res" || folderName == "assets")
                        {
                            assemblyPackageList.TryAdd(folderPath,Path.Combine("game", folderName));
                        }
                        else
                        {
                            Debug.Log($"XMYResourceFolderPaths 发现无效目录名：{folderPath}, {folderName}");
                        }
                        
                    }
                    else
                    {
                        Debug.Log($"XMYResourceFolderPaths 发现无效目录：{folderPath}");
                    }
                }
            }
            

            string json = null;
            var configFileName = "sdk_pack_config.json";
            var sdkPackConfig = new PublishXMYGoogle.SdkPackConfig();

            CombineXMYSource.Features cacheFeatures = null;
            CombineXMYPlayAssetDeliverSource.Pads cachePads = null;

            if (setting.OpenSplitAAResByCountry && setting.SplitAaDataOption.Count > 0 &&
                !string.IsNullOrEmpty(setting.SplitAAExportPath) &&
                Directory.Exists(setting.SplitAAExportPath))
            {
                var xmyProj = Path.Combine(setting.SplitAAExportPath, "xmy_proj");
                if (Directory.Exists(xmyProj))
                {
                    var xmyProjFiles = Directory.GetFiles(xmyProj, "*", SearchOption.AllDirectories);
                    foreach (var countryXmyProjFile in xmyProjFiles)
                    {
                        if (Path.GetFileName(countryXmyProjFile) == configFileName)
                        {
                            cacheFeatures =
                                XJson.FromJson<CombineXMYSource.Features>(File.ReadAllText(countryXmyProjFile));
                            continue;
                        }

                        assemblyPackageList.Add(countryXmyProjFile,
                            Path.GetRelativePath(xmyProj, Path.GetDirectoryName(countryXmyProjFile)));
                    }
                }
            }

            if (setting.PlayAssetDeliveryIsEnable && setting.PlayAssetDeliveryOptions.Count > 0 &&
                !string.IsNullOrEmpty(setting.PlayAssetDeliveryExportPath) &&
                Directory.Exists(setting.PlayAssetDeliveryExportPath))
            {
                var xmyProj = Path.Combine(setting.PlayAssetDeliveryExportPath, "xmy_proj");
                if (Directory.Exists(xmyProj))
                {
                    var xmyProjFiles = Directory.GetFiles(xmyProj, "*", SearchOption.AllDirectories);
                    foreach (var countryXmyProjFile in xmyProjFiles)
                    {
                        if (Path.GetFileName(countryXmyProjFile) == configFileName)
                        {
                            cachePads = XJson.FromJson<CombineXMYPlayAssetDeliverSource.Pads>(
                                File.ReadAllText(countryXmyProjFile));
                            continue;
                        }

                        assemblyPackageList.Add(countryXmyProjFile,
                            Path.GetRelativePath(xmyProj, Path.GetDirectoryName(countryXmyProjFile)));
                    }
                }
            }

            if (null != cacheFeatures)
            {
                sdkPackConfig.features = cacheFeatures.features;
            }

            if (null != cachePads)
            {
                sdkPackConfig.pads = cachePads.pads;
            }

            var dependency = ToolPreference.Global.XMYGradleDependencyLibrary;
            if (null != dependency && dependency.Count > 0)
            {
                var g = new PublishXMYGoogle.SdkPackConfig.GradleDependency();
                var list = new List<PublishXMYGoogle.SdkPackConfig.GradleDependency.Library>();
                foreach (var lib in dependency)
                {
                    var d = lib.Trim();
                    if (d.Length <= 0) continue;
                    var l = new PublishXMYGoogle.SdkPackConfig.GradleDependency.Library
                    {
                        name = d
                    };
                    list.Add(l);
                }

                if (list.Count > 0)
                {
                    g.dependencies = list.ToArray();
                    sdkPackConfig.gradle = g;
                }
            }

            if (sdkPackConfig.features != null || sdkPackConfig.pads != null || sdkPackConfig.gradle != null)
            {
                json = XGameEditorUtil.ToJsonIgnoreNull(sdkPackConfig);
            }


            var writeTo = Path.Combine(tempDir, configFileName);
            if (!string.IsNullOrEmpty(json))
            {
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                File.WriteAllText(writeTo, json);

                assemblyPackageList.Add(writeTo, "");
            }


            if (assemblyPackageList.Count > 0)
            {
                assemblyPackageList.Add(aarExportPath, Path.Combine("game", "libs"));

                var assemblyPackagWriteTo = Path.Combine(aarExportDir, aarFileName + "_allPackage.zip");
                ZipTool.Zip(assemblyPackagWriteTo, assemblyPackageList);
            }

            if (Directory.Exists(tempDir))
            {
                XGameEditorUtil.DeleteFolder(tempDir);
            }
        }
    }
}