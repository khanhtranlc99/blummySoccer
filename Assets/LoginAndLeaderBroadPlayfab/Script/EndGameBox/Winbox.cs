using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using XGame;

public class Winbox : BaseBox
{
    public static Winbox _instance;
    public static Winbox Setup()
    {
        if (_instance == null)
        {
            _instance = Instantiate(Resources.Load<Winbox>(PathPrefabs.WIN_BOX));
            _instance.Init();
        }
        _instance.InitState();
        return _instance;
    }

    public Button nextButton;



    //public CanvasGroup canvasGroup;
    public void Init()
    {
        nextButton.onClick.AddListener(delegate { HandleNext(); });


 
    }
    public void InitState()
    {
       // GameController.Instance.admobAds.HandleShowMerec();
        UIManager.Instance.pfb_Gameplay.HandleStopAllCorutin();


    }
    private void HandleNext()
    {
      //  GameController.Instance.admobAds.HandleHideMerec();
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
       Next();

       // GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () => { Next(); }, actionWatchLog: "InterWinBox");
        void Next()
        {

            Close();
            GameManager.Instance.NextlevelWin();

          ;


             XGameSdk.Instance.Track("UE", new KVItems()
             {
                {"button", "next_level"},
                {"level_id", Facade.Instance.PlayerPrefManager.CurrentLevel.ToString()},
             });

        }
    }
 
}
