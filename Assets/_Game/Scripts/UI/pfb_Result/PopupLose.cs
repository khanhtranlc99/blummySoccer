using MoonlightFramework;
using UnityEngine;
using UnityEngine.UI;

public class PopupLose : UIPopupBehavior
{
    [SerializeField] protected Button btnReplay, btnWatchToRevive;

    protected override void Start()
    {
        base.Start();
        this.btnReplay.onClick.AddListener(OnReplay);
        this.btnWatchToRevive.onClick.AddListener(OnWatchToRevive);
    }
    protected void OnReplay()
    {
       // AdsManager.Instance.ShowAds(AdsNetwork.Max, AdsType.Interstitial);
        GameController.Instance.admobAds.ShowInterstitial(false, actionIniterClose: () => { Next(); }, actionWatchLog: "Retry");

        void Next()
        {
            UIManager.Instance.pfb_Result.ActiveNormalPopup(false);
            GameManager.Instance.Replay();
        }
     
    }

    protected void OnWatchToRevive()
    {
        //AdsManager.Instance.ShowAds(AdsNetwork.Max, AdsType.Rewarded, delegate
        //{
        //    //+1 ball

        //});
    }
}
