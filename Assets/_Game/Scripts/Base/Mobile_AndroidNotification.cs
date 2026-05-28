using System;
using UnityEngine;
//#if UNITY_ANDROID
//using Unity.Notifications.Android;
//using UnityEngine.Android;
//#endif

public class Mobile_AndroidNotification : MonoBehaviour
{
//#if UNITY_ANDROID
//    void Start()
//    {
//        // Đặt thông báo cho Android
//        //ScheduleAndroidNotification(12, 0, "Daily Noon Notification", "It's 12 PM! Time for your daily update.");
//        //ScheduleAndroidNotification(20, 0, "Daily Evening Notification", "It's 8 PM! Don't forget to check your progress.");
//    }
//    public void RequestAuthorization()
//    {
//        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
//        {
//            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
//        }
//    }

//    public void RegisterNotificationChannel()
//    {
//        // Kênh thông báo cho Android
//        var channel = new AndroidNotificationChannel()
//        {
//            Id = "daily_channel",
//            Name = "Daily Notifications",
//            Importance = Importance.High,
//            Description = "Daily notifications",
//        };
//        AndroidNotificationCenter.RegisterNotificationChannel(channel);
//    }
//    public void ScheduleAndroidNotification(int hour, int minute, string title, string text)
//    {
//        // Đặt thời gian cho thông báo
//        var timeToNotify = DateTime.Today.AddHours(hour).AddMinutes(minute);
//        if (DateTime.Now > timeToNotify)
//        {
//            timeToNotify = timeToNotify.AddDays(1);
//        }

//        var notification = new AndroidNotification()
//        {
//            Title = title,
//            Text = text,
//            SmallIcon = "default",
//            LargeIcon = "default",
//            FireTime = timeToNotify
//        };

//        // Lập lịch thông báo
//        AndroidNotificationCenter.SendNotification(notification, "daily_channel");
//    }
//#endif
}
