using System;
using UnityEngine.Scripting;

namespace XGame
{
    [Serializable]
    [Preserve]
    public class ArchiveKeyInfo
    {
        public string key;
        public long version;

        public ArchiveKeyInfo(string key, long version)
        {
            this.key = key;
            this.version = version;
        }

        public ArchiveKeyInfo()
        {
        }
    }
}