using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace XGame
{
    [Preserve]
    public class XMYAdBridge
    {
        private AndroidJavaObject _bridge = null;

        public XMYAdBridge()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.union.sdk.unity.helper.AdBridge");
            _bridge = jc.CallStatic<AndroidJavaObject>("getInstance");
        }

        /// <summary>
        /// 带返回值
        /// </summary>
        public T CallReturn<T>(string method, params object[] param)
        {
            try
            {
                return _bridge.Call<T>(method, param);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return default(T);
        }

        /// <summary>
        /// 普通方法
        /// </summary>
        public void Call(string method, params object[] param)
        {
            try
            {
                _bridge.Call(method, param);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}