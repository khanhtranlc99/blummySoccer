using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSettings : UIPopupBehavior
{
    [SerializeField] protected Settings_CustomToggle TG_Music, TG_Sound, TG_Vibrate, TG_ShowStar;
    [SerializeField] protected Button btnCommunity, btnFeedbackUs, btnPrivacyPolicy;
    [SerializeField] protected Button btnLinkAccountGoogle, btnSaveProgress, btnRestorePurchase;
    [SerializeField] protected TextMeshProUGUI txtSignIn;
    public WaitingBlocker WaitingBlocker;
    protected bool canSaveGameData = true;
    protected override void Start()
    {
        base.Start();
        InitTGMusicAndSoundAndVibration();
    }
    private void CallbackAfterSignInOtherAccount()
    {
        this.WaitingBlocker.gameObject.SetActive(false);
        Utils.RestartApplication();
    }
    protected void InitTGMusicAndSoundAndVibration()
    {
        TG_Music.Init(Facade.Instance.PlayerPrefManager.IsMusicOn, delegate (bool isMusicOn)
        {
            Facade.Instance.PlayerPrefManager.IsMusicOn = isMusicOn;
            // SoundManager.Instance.UpdateMusicState();
        });

        TG_Sound.Init(Facade.Instance.PlayerPrefManager.IsSoundOn, delegate (bool isSoundOn)
        {
            Facade.Instance.PlayerPrefManager.IsSoundOn = isSoundOn;
            // SoundManager.Instance.UpdateSoundState();
        });

        TG_Vibrate.Init(Facade.Instance.PlayerPrefManager.IsVibrateOn, delegate (bool isVibrateOn)
        {
            Facade.Instance.PlayerPrefManager.IsVibrateOn = isVibrateOn;
        });
    }
    private bool isProcessingSaveData = false;
    private Coroutine TWSaveData;
    protected override void OnClose()
    {
        base.OnClose();
        UIManager.Instance.pfb_Settings.ActiveNormalPopup(false);
    }
}
