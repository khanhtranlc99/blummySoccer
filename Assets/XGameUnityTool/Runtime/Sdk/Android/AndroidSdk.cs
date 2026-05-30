// using UnityEngine;
// using UnityEngine.Scripting;
//
// namespace XGame
// {
//     /// <summary>
//     /// 安卓SDK实例
//     /// </summary>
//     [Preserve]
//     public abstract class AndroidSdk
//     {
//         private const string ANDROID_JAVE_CLASS_NAME = "com.unity3d.player.UnityPlayer";
//
//         public AndroidJavaClass AndroidJavaClass = null;
//         public AndroidJavaObject AndroidJavaObject = null;
//         public bool IsEnable;
//
//         protected AndroidSdk()
//         {
//         }
//
//
//         protected AndroidSdk(bool isEnable)
//         {
//             IsEnable = isEnable;
//             if (IsEnable)
//             {
//                 AndroidJavaClass = new AndroidJavaClass(ANDROID_JAVE_CLASS_NAME);
//                 AndroidJavaObject = AndroidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
//                 XGameSdk.Log("AndroidSdk instance 创建成功");
//             }
//         }
//
//         #region 调用Android非静态方法
//
//         //无参
//         public void Call(string methodName)
//         {
// #if UNITY_EDITOR
//             string log = string.Format("Call\t{0}", methodName);
//             DebugLog(log);
//             return;
// #endif
//             if (!IsEnable)
//             {
//                 return;
//             }
//
//             AndroidJavaObject.Call(methodName);
//         }
//
//         //单参数调用
//         public void Call<T>(string methodName, T param)
//         {
// #if UNITY_EDITOR
//             string log = string.Format("Call\t{0}\t[参数1]:{1}", methodName, param);
//             DebugLog(log);
//             return;
// #endif
//             if (!IsEnable)
//             {
//                 return;
//             }
//
//             AndroidJavaObject.Call(methodName, param);
//         }
//
//         //双参数调用
//         public void Call<T, U>(string methodName, T param1, U param2)
//         {
// #if UNITY_EDITOR
//             string log = string.Format("Call\t{0}\t[参数1]:{1}\t[参数2]:{2}", methodName, param1, param2);
//             DebugLog(log);
//             return;
// #endif
//             if (!IsEnable)
//             {
//                 return;
//             }
//
//             AndroidJavaObject.Call(methodName, param1, param2);
//         }
//
//         //三参数调用
//         public void Call<T, U, V>(string methodName, T param1, U param2, V param3)
//         {
// #if UNITY_EDITOR
//             string log = string.Format("Call\t{0}\t[参数1]:{1}\t[参数2]:{2}\t[参数3]:{3}", methodName, param1, param2,
//                 param3);
//             DebugLog(log);
//             return;
// #endif
//             if (!IsEnable)
//             {
//                 return;
//             }
//
//             AndroidJavaObject.Call(methodName, param1, param2, param3);
//         }
//
//         //带返回值调用，无参
//         public ReturnType CallReturn<ReturnType>(string methodName)
//         {
//             if (!IsEnable)
//             {
//                 return default(ReturnType);
//             }
//
//             return AndroidJavaObject.Call<ReturnType>(methodName);
//         }
//
//         //带返回值调用，带一个参数
//         public ReturnType CallReturn<ReturnType, T>(string methodName, T param1)
//         {
//             if (!IsEnable)
//             {
//                 return default(ReturnType);
//             }
//
//             return AndroidJavaObject.Call<ReturnType>(methodName, param1);
//         }
//
//         #endregion
//
//         #region 调用Android静态方法
//
//         //无参
//         public void CallStatic(string methodName)
//         {
// #if UNITY_EDITOR
//             string log = string.Format("CallStatic\t{0}", methodName);
//             DebugLog(log);
//             return;
// #endif
//             if (!IsEnable)
//             {
//                 return;
//             }
//
//             AndroidJavaObject.CallStatic(methodName);
//         }
//
//         //单参数调用
//         public void CallStatic<T>(string methodName, T param)
//         {
// #if UNITY_EDITOR
//             string log = string.Format("CallStatic\t{0}\t[参数1]:{1}", methodName, param);
//             DebugLog(log);
//             return;
// #endif
//             if (!IsEnable)
//             {
//                 return;
//             }
//
//             AndroidJavaObject.CallStatic(methodName, param);
//         }
//
//
//         //双参数调用
//         public void CallStatic<T, U>(string methodName, T param1, U param2)
//         {
// #if UNITY_EDITOR
//             string log = string.Format("CallStatic\t{0}\t[参数1]:{1}\t[参数2]:{2}", methodName, param1, param2);
//             DebugLog(log);
//             return;
// #endif
//             if (!IsEnable)
//             {
//                 return;
//             }
//
//             AndroidJavaObject.CallStatic(methodName, param1, param2);
//         }
//
//
//         //三参数调用
//         public void CallStatic<T, U, V>(string methodName, T param1, U param2, V param3)
//         {
// #if UNITY_EDITOR
//             string log = string.Format("CallStatic\t{0}\t[参数1]:{1}\t[参数2]:{2}\t[参数3]:{3}", methodName, param1, param2,
//                 param3);
//             DebugLog(log);
//             return;
// #endif
//             if (!IsEnable)
//             {
//                 return;
//             }
//
//             AndroidJavaObject.CallStatic(methodName, param1, param2, param3);
//         }
//
//         #endregion
//
//         #region 打印日志
//
//         private void DebugLog(string log)
//         {
//             Debug.Log("@@##xgamesdk:" + log);
//         }
//
//         #endregion
//     }
// }