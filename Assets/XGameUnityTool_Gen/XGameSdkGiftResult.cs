using UnityEngine;
using UnityEngine.Scripting;
namespace XGame
{
	/// <summary>
    /// 兑换回调类，固定名称，请不要更改
    /// </summary>
	[Preserve]
    public class XGameSdkGiftResult : IOnGiftResult
    {
        /// <summary>
        /// 兑换回调
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="count">道具数量</param>
        /// <param name="productName">道具名</param>
        public void OnGiftResult(bool success, int count, string productName)
        {
            if (!success)
            {
            
                Debug.Log($"兑换失败,count={count}，productName={productName}");
                
                return;
            }
            //TODO//兑换成功，发放奖励

            Debug.Log($"兑换成功,count={count}，productName={productName}");

        }
    }
}