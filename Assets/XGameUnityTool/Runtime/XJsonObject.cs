using System;
using System.Collections.Generic;

namespace XGame
{
    [Serializable]
    public class XJsonObject : Dictionary<string, object>
    {
        public string ToJson()
        {
            return XJson.ToJson(this);
        }

        public static XJsonObject FromJson(string json)
        {
            return XJson.FromJson<XJsonObject>(json);
        }
    }
}