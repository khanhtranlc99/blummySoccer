using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XGame.BuildApp;

namespace XGame
{
    //导出清单
    [Serializable]
    public class PlayAssetDeliveryModules
    {
        public PlayAssetDeliveryExportModule[] modules;

        public PlayAssetDeliveryModules(PlayAssetDeliveryExportModule[] modules)
        {
            this.modules = modules;
        }
    }

    [Serializable]
    public class PlayAssetDeliveryExportModule
    {
        public List<PlayAssetDeliveryFilePath> assets = new List<PlayAssetDeliveryFilePath>(); //资源文件
        public string name;                                                                    //分包名
        public string mode;                                                                    //分包模式

        public PlayAssetDeliveryExportModule(string name, PlayAssetDeliveryMode mode)
        {
            this.name = name;
            switch (mode)
            {
                case PlayAssetDeliveryMode.InstallTime:
                    this.mode = "install-time";
                    break;
                case PlayAssetDeliveryMode.FastFollow:
                    this.mode = "fast-follow";
                    break;
                case PlayAssetDeliveryMode.OnDemand:
                    this.mode = "on-demand";
                    break;
                default:
                    throw new Exception($"无法解析模式：{mode}");
            }
        }

        public void AddAsset(PlayAssetDeliveryFilePath filePath)
        {
            assets.Add(filePath);
        }

        public void AddAsset(string path, bool isFolder)
        {
            assets.Add(new PlayAssetDeliveryFilePath(path, isFolder));
        }
    }

    [Serializable]
    public class PlayAssetDeliveryFilePath
    {
        public string path;
        public bool is_folder;

        public PlayAssetDeliveryFilePath(string path, bool isFolder)
        {
            this.path = path;
            is_folder = isFolder;
        }
    }

    public class BuildPlayAssetDeliverySource
    {
        public const string PLAY_ASSET_DELIVERY_LOAD_PATH = "PlayAssetDeliveryLoadPath";
        public const string PLAY_ASSET_DELIVERY_BUILD_PATH = "PlayAssetDeliveryBuildPath";

        //导出路径
        protected class PadExportPath
        {
            //文件路径
            public string OriginalPath;

            //导出的相对路径
            public string RelativePath;

            //aa GROUP名
            public string Group;

            public PlayAssetDeliveryMode Mode;

            public PadExportPath(string originalPath, string relativePath, string group, PlayAssetDeliveryMode mode)
            {
                OriginalPath = originalPath;
                RelativePath = relativePath;
                Group = group;
                Mode = mode;
            }

            public string GetModuleName()
            {
                return ChangePackName(Group);
            }
            
            private static string ChangePackName(string packName)
            {
                var name = "";
                if (string.IsNullOrEmpty(packName)) return name;
                const string start = "pad_";
                name = packName.Replace(" ", "").ToLower();
                while (name.StartsWith(start))
                {
                    name = name.Remove(0, start.Length);
                }
                name = start + name;

                return name;

            }
            
        }

