using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XGame;
public class LeaderBoardPvPBox : Singleton<LeaderBoardPvPBox>
{
   
    [SerializeField] private LaybelLeaderBoard laybelLeaderBoard;
    public LaybelInfoPlayer laybelMe;
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnFight;
    [SerializeField] private Button btnBtnTickit;
    public string nameTableLable;
    public Text tvScore;
    public Text tvName;
    public Text tvTitkit;
    public GameObject handTut;
    public GameObject changeNameTut;
    public Button btnShowChangeName;

    public void Start()
    {

        laybelLeaderBoard.Init();
        laybelMe.Init();
        btnClose.onClick.AddListener(delegate {

            HandleClose();
        }) ;

        btnFight.onClick.AddListener(delegate {
            GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
            HandleButtonFight();
        });

        btnBtnTickit.onClick.AddListener(delegate
        {
            GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
            ShopTickitBox.Setup().Show();
            XGameSdk.Instance.Track("UE", new KVItems()
            {
                {"button", "buy_tickit"} 
            
            });
        });
        btnShowChangeName.onClick.AddListener(delegate
        {
            GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
            ChangeNameBox.Setup().Show();
        });
        InitState();
        if(UseProfile.WasClickPvP == 0)
        {
            handTut.gameObject.SetActive(true);
        }
        tvName.text = UseProfile.NamePlayer;
        if(!UseProfile.WasClickChangeName)
        {
            changeNameTut.SetActive(true);
        }
        else
        {
            changeNameTut.SetActive(false);
        }    
    }
    private void HandleClose()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
        GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () => { Next(); }, actionWatchLog: "Retry");
        XGameSdk.Instance.Track("UE", new KVItems()
        {
            {"button", "back_to_gamepage"},
            {"level_id", Facade.Instance.PlayerPrefManager.CurrentLevel.ToString()},
       
        });

        void Next()
        {
         
            Initiate.Fade("Main", Color.black, 2f);
        }
    }
 

    private void HandleButtonFight()
    {
        if(UseProfile.Titkit >= 1)
        {
            UseProfile.Titkit -= 1;
            MatchBox.Setup().Show();
            if (UseProfile.WasClickPvP == 0)
            {
                UseProfile.WasClickPvP = 1;
            }
        }
        else
        {
            ShopTickitBox.Setup().Show();
        }
    }
    public void InitState()
    {
        laybelLeaderBoard.InitState();
        laybelMe.InitStateProfileMe(nameTableLable);
        tvTitkit.text = "" + UseProfile.Titkit;

    }




}
