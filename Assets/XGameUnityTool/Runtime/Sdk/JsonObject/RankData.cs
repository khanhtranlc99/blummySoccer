using System;

namespace XGame
{
    [Serializable]
    public class RankData
    {
        //排行榜id
        public string rankId;  
        //排行榜分类编号
        public string rankCategory;
        //分组id
        public string groupId;
        //最大排行
        public int maxRank; 
    }
}