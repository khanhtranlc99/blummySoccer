using System;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// demo sdk
    /// </summary>
    [Preserve]
    public class DemoSdk : BaseSdk
    {
        protected override void OnCreate()
        {
        }

        public override void InitSdk(Action success, Action fail = null)
        {
            Log("InitSdk 直接成功");
            success?.Invoke();
        }

        public override void LoginGoogle(Action<string ,string> success = null, Action fail = null)
        {
            Log("Login 直接成功");
            string userid = "12345353w4";
            string username = "sufjbsjf";
            success?.Invoke(userid,username);
        }

        public override void Login(Action success = null, Action fail = null)
        {
            Log("Login 直接成功");
            success?.Invoke();
        }

        private void Log(string content)
        {
            XGameSdk.Log($"[DEMO_SDK] {content}");
        }
    }
}