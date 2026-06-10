using System.Collections;
using System.Collections.Generic;

using MoonlightFramework;
using Newtonsoft.Json.Bson;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pfb_Gameplay : UIBehavior
{
    [SerializeField] protected GameObject UIBall;
    [SerializeField] protected Transform BallContent;
    [SerializeField] protected Button btnReplay;
    [SerializeField] protected Button btnSkip;
    [SerializeField] protected Button btnSounds;
    [SerializeField] protected Button btnSelectLevel;
    [SerializeField] protected TextMeshProUGUI txtLevel;

    [SerializeField] protected List<GameObject> ListBalls = new();
    [SerializeField] protected TMP_InputField levelInputField;
    public GameObject handReset;

    public Button btnPVP;
    public Vector3 vec;
    public GameObject handTut;
    public GameObject objBlind;
    public Text tvCountTime;
    protected override void OnInit()
    {
        base.OnInit();
        btnReplay.onClick.AddListener(OnReplay);
        btnSkip.onClick.AddListener(OnSkip);
        btnSounds.onClick.AddListener(OnSounds);
        btnSelectLevel.onClick.AddListener(OnSelectLevel);
        btnPVP.onClick.AddListener(delegate { HandleChangeScenePvP(); });
        CheckSoundStatus();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.RegisterEvent<int>(EventID.OnBallChanged, OnHandleBallChanged);
        EventHandler.RegisterEvent<string>(EventID.OnSpawnLevel, OnHandleSpawnLevel);
        vec = new Vector3(0, 0, 0);
       
    }
    protected void OnReplay()
    {
        //AdsManager.Instance.ShowAds(AdsNetwork.Max, AdsType.Interstitial);
        //GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () => { Next(); }, actionWatchLog: "Retry");
        //void Next()
        //{
        //    GameManager.Instance.Replay();
        //}

        GameManager.Instance.Replay();
    }
    protected void OnSkip()
    {
     
        GameManager.Instance.Nextlevel();
        //GameController.Instance.admobAds.ShowVideoReward(
        //               actionReward: () =>
        //               {
        //                   GameManager.Instance.Nextlevel();
        //               },
        //               actionNotLoadedVideo: () =>
        //               {
        //                   GameController.Instance.effectController.SpawnEffectText_FlyUp
        //                    (

        //                    btnSkip.transform.position,
        //                    "No video at the moment!",
        //                    Color.white,
        //                    isSpawnItemPlayer: true
        //                    );
        //               },
        //               actionClose: null,
        //                 ActionWatchVideo.Skip_level,
        //               Facade.Instance.PlayerPrefManager.CurrentLevel.ToString());
    }
    protected void OnSounds()
    {
        AudioPlayer.Instance.ToggleMusic();
        AudioPlayer.Instance.ToggleSound();
        CheckSoundStatus();
    }
    protected void OnSelectLevel()
    {
        if (int.TryParse(levelInputField.text, out int level))
        {
            if (level < 1) level = 1;
            if (level > GameManager.Instance.ListMaps.Count) level = GameManager.Instance.ListMaps.Count;

            GameManager.Instance.SelectLevelTest(level - 1);
        }
    }
    protected void CheckSoundStatus()
    {
        btnSounds.transform.GetChild(0).gameObject.SetActive(AudioPlayer.Instance.IsMusicOn());
        btnSounds.transform.GetChild(1).gameObject.SetActive(!AudioPlayer.Instance.IsMusicOn());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.UnregisterEvent<int>(EventID.OnBallChanged, OnHandleBallChanged);
        EventHandler.UnregisterEvent<string>(EventID.OnSpawnLevel, OnHandleSpawnLevel);
    }
    protected void OnHandleSpawnLevel(string currentLevel)
    {
        txtLevel.text = $"Level {currentLevel}";
    }
    protected void OnHandleBallChanged(int ballCount)
    {
        //btnReplay.transform.GetChild(0).gameObject.SetActive(ballCount == 0);
        //btnSkip.transform.GetChild(0).gameObject.SetActive(ballCount == 0);

        ListBalls.ForEach(x => x.SetActive(false));
        for (int i = 0; i < ballCount; i++)
        {
            ListBalls[i].SetActive(true);
        }
        try
        {
          UIManager.Instance.pfb_Gameplay.handReset.SetActive(ballCount == 0);
         
            if(ballCount == 0)
            {

                txtLevel.text = "";
            }    
        }
        catch
        {

        }
       
        //if(ballCount == 0)
        //{
        //    StartCoroutine(HandleCountLose());
        //}    
    }
    private IEnumerator HandleCountLose()
    {
     
        tvCountTime.text = "4";
        yield return new WaitForSeconds(1);
        tvCountTime.text = "3";
        yield return new WaitForSeconds(1);
        tvCountTime.text = "2";
        yield return new WaitForSeconds(1);
        tvCountTime.text = "1";
        yield return new WaitForSeconds(1);
        tvCountTime.text = "";
        LoseBox.Setup().Show();
    }    
    public void HandleStopAllCorutin()
    {
        StopAllCoroutines();
        tvCountTime.text = "";
    }

    public void HandleChangeScenePvP()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
        if ( Facade.Instance.PlayerPrefManager.CurrentLevel >= 3)
        {
            Initiate.Fade("HomePvP", Color.black, 2f);
            ActiveNormalPopup(false);
        }
        else
        {
            
            GameController.Instance.effectController.SpawnEffectText_FlyUp(vec, "Pass" + "\n" + "level 3", Color.white);

        }
 
    
        //AudioPlayer.Instance.HandleStopAlll();
    }
    public  void HandleShowTutPvP()
    {
       if(UseProfile.WasClickPvP == 0 && Facade.Instance.PlayerPrefManager.CurrentLevel >= 3)
        {
            handTut.SetActive(true);
           
        } 
    
            if(Facade.Instance.PlayerPrefManager.CurrentLevel >= 3)
        {
            objBlind.SetActive(false);
        }
            else
        {
            objBlind.SetActive(true);
        }
    }    

    public void HandleTestBanner()
    {
        GameController.Instance.admobAds.ShowBanner();
    }
    public void HandleTestInter()
    {
        GameController.Instance.admobAds.ShowInterstitial();
    }
    public void HandleTestVideo()
    {
        GameController.Instance.admobAds.ShowVideoReward(null,null,null,ActionWatchVideo.BuyExtral, "");
    }


}
