using XGame.BuildApp;

namespace XGame
{
    /// <summary>
    /// 当点击应用完成时触发
    /// </summary>
    public abstract class OnSubmitComplete
    {
        public abstract void OnBeforeSubmit(BuildAppSetting data);
        public abstract void OnSubmit(BuildAppSetting data);
    }
}