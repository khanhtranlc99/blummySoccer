using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace XGame
{
    //成功返回的数据项
    [Serializable]
    [Preserve]
    public class HttpGetSuccessResult
    {
        public string Data;
        public Dictionary<string, string> Header;
        public long StatusCode;

        public HttpGetSuccessResult(string data, Dictionary<string, string> header, long statusCode)
        {
            Data = data;
            Header = header;
            StatusCode = statusCode;
        }
    }

    /// <summary>
    /// http get
    /// </summary>
    public interface IHttpGet
    {
        void HttpGet(string url, Action<HttpGetSuccessResult> success, Action<string> fail,
            Dictionary<string, string> header, int timeOut);
    }
}