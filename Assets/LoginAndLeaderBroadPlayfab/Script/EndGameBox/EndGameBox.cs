using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public enum EndGameType
{
  Win,
  Lose,
  Draw
}
public class EndGameBox : BaseBox
{
    #region instance

    private static EndGameBox instance;

    public static EndGameBox Setup(EndGameType endGameType,int score,bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<EndGameBox>(PathPrefabs.END_GAME_BOX));
            instance.Init();
        }
        instance.InitState(endGameType, score);
        return instance;
    }

    #endregion


    public Button btnContinue;
    public Button btnBack;
    public LableEndGame lableWin;
    public LableEndGame lableLose;
    public LableEndGame lableDraw;
    public List<Text> lsTvNameUser;
    public List<Text> lsTvNameAi;
    public CanvasGroup canvasGroupBtn;

    private void Init()
    {
        btnBack.onClick.AddListener(delegate { GlobalAudioPlayer.PlaySFX(eAudioType.CLICK); Initiate.Fade("HomePvP", Color.black, 2f); });
        btnContinue.onClick.AddListener(delegate { HandleButtonContinue(); });
    }

    private void InitState(EndGameType endGameType, int score)
    {
        lableWin.gameObject.SetActive(false);
        lableLose.gameObject.SetActive(false);
        lableDraw.gameObject.SetActive(false);
        switch (endGameType)
        {
            case EndGameType.Win:
                lableWin.gameObject.SetActive(true);
                lableWin.Init(delegate { canvasGroupBtn.DOFade(1, 0.5f); });
             
                break;
            case EndGameType.Lose:
                lableLose.gameObject.SetActive(true);
                lableLose.Init(delegate { canvasGroupBtn.DOFade(1, 0.5f); });
     
                break;
            case EndGameType.Draw:
                lableDraw.gameObject.SetActive(true);
                lableDraw.Init(delegate { canvasGroupBtn.DOFade(1, 0.5f); });
                break;
        }
        foreach(var item in lsTvNameUser)
        {
            item.text = UseProfile.NamePlayer; 
        }
        foreach (var item in lsTvNameAi)
        {
            item.text = GConnection.leaderBoardNearPlayer[0].name;
        }

        var temp = UseProfile.Score + score;
        if(temp < 0)
        {
            UseProfile.Score = 0;
        }    
        else
        {
            UseProfile.Score += score;

        }    
      
        GConnection.UpdateScoreToALeaderBroad("LeaderboardName", UseProfile.Score);

    }


    private void HandleButtonContinue()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
        if (UseProfile.Titkit > 0)
        {

            MatchBox.Setup().Show();

        }
         else
        {
            ShopTickitBox.Setup().Show(); 
        }
    }    
}
