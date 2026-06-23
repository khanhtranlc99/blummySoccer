using DG.Tweening;
using MoonlightFramework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XGame;

public class pfb_Loading : UIBehavior
{
   // public SlicedFilledImage Progress;
    public Slider progress;
    public GameObject NotiTrackingIOSObject;
    public float loadingTime;
    protected override void Awake()
    {
        this.NotiTrackingIOSObject.SetActive(false);
        loadingTime = Time.time;
    }
    private void Start()
    {
        try
        {
            Play();
            XGameSdk.Instance.Track("startAPP_loading", new KVItems()
            {
             {"view_state", "first_frame"},

            });
        }
        catch(Exception Error)
        {
            XGameSdk.Instance.Track("startAPP_loading", new KVItems()
            {
              {"error_reason:", "first_frame"},

            });
        }
   
    }
    public void Play()
    {
        float timeLoad = 3f;
#if UNITY_EDITOR 
        timeLoad = 0.5f;
#elif UNITY_IOS
        bool checkAskGDPR = PlayerPrefs.GetInt("IsTrackGDPR", -1) == -1 ? false : true;
        if (!checkAskGDPR) //Nếu chưa check gdpr thì cho load chậm đi tý đợi firebase
        {
            timeLoad = 4f;
        }
#endif
     
        this.progress.DOValue(1, timeLoad).From(0).OnComplete(delegate
        {
            ActiveNormalPopup(false);
            GameManager.Instance.DoneLoading();
            LoadingDone();
            XGameSdk.Instance.Track("startAPP_loading", new KVItems()
        {
            {"view_state", "last_frame"},

        });
            var temp = Time.time - loadingTime;

            XGameSdk.Instance.Track("startAPP_loading", new KVItems()
        {
            {"loading_time", temp },

        });

        });
    }

    protected void LoadingDone()
    {
#if UNITY_IOS
        bool checkAskGDPR = PlayerPrefs.GetInt("IsTrackGDPR", -1) == -1 ? false : true;
        if (!checkAskGDPR)
        {
            if (!FirebaseRemoteConfigManager.Instance.isCallSuccess)
            {
                CallAds();
                return;
            }
            else
            {
                if (FirebaseRemoteConfigManager.Instance.enableTrackingIOS)
                    AttPermissionRequest.StartGetPermissionTracking(delegate
                    {
                        PlayerPrefs.SetInt("IsTrackGDPR", 1);
                        DOVirtual.DelayedCall(.3f, delegate
                        {
                            CallAds();
                        });
                    });
                else
                {
                    CallAds();
                }
            }
        }
        else
        {
            CallAds();
        }
#endif
    }

    protected void CallAds()
    {
        //if (!AdsManager.Instance.dictAdsNetwork[AdsNetwork.Max].IsInitialize())
        //{
        //    AdsManager.Instance.InitAds(AdsNetwork.Max);
        //}
    }
}
