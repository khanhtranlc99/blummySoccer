using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// xmy google 云存档数据
    /// </summary>
    [Preserve]
    [Serializable]
    public class XMYGoogleCloudArchiveData : List<ArchiveDataItem>
    {
    }

    [Preserve]
    [Serializable]
    public class ArchiveDataItem
    {
        public string key;
        public long version;
        public string data;
    }
}