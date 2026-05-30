// using System;
// using XGameHttpModel;
//
// namespace XGame
// {
//     /// <summary>
//     /// 分享视频到排行榜
//     /// </summary>
//     public interface IShareVideoToRank
//     {
//         /// <summary>
//         /// 是否可分享视频到排行榜
//         /// </summary>
//         bool CanShareVideoToRank();
//
//         /// <summary>
//         /// 分享视频到排行榜
//         /// </summary>
//         /// <param name="title">标题</param>
//         /// <param name="desc">描述</param>
//         /// <param name="tag">视频标签，用于筛选视频</param>
//         /// <param name="success">分享成功时触发</param>
//         /// <param name="fail">分享失败时触发</param>
//         /// <param name="cancel">取消时触发</param>
//         void ShareVideoToRank(string title, string desc, string tag, string[] topics,Action<ShareAppSuccessResult> success,
//             Action<string> fail,
//             Action cancel);
//
//         /// <summary>
//         /// 请求视频点赞排行榜
//         /// </summary>
//         /// <param name="tag">视频标签，仅支持全英文和数字文</param>
//         /// <param name="success">成功时触发，返回数据</param>
//         /// <param name="fail">失败时触发，返回错误信息</param>
//         /// <param name="numOfTop">拉去的排行榜条目，最多100</param>
//         /// <param name="rankType">榜单类型，支持周榜和月榜</param>
//         /// <param name="showToast">错误时是否提示toast,默认开启</param>
//         void RequestVideoLikeRank(string tag, Action<VideoRankRsp> success, Action<string> fail,
//             int numOfTop = 100, RankType rankType = RankType.Month,
//             bool showToast = true);
//
//         /// <summary>
//         /// 请求视频榜单（按发布时间排的）
//         /// </summary>
//         /// <param name="tag">视频标签，仅支持全英文和数字文</param>
//         /// <param name="success">成功时触发，返回数据</param>
//         /// <param name="fail">失败时触发，返回错误信息</param>
//         /// <param name="numOfTop">拉去的排行榜条目，最多100</param>
//         /// <param name="showToast">错误时是否提示toast,默认开启</param>
//         void RequestVideoTimeRank(string tag, Action<VideoRankRsp> success, Action<string> fail,
//             int numOfTop = 100, bool showToast = true);
//
//         /// <summary>
//         /// 跳转到视频播放地址
//         /// </summary>
//         /// <param name="videoId">视频id</param>
//         /// <param name="success">成功回调</param>
//         /// <param name="fail">失败回调</param>
//         /// <param name="complete">完成回调（成功/失败后都会返回）</param>
//         void NavigateToVideoView(string videoId, Action success, Action fail, Action complete);
//     }
// }