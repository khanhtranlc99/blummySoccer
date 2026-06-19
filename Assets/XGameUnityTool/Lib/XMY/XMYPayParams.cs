using System;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// 支付接口需要的参数
    /// </summary>
    [Preserve]
    [Serializable]
    public class XMYPayParams
    {
        //游戏中商品ID
        public string productId;

        //游戏中商品名称，比如元宝，钻石...
        public string productName;

        //游戏中商品描述
        public string productDesc;
        //价格，单位为分
        public int price;

        // 国内默认CNY
        public string currency;

        //游戏自己的订单号
        public string cpOrderId;
    }
}