using DG.Tweening;
using MoonlightFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pfb_Loading : UIBehavior
{
   // public SlicedFilledImage Progress;
    public Slider progress;
    public GameObject NotiTrackingIOSObject;
    protected override void Awake()
    {
        this.NotiTrackingIOSObject.SetActive(false);
    }
    private void Start()
    {
        Play();
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
