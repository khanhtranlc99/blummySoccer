using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace XGame
{
    [Serializable]
    [Preserve]
    public class SessionItem
    {
        #region 静态

        /*session map*/
        public static Dictionary<long, SessionItem> SessionsMap = new Dictionary<long, SessionItem>();

        /*处理回调*/
        public static void Invoke(long session, string json)
        {
            if (SessionsMap.TryGetValue(session, out var match))
            {
                match.OnExecute?.Invoke(json);
                match.Dispose();
                KillSession(session);
            }
        }

        /*清理Session*/
        public static void KillSession(long session)
        {
            if (SessionsMap.TryGetValue(session, out var match))
            {
                match.Dispose();
                SessionsMap.Remove(session);
            }
        }


        public static long _sessionId = 0;

        #endregion

        /*SessionID*/
        public readonly long SessionId;

        /*方法名*/
        public readonly string MethodName;

        /*数据*/
        private object _sendData;

        /*回调*/
        private Action<string> _onExecute;

        public object SendData => _sendData;

        /*回调逻辑*/
        public Action<string> OnExecute => _onExecute;

        public SessionItem(string method, object sendData, Action<string> cb)
        {
       
            _sessionId += 1;
            SessionId = _sessionId;
            MethodName = method;
            _sendData = sendData;
            _onExecute = cb;
            if (_onExecute != null)
            {
                //有回调，绑定到SessionsMap
                SessionsMap.Add(SessionId, this);
            }
        }

        public long GetSession()
        {
            return SessionId;
        }

        public void Dispose()
        {
            _sendData = null;
            _onExecute = null;
        }
    }
}