using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 本地模拟云存档服务器
    /// </summary>
    public class LocalHostCloudArchiveServer : MonoBehaviour
    {
        //存储key
        private const string KEY_LOCAL_HOST_CLOUD_ARCHIVE_SERVER_PLAYER_PREFS_KEY =
            "XGAME_SDK_KEY_LOCAL_HOST_CLOUD_ARCHIVE_SERVER_PLAYER_PREFS_KEY";

        private static LocalHostCloudArchiveServer _instance = null;

        public static LocalHostCloudArchiveServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    var clone = new GameObject("本地云存档模拟");
                    _instance = clone.AddComponent<LocalHostCloudArchiveServer>();
                    DontDestroyOnLoad(clone);
                }

                return _instance;
            }
        }

        //云数据
        public LocalHostCloudArchive Cloud;

        //本地缓存数据
        public LocalHostCloudArchive Cache;

        //缓存数据
        [Header("本地模拟云存档")]
        //同步时间
        public float AutoSyncTime = 30;

        //是否运行中
        public bool IsRunning = false;

        //修改过
        public bool Modify = false;

        //当前时间
        public float CurrentTime = 0;

#if UNITY_EDITOR
        public const bool IsUnityEditor = true;
#else
        public const bool IsUnityEditor = false;
#endif


        //开启服务器
        public void StartServer(int autoSyncTime)
        {
            if (autoSyncTime <= 0)
            {
                //最小同步时间5秒钟
                autoSyncTime = 5;
            }
            
            //编辑器下
            if (IsUnityEditor)
            {
                Cloud = LocalHostCloudArchive.Global;
            }
            else
            {
                //从PlayerPrefs读取
                Cloud = new LocalHostCloudArchive();
                Cloud.FromJson(PlayerPrefs.GetString(KEY_LOCAL_HOST_CLOUD_ARCHIVE_SERVER_PLAYER_PREFS_KEY));
            }

            //创建缓存实例
            Cache = new LocalHostCloudArchive();
            //复制本地数据到缓存
            CopyData(Cloud, Cache);
            AutoSyncTime = autoSyncTime;
            CurrentTime = 0;
            Modify = false;
            IsRunning = true;
        }

        //设置数据
        public void SetData(string key, long version, string content)
        {
            if (IsUnityEditor)
            {
                // 编辑器下
                Cache.Data[key] = new LocalHostCloudArchive.DataItem(key, version, content);
                Modify = true;
            }
            else
            {
                //非编辑器下
                Cloud.Data[key] = new LocalHostCloudArchive.DataItem(key, version, content);
                Modify = false;
                SaveCloudToPlayerPrefs();
            }
        }

        //获取数据
        public void GetData(string key, Action<CloudArchiveGetDateResult> success, Action<string> fail)
        {
            //编辑器模式下
            if (IsUnityEditor)
            {
                if (IsRunning)
                {
                    var data = Cloud.Data;
                    if (data.ContainsKey(key))
                    {
                        var item = data[key];
                        success?.Invoke(new CloudArchiveGetDateResult(item.Key, item.Version, item.Content));
                    }
                    else
                    {
                        success?.Invoke(new CloudArchiveGetDateResult(key, 0, ""));
                    }
                }
                else
                {
                    fail?.Invoke("连接服务器失败！");
                }
            }
            else
            {
                //非编辑器模式下
                var data = Cloud.Data;
                if (data.ContainsKey(key))
                {
                    var item = data[key];
                    success?.Invoke(new CloudArchiveGetDateResult(item.Key, item.Version, item.Content));
                }
                else
                {
                    success?.Invoke(new CloudArchiveGetDateResult(key, 0, ""));
                }
            }
        }

        //获取keys
        public void GetKeys(Action<CloudArchiveGetKeysResult> success, Action<string> fail)
        {
            if (IsUnityEditor)
            {
                //编辑器模式下
                if (IsRunning)
                {
                    var data = Cloud.Data.Values.ToList();

                    var result = new ArchiveKeyInfo[data.Count];
                    for (int i = 0; i < result.Length; i++)
                    {
                        var element = data[i];
                        result[i] = new ArchiveKeyInfo(element.Key, element.Version);
                    }

                    success?.Invoke(new CloudArchiveGetKeysResult(result));
                }
                else
                {
                    fail?.Invoke("访问服务器失败！");
                }
            }
            else
            {
                //非编辑器模式下
                var data = Cloud.Data.Values.ToList();
                var result = new ArchiveKeyInfo[data.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    var element = data[i];
                    result[i] = new ArchiveKeyInfo(element.Key, element.Version);
                }

                success?.Invoke(new CloudArchiveGetKeysResult(result));
            }
        }

        //上传存档
        public void UploadSync()
        {
            if (IsUnityEditor)
            {
                if (IsRunning)
                {
                    //触发同步开始
                    XGameSdk.InvokeOnCloudArchiveSyncBegin();
                    //复制本地缓存数据打破云端
                    CopyData(Cache, Cloud);
                    //保存到本地
                    Cloud.SaveInEditor();
                    //同步结束
                    XGameSdk.InvokeOnCloudArchiveSyncEnd();
                    Modify = false;
                }
            }
            else
            {
                SaveCloudToPlayerPrefs();
                Modify = false;
            }
        }

        //复制数据
        public void CopyData(LocalHostCloudArchive from, LocalHostCloudArchive to)
        {
            if (from == null || to == null)
            {
                return;
            }

            to.Data = new Dictionary<string, LocalHostCloudArchive.DataItem>();
            var fromData = from.Data;
            if (fromData == null)
            {
                return;
            }

            foreach (var kv in fromData)
            {
                to.Data.Add(kv.Key, kv.Value);
            }
        }

        //自动同步
        private void TryAutoUploadSync()
        {
            //有修改
            if (Modify)
            {
                UploadSync();
            }
        }


        private void SaveCloudToPlayerPrefs()
        {
            //保存
            var json = Cloud.ToJson();
            PlayerPrefs.SetString(KEY_LOCAL_HOST_CLOUD_ARCHIVE_SERVER_PLAYER_PREFS_KEY, json);
            PlayerPrefs.Save();
        }

        private void Update()
        {
            if (IsRunning && IsUnityEditor)
            {
                CurrentTime += Time.deltaTime;
                if (CurrentTime > AutoSyncTime)
                {
                    //触发自动同步
                    TryAutoUploadSync();
                    //重置时间
                    CurrentTime = 0;
                }
            }
        }


#if UNITY_EDITOR

        private void OnDestroy()
        {
            if (IsUnityEditor && IsRunning && Cloud != null)
            {
                Cloud.SaveInEditor();
            }
        }
#endif
    }
}