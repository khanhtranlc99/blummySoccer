using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameSceneEvent : MonoBehaviour
{
    public InformationUI_Base infoUser;
    public InformationUI_Base infoCompetitor;
    public Button btnOpenLeaderBroadBox;
    public Button btnTestPushScoreLeaderBroad;

    public void Init()
    {
        btnOpenLeaderBroadBox.onClick.AddListener(delegate { HandleShowLeaderBroad(); });
        btnTestPushScoreLeaderBroad.onClick.AddListener(delegate { HandleOnClickTestPushScore(); });
    }    




    public void HandleOnClickTestPushScore()
    {
        UseProfile.Score += 10;
        GConnection.UpdateScoreToALeaderBroad("LeaderboardName", UseProfile.Score, delegate { Debug.LogError("đã đẩy điểm thành công");   });
    }

    public void HandleShowLeaderBroad()
    {
     ///   LeaderBoardPvPBox.Setup().Show();
    }




}
