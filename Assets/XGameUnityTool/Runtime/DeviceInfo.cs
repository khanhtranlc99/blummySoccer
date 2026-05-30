using System;
using UnityEngine.Scripting;

namespace XGame
{
    [Serializable]
    [Preserve]
    public class DeviceInfo
    {
        [Obsolete("这个属性已被废弃，请使用 gaid 替代。")]
        public string GAID;
        /// <summary>
        /// 谷歌广告唯一ID
        /// </summary>
        public string gaid;
        /// <summary>
        /// 可用内存GB
        /// </summary>
        public double availRuntimeMemory;
        /// <summary>
        /// 总内存GB
        /// </summary>
        public string systemRuntimeMemory;
        /// <summary>
        /// 游戏版本
        /// </summary>
        public string gameVersion;
        /// <summary>
        /// 游戏版本号
        /// </summary>
        public int gameCode;
        /// <summary>
        /// 游戏包名
        /// </summary>
        public string gamePackage;
        /// <summary>
        /// 游戏sha1签名
        /// </summary>
        public string gameSHA1;
        /// <summary>
        /// 国家代码
        /// </summary>
        public string country;
        
        
    }
}