using System;

namespace XGame
{
    /// <summary>
    /// 触发回调
    /// </summary>
    /// <param name="t"></param>
    /// <param name="clear"></param>
    public class ActionHandler
    {
        public Action Complete;

        public ActionHandler(Action complete)
        {
            Complete = complete;
        }

        /// <summary>
        /// 清理回调
        /// </summary>
        public void Clear()
        {
            Complete = null;
        }

        /// <summary>
        /// 触发回调
        /// </summary>
        /// <param name="t"></param>
        /// <param name="clear"></param>
        public void Invoke(bool clear = true)
        {
            Complete?.Invoke();
            if (clear)
            {
                Clear();
            }
        }
    }

    /// <summary>
    /// ActionHandle
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionHandler<T>
    {
        public Action<T> Complete;

        public ActionHandler(Action<T> complete)
        {
            Complete = complete;
        }

        /// <summary>
        /// 清理回调
        /// </summary>
        public void Clear()
        {
            Complete = null;
        }

        /// <summary>
        /// 触发回调
        /// </summary>
        /// <param name="t"></param>
        /// <param name="clear"></param>
        public void Invoke(T t, bool clear = true)
        {
            Complete?.Invoke(t);
            if (clear)
            {
                Clear();
            }
        }
    }
}