using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace XGame
{
    //API GET 代理
    public delegate void ApiGetDelegate(string url, Action<HttpGetSuccessResult> success, Action<string> fail);

    //API POST 代理
    public delegate void ApiPostDelegate(string url, string body, Action<HttpPostSuccessResult> success,
        Action<string> fail);

    //云存档比对完毕任务代理
    public delegate void ArchiveComparisonCompleteTaskDelegate(bool success, string err);

    /// <summary>
    /// XSDK 云存档 功能
    /// </summary>
    [Preserve]
    public class XSDKCloudArchive : MonoBehaviour
    {
        //自动上传间隔
        private const float AUTO_UPLOAD_DELAY = 30;

        private enum ComparisonStatus
        {
            None, //未比对
            InComparison, //比对中
            Success, //比对完毕
        }


        [Serializable]
        [Preserve]
        //云存档数据
        public class CloudArchiveData
        {
            //写入时间戳（单位，秒）
            public long WriteTime = -1L;

            //数据条目
            public List<ArchiveDataItem> Items = new List<ArchiveDataItem>();

            //写入数据
            public void SetItem(string key, long version, string content)
            {
                var match = Items.FirstOrDefault(e => e.key == key);
                if (match != null)
                {
                    match.version = version;
                    match.content = content;
                }
                else
                {
                    Items.Add(new ArchiveDataItem()
                    {
                        key = key,
                        version = version,
                        content = content,
                    });
                }
            }
        }

        //云存档数据项Item
        [Serializable]
        [Preserve]
        public class ArchiveDataItem
        {
            public string key;
            public long version;
            public string content;
        }

        [Serializable]
        [Preserve]
        public class SyncGameArchiveRsp
        {
            public bool succeed;
            public long code;
            public string codeMsg;
            public object data;
        }

        //获取云存档响应信息
        [Serializable]
        [Preserve]
        public class GetGameArchiveRsp
        {
            public bool succeed;
            public long code;
            public string codeMsg;
            public DateItem data;

            [Serializable]
            public class DateItem
            {
                public string archive; //云存档
                public bool hasArchive; //是否有云存档
            }
        }

        //主机地址
        private static string HOST_URL = "https://sdk.mini.stargame.group";

        //云存档本地缓存Key
        public string PLAYER_PREF_CLOUD_ARCHIVE_KEY_CACHE_PREFIX = "XSDK_PLAYER_PREF_CLOUD_ARCHIVE_KEY_CACHE";

        //云存档key
        private string PlayerPrefKey => $"{PLAYER_PREF_CLOUD_ARCHIVE_KEY_CACHE_PREFIX}_{_userId}";

        //同步代理
        private Action UploadDelegate;

        private string _userId;

        private string _token;

        //云存档数据
        public CloudArchiveData ArchiveData = null;

        //偏移时间
        private double _offsetTime;


        //上传云存档
        private string API_URL_SYNC_GAME_ARCHIVE => $"{HOST_URL}/api/game_user/sync_game_archive";

        //请求云存档
        private string API_URL_GET_GAME_ARCHIVE => $"{HOST_URL}/api/game_user/get_game_archive";

        //GET请求代理
        private ApiGetDelegate _apiGetDelegate;

        //POST请求代理
        private ApiPostDelegate _apiPostDelegate;

        //比对完毕队列
        private Queue<ArchiveComparisonCompleteTaskDelegate> _comparisonCompleteTaskQueue =
            new Queue<ArchiveComparisonCompleteTaskDelegate>();

        //比对状态
        private ComparisonStatus _comparisonStatus = ComparisonStatus.None;

        //存档是否被改动
        private bool _modifyFlag = false;

        //同步机制触发代理
        private Action _syncBeginDelegate;
        private Action _syncEndDelegate;


        private XSDKCloudArchive()
        {
        }

        public static XSDKCloudArchive CreateInstance(bool neverDestroy)
        {
            var clone = new GameObject("XSDKCloudArchive");
            if (neverDestroy)
            {
                GameObject.DontDestroyOnLoad(clone);
            }

            var com = clone.AddComponent<XSDKCloudArchive>();
            return com;
        }

        //初始化云存档
        public void Initialize(string userId, long timeStamp, long serviceArchiveVersion,
            ApiGetDelegate apiGet,
            ApiPostDelegate apiPost, Action syncBegin, Action syncEnd)
        {
            _syncBeginDelegate = syncBegin;
            _syncEndDelegate = syncEnd;
            _userId = userId;
            //计算偏移时间,timeStamp为毫秒,转成秒
            _offsetTime = timeStamp * 0.001f - Time.realtimeSinceStartup;
            _apiGetDelegate = apiGet;
            _apiPostDelegate = apiPost;
            //读取本地存档信息
            var json = PlayerPrefs.GetString(PlayerPrefKey, "");
            ArchiveData = new CloudArchiveData();
            //读取本地存档
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    //尝试解析数据
                    ArchiveData = XJson.FromJson<CloudArchiveData>(json);
                    Debug.Log(
                        $"[XSDKCloudArchive] 解析存档到 CloudArchiveData 成功");
                }
                catch (Exception e)
                {
                    Debug.Log(
                        $"[XSDKCloudArchive] 解析存档到 CloudArchiveData 失败，创建新档  error：{e}");
                    ArchiveData = new CloudArchiveData();
                }
            }

            //如果大于服务器写入版本号,本地的存档内容为最新，跳过存档比对
            if (ArchiveData.WriteTime >= serviceArchiveVersion)
            {
                Debug.Log(
                    $"[XSDKCloudArchive] 跳过存档比对 本地版本：{ArchiveData.WriteTime} > 服务器版本:{serviceArchiveVersion}...");
                _comparisonStatus = ComparisonStatus.Success;
            }

            //开启协程，同步存档
            StartCoroutine(IEAutoUpload());
        }

        //当前时间戳
        private long DateNowTimeStampMillisecond()
        {
            return (long)((Time.realtimeSinceStartup + _offsetTime) * 1000L);
        }

        //获取所有keys
        public void GetKeys(Action<CloudArchiveGetKeysResult> success, Action<string> fail)
        {
            Debug.Log($"[XSDKCloudArchive] GetKeys...");
            //添加到比对队列
            EnqueueComparisonCompleteTask((ret, err) =>
            {
                if (ret)
                {
                    Debug.Log($"[XSDKCloudArchive] GetKeys 成功！");
                    //将数据映射到ArchiveKeyInfo列表
                    var keyInfos = ArchiveData.Items.Select(e => new ArchiveKeyInfo(e.key, e.version));
                    var result = new CloudArchiveGetKeysResult(keyInfos.ToArray());
                    success?.Invoke(result);
                }
                else
                {
                    Debug.Log($"[XSDKCloudArchive] GetKeys 失败！比对云存档数据失败 err:{err}");
                    fail?.Invoke(err);
                }
            });
        }

        //获取数据
        public void GetData(string key, Action<CloudArchiveGetDateResult> success,
            Action<string> fail)
        {
            EnqueueComparisonCompleteTask((ret, err) =>
            {
                if (ret)
                {
                    var match = ArchiveData.Items.FirstOrDefault(e => e.key == key);
                    if (match == null)
                    {
                        //返回默认数据
                        success?.Invoke(new CloudArchiveGetDateResult(key, -1, ""));
                    }
                    else
                    {
                        //返回匹配数据
                        success?.Invoke(new CloudArchiveGetDateResult(match.key, match.version, match.content));
                    }
                }
                else
                {
                    fail?.Invoke(err);
                }
            });
        }

        //设置数据
        public void SetData(string key, long version, string content)
        {
            Debug.Log($"[XSDKCloudArchive] SetData key:{key} version:{version} content:{content}...");
            EnqueueComparisonCompleteTask((ret, err) =>
            {
                if (ret)
                {
                    Debug.Log($"[XSDKCloudArchive] SetData 成功");
                    ArchiveData.SetItem(key, version, content);
                    //写入云存档数据到本地
                    WriteArchiveDataCache();
                    //标记修改
                    _modifyFlag = true;
                }
                else
                {
                    //无法写入，比对云存档数据失败
                    Debug.Log($"[XSDKCloudArchive] SetData 失败！比对云存档数据失败 err:{err}");
                }
            });
        }

        //上传云存档
        public void UploadSync()
        {
            _syncBeginDelegate?.Invoke();
            Debug.Log("[XSDKCloudArchive] UploadSync...");
            EnqueueComparisonCompleteTask((ret, err) =>
            {
                if (ret)
                {
                    Debug.Log("[XSDKCloudArchive] UploadSync 成功!");
                    var json = ArchiveData.ToXJson();
                    var body = new { archive = json, version = ArchiveData.WriteTime }.ToXJson();
                    ApiPost(API_URL_SYNC_GAME_ARCHIVE, body, (res) =>
                    {
                        if (res.StateCode == 200)
                        {
                            Debug.Log("[XSDKCloudArchive] 触发同步成功!");
                            //触发同步成功
                            _syncEndDelegate?.Invoke();
                        }
                    }, null);
                }
                else
                {
                    Debug.Log($"[XSDKCloudArchive] UploadSync 失败！比对云存档数据失败 err:{err}");
                }
            });
        }

        //尝试添加到完毕队列
        private void EnqueueComparisonCompleteTask(ArchiveComparisonCompleteTaskDelegate taskDelegate)
        {
            //添加到任务队列，等待比对完毕
            _comparisonCompleteTaskQueue.Enqueue(taskDelegate);
            ComparisonServiceArchive();
        }

        //解析到云存档数据实例
        private CloudArchiveData ParserToCloudArchiveData(string json)
        {
            try
            {
                return XJson.FromJson<CloudArchiveData>(json);
            }
            catch (Exception e)
            {
                return new CloudArchiveData();
            }
        }

        //比对服务器存档，取最新的作为云存档数据
        private void ComparisonServiceArchive()
        {
            switch (_comparisonStatus)
            {
                case ComparisonStatus.None: //切换到比对状态,开始比对
                {
                    _comparisonStatus = ComparisonStatus.InComparison;
                    Debug.Log($"[XSDKCloudArchive] 拉取云存档...");
                    ApiGet(API_URL_GET_GAME_ARCHIVE, (res) =>
                    {
                        Debug.Log($"[XSDKCloudArchive] 拉取云存档 响应成功 res:{res.ToXJson()}");
                        if (res.StatusCode == 200)
                        {
                            try
                            {
                                var rsp = XJson.FromJson<GetGameArchiveRsp>(res.Data);
                                if (!rsp.succeed)
                                {
                                    Debug.Log($"[XSDKCloudArchive] 拉取云存档 失败 succeed={rsp.succeed}");
                                    return;
                                }

                                var archiveJson = rsp.data.hasArchive ? rsp.data.archive : "";
                                //云端版本
                                var cloud = ParserToCloudArchiveData(archiveJson);
                                //如果本地版本小于云端版本，采用云端版本
                                if (ArchiveData.WriteTime < cloud.WriteTime)
                                {
                                    Debug.Log(
                                        $"[XSDKCloudArchive] 本地版本 {ArchiveData.WriteTime} < 云端版本:{cloud.WriteTime}，采用云端版本");
                                    //以服务器为准
                                    ArchiveData = cloud;
                                    //写入本地
                                    WriteArchiveDataCache();
                                }

                                //比对成功
                                OnComparisonComplete(true, "");
                            }
                            catch (Exception e)
                            {
                                //解析云存档失败
                                Debug.Log($"[XSDKCloudArchive] 拉取云存档 解析数据失败！{e}");
                                //比对失败
                                OnComparisonComplete(true, "云存档解析数据失败");
                                return;
                            }
                        }
                        else
                        {
                            Debug.Log($"[XSDKCloudArchive] 获取云存档失败 StatusCode={res.StatusCode} ");
                            //比对失败
                            OnComparisonComplete(true, $"获取云存档失败 StatusCode={res.StatusCode} ");
                        }
                    }, (err) =>
                    {
                        //比对失败
                        OnComparisonComplete(true, $"获取云存档失败 {err} ");
                    });
                }
                    break;
                case ComparisonStatus.InComparison: //比对中，直接返回
                    return;
                case ComparisonStatus.Success: //已经比对过，触发任务队列
                {
                    //触发完成回调
                    HandleTask(true, "");
                }
                    break;
            }
        }

        //写入数据到本地缓存
        private void WriteArchiveDataCache()
        {
            //设置写入时间戳
            ArchiveData.WriteTime = DateNowTimeStampMillisecond();
            var json = ArchiveData.ToXJson();
            PlayerPrefs.SetString(PlayerPrefKey, json);
            PlayerPrefs.Save();
        }

        //比对完成时触发
        private void OnComparisonComplete(bool success, string err)
        {
            if (success)
            {
                //比对成功，修改状态
                _comparisonStatus = ComparisonStatus.Success;
            }
            else
            {
                //比对失败，恢复到NONE
                _comparisonStatus = ComparisonStatus.None;
            }

            HandleTask(success, err);
        }

        //处理任务队列
        private void HandleTask(bool success, string err)
        {
            var tasks = _comparisonCompleteTaskQueue.ToList();
            _comparisonCompleteTaskQueue.Clear();

            //触发回调
            foreach (var task in tasks)
            {
                task?.Invoke(success, err);
            }
        }

        private IEnumerator IEAutoUpload()
        {
            while (true)
            {
                yield return new WaitForSeconds(AUTO_UPLOAD_DELAY);
                if (_comparisonStatus == ComparisonStatus.Success)
                {
                    if (_modifyFlag)
                    {
                        _modifyFlag = false;
                        UploadSync(); //同步到云存档
                    }
                }
            }
        }

        //get请求
        private void ApiGet(string url, Action<HttpGetSuccessResult> success, Action<string> fail)
        {
            _apiGetDelegate?.Invoke(url, success, fail);
        }

        //post请求
        private void ApiPost(string url, string body, Action<HttpPostSuccessResult> success, Action<string> fail)
        {
            _apiPostDelegate?.Invoke(url, body, success, fail);
        }
    }
}