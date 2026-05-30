using System;

namespace XGame
{
    /// <summary>
    /// 广告结果
    /// </summary>
    public class ADResultHandler : IADResultHandler, IADResultHandlerInvoker
    {
        public event Action OnShowSuccess = null;
        public event Action OnShowFail = null;
        public event Action OnAdClicked = null;
        public event Action OnAdClosed = null;
        private string _handlerName = "unknown";

        public ADResultHandler(string handlerName)
        {
            _handlerName = handlerName;
        }

        public void InvokeOnShowSuccess()
        {
            XGameSdk.Log($"{_handlerName} OnShowSuccess");
            OnShowSuccess?.Invoke();
        }

        public void InvokeOnShowFail()
        {
            XGameSdk.Log($"{_handlerName} OnShowFail");
            OnShowFail?.Invoke();
        }

        public void InvokeOnAdClicked()
        {
            XGameSdk.Log($"{_handlerName} OnAdClicked");
            OnAdClicked?.Invoke();
        }

        public void InvokeOnAdClosed()
        {
            XGameSdk.Log($"{_handlerName} OnAdClosed");
            OnAdClosed?.Invoke();
        }
    }

    public interface IADResultHandler
    {
        /// <summary>
        /// 广告展示成功时触发
        /// </summary>
        event Action OnShowSuccess;

        /// <summary>
        /// 广告展示失败时触发
        /// </summary>
        event Action OnShowFail;

        /// <summary>
        /// 广告点击时触发
        /// </summary>
        event Action OnAdClicked;

        /// <summary>
        /// 广告关闭时触发
        /// </summary>
        event Action OnAdClosed;
    }

    public interface IADResultHandlerInvoker
    {
        void InvokeOnShowSuccess();
        void InvokeOnShowFail();
        void InvokeOnAdClicked();
        void InvokeOnAdClosed();
    }
}