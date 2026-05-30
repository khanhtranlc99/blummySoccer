// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using XGP.Model;
//
// namespace XGame
// {
//     /// <summary>
//     /// 开发模式下 XGP API 测试
//     /// </summary>
// //     public class XGPApiDevelopment : MonoBehaviour, IXGPApi
// //     {
// //         /// <summary>
// //         /// 登录接口响应结果
// //         /// </summary>
// //         [Serializable]
// //         public class LoginRspData
// //         {
// //             [Serializable]
// //             public class Data
// //             {
// //                 public string open_id { get; set; }
// //                 public User user { get; set; }
// //                 public long account_id { get; set; }
// //                 public Account account_info { get; set; }
// //             }
// //
// //             public string ret { get; set; }
// //             public Data data { get; set; }
// //         }
// //
// //         //user api 地址
// //         private static string _user_api_base_address = XGPSetting.DevelopmentAddress.UserApiBaseAddress;
// //
// //         //登录
// //         private static string _login_url = $"{_user_api_base_address}/login/platform_login";
// //
// //
// //         //开发环境下测试的channel
// //         private static long _development_channel_id = 20;
// //
// //         private static XGPApiDevelopment _instance = null;
// //         private static object _lock = new object();
// //
// //         public static XGPApiDevelopment Instance
// //         {
// //             get
// //             {
// //                 if (_instance == null)
// //                 {
// //                     lock (_lock)
// //                     {
// //                         if (_instance == null)
// //                         {
// //                             GameObject clone = new GameObject("XGPApiDevelopment");
// //                             _instance = clone.AddComponent<XGPApiDevelopment>();
// //                             _instance.Init();
// //                         }
// //                     }
// //                 }
// //
// //                 return _instance;
// //             }
// //         }
// //
// //         private XHttpClient _httpClient;
// //
// //         private SdkPreference _sdkPreference;
// //
// //         //换粗的登录信息
// //         private long user_id;
// //         private string user_token;
// //         private long account_id;
// //
// //         private void Init()
// //         {
// //             _httpClient = XHttpClient.CreateInstance();
// //             DontDestroyOnLoad(gameObject);
// //             DontDestroyOnLoad(_httpClient.gameObject);
// // #if UNITY_EDITOR
// //             _sdkPreference = SdkPreference.Global;
// //             if (_sdkPreference == null)
// //             {
// //                 throw new Exception("找不到 SdkPreference 设置，请点击 \"XGame/设置\" 进行部署");
// //             }
// // #endif
// //         }
// //
// //         //登录
// //         public void XGPLogin(Action success, Action fail)
// //         {
// //             var username = _sdkPreference.GetXGPApiUserName();
// //             var body = new
// //             {
// //                 channel_id = 20,
// //                 data = new
// //                 {
// //                     plug = 1,
// //                     info = new
// //                     {
// //                         username = username,
// //                         password = "test",
// //                     }
// //                 }
// //             };
// //
// //             _httpClient.Post(_login_url, XJson.ToJson(body), (rsp) =>
// //                 {
// //                     // LogInDevelopment($"登录响应结果：{XJson.ToJson(rsp)} \ndata:{rsp.Data}");
// //                     if (rsp.StateCode == 200)
// //                     {
// //                         var rspData = XJson.FromJson<LoginRspData>(rsp.Data);
// //                         //缓存登录信息
// //                         account_id = rspData.data.account_id;
// //                         user_id = rspData.data.user.user_id;
// //                         user_token = rspData.data.user.token;
// //                         success?.Invoke();
// //                         LogInDevelopmentSuccess($"登录成功:{rspData.ToXJson()}");
// //                     }
// //                     else
// //                     {
// //                         fail?.Invoke();
// //                         LogInDevelopmentError($"登录失败 state code:{rsp.StateCode}");
// //                     }
// //                 },
// //                 (err) => { LogInDevelopmentError($"登录失败 {err}"); });
// //         }
// //
// //         public void XGPApi_User(string route, object body, Action<XGPApiResponse> success,
// //             Action<string> fail)
// //         {
// //             if (string.IsNullOrEmpty(user_token))
// //             {
// //                 XGPLogin(() => XGPApi_User(route, body, success, fail), null);
// //                 return;
// //             }
// //
// //             if (!route.StartsWith("/"))
// //             {
// //                 route = $"/{route}";
// //             }
// //
// //             var url = $"{_user_api_base_address}{route}";
// //             var postBody = new
// //             {
// //                 channel_id = _development_channel_id,
// //                 user_id,
// //                 user_token,
// //                 account_id,
// //                 data = body,
// //             };
// //             LogInDevelopment($"请求:{new { url, body = postBody }.ToXJson()}");
// //
// //             _httpClient.Post(url, XJson.ToJson(postBody), (rsp) =>
// //                 {
// //                     LogInDevelopmentSuccess(
// //                         $"响应成功:{new { requeset = new { url, body = postBody, }, response = rsp, }.ToXJson()}");
// //                     success?.Invoke(new XGPApiResponse(rsp));
// //                 },
// //                 (err) =>
// //                 {
// //                     LogInDevelopmentError($"响应失败：{err}");
// //                     fail?.Invoke(err);
// //                 });
// //         }
// //
// //         private static void LogInDevelopment(string content)
// //         {
// //             Debug.Log($"<color=#26b2f3>[XGP API] {content}</color>");
// //         }
// //
// //         private static void LogInDevelopmentSuccess(string content)
// //         {
// //             Debug.Log($"<color=#50d67c>[XGP API] {content}</color>");
// //         }
// //
// //         private static void LogInDevelopmentError(string content)
// //         {
// //             Debug.Log($"<color=#e84033>[XGP API] {content}</color>");
// //         }
// //     }
// }