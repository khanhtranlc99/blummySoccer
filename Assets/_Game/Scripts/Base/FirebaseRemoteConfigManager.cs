using UnityEngine;
using System.Threading.Tasks;
using System;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using MoonlightFramework;
public class FirebaseRemoteConfigManager : MonoSingleton<FirebaseRemoteConfigManager>
{

    public FirebaseRemoteConfig remoteConfigInstance;

    private const string Interstitial_Delay = "Inter_time";
    public bool isCallSuccess = false;
    public bool enableTrackingIOS = true;

    private void Start()
    {
        remoteConfigInstance = FirebaseRemoteConfig.GetInstance(FirebaseApp.DefaultInstance);
        FetchDataAsync();
    }
    // Start a fetch request.
    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask = remoteConfigInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWith(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                remoteConfigInstance.ActivateAsync().ContinueWithOnMainThread(task =>
                {
                    Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                        info.FetchTime));
                    AdsConfig.Interstitial_Delay = (int)FirebaseRemoteConfig.DefaultInstance.GetValue(Interstitial_Delay).LongValue;

                
                    isCallSuccess = true;
#if UNITY_IOS
                    enableTrackingIOS = FirebaseRemoteConfig.DefaultInstance.GetValue("enableTrackingIOS").BooleanValue;
                    if (enableTrackingIOS)
                    {
                        bool checkAskGDPR = PlayerPrefs.GetInt("IsTrackGDPR", -1) == -1 ? false : true;
                        bool canCheckAtt = AttPermissionRequest.CanTrackATT();

                        if (!checkAskGDPR && canCheckAtt)
                            UIManager.Instance.pfb_Loading.NotiTrackingIOSObject.SetActive(true);
                    }
#endif
                
                    Debug.LogError(AdsConfig.Interstitial_Delay);
                
                });
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
}

