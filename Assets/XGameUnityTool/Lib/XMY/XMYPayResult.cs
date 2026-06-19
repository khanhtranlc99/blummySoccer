using System;
using UnityEngine.Scripting;

namespace XGame
{
    [Preserve]
    [Serializable]
    public class XMYPayResult
    {
        ///是否成功
        public bool state;
        ///商品ID
        public string productId;
        ///游戏订单号
        public string cpOrderId;
        ///失败码
        public string code;
        ///失败信息
        public string msg;
        
        ///购买token (谷歌的购买token)
        public string token;
        
    }
}