        public void Execute(dynamic result, string folder, List<PlayAssetDeliveryOption> options)
        {
            if (!string.IsNullOrEmpty(result.Error))
            {
                //发生错误
                Debug.LogError("pad分包停止，构建aa错误信息："+result.Error);
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                XGameEditorUtil.ShowMessageBox("PAD split failed: export path is missing.");
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
                Debug.LogError("XMY: 开启了pad分包，但是没找到配置！！");
                return;
            }

            var exportFiles = new List<PadExportPath>();
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

            var modules = new Dictionary<string, PlayAssetDeliveryExportModule>();
            //进行分割
            foreach (var element in exportFiles)
            {
                var from = element.OriginalPath;
                var to = $"{folder}/{element.RelativePath}";
                var moduleName = element.GetModuleName();
                if (Directory.Exists(from))
                {
                    var parent = Path.GetDirectoryName(to);
                    XGameEditorUtil.CheckCreateFolder(parent);
                    Directory.Move(from, to);
                    if (!modules.ContainsKey(moduleName))
                    {
                        modules[moduleName] = new PlayAssetDeliveryExportModule(moduleName, element.Mode);
                    }

                    modules[moduleName].AddAsset(element.RelativePath, true);
                }

                if (File.Exists(from))
                {
                    var dir = Path.GetDirectoryName(to);
                    XGameEditorUtil.CheckCreateFolder(dir);
                    File.Move(from, to);
                    if (!modules.ContainsKey(moduleName))
                    {
                        modules[moduleName] = new PlayAssetDeliveryExportModule(moduleName, element.Mode);
                    }

                    modules[moduleName].AddAsset(element.RelativePath, false);
                }
            }

            var padModules = new PlayAssetDeliveryModules(modules.Values.ToArray());
            var json = JsonUtility.ToJson(padModules);
            var jsonPath = $"{folder}/option.json";
            File.WriteAllText(jsonPath, json);
            if (Directory.Exists(folder))
            {
                //组合XMY分包资源
                Debug.Log("生成XMY PlayAssetDeliver分包资源");
                var combine = new CombineXMYPlayAssetDeliverSource();
                combine.Execute(folder);
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
            
        }

        private static List<PadExportPath> CalculateGroupSplitFiles(PlayAssetDeliveryOption option, List<string> files)
        {
            var typeOfSchema = AddressableReflection.GetType(AddressableReflection.TYPE_NAME_BundledAssetGroupSchema);
            //导出目录[文件完成路径,导出相对路径]
            var result = new List<PadExportPath>();
            var match = new List<string>();
            if (option.AssetGroup != null)
            {
                dynamic group = (option.AssetGroup); //as AddressableAssetGroup;
                //group.GetSchema()
                dynamic schema = group.GetSchema(typeOfSchema); // as <BundledAssetGroupSchema>();
                //解析导出路径
                //构建路径
                var buildPath = schema.BuildPath.GetValue(group.Settings);
                //加载路径
                var loadPath = schema.LoadPath.GetValue(group.Settings);
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
                    result.Add(new PadExportPath(element, relativePath, option.AssetGroup.name, option.Mode));
                }
            }

            return result;
        }


        public static void Repair(List<PlayAssetDeliveryOption> options)
        {
            RepairVariable();
            RepairPlayAssetDeliveryAssetGroups(options);
        }


        private static void RepairVariable()
        {
            //创建PlayAssetDeliveryLoadPath参数
            var loadPath = "{XGame.PlayAssetDeliveryPaths.RuntimePath}/[BuildTarget]";
            BuildAddressableAssetTool.WriteVariableToCurrentProfile(PLAY_ASSET_DELIVERY_LOAD_PATH, loadPath);

            //buildpath
            var buildPath = "[XGame.PlayAssetDeliveryPaths.BuildPath]/[BuildTarget]";
            BuildAddressableAssetTool.WriteVariableToCurrentProfile(PLAY_ASSET_DELIVERY_BUILD_PATH, buildPath);
        }

        private static void RepairPlayAssetDeliveryAssetGroups(List<PlayAssetDeliveryOption> options)
        {
            foreach (var option in options)
            {
                if (option.AssetGroup == null)
                {
                    continue;
                }

                if (option.Mode == PlayAssetDeliveryMode.InstallTime)
                {
                    BuildAddressableAssetTool.SetBundledAssetGroupSchemaLoadPathVariable(option.AssetGroup,
                        "LocalLoadPath");
                    BuildAddressableAssetTool.SetBundledAssetGroupSchemaBuildPathVariable(option.AssetGroup,
                        "LocalBuildPath");
                    continue;
                }

                BuildAddressableAssetTool.SetBundledAssetGroupSchemaLoadPathVariable(option.AssetGroup,
                    PLAY_ASSET_DELIVERY_LOAD_PATH);
                BuildAddressableAssetTool.SetBundledAssetGroupSchemaBuildPathVariable(option.AssetGroup,
                    PLAY_ASSET_DELIVERY_BUILD_PATH);
            }

            EditorUtility.SetDirty(AddressableReflection.Settings);
        }
    }
}