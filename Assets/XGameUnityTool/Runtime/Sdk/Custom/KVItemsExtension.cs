using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 拓展KVItems
    /// </summary>
    public static class KVItemsExtension
    {
        /// <summary>
        /// 转换为上报数据
        /// </summary>
        public static Dictionary<string, object> ToTrackDictionaryData(this KVItems items)
        {
            var result = new Dictionary<string, object>();
            foreach (var item in items)
            {
                var key = item.Key;
                var v = item.Value;
                if (v == null)
                {
                    result[key.ToString()] = 1;
                    continue;
                }
                if (v is float || v is double || v is int || v is long)
                {
                    result[key.ToString()] = v;
                }
                else if (v is bool)
                {
                    result[key.ToString()] = (bool)v ? "true" : "false";
                }
                else
                {
                    result[key.ToString()] = v.ToString();
                }
            }

            return result;
        }
    }
}