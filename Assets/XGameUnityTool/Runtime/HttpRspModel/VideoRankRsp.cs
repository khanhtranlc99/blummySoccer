// using System;
// using System.Collections.Generic;
// using XGame;
//
// namespace XGameHttpModel
// {
//     /// <summary>
//     /// 视频排行榜返回结果
//     /// </summary>
//     [Serializable]
//     public class VideoRankRsp
//     {
//         public int code;
//         public string codeMsg;
//         public List<VideoRankItem> data;
//
//         public List<VideoRankItem> GetRankData()
//         {
//             if (data == null)
//             {
//                 return new List<VideoRankItem>();
//             }
//
//             return data;
//         }
//     }
//
//     /// <summary>
//     /// 视频排行榜条目
//     /// </summary>
//     [Serializable]
//     public class VideoRankItem
//     {
//         /// <summary>
//         ///  string 视频 id
//         /// </summary>
//         public string videoId;
//
//         /// <summary>
//         /// 点赞数
//         /// </summary>
//         public string diggCount;
//
//         /// <summary>
//         /// 视频封面图链接地址
//         /// </summary>
//         public string coverUrl;
//
//         /// <summary>
//         /// 宿主端标识符，1128(抖音)，1112（抖音火山），13（头条）
//         /// </summary>
//         public string source;
//
//         /// <summary>
//         /// 作者昵称
//         /// </summary>
//         public string userName;
//
//         /// <summary>
//         /// 视频排行榜排名，从 1 开始累加
//         /// </summary>
//         public int rank;
//
//         /// <summary>
//         /// 视频的 videoTag 值
//         /// </summary>
//         public string videoTag;
//
//         /// <summary>
//         /// 跳转到视频播放页
//         /// </summary>
//         public void NavigateToVideoView(Action success, Action fail, Action complete)
//         {
//             XGameSdk.Instance.NavigateToVideoView(videoId, success, fail, complete);
//         }
//     }
// }