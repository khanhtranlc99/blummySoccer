// using System;
// using UnityEngine;
// using UnityEngine.Scripting;
//
// namespace XGame
// {
//     /// <summary>
//     /// 自定义toast
//     /// </summary>
//     [Preserve]
//     public class CustomToast : MonoBehaviour
//     {
//         private static XGameSdkToast _instance = null;
//
//         private static XGameSdkToast GetInstance()
//         {
//             if (_instance == null)
//             {
//                 var res = Resources.Load<GameObject>("XGameUnityTool/CustomToast");
//                 if (res == null)
//                 {
//                     var inEditor = false;
// #if UNITY_EDITOR
//                     inEditor = true;
// #endif
//                     if (inEditor)
//                     {
//                         throw new Exception("找不到XGameUnityTool/SDKToast，请从上方菜单执行： XGameUnityTool/导入Toast模块");
//                     }
//                     else
//                     {
//                         _instance = SdkToast.GetToast();
//                         SdkToast.Toast("please import toast.package");
//                     }
//                 }
//                 else
//                 {
//                     var clone = GameObject.Instantiate(res);
//                     GameObject.DontDestroyOnLoad(clone);
//                     _instance = clone.AddComponent<XGameSdkToast>();
//                     _instance.Initialize();
//                 }
//             }
//
//             return _instance;
//         }
//
//
//         /// <summary>
//         /// Toast提示
//         /// </summary>
//         public static int Toast(string text, bool blocksRaycasts = false, float duration = 1f)
//         {
//             return GetInstance().ShowToast(text, blocksRaycasts, duration);
//         }
//
//         /// <summary>
//         /// Toast提示
//         /// </summary>
//         public static XGameSdkToast GetToast()
//         {
//             return GetInstance();
//         }
//     }
// }

