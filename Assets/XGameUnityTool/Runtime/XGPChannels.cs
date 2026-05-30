// using UnityEngine;
//
// namespace XGame
// {
//     public class XGPChannels : ScriptableObject
//     {
//         public static string LOAD_PATH = "XGameUnityTool/XGPChannels";
//         public static string ASSET_DATABASE_PATH = $"Assets/XGameUnityTool_Gen/Resources/{LOAD_PATH}.asset";
//         // public int ByteDanceChannelID = -1;
//         public int DouyinXSDKChannelID = -1;
//
//
//         // public int WechatChannelID = -1;
//         public int WechatXSDKChannelID = -1;
//         // public int WechatASCChannelID = -1;
//
//         // public int OppoChannelID = -1;
//         // public int OppoASCChannelID = -1;
//         public int OppoXSDKChannelID = -1;
//
//         // public int VivoChannelID = -1;
//         // public int VivoASCChannelID = -1;
//         public int VivoXSDKChannelID = -1;
//
//
//         private static XGPChannels _instance;
//
//
//         public static int GetChannelID()
//         {
//             if (_instance == null)
//             {
//                 _instance = Resources.Load<XGPChannels>(LOAD_PATH);
//             }
//
//             if (_instance != null)
//             {
//                 switch (AppConfig.CHANNEL)
//                 {
//                     //微信小游戏
//                     // case AppChannel.WeChat:
//                     //     return _instance.WechatChannelID;
//                     // case AppChannel.WeChat_ASC:
//                     //     return _instance.WechatASCChannelID;
//                     case AppChannel.WeChat_XSDK:
//                         return _instance.WechatXSDKChannelID;
//                     //VIVO小游戏
//                     // case AppChannel.VivoMini:
//                     //     return _instance.VivoChannelID;
//                     // case AppChannel.Vivo_ASC:
//                     //     return _instance.VivoASCChannelID;
//                     case AppChannel.Vivo_XSDK:
//                         return _instance.VivoXSDKChannelID;
//                     //OPPO小游戏
//                     // case AppChannel.OppoMini:
//                     //     return _instance.OppoChannelID;
//                     // case AppChannel.Oppo_ASC:
//                     //     return _instance.OppoASCChannelID;
//                     case AppChannel.Oppo_XSDK:
//                         return _instance.OppoXSDKChannelID;
//                     //抖音小游戏
//                     // case AppChannel.ByteDanceMiniGameIOS:
//                     // case AppChannel.ByteDanceMiniGame:
//                     //     return _instance.ByteDanceChannelID;
//                     case AppChannel.Douyin_XSDK_Android:
//                     case AppChannel.Douyin_XSDK_IOS:
//                         return _instance.DouyinXSDKChannelID;
//                 }
//             }
//
//             return -1;
//         }
//     }
// }