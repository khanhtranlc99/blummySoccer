namespace XGame
{
    /// <summary>
    /// SDK 监听
    /// </summary>
    public class BaskSdkListener
    {
        public BaskSdkListener(IOnPayResult payResult, IOnGiftResult giftResult)
        {
            _payResult = payResult;
            _giftResult = giftResult;
        }

        #region 支付回调

        /// <summary>
        /// 支付回调
        /// </summary>
        private IOnPayResult _payResult;


        /// <summary>
        /// 触发支付回调
        /// </summary>
        public void InvokeOnPayResult(bool success, string product, string order, string token = "", string failedCode = "",
            string failedMsg = "")
        {
            if (_payResult is PayResultListener payResultListener)
            {
                var res = new PayResult
                {
                    productId = product,
                    success = success,
                    token = token,
                    orderId = order,
                    failedCode = failedCode,
                    failedMsg = failedMsg
                };
                payResultListener.OnPayResult(res);
            }
            else
            {
                _payResult.OnPayResult(success, product, order, failedCode, failedMsg);
            }
        }
        
        #endregion

        #region 兑换码回调

        private IOnGiftResult _giftResult;

        /// <summary>
        /// 触发兑换回调
        /// </summary>
        public void InvokeOnGiftResult(bool success, int count, string productName)
        {
            _giftResult?.OnGiftResult(success, count, productName);
        }

        #endregion
    }
}