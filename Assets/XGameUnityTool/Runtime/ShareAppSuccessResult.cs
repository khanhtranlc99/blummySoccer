// using System;
// using System.Collections.Generic;
//
// namespace XGame
// {
//     /// <summary>
//     /// 分享成功结果
//     /// </summary>
//     [Serializable]
//     public class ShareAppSuccessResult 
//     {
//         public Dictionary<string, object> Data;
//
//         public string GetVideoId()
//         {
//             if (Data.TryGetValue("videoId", out var match))
//             {
//                 return match.ToString();
//             }
//
//             return string.Empty;
//         }
//
//         public ShareAppSuccessResult(Dictionary<string, object> data)
//         {
//             Data = data;
//         }
//     }
// }