using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


namespace XGame
{
    public class XHttpClient : MonoBehaviour
    {
        public static XHttpClient CreateInstance()
        {
            return new GameObject(nameof(XHttpClient)).AddComponent<XHttpClient>();
        }

        /// <summary>
        /// post 请求
        /// </summary>
        public void Post(string url, string postData, Action<HttpPostSuccessResult> success, Action<string> fail,
            Dictionary<string, string> header = null, int timeOut = 60)
        {
            //默认走unity 的post
            StartCoroutine(IEUnityHttpPost(url, postData, success, fail, header, timeOut));
        }

        private IEnumerator IEUnityHttpPost(string url, string data, Action<HttpPostSuccessResult> success,
            Action<string> fail, Dictionary<string, string> header, int timeOut)
        {
            var uri = new Uri(url);
#if UNITY_2022_2_OR_NEWER
            using (UnityWebRequest www = UnityWebRequest.PostWwwForm(uri, data))
#else
            using (UnityWebRequest www = UnityWebRequest.Post(uri, data))
#endif
            {
                try
                {
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
                    www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                }
                catch (Exception e)
                {
                }
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                if (timeOut > 0)
                {
                    www.timeout = timeOut;
                }

                if (header != null)
                {
                    foreach (var item in header)
                    {
                        www.SetRequestHeader(item.Key, item.Value);
                    }
                }

                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    //错误
                    fail?.Invoke(www.error);
                }
                else
                {
                    //成功
                    var resultData = new HttpPostSuccessResult(www.downloadHandler.text, www.GetResponseHeaders(),
                        www.responseCode);
                    success?.Invoke(resultData);
                }
            }
        }


        /// <summary>
        /// get 请求
        /// </summary>
        public void Get(string url, Action<HttpGetSuccessResult> success, Action<string> fail,
            Dictionary<string, string> header = null, int timeOut = 60)
        {
            StartCoroutine(IEUnityHttpGet(url, success, fail, header, timeOut));
        }


        private IEnumerator IEUnityHttpGet(string url, Action<HttpGetSuccessResult> success, Action<string> fail,
            Dictionary<string, string> header, int timeOut)
        {
            var uri = new Uri(url);
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                //设置请求头
                if (header != null)
                {
                    foreach (var kv in header)
                    {
                        www.SetRequestHeader(kv.Key, kv.Value);
                    }
                }

                //设置超时
                if (timeOut > 0)
                {
                    www.timeout = timeOut;
                }

                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    //失败
                    fail?.Invoke(www.error);
                }
                else
                {
                    //成功
                    var resultData = new HttpGetSuccessResult(www.downloadHandler.text, www.GetResponseHeaders(),
                        www.responseCode);
                    success?.Invoke(resultData);
                }
            }
        }
    }
}