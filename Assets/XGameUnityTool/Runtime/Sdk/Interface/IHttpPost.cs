using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace XGame
{
    //成功返回的数据项
    [Serializable]
    [Preserve]
    public class HttpPostSuccessResult
    {
        //数据
        public string Data;

        //请求头
        public Dictionary<string, string> Header;

        //状态码
        public long StateCode;

        public HttpPostSuccessResult(string data, Dictionary<string, string> header, long stateCode)
        {
            Data = data;
            Header = header;
            StateCode = stateCode;
        }
    }

    /// <summary>
    /// post 接口
    /// </summary>
    public interface IHttpPost
    {
        void HttpPost(string url, string data, Action<HttpPostSuccessResult> success, Action<string> fail,
            Dictionary<string, string> header, int timeOut);
    }
}