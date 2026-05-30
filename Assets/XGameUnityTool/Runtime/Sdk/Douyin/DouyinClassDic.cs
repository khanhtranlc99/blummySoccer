using System;

namespace XGame
{
    // [Serializable]
    // public enum FeedDirectPlayChange
    // {
    //     FeedToGame,
    //     GameToFeed,
    // }
    
    [Serializable]
    public class FeedDirectPlayInfo
    {
        [Serializable]
        public enum GameScene
        {
            /// <summary>
            /// 离线收益场景 
            /// </summary>
            OfflineEarnings = 1,
            /// <summary>
            /// 体力恢复场景
            /// </summary>
            StaminaRecovery = 2,
            /// <summary>
            /// 重要事件掉落
            /// </summary>
            ImportantEventDrops = 3
        }
        
        /// <summary>
        /// 游戏启动需要到达的场景
        /// </summary>
        public GameScene gameScene;
        
        /// <summary>
        /// 开发者自定义字段，可通过 推荐流直出能力 OpenAPI 接入文档 接口的 extra 字段进行赋值
        /// </summary>
        public string extra;
        /// <summary>
        /// 本次启动对应的文案 ID
        /// </summary>
        public string contentId;
        /// <summary>
        /// 1:复访用户,2:获客用户
        /// </summary>
        public string channel;

    }
    
    
    
    
    
}