using UnityEngine;
using UnityEngine.Scripting;
namespace XGame
{
    /// <summary>
    /// XGameSdk支付回调,固定名称，请不要更改
    /// </summary>
	[Preserve]
    public class XGameSdkPayResult : PayResultListener
    {
        public override void OnPayResult(PayResult payResult)
        {
            if (!payResult.success)
            {

                Debug.Log($"支付失败，productId={payResult.productId}，failedCode={payResult.failedCode}，failedMsg={payResult.failedMsg}");
            
                return;
            }

            //TODO//发放奖励
            Debug.Log($"支付成功：{payResult.productId}");
        }
    }
}