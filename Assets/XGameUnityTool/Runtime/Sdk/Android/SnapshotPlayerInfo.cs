using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace XGame
{
    [Serializable]
    [Preserve]
    public enum GooglePlayGameCode
    {
        /**
         * 没有登录
         * */
        LoginNot = 2000,

        /**
         * 登录成功
         * */
        LoginPass = 2001,

        /**
         * 加载存档失败
         * */
        OpenFailed = 1001,

        /**
         * 读取存档失败
         * */
        ReadFailed = 1002,

        /**
         * 未找到存档
         * */
        NotFind = 1003,

        /**
         * 存档数据为空保存失败
         * */
        SaveEmpty = 1004,

        /**
         * 保存存档失败
         * */
        SaveError = 1005,

        /**
        * 获取当前玩家排行榜成绩失败
        * */
        ActiveEventLoadCompletedFailed = 1006,

        /**
         * 获取当前玩家排行榜成绩失败
         * */
        CurrentPlayerLeaderboardScoreLoadFailed = 1007,
        
        /**
         * 获取顶部玩家排行榜成绩失败
         * */
        TopScoresLoadFailed = 1008,
        
        /**
         * 登录认证失败
         * */
        LoginAuthFailed = 1009,
        
        /**
         * 读取存档成功
         * */
        LoadCompleted = 8000,

        /**
         * 保存存档成功
         * */
        SaveCompleted = 8001,

        /**
        * 获取活动事件信息成功
        * */
        ActiveEventLoadCompleted = 8003,

        /**
         * 获取当前玩家排行榜成绩成功
         * */
        CurrentPlayerLeaderboardScoreLoadCompleted = 8004,
        
        /**
         * 获取顶部玩家排行榜成绩成功
         * */
        TopScoresLoadCompleted = 8005,
        
        /**
        * 已经登录认证成功
        * */
        LoginAuthPass = 8006,
        
    }

    /**玩家信息*/
    [Serializable]
    [Preserve]
    public class LoginGooglePlayGameResult
    {
        /**状态码 */
        public GooglePlayGameCode code;

        /**玩家ID */
        public string playerId;

        /**玩家名称 */
        public string playerName;

        /**玩家头像URL或者本地图片路径 */
        public string playerIcon;
        
        /**失败信息 */
        public string failMsg;
    }

    [Serializable]
    [Preserve]
    public class LoadSnapshotResult
    {
        /**状态码 */
        public GooglePlayGameCode code;

        /**失败信息 */
        public string failMsg;

        /**存档数据 */
        public string data;
    }

    [Serializable]
    [Preserve]
    public class SaveSnapshotResult
    {
        /**状态码 */
        public GooglePlayGameCode code;

        /**失败信息 */
        public string failMsg;
    }


    /// 要检索数据的时间范围
    [Serializable]
    [Preserve]
    public enum LeaderboardTimeSpan
    {
        /**
         * 每日
         * */
        Daily = 0,

        /**
         * 每周
         * */
        Weekly = 1,

        /**
         * 所有时间
         * */
        AllTime = 2,
    }

    ///要检索分数的排行榜集合
    [Serializable]
    [Preserve]
    public enum LeaderboardCollection
    {
        /**
         * 公共，全部人
         * */
        Public = 0,

        /**
         * 好友
         * */
        Friends = 3,
    }

    [Serializable]
    [Preserve]
    public class LeaderboardScoreInfo
    {
     /**
     *排名 long类型字符串
     */
     public string rank;

     /**
     *提交的成绩 long类型字符串
     */
     public string rawScore;

     /**
     *获取该分数达成时的时间戳（毫秒）。 long类型字符串
     */
     public string timestampMillis;

     /**
     *格式化的排名，用于UI显示，比如第几名
     */
     public string displayRank;

     /**
     *格式化的成绩，用于UI显示，比如100分
     */
     public string displayScore;

     /**
     *成绩持有者的名称，用于UI显示，比如：1000玩家
     */
     public string scoreHolderDisplayName;

     /**
     *成绩持有者的icon，用于UI显示
     */
     public string scoreHolderIconImageUrl;
     
    }
    
    [Serializable]
    [Preserve]
    public class CurrentPlayerLeaderboardScoreLoadResult
    {
     
        /**状态码 */
        public GooglePlayGameCode code;

        /**
       *排行榜ID
       */
        public string leaderboardId;

        /**
        *时间范围
        */
        public LeaderboardTimeSpan span;

        /**
        *排行榜集合
        */
        public LeaderboardCollection leaderboardCollection;

        /**
        *排行榜成绩
        */
        public LeaderboardScoreInfo info;
        
        /**失败信息 */
        public string failMsg;
        
    }

    
    [Serializable]
    [Preserve]
    public class TopScoresLoadResult
    {
     
     /**状态码 */
     public GooglePlayGameCode code;

     /**
    *排行榜ID
    */
     public string leaderboardId;

     /**
     *时间范围
     */
     public LeaderboardTimeSpan span;

     /**
     *排行榜集合
     */
     public LeaderboardCollection leaderboardCollection;

     /**
     *排行榜成绩数组
     */
     public List<LeaderboardScoreInfo> infos;
        
     /**失败信息 */
     public string failMsg;
     
    }
    
}