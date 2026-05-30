using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace XGame
{
    public class CombineXMYPlayAssetDeliverSource
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
            public string name;
            public string mode;
        }

        [Serializable]
        public class AssetItem
        {
            public string path;
            public bool is_folder;
        }

        [Serializable]
        public class Pads
        {
            public Pad[] pads;

            public Pads(Pad[] pads)
            {
                this.pads = pads;
            }
        }

        [Serializable]
        public class Pad
        {
            public string name;
            public string type;

            public Pad(string name, string type)
            {
                this.name = name;
                this.type = type;
            }
        }


        public void Execute(string folder)
        {
            //分包导出目录
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
                // optionJson = optionJson.Replace("{XGame.PlayAssetDeliveryPaths.RuntimePath}", "aa");
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
                var pads = new List<Pad>();
                foreach (var module in modules)
                {
                    var folderName = module.name;
                    var type = module.mode;
                    var pad = new Pad(folderName, type);
                    var assets = module.assets;
                    var moduleFolder = $"{xmy_proj}/{module.name}";
                    Directory.CreateDirectory(moduleFolder);
                    foreach (var asset in assets)
                    {
                        if (asset.is_folder)
                        {
                            //复制文件夹
                            var from = $"{folder}/{asset.path}";
                            var to = $"{moduleFolder}/assets/{asset.path}";
                            if (type == "install-time")
                            {
                                to = $"{moduleFolder}/{asset.path}";
                            }
                            // Debug.Log($"pad folder path {from} -> {to}");

                            to = to.Replace("{XGame.PlayAssetDeliveryPaths.RuntimePath}", "aa");
                            // XGameEditorUtil.CopyFolder(from, to);
                            
                            //判断父目录是否存在，不存在时创建
                            var toDir = Path.GetDirectoryName(to);
                            if (!Directory.Exists(toDir))
                            {
                                Directory.CreateDirectory(toDir);
                            }
                            Directory.Move(from,to);
                        }
                        else
                        {
                            //复制文件
                            var from = $"{folder}/{asset.path}";
                            var to = $"{moduleFolder}/assets/{asset.path}";
                            if (type == "install-time")
                            {
                                to = $"{moduleFolder}/{asset.path}";
                            }
                            // Debug.Log($"pad file path {from} -> {to}");
                            
                            to = to.Replace("{XGame.PlayAssetDeliveryPaths.RuntimePath}", "aa");
                            var dir = Path.GetDirectoryName(to);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            // File.Copy(from, to, true);
                            File.Move(from,to);
                            
                        }
                    }

                    pads.Add(pad);
                }
                
                string json = XGameEditorUtil.ToJsonIgnoreNull( new Pads(pads.ToArray()));
                
                //生成json
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