using System;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 反射创建WXTouchInputOverride,适配微信端UGUI多点触控问题
    /// </summary>
    public class WXTouchInputAdapter : MonoBehaviour
    {
        private void Awake()
        {
            bool inUnityEditor = false;

#if UNITY_EDITOR
            inUnityEditor = true;
#endif
            if (!inUnityEditor)
            {
                switch (AppConfig.CHANNEL)
                {
                    case AppChannel.WeChat:
                    case AppChannel.WeChat_ASC:
                    case AppChannel.WeChat_XSDK:
                    {
                        var type = Type.GetType("WXTouchInputOverride,Wx");

                        if (type != null)
                        {
                            Debug.Log("#xgame sdk 创建：WXTouchInputOverride 成功");
                            gameObject.AddComponent(type);
                        }
                        else
                        {
                            Debug.Log("#xgame sdk 创建：WXTouchInputOverride 失败");
                        }
                    }
                        break;
                }
            }
        }

#if UNITY_EDITOR
        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            GUILayout.Label("说明：\n反射创建WXTouchInputOverride\n适配微信小游戏UGUI多点触控问题");
        }
#endif
    }
}