using System;
using UnityEngine.Scripting;

namespace XGame
{
    [Preserve]
    [Serializable]
    public class XMYSnapshotResult
    {
        
        public const string TYPE_LOGIN = "login";
        public const string TYPE_AUTH = "auth";
        public const string TYPE_LOAD = "load";
        public const string TYPE_SAVE = "save";
        public const string TYPE_ACTIVE = "active";
        public const string TYPE_SCORE = "score";
        public const string TYPE_TOP_SCORES = "topScores";
        //游戏存档类型：login：gameplay是否登录成功, load：加载快照，save：保存快照
        public string type;
        
        //状态码
        public string code;

        //游戏加载存档返回的数据 在type = load 时是快照数据，否则是错误信息
        public string data;
    }
}