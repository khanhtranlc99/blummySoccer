using System;
using UnityEngine.Scripting;

namespace XGame
{
    [Serializable] 
    [Preserve] 
    public class ImRankData
    {
        ///必要， 0：数字类型、1：枚举类型;数字类型（0）往往适用于游戏的通关分数（103分、105分），枚举类型（1）适用于段位信息（青铜、白银)
        public int dataType;
        ///必要，展示出来的数值，dataType == 0 时只能传正数的字符串，否则会报错。value为具体的值，若dataType为0，请传入数字字符串（eg：103、105）；若dataType为1，则传入字符串（eg：青铜、白银）
        public string value;  
        ///必要，dataType 为 1 时，需要传入这个值判断权重，dataType 为 0 时，不填即可。越大越排在前面
        public int priority;
        ///可选，排行榜分区标识，将用户数据分区隔离，默认为 default
        public string zoneId;
        ///可选，预留字段
        public string extra;
    }
}