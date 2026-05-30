using System;

namespace XGame
{
    /// <summary>
    /// 云存档keys信息
    /// </summary>
    [Serializable]
    public struct CloudArchiveGetKeysResult
    {
        public ArchiveKeyInfo[] KeyVersion;

        public CloudArchiveGetKeysResult(ArchiveKeyInfo[] keyVersion)
        {
            KeyVersion = keyVersion;
        }
    }
}