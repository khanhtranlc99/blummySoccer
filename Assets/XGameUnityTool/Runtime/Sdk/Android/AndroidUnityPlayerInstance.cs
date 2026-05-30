using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// Android UnityPlayer java实例
    /// </summary>
    [Preserve]
    public class AndroidUnityPlayerInstance
    {
        private const string ANDROID_JAVE_CLASS_NAME = "com.unity3d.player.UnityPlayer";

        public AndroidJavaClass AndroidJavaClass = null;
        public AndroidJavaObject AndroidJavaObject = null;


        public AndroidUnityPlayerInstance()
        {
            AndroidJavaClass = new AndroidJavaClass(ANDROID_JAVE_CLASS_NAME);
            AndroidJavaObject = AndroidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        #region 调用Android非静态方法

        //无参
        public void Call(string methodName)
        {
            DebugLog($"Call methodName:{methodName}");
            AndroidJavaObject.Call(methodName);
        }

        //单参数调用
        public void Call<T>(string methodName, T param)
        {
            DebugLog($"Call methodName:{methodName} param:{param}");
            AndroidJavaObject.Call(methodName, param);
        }

        //双参数调用
        public void Call<T, U>(string methodName, T param1, U param2)
        {
            DebugLog($"Call methodName:{methodName} param1:{param1} param2:{param2}");
            AndroidJavaObject.Call(methodName, param1, param2);
        }

        //三参数调用
        public void Call<T, U, V>(string methodName, T param1, U param2, V param3)
        {
            DebugLog($"Call methodName:{methodName} param1:{param1} param2:{param2} param3：{param3}");
            AndroidJavaObject.Call(methodName, param1, param2, param3);
        }

        //四参数调用
        public void Call<T ,U ,V ,B>(string methodName, T param1, U param2, V param3, B param4)
        {
            DebugLog($"Call methodName:{methodName} param1:{param1} param2:{param2} param3：{param3} param4：{param4}");
            AndroidJavaObject.Call(methodName, param1, param2, param3, param4);
        }
        
        public void Call<T ,U ,V ,B, C>(string methodName, T param1, U param2, V param3, B param4, C param5)
        {
            DebugLog($"Call methodName:{methodName} param1:{param1} param2:{param2} param3：{param3} param4：{param4} param5：{param5}");
            AndroidJavaObject.Call(methodName, param1, param2, param3, param4, param5);
        }
        
        //带返回值调用，无参
        public ReturnType CallReturn<ReturnType>(string methodName)
        {
            DebugLog($"CallReturn methodName:{methodName}");
            return AndroidJavaObject.Call<ReturnType>(methodName);
        }

        //带返回值调用，带一个参数
        public ReturnType CallReturn<ReturnType, T>(string methodName, T param1)
        {
            DebugLog($"CallReturn methodName:{methodName} param1:{param1}");
            return AndroidJavaObject.Call<ReturnType>(methodName, param1);
        }

        //带返回值调用，带两个参数
        public ReturnType CallReturn<ReturnType, T, T2>(string methodName, T param1, T2 param2)
        {
            DebugLog($"CallReturn methodName:{methodName} param1:{param1}  param2:{param2}");
            return AndroidJavaObject.Call<ReturnType>(methodName, param1,param2);
        }
        
        #endregion

        #region 调用Android静态方法

        //无参
        public void CallStatic(string methodName)
        {
            DebugLog($"CallStatic methodName:{methodName}");
            AndroidJavaObject.CallStatic(methodName);
        }

        //单参数调用
        public void CallStatic<T>(string methodName, T param)
        {
            DebugLog($"CallStatic methodName:{methodName} param:{param}");
            AndroidJavaObject.CallStatic(methodName, param);
        }


        //双参数调用
        public void CallStatic<T, U>(string methodName, T param1, U param2)
        {
            DebugLog($"CallStatic methodName:{methodName} param1:{param1} param2:{param2}");
            AndroidJavaObject.CallStatic(methodName, param1, param2);
        }


        //三参数调用
        public void CallStatic<T, U, V>(string methodName, T param1, U param2, V param3)
        {
            DebugLog($"CallStatic methodName:{methodName} param1:{param1} param2:{param2} param3：{param3}");
            AndroidJavaObject.CallStatic(methodName, param1, param2, param3);
        }

        #endregion

        #region 打印日志

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private void DebugLog(string log)
        {
            XGameSdk.Log($"[AndroidUnityPlayerInstance] {log}");
        }

        #endregion
    }
}