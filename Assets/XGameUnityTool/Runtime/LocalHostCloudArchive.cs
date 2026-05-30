using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace XGame
{
    /// <summary>
    /// 本地云存档数据。编辑器下持久化为工程内 JSON；运行时由 <see cref="LocalHostCloudArchiveServer"/> 使用内存实例与 PlayerPrefs。
    /// </summary>
    [Serializable]
    public class LocalHostCloudArchive
    {
        public const string JsonPath = "Assets/XGameUnityTool_Gen/Editor/LocalHostCloudArchive.json";
        
        [Serializable]
        public struct DataItem
        {
            public string Key;
            public long Version;
            public string Content;

            public DataItem(string key, long version, string content)
            {
                Key = key;
                Version = version;
                Content = content;
            }
        }

        private static LocalHostCloudArchive _global;

        public static LocalHostCloudArchive Global
        {
            get
            {
#if UNITY_EDITOR
                if (_global == null)
                {
                    _global = new LocalHostCloudArchive();
                    var fullPath = GetDiskPath(JsonPath);
                    var directory = Path.GetDirectoryName(fullPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                        AssetDatabase.Refresh();
                    }

                    if (File.Exists(fullPath))
                    {
                        _global.FromJson(File.ReadAllText(fullPath));
                    }
                    else
                    {
                        _global.Data = new Dictionary<string, DataItem>();
                        _global.SaveInEditor();
                    }
                }

                return _global;
#else
                throw new InvalidOperationException(
                    "LocalHostCloudArchive.Global 仅在编辑器内加载磁盘 JSON；真机/Player 请使用 LocalHostCloudArchiveServer 持有的实例。");
#endif
            }
        }

#if UNITY_EDITOR
        private static string GetDiskPath(string assetPath)
        {
            assetPath = assetPath.Replace('\\', '/');
            if (!assetPath.StartsWith("Assets/", StringComparison.Ordinal))
            {
                throw new ArgumentException("路径须位于 Assets/ 下", nameof(assetPath));
            }

            return Path.GetFullPath(Path.Combine(Application.dataPath, assetPath.Substring(7)));
        }

        /// <summary>删除 JSON 文件并丢弃内存单例。</summary>
        public static void ClearAllEditorStorage()
        {
            _global = null;

            var jsonDisk = GetDiskPath(JsonPath);
            if (File.Exists(jsonDisk))
            {
                File.Delete(jsonDisk);
            }

            AssetDatabase.Refresh();
        }
#endif

        public Dictionary<string, DataItem> Data = new Dictionary<string, DataItem>();

        public string ToJson()
        {
            return XJson.ToJson(Data);
        }

        public void FromJson(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    var parsed = XJson.FromJson<Dictionary<string, DataItem>>(content);
                    Data = parsed ?? new Dictionary<string, DataItem>();
                }
                catch (Exception)
                {
                    Data = new Dictionary<string, DataItem>();
                }
            }
            else
            {
                Data = new Dictionary<string, DataItem>();
            }
        }

        public void SaveInEditor()
        {
#if UNITY_EDITOR
            var fullPath = GetDiskPath(JsonPath);
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(fullPath, ToJson());
            AssetDatabase.ImportAsset(JsonPath, ImportAssetOptions.ForceSynchronousImport);
            if (!Application.isPlaying)
            {
                AssetDatabase.SaveAssets();
            }
#endif
        }
    }
}
