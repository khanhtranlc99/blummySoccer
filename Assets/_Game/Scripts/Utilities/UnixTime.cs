using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnixTime 
{
    public static DateTimeOffset UnixEpoch = new (1970, 1, 1, 0 ,0 ,0 ,TimeSpan.Zero);

    public static int GetUnixTime()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970,1,1)).TotalSeconds;
    }

    public static long GetUnixTimeMicro()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        return ToUnixTimeMicroseconds(now);
    }

    public static long ToUnixTimeMicroseconds(this DateTimeOffset timestamp)
    {
        TimeSpan duration = timestamp - UnixEpoch;
        // There are 10 ticks per microsecond.
        return duration.Ticks / 10;
    }

    public static float GetTimeDiffToNow(long startTime)
    {
        long nowTime = GetUnixTimeMicro();
        return (float)(nowTime - startTime)/1000000;
    }
}

//public class TestUnixTime : MonoBehaviour
//{
//    private void Start()
//    {
//        long startTime = UnixTime.GetUnixTimeMicro();

//        Debug.LogError("StartTime: " + startTime);

//        //Function
//        //Function


//        float timeDiff = UnixTime.GetTimeDiffToNow(startTime);
//        Debug.LogError("TimeDif: " + timeDiff);
//    }
//}