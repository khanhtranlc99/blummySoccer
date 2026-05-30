// using System;
// using System.Collections.Generic;
//
// namespace XGame
// {
//     /// <summary>
//     /// 获取云存档回调
//     /// </summary>
//     public class GetCloudArchiveKeysHandler : CallBackHandler
//     {
//         //数据
//         public struct ResultData
//         {
//             public bool IsSuccess;
//             public ArchiveKeyInfo[] Keys;
//             public string Ret;
//
//             public ResultData(bool isSuccess, ArchiveKeyInfo[] keys, string ret)
//             {
//                 IsSuccess = isSuccess;
//                 Keys = keys;
//                 Ret = ret;
//             }
//         }
//
//         private Action<CloudArchiveGetKeysResult> _onSuccess;
//         private Action<string> _onFail;
//
//         public GetCloudArchiveKeysHandler(Action<CloudArchiveGetKeysResult> onSuccess, Action<string> onFail)
//         {
//             _onSuccess = onSuccess;
//             _onFail = onFail;
//         }
//
//         protected override void OnExecute(object data)
//         {
//             //转成格式
//             var result = (ResultData)data;
//             if (result.IsSuccess)
//             {
//                 _onSuccess?.Invoke(new CloudArchiveGetKeysResult(result.Keys));
//             }
//             else
//             {
//                 _onFail?.Invoke(result.Ret);
//             }
//
//             _onFail = null;
//             _onSuccess = null;
//         }
//     }
// }