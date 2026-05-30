namespace XGame
{
    /// <summary>
    /// XGame sdk 广告 拓展事件监听
    /// </summary>
    public class XGameADListener
    {
        /// <summary>
        /// Banner 拓展事件
        /// </summary>
        public static readonly IADResultHandler BannerHandler = new ADResultHandler("BannerHandler");

        /// <summary>
        /// 视频广告 拓展事件
        /// </summary>
        public static readonly IADResultHandler VideoHandler = new ADResultHandler("VideoHandler");

        /// <summary>
        /// 插页广告 拓展事件
        /// </summary>
        public static readonly IADResultHandler InterHandler = new ADResultHandler("InterHandler");

        /// <summary>
        /// 原生大图 拓展事件
        /// </summary>
        public static readonly IADResultHandler BigNativeHandler = new ADResultHandler("BigNativeHandler");

        /// <summary>
        /// 悬浮广告（Mar） 拓展事件
        /// </summary>
        public static readonly IADResultHandler FloatIconHandler = new ADResultHandler("FloatIconHandler");
    }
}