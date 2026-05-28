using System;
using System.Collections;
//#if UNITY_IOS
//using Unity.Notifications.iOS;
//#endif
using UnityEngine;

public class Mobile_IOSNotification : MonoBehaviour
{
//#if UNITY_IOS
//    void Start()
//    {
//        // Đặt thông báo cho iOS
//        ScheduleIOSNotification(12, 0, "Daily Noon Notification", "It's 12 PM! Time for your daily update.");
//        ScheduleIOSNotification(20, 0, "Daily Evening Notification", "It's 8 PM! Don't forget to check your progress.");
//    }
//    public IEnumerator RequestAuthorization()
//    {
//        using var request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true);
//        while (!request.IsFinished)
//        {
//            yield return null;
//        }
//    }

//    public void ScheduleIOSNotification(int hour, int minute, string title, string text)
//    {
//        // Đặt thời gian cho thông báo
//        var timeToNotify = DateTime.Today.AddHours(hour).AddMinutes(minute);
//        if (DateTime.Now > timeToNotify)
//        {
//            timeToNotify = timeToNotify.AddDays(1);
//        }

//        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
//        {
//            TimeInterval = timeToNotify - DateTime.Now,
//            Repeats = true
//        };

//        var notification = new iOSNotification()
//        {
//            Identifier = "_daily_notification_" + hour + "_" + minute,
//            Title = title,
//            Body = text,
//            ShowInForeground = true,
//            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
//            CategoryIdentifier = "daily_category",
//            Trigger = timeTrigger
//        };

//        // Lập lịch thông báo
//        iOSNotificationCenter.ScheduleNotification(notification);
//    }

//    public void SendNotificationNow(string title, string body, string subtitle, int fireTimeInHours, int fireTimeInMinutes = 0, int fireTimeInSeconds = 0)
//    {
//        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
//        {
//            TimeInterval = new System.TimeSpan(fireTimeInHours, fireTimeInMinutes, fireTimeInSeconds),
//            Repeats = false
//        };
//        var notification = new iOSNotification()
//        {
//            Identifier = "fuck life full",
//            Title = title,
//            Body = body,
//            Subtitle = subtitle,
//            ShowInForeground = true,
//            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Badge),
//            CategoryIdentifier = "default_category",
//            ThreadIdentifier = "thread1",
//            Trigger= timeTrigger
//        };

//        iOSNotificationCenter.ScheduleNotification(notification);
//    }
//#endif
}
