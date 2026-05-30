// using System;
//
// namespace XGame
// {
//     public interface IExecute
//     {
//         void Execute();
//     }
//
//
//     public interface IParser
//     {
//         void Parser(string json);
//     }
//
//     public interface ISessionCallBackResult : IParser, IExecute
//     {
//         long Session { get; set; }
//     }
//
//
//     [Serializable]
//     public class SessionCallBackResult<T> : ISessionCallBackResult
//     {
//         public long Session { get; set; }
//
//         public T Rsp;
//
//         private Action<T> _onExecute;
//
//
//         public SessionCallBackResult(long session, Action<T> onExecute)
//         {
//             Session = session;
//             _onExecute = onExecute;
//         }
//
//         public void Execute()
//         {
//             _onExecute?.Invoke(Rsp);
//         }
//
//
//         public void Parser(string json)
//         {
//             Rsp = XJson.FromJson<T>(json);
//         }
//     }
// }