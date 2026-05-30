using System;

namespace XGame
{
    /// <summary>
    /// 游戏圈数据条目(对应微信文档：GameClubDataByType )
    /// 详细查看<see href="https://developers.weixin.qq.com/minigame/dev/api/open-api/game-club/wx.getGameClubData.html#GameClubDataByType-%E7%9A%84%E7%BB%93%E6%9E%84"/><para/>
    /// dataType==1,value:加入该游戏圈时间(秒级Unix时间戳)<para/>
    /// dataType==3,value:用户禁言状态(0：正常 1：禁言)<para/>
    /// dataType==4,value:当天(自然日)点赞贴子数<para/>
    /// dataType==5,value:当天(自然日)评论贴子数<para/>
    /// dataType==6,value:当天(自然日)发表贴子数<para/>
    /// dataType==7,value:当天(自然日)发表视频贴子数<para/>
    /// dataType==8,value:当天(自然日)赞官方贴子数<para/>
    /// dataType==9,value:当天(自然日)评论官方贴子数<para/>
    /// dataType==10,value:当天(自然日)发表到本圈子话题的贴子数<para/>
    /// </summary>
    [Serializable]
    public class GameClubDataByType
    {
        /// <summary>
        /// 数据类型，与传入的type
        /// </summary>
        public long dataType; //数据类型，对应传入的

        /// <summary>
        /// 值，不同dataType不同含义
        /// </summary>
        public long value;
    }
}