using System;
using UnityEngine.Scripting;
using UnityEngine;

namespace XGame
{
    [Serializable]
    [Preserve]
    public class PayResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success;

        /// <summary>
        /// 商品ID
        /// </summary>
        public string productId;

        /// <summary>
        /// SDK订单ID
        /// </summary>
        public string orderId;

        /// <summary>
        /// 购买token （支持谷歌内购）
        /// </summary>
        public string token;

        /// <summary>
        /// 失败码
        /// </summary>
        public string failedCode;

        /// <summary>
        /// 失败信息
        /// </summary>
        public string failedMsg;
    }


    public abstract class PayResultListener : IOnPayResult
    {
        public void OnPayResult(bool success, string productId, string orderId, string failedCode, string failedMsg)
        {
            Debug.Log("PayResultListener OnPayResult旧接口空实现");
        }

        public abstract void OnPayResult(PayResult payResult);
    }


    /// <summary>
    /// 支付回调
    /// </summary>
    public interface IOnPayResult
    {
        public void OnPayResult(bool success, string productId, string orderId, string failedCode, string failedMsg);
    }
}