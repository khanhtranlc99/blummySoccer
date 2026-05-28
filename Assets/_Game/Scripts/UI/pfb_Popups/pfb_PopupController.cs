

public class pfb_PopupController : UIBehavior
{
    public PopupRateUs PopupRateUs;
    public override void ActiveNormalPopup(bool active = true)
    {
        base.ActiveNormalPopup(active);
        if (active)
        {
            this.PopupRateUs.ForceActiveNormalPopup(false);
        }
    }
}
