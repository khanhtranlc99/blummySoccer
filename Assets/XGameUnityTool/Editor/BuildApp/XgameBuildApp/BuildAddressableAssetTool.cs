using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace XGame.BuildApp
{
    public class BuildAddressableAssetTool
    {
        //输出文件夹
        private const string VARIABLE_OUT_PUT_PATH = "OutPutPath";

        //远端路径
        private const string REMOTE_BUILD_PATH = "RemoteBuildPath";

        [MenuItem("XGameUnityTool/Cleanup AA Resources", priority = 3)]
        private static void ClearAABuild()
        {
            if (!AddressableReflection.HasDefaultSettings())
            {
                return;
            }

            var settings = AddressableReflection.Settings;
            foreach (var builder in settings.DataBuilders)
            {
                Debug.Log(builder.Name);
                AddressableReflection.CallStaticMethod(AddressableReflection.TYPE_NAME_AddressableAssetSettings,
                    "CleanPlayerContent", BindingFlags.Static | BindingFlags.Public,
                    new[] { builder });
            }

            // //清理构建
            // AddressableAssetSettings.CleanPlayerContent(
            //     AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        }

        [MenuItem("XGameUnityTool/Build AA Resources", priority = 3)]
        private static void BuildAA()
        {
            BuildAA(false, null, null, false, null, null, null);
        }

        private static void BuildAA(bool splitAA, string folder, List<SplitAAOption> options, bool enablePad,
            string padFolder, List<PlayAssetDeliveryOption> padOptions, BuildAppSetting setting)
        {
            if (!AddressableReflection.HasDefaultSettings())
            {
                return;
            }

            var isPublishXMYGoogle = false;
            var androidSetting = setting as XGameAndroidAppSetting;
            if (null != androidSetting)
            {
                isPublishXMYGoogle = androidSetting.IsPublishXMYGoogle;
            }

            if (isPublishXMYGoogle)
            {
                if (splitAA && enablePad && folder == padFolder)
                {
                    XGameEditorUtil.ShowMessageBox("Addressables build failed: country-split export path cannot be the same as the PAD export path.");
                    return;
                }

                if (enablePad)
                {
                    Debug.Log("调整PlayAssetDelivery分包 AssetGroup资源设置");
                    BuildPlayAssetDeliverySource.Repair(padOptions);
                }

                if (null != options && options.Count() > 0)
                {
                    //判断国家分包的path是否是pad的路径，如果是就改回默认值，防止接入方配pad分包后再改为国家分包导致path是pad的路径
                    foreach (var splitAAOption in options)
                    {
                        resumeAAPath(splitAAOption.AssetGroup);
                    }
                    EditorUtility.SetDirty(AddressableReflection.Settings);
                }
            }

            
            //反射调用,构建AA
            var settings = AddressableReflection.Settings;
            var method = AddressableReflection.GetMethod(AddressableReflection.TYPE_NAME_AddressableAssetSettings,
                "BuildPlayerContent", BindingFlags.Static | BindingFlags.Public, 1);
            if (method != null)
            {
                var outResult = new object[] { null };
                method.Invoke(null, outResult);
                dynamic result = outResult[0];
                
                if (isPublishXMYGoogle && splitAA)
                {
                    //尝试拆分AA
                    SplitAA(result, folder, options);
                }

                if (isPublishXMYGoogle && enablePad)
                {
                    //尝试生成PAD分包
                    var builder = new BuildPlayAssetDeliverySource();
                    builder.Execute(result, padFolder, padOptions);
                }
            }
            else
            {
                Debug.Log(AddressableReflection.TYPE_NAME_AddressableAssetSettings+".BuildPlayerContent方法没找到！！");
            }
            
            //触发完成
            InvokeAABuildComplete(setting);
            
        }

        public static void resumeAAPath(dynamic assetGroup)
        {
            var typeOfSchema = AddressableReflection.GetType(AddressableReflection.TYPE_NAME_BundledAssetGroupSchema);
            dynamic group = (assetGroup); //as AddressableAssetGroup;
            //group.GetSchema()
            dynamic schema = group.GetSchema(typeOfSchema); // as <BundledAssetGroupSchema>();
                    
            var buildPathName = schema.BuildPath.GetName(group.Settings);
            var loadPathName = schema.LoadPath.GetName(group.Settings);
                    
            // Debug.Log($"国家分包的资源组={group.name}, buildPathName={buildPathName}, loadPathName={loadPathName}");
            if (buildPathName == BuildPlayAssetDeliverySource.PLAY_ASSET_DELIVERY_BUILD_PATH)
            {
                schema.BuildPath.SetVariableByName(group.Settings, "LocalBuildPath");
            }

            if (loadPathName == BuildPlayAssetDeliverySource.PLAY_ASSET_DELIVERY_LOAD_PATH)
            {
                schema.LoadPath.SetVariableByName(group.Settings, "LocalLoadPath");
            }
        }
        
        
        
        [MenuItem("XGameUnityTool/Cleanup AA Resources", true)]
        private static bool ClearAABuildToggle()
        {
            return AddressableReflection.HasModule();
        }


        [MenuItem("XGameUnityTool/Build AA Resources", true)]
        private static bool BuildAAToggle()
        {
            return AddressableReflection.HasModule();
        }


        [MenuItem("Window/Addressable Window", priority = 3)]
        private static void OpenAddressableAssetsWindow()
        {
            var type = AddressableReflection.GetType("UnityEditor.AddressableAssets.GUI.AddressableAssetsWindow");
            var window = UnityEditor.EditorWindow.GetWindow(type);
            window.titleContent = new GUIContent("Addressables Groups");
            window.Show();
        }


        [MenuItem("Window/Addressable Window", true)]
        private static bool OpenAddressableAssetsWindowToggle()
        {
            return AddressableReflection.HasModule();
        }

        /// <summary>
        /// 解析addressable指定配置参数最终路径
        /// </summary>
        public static string ParserAddressablePath(string profile, string variable)
        {
            try
            {
                var settings = AddressableReflection.Settings;
                var profileId = settings.profileSettings.GetProfileId(profile);
                return AnalysisAddressablePath(profileId, variable);
            }
            catch (Exception e)
            {
                Debug.Log($"解析失败，profile：{profile} variable:{variable} ");
                Console.WriteLine(e);
                throw;
            }
        }

        //解析路径
        private static string AnalysisAddressablePath(string profileId, string valueName)
        {
            var settings = AddressableReflection.Settings;
            //如果不存在
            CheckCreateVariable(valueName);
            dynamic value = settings.profileSettings.GetValueByName(profileId, valueName);
            dynamic path = settings.profileSettings.EvaluateString(profileId, value);
            return path;
        }

        public static void SetAddressableValueByName(string profileName, string variableName, string value)
        {
            var settings = AddressableReflection.Settings;
            var profileId = settings.profileSettings.GetProfileId(profileName);
            SetAddressableValue(profileId, variableName, value);
        }

        public static void SetAddressableValue(string profileId, string variableName, string value)
        {
            var settings = AddressableReflection.Settings;
            settings.profileSettings.SetValue(profileId, variableName, value);
        }


        public static void SetBundledAssetGroupSchemaLoadPathVariable(dynamic assetGroup, string variableName)
        {
            var settings = AddressableReflection.Settings;
            var groupType = AddressableReflection.TypeOfAddressableAssetGroup;
            var getSchemaMethod =
                groupType.GetMethods().First(e => e.Name == "GetSchema" && e.GetParameters().Length == 0);
            var schemaType = AddressableReflection.TypeOfBundledAssetGroupSchema;
            object[] parameters = new object[] { };
            var schema = getSchemaMethod.MakeGenericMethod(schemaType).Invoke(assetGroup, parameters);
            var profileValueReferenceType = AddressableReflection.TypeOfProfileValueReference;
            var loadPathPropertyInfo =
                schemaType.GetProperty("LoadPath", BindingFlags.Instance | BindingFlags.Public);
            var loadPath = loadPathPropertyInfo.GetValue(schema, null);
            var methodSetVariableByName = profileValueReferenceType.GetMethods()
                .First(e => e.Name == "SetVariableByName" && e.GetParameters().Length == 2);
            methodSetVariableByName.Invoke(loadPath, new object[] { settings, variableName });
        }


        public static void SetBundledAssetGroupSchemaBuildPathVariable(dynamic assetGroup, string variableName)
        {
            var settings = AddressableReflection.Settings;
            var groupType = AddressableReflection.TypeOfAddressableAssetGroup;
            var getSchemaMethod =
                groupType.GetMethods().First(e => e.Name == "GetSchema" && e.GetParameters().Length == 0);
            var schemaType = AddressableReflection.TypeOfBundledAssetGroupSchema;
            object[] parameters = new object[] { };
            var schema = getSchemaMethod.MakeGenericMethod(schemaType).Invoke(assetGroup, parameters);
            var profileValueReferenceType = AddressableReflection.TypeOfProfileValueReference;
            var loadPathPropertyInfo =
                schemaType.GetProperty("BuildPath", BindingFlags.Instance | BindingFlags.Public);
            var loadPath = loadPathPropertyInfo.GetValue(schema, null);
            var methodSetVariableByName = profileValueReferenceType.GetMethods()
                .First(e => e.Name == "SetVariableByName" && e.GetParameters().Length == 2);
            methodSetVariableByName.Invoke(loadPath, new object[] { settings, variableName });
        }
        

        public static void BuildExisting(string profileName, bool splitAA = false, string folder = null,
            List<SplitAAOption> list = null, bool enablePad = false, string padFolder = null,
            List<PlayAssetDeliveryOption> padOptions = null, BuildAppSetting setting = null)
        {
            if (!AddressableReflection.HasModule())
            {
                return;
            }

            if (!AddressableReflection.HasDefaultSettings())
            {
                Debug.LogError("未初始化 Addressable settings");
                return;
            }

            var settings = AddressableReflection.Settings;
            //切换对应配置
            settings.activeProfileId = settings.profileSettings.GetProfileId(profileName);
            //构建AA资源
            BuildAA(splitAA, folder, list, enablePad, padFolder, padOptions, setting);
        }

        //安装
        public static void CheckCreateVariable(string variableName)
        {
            //检车并创建属性
            var settings = AddressableReflection.Settings;
            dynamic variableNames = settings.profileSettings.GetVariableNames();
            if (!variableNames.Contains(variableName))
            {
                settings.profileSettings.CreateValue(variableName, string.Empty);
            }
        }

        public static void CheckCreateVariable(string variableName, string defaultValue)
        {
            var settings = AddressableReflection.Settings;
            dynamic variableNames = settings.profileSettings.GetVariableNames();
            if (!variableNames.Contains(variableName))
            {
                settings.profileSettings.CreateValue(variableName, defaultValue);
            }
        }

        public static void WriteVariableToCurrentProfile(string variableName, string variableValue)
        {
            var settings = AddressableReflection.Settings;
            CheckCreateVariable(variableName, variableValue);
            SetAddressableValue(settings.activeProfileId, variableName, variableValue);
        }


        //拆分aa//result:AddressablesPlayerBuildResult
        private static void SplitAA(dynamic result, string folder, List<SplitAAOption> options)
        {
            if (!string.IsNullOrEmpty(result.Error))
            {
                //发生错误
                Debug.LogError("国家分包停止，构建aa错误信息："+result.Error);
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                XGameEditorUtil.ShowMessageBox("Country/region AA split failed: export path is missing.");
                return;
            }

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            XGameEditorUtil.CheckCreateFolder(folder);
            if (options == null || options.Count <= 0)
            {
                //打开文件夹
                // EditorUtility.RevealInFinder(folder);
                Debug.LogError("XMY: 开启了国家分包，但是没找到配置！！");
                return;
            }

            var exportFiles = new List<ExportPath>();
            var fileRegistry = result.FileRegistry;
            var paths = new List<string>();
            foreach (var element in fileRegistry.GetFilePaths())
            {
                paths.Add(element);
            }

            //计算关联资源
            foreach (var element in options)
            {
                var match = CalculateGroupSplitFiles(element, paths);
                exportFiles.AddRange(match);
            }

            var modules = new Dictionary<string, JsonSplitModule>();
            //进行分割
            foreach (var element in exportFiles)
            {
                var from = element.OriginalPath;
                var to = $"{folder}/{element.RelativePath}";
                var moduleName = element.GetModuleName();
                if (Directory.Exists(from))
                {
                    // Debug.Log($"from:{from} to:{to}");
                    var parent = Path.GetDirectoryName(to);
                    XGameEditorUtil.CheckCreateFolder(parent);
                    Directory.Move(from, to);
                    if (!modules.ContainsKey(moduleName))
                    {
                        modules[moduleName] = new JsonSplitModule(moduleName, element.Countries, element.Include,
                            element.Group);
                    }

                    modules[moduleName].AddAsset(element.RelativePath, true);
                }

                if (File.Exists(from))
                {
                    var dir = Path.GetDirectoryName(to);
                    XGameEditorUtil.CheckCreateFolder(dir);
                    // Debug.Log($"from:{from}      to:{to}");
                    File.Move(from, to);
                    if (!modules.ContainsKey(moduleName))
                    {
                        modules[moduleName] = new JsonSplitModule(moduleName, element.Countries, element.Include,
                            element.Group);
                    }

                    modules[moduleName].AddAsset(element.RelativePath, false);
                }
            }

            var splitModules = new JsonSplitModules(modules.Values.ToArray());
            var json = JsonUtility.ToJson(splitModules);
            // var json = JsonMapper.ToJson(new JsonSplitModules(modules.Values.ToArray()));
            var jsonPath = $"{folder}/option.json";
            File.WriteAllText(jsonPath, json);
            // Debug.Log(json);
            if (Directory.Exists(folder))
            {
                //组合XMY分包资源
                CombineXMYSplitAASource(folder, splitModules);
            }

            // if (!File.Exists(jsonPath))
            // {
            //     FileInfo filinfo = new FileInfo(jsonPath);
            //     filinfo.Refresh();
            // }

            if (File.Exists(jsonPath))
            {
                // EditorUtility.RevealInFinder(jsonPath);
                File.Delete(jsonPath);
            }
            
            XGameEditorUtil.DeleteEmptyDirectories(folder);
            
            Debug.Log($"按国家/地区拆分aa资源 完毕! 导出目录:{Path.GetFullPath(folder)}");
        }
        
        
        //触发AA构建完毕
        public static void InvokeAABuildComplete(BuildAppSetting data)
        {
            Debug.Log($"构建完毕：{data} 想在AA构建完毕后做点事情？创建自定义类继承XGame.OnAddressableBuildComplete");
            var matchs = new List<Type>();
            var baseClass = typeof(OnAddressableBuildComplete);
            //反射
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && type != baseClass && type.IsSubclassOf(baseClass))
                    {
                        matchs.Add(type);
                    }
                }
            }

            //触发
            foreach (var match in matchs)
            {
                dynamic instance = Activator.CreateInstance(match);
                instance.OnComplete(data);
            }
        }


        //分析匹配的资源项
        private static List<ExportPath> CalculateGroupSplitFiles(SplitAAOption option, List<string> files)
        {
            var typeOfSchema = AddressableReflection.GetType(AddressableReflection.TYPE_NAME_BundledAssetGroupSchema);
            //导出目录[文件完成路径,导出相对路径]
            var result = new List<ExportPath>();
            var match = new List<string>();
            var countries = option.GetCountriesCodes();
            if (option.AssetGroup != null && countries != null && countries.Length > 0)
            {
                dynamic group = (option.AssetGroup); //as AddressableAssetGroup;
                //group.GetSchema()
                dynamic schema = group.GetSchema(typeOfSchema); // as <BundledAssetGroupSchema>();
                //解析导出路径
                //构建路径
                var buildPath = schema.BuildPath.GetValue(group.Settings);
                //加载路径
                var loadPath = schema.LoadPath.GetValue(group.Settings);
                //解析
                // Debug.Log($"构建:{buildPath} 加载:{loadPath}");
                //去除空格
                var groupName = group.Name.Replace(" ", "");
                groupName = groupName.ToLower();
                // var prefix = string.Empty;
                var prefixList = new List<string>();

                
                var onlyFolder = false;
                switch (schema.BundleMode.ToString())
                {
                    case "PackTogether":
                        // prefix = $"{groupName}_assets_all";
                        prefixList.Add($"{groupName}_assets_all");
                        prefixList.Add($"{groupName}_scenes_all");
                        break;
                    case "PackSeparately":
                        // prefix = $"{groupName}_assets_assets";
                        prefixList.Add($"{groupName}_assets_assets");
                        prefixList.Add($"{groupName}_scenes_assets");
                        onlyFolder = true;
                        break;
                    case "PackTogetherByLabel":
                        //获取标签
                        // prefix = $"{groupName}_assets";
                        prefixList.Add($"{groupName}_assets");
                        prefixList.Add($"{groupName}_scenes");
                        break;
                }

                if (onlyFolder)
                {
                    foreach (var prefix in prefixList)
                    {
                        var folder = $"{buildPath}/{prefix}";
                        //文件夹
                        if (Directory.Exists(folder))
                        {
                            var info = new DirectoryInfo(folder);
                            var isMatch = info.Name.StartsWith(prefix);
                            // Debug.Log($"匹配:{isMatch}    {folder}");
                            if (isMatch)
                            {
                                match.Add(folder);
                            }
                        }
                    }

                }
                else
                {
                    foreach (var prefix in prefixList)
                    {
                        //文件
                        foreach (var file in files)
                        {
                            if (File.Exists(file))
                            {
                                var info = new FileInfo(file);
                                // Debug.Log($"后缀:{info.Extension}");
                                var isMatch = info.Name.StartsWith(prefix) && info.Extension == ".bundle";
                                // Debug.Log($"匹配:{isMatch}    {file}");
                                if (isMatch)
                                {
                                    match.Add(file);
                                }
                            }
                        }
                    }

                }

                var loadPathPrefix = loadPath.Replace("{UnityEngine.AddressableAssets.Addressables.RuntimePath}", "aa");
                foreach (var element in match)
                {
                    var relativePath = element.Replace(buildPath, "");
                    relativePath = $"{loadPathPrefix}{relativePath}";
                    relativePath = relativePath.Replace('\\', '/');
                    //写入结果
                    // Debug.Log($"原路径:{element}   相对路径:{relativePath}");
                    result.Add(new ExportPath(element, relativePath, countries,
                        option.Mode == SplitAAIncludeType.Include, option.AssetGroup.name));
                }
            }

            return result;
        }
        
        //组合XMY分包资源
        private static void CombineXMYSplitAASource(string folder, JsonSplitModules splitModules)
        {
            Debug.Log("生成国家分包资源");
            var combineXmySource = new CombineXMYSource();
            combineXmySource.Execute(folder);
        }

        //导出路径
        protected struct ExportPath
        {
            //文件路径
            public string OriginalPath;

            //导出的相对路径
            public string RelativePath;

            public string[] Countries;

            //是否包含
            public bool Include;

            //aa GROUP名
            public string Group;

            public ExportPath(string originalPath, string relativePath, string[] countries, bool include, string group)
            {
                OriginalPath = originalPath;
                RelativePath = relativePath;
                Countries = countries;
                Include = include;
                Group = group;
            }

            public string GetModuleName()
            {
                var sb = new StringBuilder();
                var index = 0;
                foreach (var country in Countries)
                {
                    if (index > 0)
                    {
                        sb.Append("_");
                    }

                    sb.Append(country);
                    index++;
                }

                if (Include)
                {
                    return ($"include_{sb.ToString()}").ToLower();
                }
                else
                {
                    return ($"exclude_{sb.ToString()}").ToLower();
                }
            }
        }

        [Serializable]
        public class JsonSplitModules
        {
            public JsonSplitModule[] modules;

            public JsonSplitModules(JsonSplitModule[] module)
            {
                this.modules = module;
            }
        }

        [Serializable]
        public class JsonSplitModule
        {
            //路径
            public List<JsonFilePath> assets = new List<JsonFilePath>();
            public string[] countries;
            public string name;
            public bool include;
            public string group;

            public JsonSplitModule()
            {
            }

            public JsonSplitModule(string name, string[] countries, bool include, string group)
            {
                this.countries = countries;
                this.name = name;
                this.include = include;
                this.group = group;
            }

            public void AddAsset(JsonFilePath filePath)
            {
                assets.Add(filePath);
            }

            public void AddAsset(string path, bool isFolder)
            {
                assets.Add(new JsonFilePath(path, isFolder));
            }
        }

        [Serializable]
        public class JsonFilePath
        {
            public string path;
            public bool is_folder;

            public JsonFilePath(string path, bool isFolder)
            {
                this.path = path;
                is_folder = isFolder;
            }
        }
    }
}