using System;
using UnityEngine.Scripting;

namespace XGame
{
    [Serializable] 
    [Preserve] 
    public class UpScore
    {
        //排行榜id
        public string rankId ;  
        //排行榜分类编号
        public string rankCategory ;
        //分组id
        public string groupId ;
        //得分
        public int score;
        //额外的
        public string extra;
    }
}