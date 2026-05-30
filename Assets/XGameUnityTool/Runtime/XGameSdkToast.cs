// using System;
// using TMPro;
// using UnityEngine;
//
// namespace XGame
// {
//     /// <summary>
//     /// Toast
//     /// </summary>
//     public class XGameSdkToast : MonoBehaviour
//     {
//         public CanvasGroup CanvasGroup;
//         public RectTransform RectToast;
//         public TextMeshProUGUI TxtContent;
//         private const float FADE_TIME = 0.15f;
//         private const float FADE_HIDE_TIME = 0.5f;
//         public bool IsShow = false;
//         public float PlayingTime = 0;
//         public float Duration = 0;
//         public int ToastInstanceID = 0;
//
//         public void Initialize()
//         {
//             CanvasGroup = gameObject.GetComponent<CanvasGroup>();
//             RectToast = transform.Find("Toast") as RectTransform;
//             TxtContent = transform.Find("Toast/Img/TxtContent").GetComponent<TextMeshProUGUI>();
//         }
//
//         public void Hide()
//         {
//             CanvasGroup.alpha = 0;
//             CanvasGroup.blocksRaycasts = false;
//             IsShow = false;
//         }
//
//         public int ShowToast(string content, bool blocksRaycasts = false, float duration = 1f)
//         {
//             ToastInstanceID++;
//             TxtContent.text = content;
//             IsShow = true;
//             Duration = duration;
//             PlayingTime = 0;
//             CanvasGroup.blocksRaycasts = blocksRaycasts;
//             return ToastInstanceID;
//         }
//
//         private void Update()
//         {
//             if (!IsShow)
//             {
//                 return;
//             }
//
//             PlayingTime += Time.deltaTime;
//             if (PlayingTime >= 0 && PlayingTime < FADE_TIME)
//             {
//                 var percent = Mathf.Clamp01(Mathf.InverseLerp(0, FADE_TIME, PlayingTime));
//                 //插值显示
//                 var scaleAlpha = Mathf.Lerp(0, 1, percent);
//                 RectToast.localScale = Vector3.one * scaleAlpha;
//                 CanvasGroup.alpha = scaleAlpha;
//             }
//             else if (PlayingTime > FADE_TIME && PlayingTime < (FADE_TIME + Duration))
//             {
//                 //持续显示
//                 CanvasGroup.alpha = 1;
//                 RectToast.localScale = Vector3.one;
//             }
//             else
//             {
//                 var hideTime = PlayingTime - (FADE_TIME + Duration);
//                 var percent = Mathf.Clamp01(Mathf.InverseLerp(0, FADE_HIDE_TIME, hideTime));
//                 var alpha = Mathf.Lerp(1, 0, percent);
//                 CanvasGroup.alpha = alpha;
//             }
//
//             if (PlayingTime > (FADE_TIME + FADE_HIDE_TIME + Duration))
//             {
//                 //时间到，隐藏
//                 Hide();
//             }
//         }
//     }
// }

