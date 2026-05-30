using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace XGame
{
    [Serializable] 
    [Preserve] 
    public class RankingInfo
    {
        //排名id
        public string rankId ;  
        //排行榜分类编号
        public string rankCategory ;
        //分组id
        public string groupId ;
        //最大排行
        public int maxRank; 
      
        

    }
   
}
