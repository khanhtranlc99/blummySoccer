using MoonlightFramework;
// using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;



public enum GAME_STATE
{
    INTRO,
    PLAYING,
    END,
}
public class GameManager : MonoSingleton<GameManager>
{
    public MapController CurrentMap;
    public bool isTest = false;
    public MapController LevelTest;
    [SerializeField] SpriteShape waterShape;
    public List<MapController> ListMaps = new();
    [SerializeField] private GAME_STATE m_game_state = GAME_STATE.PLAYING;

    public GAME_STATE GAME_STATE
    {
        get => m_game_state;
        set
        {
            if (m_game_state == value) return;
            m_game_state = value;
            switch (m_game_state)
            {
                case GAME_STATE.INTRO:
                    Debug.Log("Intro");
                    break;
                case GAME_STATE.PLAYING:
                    Debug.Log("Playing");
                    break;
                case GAME_STATE.END:
                    Debug.Log("End");
                    break;
                default:
                    break;
            }
        }
    }

    // public GAME_STATE GAME_STATE
    // {
    //     get
    //     {
    //         return this.m_game_state;
    //     }
    //     set
    //     {
    //         if (this.m_game_state == value) return;
    //         this.m_game_state = value;
    //         switch (this.m_game_state)
    //         {
    //             case GAME_STATE.DEFAULT:
    //                 m_StateMachine.ChangeState(GM_DefaultState.Instance);
    //                 break;
    //             case GAME_STATE.START:
    //                 m_StateMachine.ChangeState(GM_StartState.Instance);
    //                 break;
    //             case GAME_STATE.RUNNING:
    //                 m_StateMachine.ChangeState(GM_RunningState.Instance);
    //                 break;
    //             case GAME_STATE.WIN:
    //                 m_StateMachine.ChangeState(GM_WinState.Instance);
    //                 break;
    //             case GAME_STATE.LOSE:
    //                 m_StateMachine.ChangeState(GM_LoseState.Instance);
    //                 break;
    //             case GAME_STATE.REVIVE:

    //                 break;
    //             case GAME_STATE.PAUSE:

    //                 break;
    //             case GAME_STATE.INREVIEW:

    //                 break;
    //             case GAME_STATE.END:

