
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Playables;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using System.Text.RegularExpressions;
public static class Utils
{
    public static Tween DelayCallForUpdate(float timeDelay, TweenCallback callback, bool IgnoreTimescale = false)
    {
        return DOVirtual.DelayedCall(timeDelay, callback, IgnoreTimescale);
    }
    /// <summary>
    /// Ở đây playpref chạy ngầm có thể bị kill khi thực hiện hàm bên dưới
    /// Do vậy cần save data và delay một chút để device kịp lưu lại data
    /// </summary>
    public static void RestartApplication()
    {
        PlayerPrefs.Save();
        DOVirtual.DelayedCall(.3f, delegate
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                const int kIntent_FLAG_ACTIVITY_CLEAR_TASK = 0x00008000;
                const int kIntent_FLAG_ACTIVITY_NEW_TASK = 0x10000000;

                var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                var pm = currentActivity.Call<AndroidJavaObject>("getPackageManager");
                var intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);

                intent.Call<AndroidJavaObject>("setFlags", kIntent_FLAG_ACTIVITY_NEW_TASK | kIntent_FLAG_ACTIVITY_CLEAR_TASK);
                currentActivity.Call("startActivity", intent);
                currentActivity.Call("finish");
                var process = new AndroidJavaClass("android.os.Process");
                int pid = process.CallStatic<int>("myPid");
                process.CallStatic("killProcess", pid);
            }
        });
    }

    //Type: "2024-12-26T04:29:13.4557246Z"
    public static DateTime ParseDateTime(string datetime)
    {
        //match 0000-00-00
        string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;

        //match 00:00:00
        string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }
    public static bool IsConnectedToNetwork()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    /// <summary>
    /// Quay một vector quanh gốc tọa độ một góc nhất định.
    /// </summary>
    /// <param name="vector">Vector ban đầu.</param>
    /// <param name="angleInDegrees">Góc quay (tính bằng độ).</param>
    /// <returns>Vector mới sau khi đã quay.</returns>
    public static Vector2 RotateVector(Vector2 vector, float angleInDegrees)
    {
        // Chuyển đổi góc từ độ sang radian
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

        // Tính toán cos và sin của góc
        float cosTheta = Mathf.Cos(angleInRadians);
        float sinTheta = Mathf.Sin(angleInRadians);

        // Áp dụng ma trận quay
        float xNew = cosTheta * vector.x - sinTheta * vector.y;
        float yNew = sinTheta * vector.x + cosTheta * vector.y;

        // Trả về vector mới
        return new Vector2(xNew, yNew);
    }
    public static TweenerCore<float, float, FloatOptions> DOFillAmount(this SlicedFilledImage target, float endValue, float duration)
    {
        if (endValue > 1) endValue = 1;
        else if (endValue < 0) endValue = 0;
        TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.fillAmount, x => target.fillAmount = x, endValue, duration);
        t.SetTarget(target);
        return t;
    }
    public static T ResourcesLoad<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }
    public static void LogError(this string LogText)
    {
        Debug.LogError(LogText);
    }
    public static void SetLayerRecursively(GameObject go, string layerName)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    public static void DestroyChildren(this Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name == "TMP")
                continue;
            GameObject.Destroy(parent.GetChild(i).gameObject);
        }
    }

    public static T SetAlpha<T>(this T g, float newAlpha) where T : Graphic
    {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
        return g;
    }
    public static T ToEnum<T>(this string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }
    public static Enum GetRandomEnumValue(this Type t)
    {
        return Enum.GetValues(t)          // get values from Type provided
            .OfType<Enum>()               // casts to Enum
            .OrderBy(e => Guid.NewGuid()) // mess with order of results
            .FirstOrDefault();            // take first item in result
    }
    public static T RandomEnumValue<T>()
    {
        var values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(random);
    }

    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static IList<T> Shuffle2<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
        return ts;
    }
    public static void Transfer2List<T>(this List<T> Sender, List<T> Receiver)
    {
        foreach (var item in Sender)
            Receiver.Add(item);
    }

    //
    // Summary:
    //     Return a random int within [minInclusive..maxExclusive) (Read Only).
    //
    // Parameters:
    //   min:
    //
    //   max:
    public static int RandomNumberExclusive(int minInclusive, int maxExclusive, params int[] ExcArr)
    {
        if (ExcArr.Length >= Mathf.Abs(minInclusive - maxExclusive))
            return -1;
        int randomNum = UnityEngine.Random.Range(minInclusive, maxExclusive);
        if (ExcArr.Contains(randomNum))
            return RandomNumberExclusive(minInclusive, maxExclusive, ExcArr);
        return randomNum;
    }
    public static int RandomNumberExclusive(int minInclusive, int maxExclusive, List<int> ExcList)
    {
        if (ExcList.Count >= Mathf.Abs(minInclusive - maxExclusive))
            return -1;
        int randomNum = UnityEngine.Random.Range(minInclusive, maxExclusive);
        if (ExcList.Contains(randomNum))
            return RandomNumberExclusive(minInclusive, maxExclusive, ExcList);
        return randomNum;
    }
    public static bool IsOneOf<T>(this T target, params T[] array)
    {
        return array.Contains(target);
    }

    //reset timeline
    public static void TLReset(this PlayableDirector Timeline)
    {
        Timeline.Stop();
        Timeline.time = 0;
        Timeline.Evaluate();
    }
    public static int GetVersionCode()
    {
        using (AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager"))
                {
                    string packageName = context.Call<string>("getPackageName");
                    using (AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0))
                    {
                        return packageInfo.Get<int>("versionCode");
                    }
                }
            }
        }
    }
    public static T GetRandom<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null || enumerable.Count() == 0)
            return default(T);
        int n = UnityEngine.Random.Range(0, enumerable.Count());
        return enumerable.ElementAt(n);
    }
    public static T GetLastItem<T>(this List<T> List)
    {
        if (List.Count == 0) return default(T);
        return List[List.Count - 1];
    }
    public static List<T> GetRandom<T>(this IEnumerable<T> enumerable, int count)
    {
        if (enumerable == null)
            return null;

        if (enumerable.Count() <= count)
        {
            return enumerable.ToList();
        }

        List<T> ret = new List<T>();
        while (ret.Count < count)
        {
            int n = UnityEngine.Random.Range(0, enumerable.Count());
            T e = enumerable.ElementAt(n);
            if (!ret.Contains(e))
            {
                ret.Add(e);
            }
        }

        return ret;
    }

    public static void SetText(this Text txt, string content)
    {
        txt.text = content;
    }
    public static int IndexOf<T>(this IEnumerable<T> source, T value)
    {
        int index = 0;
        var comparer = EqualityComparer<T>.Default; // or pass in as a parameter
        foreach (T item in source)
        {
            if (comparer.Equals(item, value)) return index;
            index++;
        }
        return -1;
    }
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
    public static TimeSpan GetDifferenceTime(DateTime PrevTime, DateTime NextTime)
    {
        TimeSpan diff = NextTime.Subtract(PrevTime);
        return diff;
    }
    //Lấy số element khác nhau trong list
    public static List<T> GetDistinctElementsInAList<T>(List<T> BaseList, int numberElement)
    {
        if (BaseList.Count < numberElement)
            return null;
        HashSet<T> uniqueElements = new HashSet<T>();
        System.Random random = new System.Random();
        while (uniqueElements.Count < numberElement)
        {
            int randomIndex = random.Next(BaseList.Count);
            uniqueElements.Add(BaseList[randomIndex]);
        }

        return uniqueElements.ToList(); ;
    }
    public static List<T> GetDistinctElementsInAList02<T>(List<T> BaseList, int numberElement)
    {
        if (BaseList.Count < numberElement)
            return null;
        List<T> SuitableList = BaseList.Distinct().Take(numberElement).ToList();

        return SuitableList;
    }
    public static string FormatTimeSpan(TimeSpan timeSpan)
    {
        // Lấy các thành phần giờ, phút, giây
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;

        // Chuyển đổi thành chuỗi định dạng giờ:phút:giây
        string formattedTime = $"{timeSpan.Days} {"Day"} {timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";

        return formattedTime;
    }
    public static string FormatTimeSpan(int TotalSeconds)
    {
        string formattedTime = $"{GetDay(TotalSeconds)} {"Day"} {GetHour(TotalSeconds):00}:{GetMinutes(TotalSeconds):00}:{GetSeconds(TotalSeconds):00}";
        return formattedTime;
    }
    private static string GetDay(int seconds)
    {
        int days = seconds / (24 * 3600);
        return $"{days} Day{(days > 1 ? "s" : "")}";
    }
    private static string GetHour(int Time)
    {
        int Temptime = (Time % (24 * 3600)) / 3600;
        return (Temptime >= 10) ? Temptime.ToString() : ("0" + Temptime.ToString());
    }
    private static string GetMinutes(int Time)
    {
        int Temptime = (Time % 3600) / 60;
        return (Temptime >= 10) ? Temptime.ToString() : ("0" + Temptime.ToString());
    }
    private static string GetSeconds(int Time)
    {
        int Temptime = Time % 60;
        return (Temptime >= 10) ? Temptime.ToString() : "0" + Temptime.ToString();
    }
}
