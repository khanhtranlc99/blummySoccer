//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Google.Play.Review;

//public class InAppReviewManager : MonoBehaviour
//{
//    public static InAppReviewManager i;
//    private ReviewManager _reviewManager;
//    private PlayReviewInfo _playReviewInfo;


//    private void Awake()
//    {
//        _reviewManager = new ReviewManager();
//        i = this;
//    }
//    private IEnumerator Start()
//    {
//        var requestFlowOperation = _reviewManager.RequestReviewFlow();
//        yield return requestFlowOperation;
//        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
//        {
//            // Log error. For example, using requestFlowOperation.Error.ToString().
//            yield break;
//        }
//        _playReviewInfo = requestFlowOperation.GetResult();
//    }

//    public void Request()
//    {
//        //if (RateGame.Instance.CanShowRate())
//        //{
//            RateGame.Instance.ShowRatePopup();
//        //}

//    }
//    public IEnumerator RequestReview()
//    {
//        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
//        yield return launchFlowOperation;
//        _playReviewInfo = null; // Reset the object
//        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
//        {
//            // Log error. For example, using launchFlowOperation.Error.ToString().
//            yield break;
//        }
//        // The flow has finished. The API does not indicate whether the user
//        // reviewed or not, or even whether the review dialog was shown. Thus, no
//        // matter the result, we continue our app flow.
//    }
//}
