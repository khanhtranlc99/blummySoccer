namespace XGame
{
    public interface ICallBackHandler : IGetCallBackID
    {
        void Execute(object msg);
    }
}