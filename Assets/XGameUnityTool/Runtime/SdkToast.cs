using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// sdk内部使用的toast
    /// </summary>
    [Preserve]
    public class SdkToast
    {
        /// <summary>
        /// Toast提示
        /// </summary>
        public static int Toast(string text, bool blocksRaycasts = false, float duration = 1.5f)
        {
            return XGameSdk.Instance.ShowToast(text, blocksRaycasts, duration);
        }
    }
}