// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.Scripting;
//
// namespace XGame
// {
//     /// <summary>
//     /// 云存档加载器
//     /// </summary>
//     [Preserve]
//     public class XMYCloudArchiveLoader : MonoBehaviour
//     {
//         //自动上传的间隔时间
//         private const float AUTO_UPLOAD_DELAY = 30;
//
//         //加载状态
//         public bool Loading = false;
//
//         //存档数据
//         public XMYGoogleCloudArchiveData ArchiveData = null;
//
//         //是否加载成功
//         public bool IsSuccess => ArchiveData != null;
//
//         //是否完成初始化
//         public bool IsInitialize => !string.IsNullOrEmpty(_localArchivePath);
//
//         //错误信息
//         public string Error = "";
//
//         //本地存档路径
//         private string _localArchivePath = "";
//
//         //是否有修改
//         private bool _isModify = false;
//
//         //下一次上传的时间
//         private float _nextUploadTime;
//
//         //加载函数
//         private Action _loadMethod;
//
//         //上传函数
//         private Action<string> _uploadMethod;
//
//         private XMYCloudArchiveLoader()
//         {
//         }
//
//         public static XMYCloudArchiveLoader CreateInstance(Action loadMethod, Action<string> uploadMethod)
//         {
//             var clone = new GameObject(nameof(XMYCloudArchiveLoader));
//             DontDestroyOnLoad(clone);
//             var loader = clone.AddComponent<XMYCloudArchiveLoader>();
//             loader._loadMethod = loadMethod;
//             loader._uploadMethod = uploadMethod;
//             return loader;
//         }
//
//         //初始化
//         public void Initialize(string user)
//         {
//             if (string.IsNullOrEmpty(_localArchivePath))
//             {
//                 _localArchivePath = $"{Application.persistentDataPath}/_{user}_archive.bin";
//                 _nextUploadTime = Time.unscaledTime + AUTO_UPLOAD_DELAY;
//             }
//         }
//
//         private void Update()
//         {
//             if (string.IsNullOrEmpty(_localArchivePath))
//             {
//                 return;
//             }
//
//             if (!IsSuccess)
//             {
//                 return;
//             }
//
//             if (Time.unscaledTime > _nextUploadTime)
//             {
//                 Upload();
//             }
//         }
//
//         //完成加载
//         public void LoadComplete(bool success, string result)
//         {
//             Loading = false;
//             //不处理
//             if (ArchiveData != null)
//             {
//                 return;
//             }
//
//             if (success)
//             {
//                 XMYGoogleCloudArchiveData localArchive = null;
//                 var localVersion = long.MinValue;
//                 //读取本地云存档
//                 if (File.Exists(_localArchivePath))
//                 {
//                     var json = File.ReadAllText(_localArchivePath);
//                     //本地存档
//                     localArchive = XJson.FromJson<XMYGoogleCloudArchiveData>(json);
//                     //比对存档key版本号确定使用的存档版本
//                     foreach (var item in localArchive)
//                     {
//                         if (localVersion < item.version)
//                         {
//                             localVersion = item.version;
//                         }
//                     }
//                 }
//
//                 //云存档版本
//                 var cloudVersion = -1L;
//                 //读取成功，解析成指定类型
//                 try
//                 {
//                     ArchiveData = XJson.FromJson<XMYGoogleCloudArchiveData>(result);
//                     XGameSdk.Log("解析云存档成功...");
//                 }
//                 catch (Exception e)
//                 {
//                     ArchiveData = new XMYGoogleCloudArchiveData();
//                     XGameSdk.Log("解析失败，新建云存档数据...");
//                 }
//
//                 foreach (var item in ArchiveData)
//                 {
//                     if (cloudVersion < item.version)
//                     {
//                         cloudVersion = item.version;
//                     }
//                 }
//
//                 //本地版本号更高，取本地作为最终数据
//                 if (localArchive != null && localVersion > cloudVersion)
//                 {
//                     ArchiveData = localArchive;
//                 }
//
//                 //写入到本地
//                 WriteToLocalAsync();
//             }
//             else
//             {
//                 Error = "load error";
//             }
//         }
//
//         //异步写入到本地
//         private async void WriteToLocalAsync()
//         {
//             if (ArchiveData != null)
//             {
//                 await WriteToFileAsync(_localArchivePath, ArchiveData.ToXJson());
//             }
//         }
//
//         static async Task WriteToFileAsync(string filePath, string content)
//         {
//             using (StreamWriter writer = new StreamWriter(filePath))
//             {
//                 await writer.WriteAsync(content);
//             }
//         }
//
//         // 异步加载
//         public async Task LoadingAsync()
//         {
//             if (!IsInitialize)
//             {
//                 throw new Exception("未登录，请先登录成功后再使用云存档功能");
//             }
//
//             if (ArchiveData == null && !Loading)
//             {
//                 Loading = true;
//                 _loadMethod?.Invoke();
//             }
//
//             while (ArchiveData == null && Loading)
//             {
//                 await Task.Delay(50);
//             }
//         }
//
//         //获取所有key
//         public CloudArchiveGetKeysResult GetKeys()
//         {
//             var infos = new List<ArchiveKeyInfo>();
//             foreach (var element in ArchiveData)
//             {
//                 infos.Add(new ArchiveKeyInfo(element.key, element.version));
//             }
//
//             var result = new CloudArchiveGetKeysResult() { KeyVersion = infos.ToArray() };
//             return result;
//         }
//
//         // 获取云存档数据
//         public CloudArchiveGetDateResult GetArchiveData(string key)
//         {
//             foreach (var element in ArchiveData)
//             {
//                 if (element.key == key)
//                 {
//                     return new CloudArchiveGetDateResult()
//                     {
//                         Key = element.key,
//                         Version = element.version,
//                         Content = element.data,
//                     };
//                 }
//             }
//
//             //返回缺省值
//             return new CloudArchiveGetDateResult()
//             {
//                 Key = key,
//                 Version = -1,
//                 Content = "",
//             };
//         }
//
//         //设置存档数据
//         public void SetArchiveData(string key, long version, string content)
//         {
//             foreach (var element in ArchiveData)
//             {
//                 if (element.key == key)
//                 {
//                     element.version = version;
//                     element.data = content;
//                     return;
//                 }
//             }
//
//             ArchiveData.Add(new ArchiveDataItem()
//             {
//                 key = key,
//                 version = version,
//                 data = content,
//             });
//             //异步写入到本地
//             WriteToLocalAsync();
//             //标记为修改过
//             _isModify = true;
//         }
//
//         //自动同步到云存档
//         public void Upload()
//         {
//             XGameSdk.Log("check archive upload...");
//             _nextUploadTime = Time.unscaledTime + AUTO_UPLOAD_DELAY;
//             if (_isModify)
//             {
//                 if (IsSuccess && IsInitialize)
//                 {
//                     XGameSdk.InvokeOnCloudArchiveSyncBegin();
//                     XGameSdk.Log("archive upload...");
//                     _uploadMethod?.Invoke(ArchiveData.ToXJson());
//                 }
//
//                 _isModify = false;
//             }
//         }
//
//         public void OnSaveResult(bool state)
//         {
//             if (state)
//             {
//                 XGameSdk.InvokeOnCloudArchiveSyncEnd();
//             }
//         }
//     }
// }