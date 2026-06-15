// using System;
// using System.Collections.Generic;
//
// namespace XGame
// {
//     public class XMYCloudArchiveManager
//     {
//         //加载函数
//         private Action LoadMethod;
//
//         //上传函数
//         private Action<string> UploadMethod;
//         
//         
//         private XMYGoogleCloudArchiveData _cloudArchive = null;
//
//         
//         private readonly List<XMYCloudArchiveGetKeys> _archiveGetKeysList = new List<XMYCloudArchiveGetKeys>();
//         private readonly List<XMYCloudArchiveGetData> _archiveGetDataList = new List<XMYCloudArchiveGetData>();
//         private readonly List<XMYCloudArchiveSetData> _archiveSetDataList = new List<XMYCloudArchiveSetData>();
//         
//         public class XMYCloudArchiveGetKeys
//         {
//             public Action<CloudArchiveGetKeysResult> CloudArchiveGetKeysSuccessAction;
//             public Action<string> CloudArchiveGetKeysFailedAction;
//         }
//
//         public class XMYCloudArchiveGetData
//         {
//             public string DataKey;
//             public Action<CloudArchiveGetDateResult> CloudArchiveGetDataSuccessAction;
//             public Action<string> CloudArchiveGetDataFailedAction;
//         }
//         
//         public class XMYCloudArchiveSetData
//         {
//             public long Version;
//             public string Key;
//             public string Content;
//         }
//         
//         public static XMYCloudArchiveManager CreateInstance(Action loadMethod, Action<string> uploadMethod)
//         {
//             var cloudArchiveManager = new XMYCloudArchiveManager();
//             cloudArchiveManager.LoadMethod = loadMethod;
//             cloudArchiveManager.UploadMethod = uploadMethod;
//             return cloudArchiveManager;
//         }
//         
//         public void GetData(XMYCloudArchiveGetData archiveGetData)
//         {
//             if (null == archiveGetData)
//             {
//                 return;
//             }
//             
//             _archiveGetDataList.Add(archiveGetData);
//             
//             if (null == _cloudArchive)
//             {
//                 LoadMethod?.Invoke();
//             }
//             else
//             {
//                 HandleCloudArchiveEvent(true,"");
//             }
//         }
//
//
//         public void GetKeys(XMYCloudArchiveGetKeys archiveGetKeys)
//         {
//             if (null == archiveGetKeys)
//             {
//                 return;
//             }
//
//             _archiveGetKeysList.Add(archiveGetKeys);
//
//             if (null == _cloudArchive)
//             {
//                 LoadMethod?.Invoke();
//             }
//             else
//             {
//                 HandleCloudArchiveEvent(true,"");
//             }
//             
//         }
//
//         
//         public void SetData(XMYCloudArchiveSetData cloudArchiveSetData)
//         {
//             if (null != cloudArchiveSetData)
//             {
//                 _archiveSetDataList.Add(cloudArchiveSetData);
//             }
//
//             if (null != _cloudArchive)
//             {
//
//                 foreach (var archiveSetData in _archiveSetDataList)
//                 {
//                     var isChange = false;
//                     foreach (var element in _cloudArchive)
//                     {
//                         if (element.key != archiveSetData.Key) continue;
//                         element.version = archiveSetData.Version;
//                         element.data = archiveSetData.Content;
//                         isChange = true;
//                         break;
//                     }
//
//                     if (isChange) continue;
//                     _cloudArchive.Add(new ArchiveDataItem()
//                     {
//                         key = archiveSetData.Key,
//                         version = archiveSetData.Version,
//                         data = archiveSetData.Content,
//                     });
//                 }
//                 
//                 UploadMethod?.Invoke(_cloudArchive.ToXJson());
//                 
//             }
//             else
//             {
//                 var errMsg = "保存存档失败, 因为还没获取到存档，缓存数据等到下次保存";
//                 Log(errMsg);
//             }
//         }
//         
//         public void LoadComplete(bool success, string result)
//         {
//             var errMsg = "";
//             var isSuc = success;
//             if (success)
//             {
//                 if (string.IsNullOrEmpty(result))
//                 {
//                     Log("读取存档成功，但是data为empty");
//                     if (null == _cloudArchive)
//                     {
//                         _cloudArchive = new XMYGoogleCloudArchiveData();
//                     }
//                 }
//                 else
//                 {
//                     try
//                     {
//                         _cloudArchive = XJson.FromJson<XMYGoogleCloudArchiveData>(result);
//                         Log("解析云存档成功...");
//                     }
//                     catch (Exception e)
//                     {
//                         isSuc = false;
//                         errMsg = "解析云存档失败，" + e;
//                         Log(errMsg);
//                     }
//                 }
//                 
//             }
//             else
//             {
//                 errMsg = "读取云存档失败, "+result;
//                 Log(errMsg);
//             }
//
//
//             //获取到存档，先添加游戏传入的数据
//             if (null != _cloudArchive)
//             {
//                 SetData(null);
//             }
//             
//             HandleCloudArchiveEvent(isSuc, errMsg);
//
//
//         }
//
//         public void OnSaveResult(bool success, string msg)
//         {
//             Log($"保存存档结果={success}, msg={msg}");
//             if (success)
//             {
//                 XGameSdk.InvokeOnCloudArchiveSyncEnd();
//             }
//             
//         }
//         
//         private void HandleCloudArchiveEvent(bool isSuc, string errMsg)
//         {
//             foreach (var getKeysObj in _archiveGetKeysList)
//             {
//
//                 if (isSuc)
//                 {
//                     getKeysObj.CloudArchiveGetKeysSuccessAction?.Invoke(GetKeys(_cloudArchive));
//                 }
//                 else
//                 {
//                     getKeysObj.CloudArchiveGetKeysFailedAction?.Invoke(errMsg);
//                 }
//                 
//             }
//             _archiveGetKeysList.Clear();
//             
//             foreach (var getDataObj in _archiveGetDataList)
//             {
//
//                 if (isSuc)
//                 {
//                     getDataObj.CloudArchiveGetDataSuccessAction?.Invoke(GetArchiveData(_cloudArchive,getDataObj.DataKey));
//                 }
//                 else
//                 {
//                     getDataObj.CloudArchiveGetDataFailedAction?.Invoke(errMsg);
//                 }
//                 
//             }
//             _archiveGetDataList.Clear();
//         }
//         
//
//         private CloudArchiveGetKeysResult GetKeys(XMYGoogleCloudArchiveData cloudArchiveData)
//         {
//             var infos = new List<ArchiveKeyInfo>();
//             if (null != cloudArchiveData)
//             {
//                 foreach (var element in cloudArchiveData)
//                 {
//                     infos.Add(new ArchiveKeyInfo(element.key, element.version));
//                 }
//             }
//
//             var result = new CloudArchiveGetKeysResult() { KeyVersion = infos.ToArray() };
//             return result;
//         }
//         
//         
//         private CloudArchiveGetDateResult GetArchiveData(XMYGoogleCloudArchiveData cloudArchiveData, string key)
//         {
//             if (null != cloudArchiveData)
//             {
//                 foreach (var element in cloudArchiveData)
//                 {
//                     if (element.key == key)
//                     {
//                         return new CloudArchiveGetDateResult()
//                         {
//                             Key = element.key,
//                             Version = element.version,
//                             Content = element.data,
//                         };
//                     }
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
//         
//         private static void Log(string content)
//         {
//             XMYGoogleSdk.Log(content);
//         }
//         
//         
//     }
// }