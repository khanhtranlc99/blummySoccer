using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public MoneyEffectController effectController;
    public PlayerData playerData;
    public LoginController loginController;
    public AnalyticsController AnalyticsController;
    public AdmobAds admobAds;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        loginController.Init();
        playerData = new PlayerData();
        admobAds.Init();
        RemoteConfigController.RemoteConfigFirebaseInit();
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GConnection.UpdateScoreToALeaderBroad("LeaderboardName", 20);
            Debug.LogError("Space");
        }
    }
}
