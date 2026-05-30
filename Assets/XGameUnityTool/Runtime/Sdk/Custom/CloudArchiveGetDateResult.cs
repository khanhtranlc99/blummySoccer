using System;

namespace XGame
{
    /// <summary>
    /// 云存档数据结果
    /// </summary>
    
    [Serializable]
    public struct CloudArchiveGetDateResult
    {
        public string Key;
        public long Version;
        public string Content;

        public CloudArchiveGetDateResult(string key, long version, string content)
        {
            Key = key;
            Version = version;
            Content = content;
        }
    }
}