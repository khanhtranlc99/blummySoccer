// using System;
//
// namespace XGame
// {
//     /// <summary>
//     /// XGP API 响应结果
//     /// </summary>
//     [Serializable]
//     public class XGPApiResponse
//     {
//         /// <summary>
//         /// Http响应元数据
//         /// </summary>
//         public HttpPostSuccessResult HttpResponse;
//
//         /// <summary>
//         /// body string
//         /// </summary>
//         public string BodyString => HttpResponse.Data;
//
//         /// <summary>
//         /// 状态码
//         /// </summary>
//         public long StateCode => HttpResponse.StateCode;
//
//         /// <summary>
//         /// 是否成功
//         /// </summary>
//         public bool IsSuccess => StateCode == 200;
//
//         public XGPApiResponse(HttpPostSuccessResult result)
//         {
//             HttpResponse = result;
//         }
//
//         /// <summary>
//         /// 解析为指定类型
//         /// </summary>
//         public T ReadBodyAs<T>()
//         {
//             return XJson.FromJson<T>(BodyString);
//         }
//
//         /// <summary>
//         /// 转XGPApiResponse<T>
//         /// </summary>
//         public XGPApiResponse<T> ToResponse<T>()
//         {
//             return new XGPApiResponse<T>(HttpResponse);
//         }
//     }
//
//     /// <summary>
//     /// XGP API 响应结果
//     /// </summary>
//     [Serializable]
//     public class XGPApiResponse<T> : XGPApiResponse
//     {
//         public T Body;
//
//         public XGPApiResponse(HttpPostSuccessResult result) : base(result)
//         {
//             if (IsSuccess)
//             {
//                 Body = ReadBodyAs<T>();
//             }
//         }
//     }
// }
//
//
// /// <summary>
// /// xgp数据
// /// </summary>
// namespace XGP.Model
// {
//     [Serializable]
//     public class User
//     {
//         public long last_login_time { get; set; }
//         public long user_id { get; set; }
//         public string name { get; set; }
//         public string token { get; set; }
//     }
//
//     [Serializable]
//     public class Account
//     {
//     }
//
//
//     [Serializable]
//     public class Product
//     {
//         public int product_id;
//         public string product_no;
//         public int price;
//         public string name;
//     }
//
//     [Serializable]
//     public class OrderInfo
//     {
//         public int order_id;
//         public Product product;
//     }
//
//     [Serializable]
//     public class PayOrders
//     {
//         public int orders_num;
//         public OrderInfo[] orders;
//     }
//
//     [Serializable]
//     public class OrderInfoIOS
//     {
//         public int order_id;
//         public Product product;
//         public string pay_url;
//     }
// }