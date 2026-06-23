using Newtonsoft.Json;
 
using Sirenix.OdinInspector;
using System;
 
using UnityEngine;
using Newtonsoft.Json;
using XGame;
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public MoneyEffectController effectController;
    public PlayerData playerData;
    public LoginController loginController;
    public AnalyticsController AnalyticsController;
    public AdmobAds admobAds;
    public DataAll dataAll;
    public bool wasGetData;

    private float startTime;
  
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        wasGetData = false;
        DontDestroyOnLoad(this);
        loginController.Init();
        playerData = new PlayerData();
        admobAds.Init();
        RemoteConfigController.GetConfig();
        
      //  RemoteConfigController.RemoteConfigFirebaseInit();
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GConnection.UpdateScoreToALeaderBroad("LeaderboardName", 20);
            Debug.LogError("Space");
        }
   



    }
 
  
    public  void AddPlay(int levelId)
    {
        string key = $"Level_Play_Count_{levelId}";
        int count = PlayerPrefs.GetInt(key, 0);
        PlayerPrefs.SetInt(key, count + 1);
        PlayerPrefs.Save();

        startTime = Time.time;
 


    }

    public float GetTotalTime
    {
        get
        {
            float completeTime = Time.time - startTime;
            return completeTime;
        }
    }








    public  int GetPlayCount(int levelId)
    {
        return PlayerPrefs.GetInt($"Level_Play_Count_{levelId}", 0);
    }









    [Button]
    private void SpawnJson()
    {
       var temp = JsonConvert.SerializeObject(dataAll );
        Debug.LogError("-/" + temp );

    }

    
}
[Serializable]
public class DataAll
{
    public bool Show_Ads_Video;
    public bool Show_Ads_Inter;
    public bool Show_Ads_ShowBanner;
    public bool isUseCoolDown;
    public float TimeShowInter;
 

}
