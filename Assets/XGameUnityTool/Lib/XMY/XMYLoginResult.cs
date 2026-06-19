using System;
using UnityEngine.Scripting;

namespace XGame
{
    [Preserve]
    [Serializable]
    public class XMYLoginResult
    {
        //登录返回的userID
        public string uid;

        //登录返回的用户名
        public string name;

        //登录返回的用于登录认证的凭据
        public string token;
        
        //用户头像
        public string userPic;
        
    }
}