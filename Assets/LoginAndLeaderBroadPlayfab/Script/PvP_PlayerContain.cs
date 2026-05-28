using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using System.Linq;
using UnityEditor;
using UnityEngine.SocialPlatforms.Impl;

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
       
    }
    public void HandleGoals()
    {
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
}