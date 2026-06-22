using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
// using Firebase.Analytics;
// using Firebase;
// using Firebase.Analytics;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
//using com.adjust.sdk;
using XGame;
public class AnalyticsController : MonoBehaviour
{
    #region Init
    static UnityEvent onFinishFirebaseInit = new UnityEvent();
    private static bool m_firebaseInitialized = false;
    public static bool firebaseInitialized
    {
        get
        {
            return m_firebaseInitialized;
        }
        set
        {
            m_firebaseInitialized = value;
            if (value == true)
            {
                if (onFinishFirebaseInit != null)
                {
                    onFinishFirebaseInit.Invoke();
                    onFinishFirebaseInit.RemoveAllListeners();
                }

                //SetUserProperties();
            }
        }
    }
    #endregion



    public  void StartLevel()
    { 
      Debug.LogError("Start_Level_" + Facade.Instance.PlayerPrefManager.CurrentLevel);
    
  
    }

    public  void WinLevel()
    {
             Debug.LogError("Win_Level_" + Facade.Instance.PlayerPrefManager.CurrentLevel);
     

          XGameSdk.Instance.Track("UE", new KVItems()
            {
               {"view_show", "victory"},
               {"level_id", Facade.Instance.PlayerPrefManager.CurrentLevel.ToString()},
            });

         XGameSdk.Instance.Track("level", new KVItems()
        {
            {"level_status", "victory"},
        });


      
 
    }

    

    // public static void LogEventFirebase(string eventName, Parameter[] parameters)
    // {

    //     if (firebaseInitialized)
    //     {

    //         FirebaseAnalytics.LogEvent(eventName, parameters);
    //     }
    //     else
    //     {
    //         onFinishFirebaseInit.AddListener(() =>
    //         {
    //             FirebaseAnalytics.LogEvent(eventName, parameters);
    //         });
    //     }
    // }

    public static void LogEventFacebook(string eventName, Dictionary<string, object> parameters)
    {
       
    }

    public static void SetUserProperties()
    {
        try
        {
            //FirebaseAnalytics.SetUserProperty(StringHelper.RETENTION_D, UseProfile.RetentionD.ToString());
            //FirebaseAnalytics.SetUserProperty(StringHelper.DAYS_PLAYED, UseProfile.DaysPlayed.ToString());
            //FirebaseAnalytics.SetUserProperty(StringHelper.PAYING_TYPE, UseProfile.PayingType.ToString());
            //FirebaseAnalytics.SetUserProperty(StringHelper.LEVEL, UseProfile.CurrentLevel.ToString());
        }
        catch
        {

        }
    }

    #region Event

 

       

    public static void LogIAP(int level, string productID, string price, string currency)
    {

    }
    #endregion

    private void OnApplicationQuit()
    {
       
        //UseProfile.WinStreak = 0;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            XGameSdk.Instance.Track("UE", new KVItems()
            {
                {"view_state", "back_to_background"},
            });
        }
        else
        {
            XGameSdk.Instance.Track("UE", new KVItems()
            {
                {"view_state", "return_to_game"},
            });
        }
    }

    // public void HandleFireEvent_Total_Inter_Count()
    // {
    //     int count = GetCount("new_total_inter_count");
    //     FirebaseAnalytics.SetUserProperty("Intershow_", count.ToString());
    //     //FirebaseAnalytics.LogEvent("Intershow_" +  count.ToString());
    // }

    // public void HandleFireEvent_Total_Reward_Count()
    // {
    //     int count = GetCount("new_total_reward_count");
    //     FirebaseAnalytics.SetUserProperty("Rewardshow_", count.ToString());
    //     //FirebaseAnalytics.LogEvent("Rewardshow_" +  count.ToString());
    // }

    public int GetCount(string s)
    {
        int count = PlayerPrefs.GetInt("CountEvent_" + s, 0);
        count++;
        PlayerPrefs.SetInt("CountEvent_" + s, count);
        PlayerPrefs.Save();
        return count;
    }


}

public enum ActionClick
{
    None = 0,
    Play = 1,
    Rate = 2,
    Share = 3,
    Policy = 4,
    Feedback = 5,
    Term = 6,
    NoAds = 10,
    Settings = 11,
    ReplayLevel = 12,
    SkipLevel = 13,
    Return = 14,
    BuyStand = 15
}

public enum ActionWatchVideo
{
    None = 0,
    Skip_level = 1,
    Return = 2,
    BuyStand = 3,
    BuyExtral = 4,
    ClaimSkin = 5,
    Hint = 6,
    Daily = 7,
    UnlockPic = 9,
    RewardEndGame = 10,
    TNT_Booster =11,
    Rocket_Booster =12,
    Freeze_Booster = 13,
    Atom_Booste = 14,
    ReviveFreeLoseBox = 15,
    HeartInHearPopup = 16,
    WinBox_Claim_Coin = 17
}

public enum ActionShowInter
{
    None = 0,
    Skip_level = 1,
    Return = 2,
    BuyStand = 3,

    EndGame = 4,
    Click_Setting = 5,
    Click_Replay = 6
}
