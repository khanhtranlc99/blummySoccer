using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
 
using System.Linq;
using UnityEditor;
using XGame;

public class PvP_PlayerContain : MonoBehaviour
{
    public CameraManager cameraManager;

    public MapController CurrentMap;
 
    public GameObject ready;
    public GameObject go;
    public GameObject goals;
    public GameObject draw;
    public Image blindPanel;
    public List<MapController> ListMaps;
    public List<MapController> mapControllersWasUse;
    public List<BallPvP> lsBallPvP;
    public TypeBallPvP typeBallPvP;
 //   public MapController test;
    public MapController getRandomMap
    {
        get
        {
            var availableMaps = ListMaps
                .Where(map => !mapControllersWasUse.Contains(map))
                .ToList();

            if (availableMaps.Count == 0)
                return null;

            int randomIndex = UnityEngine.Random.Range(0, availableMaps.Count);
            MapController selectedMap = availableMaps[randomIndex];

            mapControllersWasUse.Add(selectedMap); // Đánh dấu là đã dùng

            return selectedMap;
        }
    }


    public void Init()
    {
        if(CurrentMap != null)
        {
            Destroy(CurrentMap.gameObject);
        }    
        CurrentMap = Instantiate(getRandomMap, Vector3.zero, Quaternion.identity);
        CameraManager.Instance.SetCameraPosition(CurrentMap, delegate
        {
            ready.transform.localScale = Vector3.zero;
            go.transform.localScale = Vector3.zero;
            //ready.transform.position = Vector3.zero;
            //go.transform.position = Vector3.zero;
            ready.SetActive(true);
            go.SetActive(true);
            Debug.LogError("111");
          
            ready.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack).OnComplete(delegate
            {
                GlobalAudioPlayer.PlaySFX(eAudioType.READY);
                ready.SetActive(false);
                go.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack).OnComplete(delegate
                {
                    GlobalAudioPlayer.PlaySFX(eAudioType.GO);
                    go.SetActive(false);
                    TestAI.Instance.StartAI();
                    PlayerControllerPvP.Instance.Init();
                    GameManager.Instance.GAME_STATE = GAME_STATE.PLAYING;

                    
                });
            });
        });
         XGameSdk.Instance.Track("pvp", new KVItems()
        {
            {"pvp_id", PvPController.Instance.pvpScene.round },
            {"pvp_round_status", "start"},
         
        });

      
        
        
       
    }
    public void HandleGoals(TypeBallPvP paramTypeBallPvP)
    {
        typeBallPvP = paramTypeBallPvP;
        goals.transform.localScale = Vector3.zero;
        goals.SetActive(true);
        goals.transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBack).OnComplete(delegate
        {
            blindPanel.DOFade(1, 0.5f).SetDelay(1).OnComplete(delegate
            {
               
                if (PvPController.Instance.pvpScene.round < 3)
                {
                    PvPController.Instance.pvpScene.ResetTurnBall();
                    Init();
                    goals.SetActive(false);
                    blindPanel.DOFade(0, 0.5f);
                }
                else
                {
                    HandlePushScore();
                }
                foreach (var item in lsBallPvP)
                {
                    if(item != null)
                    {
                        SimplePool2.Despawn(item.gameObject);
                    }                   
                }
                lsBallPvP.Clear();

            });


        });
        CheckingRound(true);
       

    }

    public void HandleDraw()
    {
        draw.transform.localScale = Vector3.zero;
        draw.SetActive(true);
        draw.transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBack).OnComplete(delegate
        {
            blindPanel.DOFade(1, 0.5f).SetDelay(1).OnComplete(delegate
            {
             
                if (PvPController.Instance.pvpScene.round < 3)
                {
                    PvPController.Instance.pvpScene.ResetTurnBall();
                    Init();
                    draw.SetActive(false);
                    blindPanel.DOFade(0, 0.5f);
                }
                else
                {
                    HandlePushScore();
                }
                foreach (var item in lsBallPvP)
                {
                    if (item != null)
                    {
                        SimplePool2.Despawn(item.gameObject);
                    }
                }
                lsBallPvP.Clear();
            });


        });
        CheckingRound(false);
    }

    public void CheckingRound(bool isGoals)
    {
        if(isGoals)
        {
            switch(typeBallPvP)
            {
                case TypeBallPvP.User:
                    XGameSdk.Instance.Track("pvp", new KVItems()
                    {
                        {"pvp_id", PvPController.Instance.pvpScene.round },
                        {"pvp_round_status", "goals"},
                        {"winner", "user"},
                    });
                    break;
                case TypeBallPvP.Ai:
                    XGameSdk.Instance.Track("pvp", new KVItems()
                    {
                         {"pvp_id", PvPController.Instance.pvpScene.round },
                         {"pvp_round_status", "goals"},
                         {"winner", "system"},
                    });
                    break;
            }
           
        }
        else
        {
                XGameSdk.Instance.Track("pvp", new KVItems()
            {
                   {"pvp_id", PvPController.Instance.pvpScene.round },
                  {"pvp_round_status", "draw"},
         
            });
   
        }
         

      
    }

    private void HandlePushScore()
    {
        if (PvPController.Instance.pvpScene.scoreUser > PvPController.Instance.pvpScene.scoreAi)
        {
            EndGameBox.Setup(EndGameType.Win, PvPController.Instance.pvpScene.scoreUser * 10).Show();
        }
        else
        {
            if (PvPController.Instance.pvpScene.scoreUser == PvPController.Instance.pvpScene.scoreAi)
            {
                EndGameBox.Setup(EndGameType.Draw, 0).Show();
            }
            else
            {
                EndGameBox.Setup(EndGameType.Lose, -PvPController.Instance.pvpScene.scoreUser * 10).Show();
            }
        }
    }    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
                       EndGameBox.Setup(EndGameType.Win, PvPController.Instance.pvpScene.scoreUser * 10).Show();
 
        }
         if(Input.GetKeyDown(KeyCode.B))
        {
                           EndGameBox.Setup(EndGameType.Draw, 0).Show();
 
        }
           if(Input.GetKeyDown(KeyCode.N))
        {
                           EndGameBox.Setup(EndGameType.Lose, -PvPController.Instance.pvpScene.scoreUser * 10).Show();
     
        }
    }
}