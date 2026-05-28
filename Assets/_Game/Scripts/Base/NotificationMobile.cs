using System.Collections;
using System.Collections.Generic;
//#if UNITY_ANDROID
//using Unity.Notifications.Android;
//#endif
//#if UNITY_IOS
//using Unity.Notifications.iOS;
//#endif
using UnityEngine;

public class NotificationMobile : MonoBehaviour
{
//    public Mobile_AndroidNotification Mobile_AndroidNotification;
//    public Mobile_IOSNotification Mobile_IOSNotification;
//    void Awake()
//    {
//#if UNITY_ANDROID
//        this.Mobile_AndroidNotification.RequestAuthorization();
//        this.Mobile_AndroidNotification.RegisterNotificationChannel();
//#endif
//#if UNITY_IOS
//        StartCoroutine(this.Mobile_IOSNotification.RequestAuthorization());
//#endif
//    }

//    private void OnApplicationFocus(bool focus)
//    {
//        if (focus == false)
//        {
//#if UNITY_ANDROID
//            AndroidNotificationCenter.CancelAllNotifications();
//            this.Mobile_AndroidNotification.ScheduleAndroidNotification(12, 0, "Daily Noon Notification", "It's 12 PM! Time for your daily update.");
//            this.Mobile_AndroidNotification.ScheduleAndroidNotification(20, 0, "Daily Evening Notification", "It's 8 PM! Don't forget to check your progress.");
//#endif

//#if UNITY_IOS
//            iOSNotificationCenter.RemoveAllScheduledNotifications();
//            this.Mobile_IOSNotification.ScheduleIOSNotification(12, 0, "Daily Noon Notification", "It's 12 PM! Time for your daily update.");
//            this.Mobile_IOSNotification.ScheduleIOSNotification(20, 0, "Daily Evening Notification", "It's 8 PM! Don't forget to check your progress.");
//#endif
//        }
//    }
}
