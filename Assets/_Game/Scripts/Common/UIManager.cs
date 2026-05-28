using System.Collections;
using System.Collections.Generic;
using MoonlightFramework;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public pfb_Loading pfb_Loading;
    public pfb_VFXUI pfb_VFXUI;

    public pfb_PopupController pfb_PopupController;
    public pfb_Settings pfb_Settings;
    public pfb_Message pfb_Message;
    public pfb_Gameplay pfb_Gameplay;
    public pfb_Result pfb_Result;
    public pfb_Intro pfb_Intro;
    public pfb_BanNoAds pfb_BanNoAds;


    [SerializeField] private List<UIBehavior> listPopup = new List<UIBehavior>();
    public bool IsTaskSetupDone = false;
    public void PreInit()
    {
        this.pfb_Loading = Instantiate(Resources.Load<pfb_Loading>("pfb_Loading"));
        this.pfb_VFXUI = Instantiate(Resources.Load<pfb_VFXUI>("pfb_VFXUI"));
    }
    //có thể dùng addressable để load phần này
    public IEnumerator TaskSetup()
    {
        this.pfb_PopupController = InstantiateAndSetExistUIByName<pfb_PopupController>("pfb_PopupController");
        this.pfb_Settings = InstantiateAndSetExistUIByName<pfb_Settings>("pfb_Settings");
        this.pfb_Message = InstantiateAndSetExistUIByName<pfb_Message>("pfb_Message");
        this.pfb_Gameplay = InstantiateAndSetExistUIByName<pfb_Gameplay>("pfb_Gameplay");
        this.pfb_Result = InstantiateAndSetExistUIByName<pfb_Result>("pfb_Result");
        this.pfb_Intro = InstantiateAndSetExistUIByName<pfb_Intro>("pfb_Intro");
        this.pfb_BanNoAds = InstantiateAndSetExistUIByName<pfb_BanNoAds>("pfb_BanNoAds");

        this.pfb_PopupController.ForceActiveNormalPopup(false);
        this.pfb_Settings.ForceActiveNormalPopup(false);
        this.pfb_Message.ForceActiveNormalPopup(false);
        this.pfb_Gameplay.ForceActiveNormalPopup(false);
        this.pfb_VFXUI.ForceActiveNormalPopup(false);
        this.pfb_Result.ForceActiveNormalPopup(false);
        this.pfb_Intro.ForceActiveNormalPopup(false);
        this.pfb_BanNoAds.ForceActiveNormalPopup(false);


        listPopup.Remove(pfb_VFXUI);
        listPopup.Remove(pfb_Loading);
        IsTaskSetupDone = true;
        yield return null;
    }
    private T InstantiateAndSetExistUIByName<T>(string Path) where T : UIBehavior
    {
        var obj = Instantiate<T>(Resources.Load<T>(Path));
        DontDestroyOnLoad(obj);
        listPopup.Add(obj);
        return obj;
    }
    public void DeactivateAllPopup()
    {
        listPopup.ForEach(x => x.ActiveNormalPopup(false));
    }
}
