using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class MatchBox : BaseBox
{
 
    #region instance

    private static MatchBox instance;

    public static MatchBox Setup( bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<MatchBox>(PathPrefabs.MATCH_BOX));
            instance.Init();
        }
        instance.InitState(); 
        return instance;
    }

    #endregion

    public Transform postAIlable;
    public Transform postDecor;
    public Transform lableAi;
    public Transform decor;
    public Image decorVs;

    public Text nameUser;
    public Text nameAi;

    public Button btnBack;
    PlayFabLeaderboardMember playFabLeaderboardMember;
    private void Init()
    {
         btnBack.onClick.AddListener(delegate { Close();  });
    }    
    private void InitState()
    {
       GConnection.GetLeaderBoardAroundPlayer("LeaderboardName", 2 , delegate { StartCoroutine(HandleShow());});
    }    


    private IEnumerator HandleShow()
    {

        yield return new WaitForSeconds(UnityEngine.Random.RandomRange(2,4));
        postDecor.gameObject.SetActive(false);
        decorVs.transform.localScale = Vector3.zero;
        playFabLeaderboardMember = GConnection.leaderBoardNearPlayer[0];
        nameAi.text = playFabLeaderboardMember.name;
        nameUser.text = UseProfile.NamePlayer;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(lableAi.transform.DOMove(postAIlable.position, 0.5f)).SetEase(Ease.OutBack);
        sequence.Append(decor.transform.DOMove(postDecor.position, 1)).SetEase(Ease.OutBack);
        sequence.Append(decorVs.DOFade(1, 0.5f)).SetEase(Ease.OutBack);
        sequence.Append(decorVs.transform.DOScale(Vector3.one, 1)).SetEase(Ease.OutBack);
        sequence.OnComplete(delegate
        {
           // GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () => { Next(); }, actionWatchLog: "Match");
            Next();
        });

       


        void Next()
        {

            Initiate.Fade("PvP", Color.black, 2f);
        }
    }
}