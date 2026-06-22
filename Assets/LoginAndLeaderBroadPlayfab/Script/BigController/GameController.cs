using Newtonsoft.Json;
 
using Sirenix.OdinInspector;
using System;
 
using UnityEngine;
using Newtonsoft.Json;
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
