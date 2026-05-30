namespace XGame
{
    /// <summary>
    /// 兑换回调
    /// </summary>
    public interface IOnGiftResult
    {
        void OnGiftResult(bool success, int count, string productName);
    }
}