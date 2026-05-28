using System.Collections.Generic;
using MoonlightFramework;
using UnityEngine;

public class LableAds : LableTitkit
{
    public override void Init(ShopTickitBox param)
    {
        shopTickitBox = param;
        btnBuy.onClick.AddListener(HandleWatchAds); 
    }

    private void HandleWatchAds()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
        //AdsManager.Instance.ShowAds(AdsNetwork.Max, AdsType.Rewarded, () =>
        //{
        //    UseProfile.Titkit += 3;
        //    shopTickitBox.HandleShowReward(3);
        //    LeaderBoardPvPBox.Instance.InitState();
        //}, "Titkit");
        GameController.Instance.admobAds.ShowVideoReward(
                    actionReward: () =>
                    {
                        UseProfile.Titkit += 3;
                        shopTickitBox.HandleShowReward(3);
                        LeaderBoardPvPBox.Instance.InitState();
                    },
                    actionNotLoadedVideo: () =>
                    {
                        GameController.Instance.effectController.SpawnEffectText_FlyUp
                         (

                         btnBuy.transform.position,
                         "No video at the moment!",
                         Color.white,
                         isSpawnItemPlayer: true
                         );
                    },
                    actionClose: null,
                      ActionWatchVideo.HeartInHearPopup,
                     "100");
    }
}