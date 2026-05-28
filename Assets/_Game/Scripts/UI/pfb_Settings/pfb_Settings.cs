public class pfb_Settings : UIBehavior
{
    public PopupSettings PopupSettings;

    public override void ActiveNormalPopup(bool active = true)
    {
        base.ActiveNormalPopup(active);
        if (active)
        {
            this.PopupSettings.ActiveNormalPopup();
        }
    }
}
