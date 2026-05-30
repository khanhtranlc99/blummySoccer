using XGame.BuildApp;

namespace XGame
{
    /// <summary>
    /// 发布结果
    /// </summary>
    public class PublishResult
    {
        /// <summary>
        /// 打包设置
        /// </summary>
        public BuildAppSetting AppSetting;

        /// <summary>
        /// apk目录
        /// </summary>
        public string ApkPath;

        /// <summary>
        /// android工程目录
        /// </summary>
        public string AndroidProjectPath;

        /// <summary>
        /// aab生成路径
        /// </summary>
        public string AABPath;

        /// <summary>
        /// aar路径
        /// </summary>
        public string AARPath;

        /// <summary>
        /// XCode工程
        /// </summary>
        public string XCodeProjectPath;
        
        /// <summary>
        /// web工程
        /// </summary>
        public string webProjectPath;
        
        /// <summary>
        /// 微信小游戏工程
        /// </summary>
        public string WXMiniProjectPath;

        /// <summary>
        /// oppo 小游戏 webgl工程路径
        /// </summary>
        public string OppoMinWebglProjectPath;

        /// <summary>
        /// oppo 小游戏 quick game工程路径
        /// </summary>
        public string OppoMiniQuickGameProjectPath;

        /// <summary>
        /// oppo 小游戏 rpk  导出路径
        /// </summary>
        public string OppoMiniRpkPath;
        
        
        /// <summary>
        /// 鸿蒙工程目录
        /// </summary>
        public string OpenHarmonyProjectPath;
        
        /// <summary>
        /// 鸿蒙hap目录
        /// </summary>
        public string HapPath;
        
    }
}