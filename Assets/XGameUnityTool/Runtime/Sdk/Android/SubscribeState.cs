using System;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// 订阅状态
    /// </summary>
    [Serializable]
    [Preserve]
    public class SubscribeState
    {
        /// <summary>
        /// 商品id
        /// </summary>
        [Preserve] public string productId;

        /// <summary>
        /// 到期时间,时间戳(毫秒)，从1970年
        /// </summary>
        [Preserve] public long expireTime;

        /// <summary>
        /// 是否有效
        /// </summary>
        [Preserve] public bool expireVaild;

        /// <summary>
        /// 时间戳转DateTime
        /// </summary>
        [Preserve]
        public DateTime GetExpireDateTime()
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(expireTime);
            var localTime = dateTimeOffset.LocalDateTime;
            return localTime;
        }
    }
}