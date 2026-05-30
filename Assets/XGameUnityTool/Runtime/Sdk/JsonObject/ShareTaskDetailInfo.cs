using System;

namespace XGame
{
    [Serializable]
    public class ShareTaskDetailInfo
    {
        /// <summary>
        /// 被分享用户的uid
        /// </summary>
        public string uid;

        /// <summary>
        /// 被分享用户的拓展参数
        /// </summary>
        public string ext;
        
        /// <summary>
        /// 被分享用户的上报时间
        /// </summary>
        public string datetime;
    }
}