using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Sirenix.Serialization;
using DG.Tweening;

public class ShopTickitBox : BaseBox 
{
    #region instance

    private static ShopTickitBox instance;

    public static ShopTickitBox Setup(bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<ShopTickitBox>(PathPrefabs.SHOP_TICKIT_BOX));
            instance.Init();
        }
        instance.InitState();
        return instance;
    }

    #endregion

    public Button btnBack;
    public List<LableTitkit> lsTickit;
    public GameObject panelReward;
    public Text tvReward;
    public Button btnOK;
    public Transform rewardTitkit;
    private void Init()
    {
        btnBack.onClick.AddListener(delegate { GlobalAudioPlayer.PlaySFX(eAudioType.CLICK); Close(); });
        btnOK.onClick.AddListener(delegate { GlobalAudioPlayer.PlaySFX(eAudioType.CLICK); panelReward.SetActive(false);  });
        foreach (var item in lsTickit)
        {
            item.Init(this);
        }
        rewardTitkit.transform.localScale = Vector3.zero;
    }
    private void InitState()
    {
        
    }

    public void HandleShowReward(int param)
    {
        rewardTitkit.transform.DOScale(Vector3.one, 0.5f);
        panelReward.SetActive(true);
        tvReward.text = param.ToString();
    }
}
