using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 发布XMY Google
    /// </summary>
    public class PublishXMYGoogle
    {
        [Serializable]
        public class SdkPackConfig
        {
            [Serializable]
            public class GradleDependency
            {
                [Serializable]
                public class Library
                {
                    public string name;
                }
                
                public Library[] dependencies;
            }
            
            
            public CombineXMYSource.Feature[] features;
            public CombineXMYPlayAssetDeliverSource.Pad[] pads;
            public GradleDependency gradle;
        }

        public void Publish(XGameAndroidAppSetting setting, string name, string publishName)
        {
            PublishAAR(setting, name, publishName);
        }
        
        // 发布 aar
        private void PublishAAR(XGameAndroidAppSetting setting, string name, string publishName)
        {
            var dateTimeNumberString = XGameEditorUtil.DateTimeToPublishSuffixString();
            var path = $"XGameOutPut/xmy_google_proj_{dateTimeNumberString}";
            if (setting.IsOverBuild)
            {
                path = $"XGameOutPut/xmy_google_proj";
                var aarFile = $"{path}/unityLibrary/build/outputs/aar/unityLibrary-release.aar";
                //如果构建失败会使用旧包，所以这删除下
                if (File.Exists(aarFile))
                {
                    File.Delete(aarFile);
                }
            }
            
            var gradlewBatPath = XGameEditorUtil.GradlewBatPath();
            var gradleWrapperJarPath = XGameEditorUtil.GradleWrapperJarPath();
            
            if (string.IsNullOrEmpty(gradlewBatPath))
            {
                var error = $"找不到unity的 gradlew.bat";
                XGameEditorUtil.ShowMessageBox(error);
                return;
            }
            
            if (string.IsNullOrEmpty(gradleWrapperJarPath))
            {
                var error = $"找不到unity的 gradle-wrapper.jar";
                XGameEditorUtil.ShowMessageBox(error);
                return;
            }
            
            //生成aar时不能重新构建AA资源
            UpdateBuildAddressablesWithPlayerBuild();

            SwitchToProject();

            var exportPath = $"XGameOutPut/{name}_{dateTimeNumberString}.aar";
            if (!string.IsNullOrWhiteSpace(publishName))
            {
                exportPath = $"XGameOutPut/{publishName}_{dateTimeNumberString}.aar";
            }
            
            Debug.Log("构建目录："+path);
            var r = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, BuildTarget.Android,
                BuildOptions.None);
            
            //有些情况，不是构建成功状态也可以使用，所以注释下
            // if (r.summary.result != BuildResult.Succeeded)
            // {
            //     var s = $"构建android工程失败：{r.SummarizeErrors()}";
            //     Debug.LogError(s);
            //     return;
            // }

            if (!Directory.Exists(path))
            {
                var s = $"导出android工程失败：{path} 不存在！！";
                Debug.Log(s);
                return;
            }
            
            var toGradlewBatPath = Path.Combine(path, Path.GetFileName(gradlewBatPath));
            toGradlewBatPath = Path.GetFullPath(toGradlewBatPath);
            File.Copy(gradlewBatPath,toGradlewBatPath,true);
                    
            var toGradleWrapperJarPath = Path.Combine(path, "gradle", "wrapper", Path.GetFileName(gradleWrapperJarPath));
            toGradleWrapperJarPath = Path.GetFullPath(toGradleWrapperJarPath);
            File.Copy(gradleWrapperJarPath,toGradleWrapperJarPath,true);
            
            RunProjectToUnityLibAar(setting, path, exportPath);
            
            //触发完成回调
            var result = new PublishResult();
            result.AppSetting = setting;
            result.AndroidProjectPath = path;
            result.AARPath = exportPath;
            setting.InvokePublishComplete(result);
        }


        //导出aar
        private void RunProjectToUnityLibAar(XGameAndroidAppSetting setting, string projectPath, string exportPath)
        {
            
            var overrideJar = ToolPreference.Global.OverrideXMYUnityClassJar;
            var jarFile = ToolPreference.Global.XMYUnityClassPath;
            
            //更换自定义unity-classes.jar
            var dest = $"{projectPath}/unityLibrary/libs/unity-classes.jar";
            if (overrideJar && File.Exists(jarFile))
            {
                Debug.Log($"更换unity-classes.jar:{jarFile} ——> {dest}");
                File.Copy(jarFile, dest, true);
            }
            
            //生成批处理
            var bat = $"{projectPath}/buildaar.bat";
            var batTxt = $@"
call ""{Path.Combine("%cd%","gradlew.bat")}"" unityLibrary:assembleRelease --console=plain
if %errorlevel% equ 0 (
    echo Build completed successfully.
    exit
) else (
    echo Build encountered an error.
    pause
)";
            Debug.Log($"生成：{bat}");
            File.WriteAllText(bat, batTxt);

            //构建aar
            XGameEditorUtil.RunBat(bat);

            var aarFile = $"{projectPath}/unityLibrary/build/outputs/aar/unityLibrary-release.aar";
            Debug.Log($"检查out put:{aarFile}");

            if (!File.Exists(aarFile))
            {
                Debug.Log($"{aarFile}不存在！");
                return;
            }
            
            Debug.Log($"复制 {aarFile} ---> {exportPath}");
            File.Copy(aarFile, exportPath);

            //组装符号文件压缩包
            AssemblySymbols(setting, projectPath, exportPath);
                
            //组装包含国家或者pad分包的母包压缩包
            AssemblyPackage(setting, exportPath);

            if (!File.Exists(exportPath))
            {
                Debug.Log($"{exportPath}不存在！");
                return;
            }
            
            Debug.Log($"生成完毕:{exportPath}");
            EditorUtility.RevealInFinder(exportPath);
        }


        private void AssemblySymbols(XGameAndroidAppSetting setting, string projectPath, string aarExportPath)
        {

#if UNITY_2021_3_OR_NEWER
            if (setting.IsCreateSymbols == AndroidCreateSymbols.Disabled)
            {
                Debug.Log("AssemblySymbols closed");
                return;
            }
#else
            if (!setting.IsCreateSymbols) {
                Debug.Log("AssemblySymbols closed");
                return;
            }
#endif
            

            
            if (!File.Exists(aarExportPath))
            {
                Debug.Log("AssemblySymbols aarExportPath is not exists");
                return;
            }
            
            //如果有，就生成符号文件压缩包
            var exportAarFileName = Path.GetFileNameWithoutExtension(aarExportPath);
            var exportSymbolsFilePath = Path.Combine(Path.GetDirectoryName(aarExportPath),
                exportAarFileName + "_symbols.zip");
            var symbolsDir = Path.Combine(projectPath, "unityLibrary", "symbols");
            var symbolsFiles = Directory.GetFiles(symbolsDir, "*", SearchOption.AllDirectories);
            // Debug.Log($"exportAarFileName={exportAarFileName}");
            // Debug.Log($"exportSymbolsFilePath={exportSymbolsFilePath}");
            // Debug.Log($"symbolsDir={symbolsDir}");
            // Debug.Log($"symbolsFiles={symbolsFiles.ToXJson()}");
            if (Directory.Exists(symbolsDir) && symbolsFiles.Length > 0)
            {
                var f = new Dictionary<string, string>();
                foreach (var symbolsFile in symbolsFiles)
                {
                    var ss = Path.GetRelativePath(symbolsDir, Path.GetDirectoryName(symbolsFile));
                    f.Add(symbolsFile,ss);
                }
                ZipTool.Zip(exportSymbolsFilePath, f);
            }
        }


        private void AssemblyPackage(XGameAndroidAppSetting setting, string aarExportPath)
        {
            
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
                    var tempGoogleServices = Path.Combine(tempDir,"google-services.json");
                    File.WriteAllText(tempGoogleServices,File.ReadAllText(googleServicesJson));
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
                    if (filePath.Length > 0 && File.Exists(filePath) && (Path.GetExtension(filePath) == ".jar" || Path.GetExtension(filePath) == ".aar"))
                    {
                        assemblyPackageList.TryAdd(filePath, Path.Combine("game", "libs"));
                    }
                    else
                    {
                        Debug.LogError($"XMYResourceFilePaths 发现无效路径：{filePath}");
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
            var sdkPackConfig = new SdkPackConfig();
            CombineXMYSource.Features cacheFeatures = null;
            CombineXMYPlayAssetDeliverSource.Pads cachePads = null;

            if (setting.OpenSplitAAResByCountry && setting.SplitAaDataOption.Count > 0 && !string.IsNullOrEmpty(setting.SplitAAExportPath) &&
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

                        assemblyPackageList.Add(countryXmyProjFile, Path.GetRelativePath(xmyProj, Path.GetDirectoryName(countryXmyProjFile)));
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

                        assemblyPackageList.Add(countryXmyProjFile, Path.GetRelativePath(xmyProj, Path.GetDirectoryName(countryXmyProjFile)));
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
                var g = new SdkPackConfig.GradleDependency();
                var list =new List<SdkPackConfig.GradleDependency.Library>();
                foreach (var lib in dependency)
                {
                    var d = lib.Trim();
                    if (d.Length <= 0) continue;
                    var l = new SdkPackConfig.GradleDependency.Library
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

        
        private void SwitchToProject()
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
#if UNITY_2019_1_OR_NEWER
            EditorUserBuildSettings.buildAppBundle = false;
#endif
        }
    }
}