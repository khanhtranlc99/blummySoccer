using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class PvPScene : MonoBehaviour
{
  
    public Button btnBack;
    public List<GameObject> lsBallUser;
    public List<GameObject> lsBallAi;
    public int scoreUser;
    public int scoreAi;
    public Text tvRound;
    public Text tvScore;
    public Text tvCountDown;
    public Text tvNameAI;
    public Text tvNameUser;
    Coroutine timeCoutDown;
    public int round;
    public bool AllBallUserOff
    {
        get
        {
            foreach(var item in lsBallUser)
            {
                if(item.activeSelf)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public bool AllBallAiOff
    {
        get
        {
            foreach (var item in lsBallAi)
            {
                if (item.activeSelf)
                {
                    return false;
                }
            }
            return true;
        }
    }

    PlayFabLeaderboardMember playFabLeaderboardMember;

    public void Init()
    {
        round = 1;
        scoreUser = 0; 
        scoreAi = 0;
        btnBack.onClick.AddListener(delegate
        {
            GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
            Initiate.Fade("HomePvP", Color.black, 2f);
        });
        tvScore.text = scoreUser + " : " + scoreAi;
        playFabLeaderboardMember = GConnection.leaderBoardNearPlayer[0];
        tvNameAI.text = playFabLeaderboardMember.name;
        tvNameUser.text = UseProfile.NamePlayer;
    }

    public void HandleSubtrackUser()
    {
       foreach(var item in lsBallUser)
        {
            if(item.activeSelf)
            {
                item.SetActive(false);
                break;
            }
        }
       if(AllBallAiOff && AllBallUserOff)
        {
            if (timeCoutDown != null)
            {
                StopCoroutine(timeCoutDown);
                timeCoutDown = null;
            }
            timeCoutDown = StartCoroutine(HandleShowDraw());
        }
     
     
       
    }
    public void HandleSubtrackAI()
    {
        foreach (var item in lsBallAi)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
                break;
            }
        }
        if (AllBallAiOff && AllBallUserOff)
        {
            if (timeCoutDown != null)
            {
                StopCoroutine(timeCoutDown);
                timeCoutDown = null;
            }
            timeCoutDown = StartCoroutine(HandleShowDraw());
        }
    }
    public void HandlePlusScoreAi()
    {
        scoreAi += 1;
        tvScore.text = scoreUser + " : " + scoreAi;
        if (scoreAi == 3 && scoreAi > scoreUser)
        {
            Debug.LogError("Ai_Win");
        }

    }
    public void HandlePlusScoreUser()
    {
        scoreUser += 1;
        tvScore.text = scoreUser + " : " + scoreAi;
        if (scoreUser == 3 && scoreUser > scoreAi)
        {
            Debug.LogError("Player_Win");
        }
    }

    private IEnumerator HandleShowDraw()
    {
        tvCountDown.text = "5";
        yield return new WaitForSeconds(1);
        tvCountDown.text = "4";
        yield return new WaitForSeconds(1);
        tvCountDown.text = "3";
        yield return new WaitForSeconds(1);
        tvCountDown.text = "2";
        yield return new WaitForSeconds(1);
        tvCountDown.text = "1";
        yield return new WaitForSeconds(1);
        tvCountDown.text = "";
        if (AllBallAiOff && AllBallUserOff)
        {
            PvPController.Instance.playerContain.HandleDraw();
        }
    }

    public void ResetTurnBall()
    {
        foreach (var item in lsBallUser)
        {
            item.SetActive(true);
        }
        foreach (var item in lsBallAi)
        {
            item.SetActive(true);
        }
     
        if (round == 1)
        {
            round = 2;
            tvRound.text = "Round II";
        }
        else
        {
            if (round == 2)
            {
                round = 3;
                tvRound.text = "Round III";
            }

        }
       

    }
}