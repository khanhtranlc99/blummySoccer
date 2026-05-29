using Firebase.Analytics;
//using GoogleMobileAds.Api;
using MoonlightFramework;
using System.Collections.Generic;

public static class FirebaseManager
{
    public static readonly string m_play_level = "play_level"; //Bắn khi bắt đầu chơi 1 level trong game
    public static readonly string m_On_ClickBuyPack = "On_ClickBuyPack"; //Bắn khi bắt đầu chơi 1 level trong game
    public static readonly string m_IAP_Success = "_success";
    private static List<string> Constraints = new List<string>();
    public static void LogEvent(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }
    public static void LogEvent(string eventName, string ParameterName, string ParameterValue)
    {
        FirebaseAnalytics.LogEvent(eventName, ParameterName, ParameterValue);
    }

    /// <summary>
    /// Thực thi hàm này khi biến 'ConstraintExclusive' chưa được gọi lần nào
    /// </summary>
    /// <param name="EventName">event</param>
    /// <param name="Constraint">Nếu biến này đã được log rồi thì event sễ không thực thi</param>
    public static void LogEventConstraint(string EventName, string ConstraintExclusive)
    {
        if (Constraints.Contains(ConstraintExclusive))
            return;
        Constraints.Add(ConstraintExclusive);
        LogEvent(EventName);
    }
    public static void LogEventConstraint(string EventName, string ParameterName, string ParameterValue, string ConstraintExclusive)
    {
        if (Constraints.Contains(ConstraintExclusive))
            return;
        Constraints.Add(ConstraintExclusive);
        LogEvent(EventName, ParameterName, ParameterValue);
    }
    // public static void FirebaseTrackRevenue(MaxSdkBase.AdInfo impressionData)
    // {
    //     double revenue = impressionData.Revenue;
    //     var impressionParameters = new[] {
    //           new Parameter("ad_platform", "AppLovin"),
    //           new Parameter("ad_source", impressionData.NetworkName),
    //           new Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
    //           new Parameter("ad_format", impressionData.AdFormat),
    //           new Parameter("value", revenue),
    //           new Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
    //         };
    //     FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    //     FirebaseAnalytics.LogEvent("ad_max", impressionParameters);
    // }
    //public static void FirebaseTrackRevenue(AdValue arg2, AdsType adstype, string _adUnitId)
    //{
    //    // Chỉ chia cho 1000000f nếu giá trị doanh thu là micro-units
    //    double revenueInUnits = arg2.Value / 1000000d;
    //    var impressionParameters = new[] {
    //                new Parameter("ad_platform",  "GoogleAdMobSDK"),
    //                new Parameter("ad_source",  "Google AdMob"),
    //                new Parameter("ad_unit_name",  _adUnitId),
    //                new Parameter("ad_format",  adstype.ToString()),
    //                new Parameter("value", revenueInUnits),
    //                new Parameter("currency", arg2.CurrencyCode),
    //            };
    //    FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    //    FirebaseAnalytics.LogEvent("ad_max", impressionParameters);
    //    // #if UNITY_EDITOR
    //    //                 Debug.LogError("ad_platform :" + "GoogleAdMobSDK" + "\n"
    //    //                     + "ad_source :" + "Google AdMob" + "\n"
    //    //                     + "ad_unit_name :" + _adUnitId + "\n"
    //    //                     + "ad_format :" + adstype.ToString() + "\n"
    //    //                     + "value :" + revenueInUnits + "\n"
    //    //                     + "currency :" + arg2.CurrencyCode + "\n"
    //    //                     );
    //    // #endif

    //}
}
