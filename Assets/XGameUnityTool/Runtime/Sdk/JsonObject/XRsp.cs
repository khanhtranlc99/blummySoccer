using System;

// namespace XGame
// {
//     /// <summary>
//     /// 返回response
//     /// </summary>
//     [Serializable]
//     public class XRsp
//     {
//         //结果，ok为成功
//         public string ret = string.Empty;
//
//         //错误码
//         public string error = string.Empty;
//
//         public bool IsOK()
//         {
//             return ret == "ok";
//         }
//     }
//
//     [Serializable]
//     public class XRsp<T> : XRsp
//     {
//         public T result;
//     }
// }