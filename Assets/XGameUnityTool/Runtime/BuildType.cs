namespace XGame
{
    /// <summary>
    /// 构建类型
    /// </summary>
    public enum BuildType
    {
        /// <summary>
        /// 基础版
        /// </summary>
        Base,

        /// <summary>
        /// 也叫α版，此版本主要是以实现软件功能为主，通常只在软件开发者内部交流，一般而言，该版本软件的Bug较多，需要继续修改。
        /// </summary>
        Alpha,

        /// <summary>
        /// 此版本已经相当成熟了，基本上不存在导致错误的BUG，与即将发行的正式版相差无几，测试人员基本通过的版本。
        /// </summary>
        Beta,

        /// <summary>
        /// 此版本已经相当成熟了，基本上不存在导致错误的BUG，与即将发行的正式版相差无几，测试人员基本通过的版本。
        /// </summary>
        RC,

        /// <summary>
        /// 此版本意味着“最终版本”、“上线版本”，，在前面版本的一系列测试版之后，终归会有一个正式版本，是最终交付用户使用的一个版本。该版本有时也称为标准版。一般情况下，Release不会以单词形式出现在软件封面上，取而代之的是符号(R)。
        /// </summary>
        Release,
    }
}