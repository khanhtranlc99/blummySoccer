// using System.Collections.Generic;
//
// namespace XGame
// {
//     /// <summary>
//     /// callback handler
//     /// </summary>
//     public abstract class CallBackHandler : ICallBackHandler
//     {
//         private static Dictionary<long, ICallBackHandler> _handlers = new Dictionary<long, ICallBackHandler>();
//         private static long _callBackId = 0;
//
//         public static void Run(long callbackId, object data)
//         {
//             if (_handlers.ContainsKey(callbackId))
//             {
//                 _handlers[callbackId].Execute(data);
//             }
//         }
//
//         protected CallBackHandler()
//         {
//             //id+1
//             _callBackId++;
//             _handlers[GetCallBackID()] = this;
//         }
//
//         public long GetCallBackID()
//         {
//             return _callBackId;
//         }
//
//         //执行回调
//         public void Execute(object data)
//         {
//             //从字典中移除
//             _handlers.Remove(GetCallBackID());
//             OnExecute(data);
//         }
//
//         //处理
//         protected abstract void OnExecute(object data);
//     }
// }