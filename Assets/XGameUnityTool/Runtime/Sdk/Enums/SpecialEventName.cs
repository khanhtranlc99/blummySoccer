namespace XGame
{
    /// <summary>
    /// 特殊事件上报的事件类型
    /// </summary>
    public enum SpecialEventName
    {
        /// <summary>
        /// 主动拉起分享时上报事件
        /// </summary>
        ShareAction = 1,
        /// <summary>
        /// 完成新手引导时上报事件
        /// </summary>
        TutorialFinish,
        /// <summary>
        /// 游戏可玩，比如进入游戏大厅
        /// </summary>
        GameReady,
        /// <summary>
        /// 用户点击看激励视频广告按钮，或视频广告点位被点击时上报
        /// </summary>
        ClickVideo,
        /// <summary>
        /// 新手教程开始时上报
        /// </summary>
        TutorialStart,
        /// <summary>
        /// 游戏加载并渲染出首帧时上报
        /// </summary>
        LoadingFirstFrame,
        /// <summary>
        /// 进入关卡时上报
        /// </summary>
        LevelEnter,
        /// <summary>
        /// 中途退出关卡时上报
        /// </summary>
        LevelExit,
        /// <summary>
        /// 关卡失败时上报
        /// </summary>
        LevelLose,
        /// <summary>
        /// 通过关卡时上报
        /// </summary>
        LevelPass,
        /// <summary>
        /// 视频广告点位出现时上报
        /// </summary>
        AdPlacementShow,
        /// <summary>
        /// 视频广告播放结束时上报
        /// </summary>
        AdVideoFinish,
    }
    
    /// <summary>
    /// 特殊事件上报的事件属性
    /// </summary>
    public static class SpecialEventPropKey
    {
        /// <summary>
        /// 场景耗时，整型，单位ms
        /// </summary>
        public const string COST_TIME_MS = "CostTimeMs";
    }
    
}