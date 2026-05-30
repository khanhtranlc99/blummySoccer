using System;
using UnityEngine.Scripting;

namespace XGame
{
    
    [Serializable] 
    [Preserve] 
    public class ImRankListInfo
    {
        ///必要，代表数据排序周期，day为当日写入的数据做排序；week为自然周，month为自然月，all为半年
        public string rankType;
        ///必要，选择排序哪些类型的数据, 0：数字类型 、 1：枚举类型
        public int dataType;
        ///可选，选择榜单展示范围。default: 好友及总榜都展示，all：仅总榜单, 默认为 default
        public string relationType;
        ///可选，数据后缀，最后展示样式为 value + suffix，若suffix传“分”，则展示 103分、104分
        public string suffix;
        ///可选，排行榜标题的文案
        public string rankTitle;
        ///可选，排行榜分区标识,默认为 default
        public string zoneId;
        
        
    }
    
}