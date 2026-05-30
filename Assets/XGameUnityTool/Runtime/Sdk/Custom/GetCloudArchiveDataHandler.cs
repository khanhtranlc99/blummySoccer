// using System;
//
// namespace XGame
// {
//     /// <summary>
//     /// 获取云存档数据代理
//     /// </summary>
//     public class GetCloudArchiveDataHandler : CallBackHandler
//     {
//         public struct ResultData
//         {
//             public bool IsSuccess;
//             public string Key;
//             public long Version;
//             public string Content;
//             public string Ret;
//
//             public ResultData(bool isSuccess, string key, long version, string content, string ret)
//             {
//                 IsSuccess = isSuccess;
//                 Key = key;
//                 Version = version;
//                 Content = content;
//                 Ret = ret;
//             }
//         }
//
//         private Action<CloudArchiveGetDateResult> _success;
//         private Action<string> _fail;
//
//         public GetCloudArchiveDataHandler(Action<CloudArchiveGetDateResult> success, Action<string> fail)
//         {
//             _success = success;
//             _fail = fail;
//         }
//
//         protected override void OnExecute(object data)
//         {
//             var resultData = (ResultData)data;
//             if (resultData.IsSuccess)
//             {
//                 _success?.Invoke(new CloudArchiveGetDateResult(resultData.Key, resultData.Version, resultData.Content));
//             }
//             else
//             {
//                 _fail?.Invoke(resultData.Ret);
//             }
//
//             _success = null;
//             _fail = null;
//         }
//     }
// }