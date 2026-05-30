namespace XGame
{
    /// <summary>
    /// 发布完成后触发
    /// </summary>
    public abstract class OnPublishComplete
    {
        public abstract void OnPublish(PublishResult data);
    }
}