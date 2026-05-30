// using System;
// using System.Collections.Generic;
//
// namespace XGame
// {
//     public class SessionInvoker
//     {
//         private static long _session = 0;
//         private static long _cacheSession = -1;
//
//         private static Dictionary<long, ISessionCallBackResult> _sessionCallBackResults =
//             new Dictionary<long, ISessionCallBackResult>();
//
//         public static ISessionCallBackResult Bind<T>(Action<T> execute)
//         {
//             _session += 1;
//             var session = new SessionCallBackResult<T>(_session, execute);
//             _sessionCallBackResults[_session] = session;
//             return session;
//         }
//
//
//         public static void ReceiveSession(long session)
//         {
//             _cacheSession = session;
//         }
//
//         public static void Invoke(string msg)
//         {
//             XGameSdk.Log($"SessionInvoker:{msg}");
//             if (_cacheSession >= 0)
//             {
//                 var result = _sessionCallBackResults[_cacheSession];
//                 _cacheSession = -1;
//                 result.Parser(msg);
//                 result.Execute();
//                 _sessionCallBackResults.Remove(result.Session);
//             }
//         }
//     }
// }