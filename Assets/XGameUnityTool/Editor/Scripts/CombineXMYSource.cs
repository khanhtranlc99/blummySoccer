using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 组合XMY AA 分包资源
    /// </summary>
    public class CombineXMYSource
    {
        [Serializable]
        public class Option
        {
            public List<Module> modules;
        }

        [Serializable]
        public class Module
        {
            public AssetItem[] assets;
            public string[] countries;
            public string name;
            public bool include;
            public string group;
        }

        [Serializable]
        public class AssetItem
        {
            public string path;
            public bool is_folder;
        }

        [Serializable]
        public class Features
        {
            public Feature[] features;

            public Features(Feature[] features)
            {
                this.features = features;
            }
        }

        [Serializable]
        public class Feature
        {
            public string type = "install_time_with_condition_countries";
            public List<FeatureModuleItem> modules = new List<FeatureModuleItem>();
        }

        [Serializable]
        public class FeatureModuleItem
        {
            public string name;
            public string[] include = null;
            public string[] exclude = null;

            public FeatureModuleItem(string name, string[] include, string[] exclude)
            {
                this.name = name;
                this.include = include;
                this.exclude = exclude;
            }
        }


        public void Execute(string folder)
        {
            //分包导出目录
            // var folder = $"countryAA";
            var optionFile = $"{folder}/option.json";
            if (!File.Exists(optionFile))
            {
                throw new Exception($"找不到：{optionFile}");
            }

            Option option = null;
            //解析json
            try
            {
                var optionJson = File.ReadAllText(optionFile);
                option = XJson.FromJson<Option>(optionJson);
            }
            catch (Exception e)
            {
                throw new Exception($"解析：{optionFile}失败");
            }

            //生成资源清单
            if (option == null)
            {
                throw new Exception($"option==null");
            }

            //创建xmy_proj
            var xmy_proj = $"{folder}/xmy_proj";
            if (Directory.Exists(xmy_proj))
            {
                Directory.Delete(xmy_proj, true);
            }

            Directory.CreateDirectory(xmy_proj);
            var modules = option.modules;
            if (modules != null)
            {
                var feature = new Feature();
                //按name分组
                var group = modules.GroupBy(e => e.name);
                foreach (var item in group)
                {
                    var name = item.Key; //key
                    var first = item.First();
                    var folderName = name;
                    //如果名字长度太长，改为guid做为文件夹名称
                    if (name.Length > 60)
                    {
                        Debug.Log("名字太长，重写为fmd5_+文件名MD5");
                        folderName = $"fmd5_{XGameEditorUtil.GetMD5WithString(name).ToLower()}";
                    }

                    var countries = first.countries.Select(e => e.ToLower()).ToArray();
                    //生成复制清单
                    var moduleItem = new FeatureModuleItem(folderName, first.include ? countries : null,
                        first.include ? null : countries);
                    feature.modules.Add(moduleItem);
                    //创建模块文件夹
                    var moduleFolder = $"{xmy_proj}/{folderName}";
                    Directory.CreateDirectory(moduleFolder);
                    foreach (var element in item)
                    {
                        foreach (var asset in element.assets)
                        {
                            if (asset.is_folder)
                            {
                                //复制文件夹
                                var from = $"{folder}/{asset.path}";
                                var to = $"{moduleFolder}/assets/{asset.path}";
                                
                                // XGameEditorUtil.CopyFolder(from, to);
                                Directory.Move(from,to);
                            }
                            else
                            {
                                //复制文件
                                var from = $"{folder}/{asset.path}";
                                var to = $"{moduleFolder}/assets/{asset.path}";
                                var dir = Path.GetDirectoryName(to);
                                if (!Directory.Exists(dir))
                                {
                                    Directory.CreateDirectory(dir);
                                }

                                // File.Copy(from, to, true);
                                File.Move(from,to);
                            }
                        }
                    }
                }
                
                //生成json
                string json = XGameEditorUtil.ToJsonIgnoreNull(new Features(new[] { feature }));
                
                var sdk_pack_config = $"{xmy_proj}/sdk_pack_config.json";
                File.WriteAllText(sdk_pack_config, json);
                if (Directory.Exists(xmy_proj))
                {
                    Debug.Log($"生成完毕：{xmy_proj}");
                    // EditorUtility.RevealInFinder(xmy_proj);
                }
            }
        }
        

        private static Type GetType(string fullName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(fullName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }
    }
}