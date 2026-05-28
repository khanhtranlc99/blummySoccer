using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoseBox : BaseBox
{
    public static LoseBox _instance;
    public static LoseBox Setup()
    {
        if (_instance == null)
        {
            _instance = Instantiate(Resources.Load<LoseBox>(PathPrefabs.LOSE_BOX));
            _instance.Init();
        }
        _instance.InitState();
        return _instance;
    }

    public Button btnRetry;
    public Button btnAds;
    private void Init()
    {
        btnRetry.onClick.AddListener(delegate { Retry(); });
        btnAds.onClick.AddListener(delegate { HandleAds(); });
    }
    private void InitState()
    {


    }
    private void Retry()
    {
        GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () => { Next(); }, actionWatchLog: "Retry");
        void Next()
        {
            Close();
            GameManager.Instance.Replay();
        }
    }    
    private void HandleAds()
    {
        GameController.Instance.admobAds.ShowVideoReward(
                           actionReward: () =>
                           {
                               Close();
                               PlayerController.Instance.currentBalls = 3;
                               EventHandler.ExecuteEvent(EventID.OnBallChanged, 3);
                           },
                           actionNotLoadedVideo: () =>
                           {
                               GameController.Instance.effectController.SpawnEffectText_FlyUp
                                (

                                btnAds.transform.position,
                                "No video at the moment!",
                                Color.white,
                                isSpawnItemPlayer: true
                                );
                           },
                           actionClose: null,
                             ActionWatchVideo.Skip_level,
                           Facade.Instance.PlayerPrefManager.CurrentLevel.ToString());
    
    }
}