    //                 break;
    //             default:
    //                 m_StateMachine.ChangeState(GM_DefaultState.Instance);
    //                 break;
    //         }
    //     }
    // }
    // protected StateMachine<GameManager> m_StateMachine;
    protected override void Awake()
    {
        base.Awake();
        // m_StateMachine = new StateMachine<GameManager>(this);
    }
    private void Start()
    {
        //Spawn Current Map
        Application.targetFrameRate = 60;
    }
    public void DoneLoading()
    {
        UIManager.Instance.pfb_Gameplay.ForceActiveNormalPopup();

        SpawnMap(Facade.Instance.PlayerPrefManager.CurrentLevel);

        GlobalAudioPlayer.PlayMusic(eAudioType.BGM);
    }
    public void Nextlevel()
    {
        UIManager.Instance.pfb_Gameplay.HandleStopAllCorutin();
        //if (Facade.Instance.PlayerPrefManager.CurrentLevel >= 1)
        //  AdsManager.Instance.ShowAds(AdsNetwork.Max, AdsType.Interstitial, delegate { Next(); });

        // GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () => { Next(); }, actionWatchLog: "Retry");
        Next();

        void Next()
        {
            if (Facade.Instance.PlayerPrefManager.CurrentLevel >= 4)
            {
                //InAppReviewManager.i.Request();
            }

            if (!isTest)
                Facade.Instance.PlayerPrefManager.CurrentLevel++;
            if (Facade.Instance.PlayerPrefManager.CurrentLevel > ListMaps.Count)
            {
                Facade.Instance.PlayerPrefManager.CurrentLevel = ListMaps.Count;
            }
            GAME_STATE = GAME_STATE.PLAYING;

            SpawnMap(Facade.Instance.PlayerPrefManager.CurrentLevel);
        
            NextlevelWin();
        }
    
    }
    public void NextlevelWin()
    {
        if (Facade.Instance.PlayerPrefManager.CurrentLevel >= 4)
        {
            //InAppReviewManager.i.Request();
        }

        if (!isTest)
            Facade.Instance.PlayerPrefManager.CurrentLevel++;
        if (Facade.Instance.PlayerPrefManager.CurrentLevel > ListMaps.Count)
        {
            Facade.Instance.PlayerPrefManager.CurrentLevel = ListMaps.Count;
        }
        GAME_STATE = GAME_STATE.PLAYING;

        SpawnMap(Facade.Instance.PlayerPrefManager.CurrentLevel);

    }
    public void SpawnMap(int index = 1)
    {
        GAME_STATE = GAME_STATE.INTRO;
        ActiveIntro();



        CreateController.Instance.DespawnAll();

        if (this.CurrentMap != null)
        {
            Destroy(this.CurrentMap.gameObject);
        }


        if (isTest && LevelTest != null)
        {
            this.CurrentMap = Instantiate(LevelTest);
        }
        else
        {
            FirebaseManager.LogEvent("Level_" + index);
            if (ListMaps[index - 1] != null)
            {
                this.CurrentMap = Instantiate(ListMaps[index - 1], Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.Log(index - 1);
            }
        }



        SetTheme();
        EventHandler.ExecuteEvent<string>(EventID.OnSpawnLevel, isTest ? "Test" : index.ToString());
        CameraManager.Instance.SetCameraPosition(CurrentMap);
        UIManager.Instance.pfb_Gameplay.HandleShowTutPvP();
    }
    private void SetTheme()
    {
        int idTheme = CurrentMap.theme - 1;
        if (idTheme >= ThemeLevel.i.themeSprites.Length)
        {
            idTheme = ThemeLevel.i.themeSprites.Length - 1;
        }
        if (idTheme < 0)
        {
            idTheme = 0;
        }
        waterShape.fillTexture = ThemeLevel.i.themeSprites[idTheme].waterTexture;
        CurrentMap.SetTheme(idTheme);
    }
    public void Replay()
    {
        if (GAME_STATE != GAME_STATE.PLAYING) return;

        FirebaseManager.LogEvent("replay_level_" + Facade.Instance.PlayerPrefManager.CurrentLevel);
        GAME_STATE = GAME_STATE.PLAYING;
        SpawnMap(Facade.Instance.PlayerPrefManager.CurrentLevel);
    }
    public void ActivePopUpWin()
    {
        UIManager.Instance.pfb_Result.ActiveNormalPopup();
        UIManager.Instance.pfb_Result.PopupWin.ActiveNormalPopup();
    }
    public void ActivePopUpLose()
    {
        UIManager.Instance.pfb_Result.ActiveNormalPopup();
        UIManager.Instance.pfb_Result.PopupLose.ActiveNormalPopup();
    }

    public void ActiveIntro()
    {
        UIManager.Instance.pfb_Intro.ActiveNormalPopup();
        UIManager.Instance.pfb_Intro.PlayIntro();
    }
    public void ActiveOutro()
    {
        FirebaseManager.LogEvent("win_level_" + Facade.Instance.PlayerPrefManager.CurrentLevel);
        GlobalAudioPlayer.PlaySFX(eAudioType.WIN);
       // Winbox.Setup().Show();
        UIManager.Instance.pfb_Intro.ActiveNormalPopup();
        UIManager.Instance.pfb_Intro.PlayOutro();
    }
    public void SelectLevelTest(int id)
    {
        LevelTest = ListMaps[id];
        Replay();
    }
    // #region STATE MACHINE CALLBACK
    // public virtual void OnDefaultEnter()
    // {

    // }
    // public virtual void OnDefaultExecute()
    // {

    // }
    // public virtual void OnDefaultExit()
    // {
    // }

    // public virtual void OnStartEnter()
    // {

    // }
    // public virtual void OnStartExecute()
    // {

    // }
    // public virtual void OnStartExit()
    // {

    // }
    // public virtual void OnRunningEnter()
    // {

    // }
    // public virtual void OnRunningExecute()
    // {

    // }
    // public virtual void OnRunningExit()
    // {
    // }
    // public virtual void OnEndEnter()
    // {

    // }
    // public virtual void OnEndExecute()
    // {

    // }
    // public virtual void OnEndExit()
    // {

    // }

    // #region WIN
    // public virtual void OnWinEnter()
    // {
    //     //enable popup win
    //     UIManager.Instance.pfb_Result.ActiveNormalPopup();
    //     UIManager.Instance.pfb_Result.PopupWin.ActiveNormalPopup();
    // }
    // public virtual void OnWinExecute()
    // {

    // }
    // public virtual void OnWinExit()
    // {
    // }
    // #endregion WIN

    // #region LOSE
    // public virtual void OnLoseEnter()
    // {

    // }
    // public virtual void OnLoseExecute()
    // {

    // }
    // public virtual void OnLoseExit()
    // {
    // }
    // #endregion LOSE

    // #endregion
}
